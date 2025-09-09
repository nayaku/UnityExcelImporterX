using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public class ExcelImporter : AssetPostprocessor
{
    class ExcelAssetInfo
    {
        public Type AssetType { get; set; }
        public ExcelAssetAttribute Attribute { get; set; }
        public string ExcelName
        {
            get
            {
                return string.IsNullOrEmpty(Attribute.ExcelName) ? AssetType.Name : Attribute.ExcelName;
            }
        }
    }

    private static Dictionary<string, ExcelAssetInfo> cachedInfos = null; // Clear on compile.

    static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
    {
        bool imported = false;
        foreach (string path in importedAssets)
        {
            if (Path.GetExtension(path) == ".xls" || Path.GetExtension(path) == ".xlsx")
            {
                if (cachedInfos == null)
                    BuildExcelAssetInfoCache();

                var excelName = Path.GetFileNameWithoutExtension(path);
                if (excelName.StartsWith("~$")) continue;

                var ok = cachedInfos.TryGetValue(excelName, out var info);
                if (!ok) continue;

                ImportExcel(path, info);
                imported = true;
            }
        }

        if (imported)
        {
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }

    private static void BuildExcelAssetInfoCache()
    {
        var dict = new Dictionary<string, ExcelAssetInfo>();
        foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            foreach (var type in assembly.GetTypes())
            {
                var attributes = type.GetCustomAttributes(typeof(ExcelAssetAttribute), false);
                if (attributes.Length == 0) continue;
                var attribute = (ExcelAssetAttribute)attributes[0];
                var info = new ExcelAssetInfo()
                {
                    AssetType = type,
                    Attribute = attribute
                };
                var excelName = string.IsNullOrEmpty(attribute.ExcelName) ?
                    type.Name : attribute.ExcelName;
                dict[excelName] = info;
            }
        }
        cachedInfos = dict;
    }

    private static UnityEngine.Object LoadOrCreateAsset(string assetPath, Type assetType)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(assetPath));

        var asset = AssetDatabase.LoadAssetAtPath(assetPath, assetType);

        if (asset == null)
        {
            asset = ScriptableObject.CreateInstance(assetType.Name);
            AssetDatabase.CreateAsset((ScriptableObject)asset, assetPath);
            //asset.hideFlags = HideFlags.NotEditable;
        }

        return asset;
    }

    private static IWorkbook LoadBook(string excelPath)
    {
        using FileStream stream = File.Open(excelPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        if (Path.GetExtension(excelPath) == ".xls") return new HSSFWorkbook(stream);
        else return new XSSFWorkbook(stream);
    }

    private static object CellToFieldObject(ICell cell, FieldInfo fieldInfo, bool isFormulaEvalute = false)
    {
        var type = isFormulaEvalute ? cell.CachedFormulaResultType : cell.CellType;

        switch (type)
        {
            case CellType.String:
                if (fieldInfo.FieldType.IsEnum)
                    return Enum.Parse(fieldInfo.FieldType, cell.StringCellValue);
                else
                    return cell.StringCellValue;
            case CellType.Boolean:
                return cell.BooleanCellValue;
            case CellType.Numeric:
                return Convert.ChangeType(cell.NumericCellValue, fieldInfo.FieldType);
            case CellType.Formula:
                if (isFormulaEvalute) return null;
                return CellToFieldObject(cell, fieldInfo, true);
            default:
                if (fieldInfo.FieldType.IsValueType)
                {
                    return Activator.CreateInstance(fieldInfo.FieldType);
                }
                return null;
        }
    }


    private static object CreateEntityFromRow(IRow row, List<string> columnNames, Type entityType, string sheetName)
    {
        var entity = Activator.CreateInstance(entityType);

        for (var i = 0; i < columnNames.Count; i++)
        {
            var entityField = entityType.GetField(
                columnNames[i],
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic
            );
            if (entityField == null) continue;
            if (!entityField.IsPublic && entityField.GetCustomAttributes(typeof(SerializeField), false).Length == 0) continue;

            var cell = row.GetCell(i);
            if (cell == null || cell.CellType == CellType.Blank) continue;
            try
            {
                var fieldValue = CellToFieldObject(cell, entityField);
                entityField.SetValue(entity, fieldValue);
            }
            catch
            {
                throw new Exception(string.Format("Invalid excel cell type at row {0}, column {1}, {2} sheet.", row.RowNum, cell.ColumnIndex, sheetName));
            }
        }

        return entity;
    }

    private static IList GetEntityListFromSheet(ISheet sheet, Type entityType)
    {
        var sheetFields = ExcelAssetHelper.GetFieldFromSheetHeader(sheet);
        var excelColumnNames = sheetFields.ConvertAll(f => f.FieldName);

        var listType = typeof(List<>).MakeGenericType(entityType);
        var entityList = (IList)Activator.CreateInstance(listType);
        
        // 前三行是表头、类型、注释，从第四行开始是数据
        for (var i = 3; i <= sheet.LastRowNum; i++)
        {
            var row = sheet.GetRow(i);
            if (row == null) continue;

            var entryCell = row.GetCell(0);
            // 空行结束
            if (entryCell == null || entryCell.CellType == CellType.Blank) break;

            // 跳过注释行
            if (entryCell.CellType == CellType.String && entryCell.StringCellValue.StartsWith("#")) continue;

            var entity = CreateEntityFromRow(row, excelColumnNames, entityType, sheet.SheetName);
            entityList.Add(entity);
        }
        return entityList;
    }

    private static void ImportExcel(string excelPath, ExcelAssetInfo info)
    {
        var assetPath = "";
        var assetName = info.AssetType.Name + ".asset";

        if (string.IsNullOrEmpty(info.Attribute.AssetPath))
        {
            var basePath = Path.GetDirectoryName(excelPath);
            assetPath = Path.Combine(basePath, assetName);
        }
        else
        {
            var path = Path.Combine("Assets", info.Attribute.AssetPath);
            assetPath = Path.Combine(path, assetName);
        }
        UnityEngine.Object asset = LoadOrCreateAsset(assetPath, info.AssetType);

        using IWorkbook book = LoadBook(excelPath);

        var assetFields = info.AssetType.GetFields();
        var sheetCount = 0;

        foreach (var assetField in assetFields)
        {
            var sheet = book.GetSheet(assetField.Name);
            if (sheet == null) continue;

            var fieldType = assetField.FieldType;
            if (!fieldType.IsGenericType || (fieldType.GetGenericTypeDefinition() != typeof(List<>))) continue;

            var types = fieldType.GetGenericArguments();
            var entityType = types[0];

            var entities = GetEntityListFromSheet(sheet, entityType);
            assetField.SetValue(asset, entities);
            sheetCount++;
        }

        if (info.Attribute.LogOnImport)
        {
            Debug.Log(string.Format("Imported {0} sheets form {1}.", sheetCount, excelPath));
        }

        EditorUtility.SetDirty(asset);
    }
}
