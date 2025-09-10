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
    private class ExcelAssetInfo
    {
        public Type AssetType { get; set; }
        public ExcelAssetAttribute Attribute { get; set; }
        public string ExcelName => string.IsNullOrEmpty(Attribute.ExcelName) ? AssetType.Name : Attribute.ExcelName;
    }

    private static Dictionary<string, ExcelAssetInfo> cachedInfos = null; // Clear on compile.

    private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
    {
        bool imported = false;
        foreach (string path in importedAssets)
        {
            if (Path.GetExtension(path) is ".xls" or ".xlsx")
            {
                if (cachedInfos == null)
                {
                    BuildExcelAssetInfoCache();
                }

                string excelName = Path.GetFileNameWithoutExtension(path);
                if (excelName.StartsWith("~$"))
                {
                    continue;
                }

                bool ok = cachedInfos.TryGetValue(excelName, out ExcelAssetInfo info);
                if (!ok)
                {
                    continue;
                }

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
        Dictionary<string, ExcelAssetInfo> dict = new();
        foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            foreach (Type type in assembly.GetTypes())
            {
                object[] attributes = type.GetCustomAttributes(typeof(ExcelAssetAttribute), false);
                if (attributes.Length == 0)
                {
                    continue;
                }

                ExcelAssetAttribute attribute = (ExcelAssetAttribute)attributes[0];
                ExcelAssetInfo info = new()
                {
                    AssetType = type,
                    Attribute = attribute
                };
                string excelName = string.IsNullOrEmpty(attribute.ExcelName) ?
                    type.Name : attribute.ExcelName;
                dict[excelName] = info;
            }
        }
        cachedInfos = dict;
    }

    private static UnityEngine.Object LoadOrCreateAsset(string assetPath, Type assetType)
    {
        _ = Directory.CreateDirectory(Path.GetDirectoryName(assetPath));

        UnityEngine.Object asset = AssetDatabase.LoadAssetAtPath(assetPath, assetType);

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
        return Path.GetExtension(excelPath) == ".xls" ? new HSSFWorkbook(stream) : new XSSFWorkbook(stream);
    }

    private static object CellToFieldObject(ICell cell, FieldInfo fieldInfo, bool isFormulaEvalute = false)
    {
        CellType type = isFormulaEvalute ? cell.CachedFormulaResultType : cell.CellType;

        switch (type)
        {
            case CellType.String:
                return fieldInfo.FieldType.IsEnum ? Enum.Parse(fieldInfo.FieldType, cell.StringCellValue) : cell.StringCellValue;
            case CellType.Boolean:
                return cell.BooleanCellValue;
            case CellType.Numeric:
                return Convert.ChangeType(cell.NumericCellValue, fieldInfo.FieldType);
            case CellType.Formula:
                if (isFormulaEvalute)
                {
                    return null;
                }
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
        object entity = Activator.CreateInstance(entityType);

        for (int i = 0; i < columnNames.Count; i++)
        {
            FieldInfo entityField = entityType.GetField(
                columnNames[i],
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic
            );
            if (entityField == null)
            {
                continue;
            }

            if (!entityField.IsPublic && entityField.GetCustomAttributes(typeof(SerializeField), false).Length == 0)
            {
                continue;
            }

            ICell cell = row.GetCell(i);
            if (cell == null || cell.CellType == CellType.Blank)
            {
                continue;
            }

            try
            {
                object fieldValue = CellToFieldObject(cell, entityField);
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
        List<SheetField> sheetFields = ExcelAssetHelper.GetFieldFromSheetHeader(sheet);
        List<string> excelColumnNames = sheetFields.ConvertAll(f => f.FieldName);

        Type listType = typeof(List<>).MakeGenericType(entityType);
        IList entityList = (IList)Activator.CreateInstance(listType);

        // 前三行是表头、类型、注释，从第四行开始是数据
        for (int i = 3; i <= sheet.LastRowNum; i++)
        {
            IRow row = sheet.GetRow(i);
            if (row == null)
            {
                continue;
            }

            ICell entryCell = row.GetCell(0);
            // 空行结束
            if (entryCell == null || entryCell.CellType == CellType.Blank)
            {
                break;
            }

            // 跳过注释行
            if (entryCell.CellType == CellType.String && entryCell.StringCellValue.StartsWith("#"))
            {
                continue;
            }

            object entity = CreateEntityFromRow(row, excelColumnNames, entityType, sheet.SheetName);
            _ = entityList.Add(entity);
        }
        return entityList;
    }

    private static void ImportExcel(string excelPath, ExcelAssetInfo info)
    {
        string assetName = info.AssetType.Name + ".asset";

        string assetPath;
        if (string.IsNullOrEmpty(info.Attribute.AssetPath))
        {
            string basePath = Path.GetDirectoryName(excelPath);
            assetPath = Path.Combine(basePath, assetName);
        }
        else
        {
            string path = Path.Combine("Assets", info.Attribute.AssetPath);
            assetPath = Path.Combine(path, assetName);
        }
        UnityEngine.Object asset = LoadOrCreateAsset(assetPath, info.AssetType);

        using IWorkbook book = LoadBook(excelPath);

        FieldInfo[] assetFields = info.AssetType.GetFields();
        int sheetCount = 0;

        foreach (FieldInfo assetField in assetFields)
        {
            ISheet sheet = book.GetSheet(assetField.Name);
            if (sheet == null)
            {
                continue;
            }

            Type fieldType = assetField.FieldType;
            if (!fieldType.IsGenericType || (fieldType.GetGenericTypeDefinition() != typeof(List<>)))
            {
                continue;
            }

            Type[] types = fieldType.GetGenericArguments();
            Type entityType = types[0];

            IList entities = GetEntityListFromSheet(sheet, entityType);
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
