using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEditor;



public class SheetStruct
{
    public string SheetName;
    public List<SheetField> Fields = new();
}

public class ExcelAssetScriptMenu
{
    private const string ScriptTemplateName = "ExcelAssetScriptTemplete.cs.txt";

    [MenuItem("Assets/Create/ExcelAssetScript", false)]
    public static void CreateScript()
    {
        // 选中文件
        UnityEngine.Object[] selectedAssets = Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.Assets);
        UnityEngine.Object selectedAsset = selectedAssets[0];
        string assetPath = AssetDatabase.GetAssetPath(selectedAsset);
        string assetName = Path.GetFileName(assetPath);
        string assetDirectory = Path.GetDirectoryName(assetPath);

        // 选择保存路径
        string newScriptName = Path.ChangeExtension(assetName, "cs");
        string savePath = EditorUtility.SaveFilePanel("Save ExcelAssetScript", assetDirectory, newScriptName, "cs");
        if (string.IsNullOrEmpty(savePath))
        {
            return;
        }

        // 生成脚本
        CreateScript(assetPath, savePath);

        // 刷新资源
        AssetDatabase.Refresh();
    }

    [MenuItem("Assets/Create/ExcelAssetScript", true)]
    public static bool CreateScriptValidation()
    {
        UnityEngine.Object[] selectedAssets = Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.Assets);
        if (selectedAssets.Length != 1)
        {
            return false;
        }

        string path = AssetDatabase.GetAssetPath(selectedAssets[0]);
        return Path.GetExtension(path) is ".xls" or ".xlsx";
    }

    private static void CreateScript(string assetPath, string savePath)
    {
        // 读取Excel文件
        List<SheetStruct> sheetStructs = GetSheetStruct(assetPath);
        if (sheetStructs.Count == 0)
        {
            return;
        }

        string assetName = Path.GetFileNameWithoutExtension(assetPath);
        string scriptContent = BuildScriptContent(assetName, sheetStructs);
        NewlineNormalizer.Write(savePath, scriptContent);
    }

    private static List<SheetStruct> GetSheetStruct(string excelPath)
    {
        List<SheetStruct> sheetStructs = new();
        using FileStream stream = File.Open(excelPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        IWorkbook book = Path.GetExtension(excelPath) == ".xls" ? new HSSFWorkbook(stream) : new XSSFWorkbook(stream);
        for (int i = 0; i < book.NumberOfSheets; i++)
        {
            ISheet sheet = book.GetSheetAt(i);
            List<SheetField> sheetfields = ExcelAssetHelper.GetFieldFromSheetHeader(sheet);
            if (sheetfields == null || sheetfields.Count == 0)
            {
                continue;
            }

            SheetStruct sheetStruct = new()
            {
                SheetName = sheet.SheetName,
                Fields = sheetfields
            };
            sheetStructs.Add(sheetStruct);
        }
        return sheetStructs;
    }

    private static string GetScriptTemplate()
    {
        string currentDirectory = Directory.GetCurrentDirectory();
        string[] filePath = Directory.GetFiles(currentDirectory, ScriptTemplateName, SearchOption.AllDirectories);
        if (filePath.Length == 0)
        {
            throw new Exception("Script template not found.");
        }

        string templateString = NewlineNormalizer.Read(filePath[0]);
        return templateString;
    }

    private static string BuildScriptEnityContent(string entityTemplateString, string excelName, string sheetName,
        List<SheetField> Fields)
    {
        string enityClassName = excelName + "Entity";
        if (!string.IsNullOrEmpty(sheetName))
        {
            enityClassName += "_" + sheetName;
        }
        entityTemplateString = entityTemplateString.Replace("#ASSETENITYNAME#", enityClassName);
        string fields = "";
        foreach (SheetField field in Fields)
        {
            if (!string.IsNullOrEmpty(field.FieldComment))
            {
                fields += $"    /// <summary>\n    /// {field.FieldComment}\n    /// </summary>\n";
            }
            fields += $"    public {field.FieldType} {field.FieldName};\n";
        }
        entityTemplateString = entityTemplateString.Replace("#ASSETENITYFIELDS#\n", fields);
        entityTemplateString += "\n";
        return entityTemplateString;
    }

    private static string BuildScriptFields(string excelName, List<SheetStruct> sheetStructs)
    {
        string scriptFieldContent = "";
        // 工作表为1个时，不区分工作表名称
        if (sheetStructs.Count == 1)
        {
            string enityClassName = excelName + "Entity";
            scriptFieldContent = $"    public List<{enityClassName}> {sheetStructs[0].SheetName};";
        }
        else
        {
            foreach (SheetStruct sheetStruct in sheetStructs)
            {
                string enityClassName = excelName + "Entity" + "_" + sheetStruct.SheetName;
                scriptFieldContent += $"    public List<{enityClassName}> {sheetStruct.SheetName};\n";
            }
        }
        return scriptFieldContent;
    }

    private static string BuildScriptContent(string excelName, List<SheetStruct> sheetStructs)
    {
        string templateString = GetScriptTemplate();
        Match entityTemplateStringMatch = Regex.Match(templateString,
            "#BEGINASSETENITYNAMEDEFINE#(.*?)#ENDASSETENITYNAMEDEFINE#", RegexOptions.Singleline);
        if (!entityTemplateStringMatch.Success)
        {
            throw new Exception("Script template format error.");
        }

        string entityTemplateString = entityTemplateStringMatch.Groups[1].Value.Trim();
        // 工作表为1个时，不区分工作表名称
        if (sheetStructs.Count == 1)
        {
            entityTemplateString = BuildScriptEnityContent(entityTemplateString, excelName, "",
                sheetStructs[0].Fields);
        }
        else
        {
            string entityStrings = "";
            foreach (SheetStruct sheetStruct in sheetStructs)
            {
                entityStrings += BuildScriptEnityContent(entityTemplateString, excelName, sheetStruct.SheetName,
                    sheetStruct.Fields) + "\n";
            }
            entityTemplateString = entityStrings;
        }
        string scriptFields = BuildScriptFields(excelName, sheetStructs);
        string result = templateString.Replace(entityTemplateStringMatch.Value, entityTemplateString);
        result = result.Replace("#ASSETSCRIPTNAME#", excelName);
        result = result.Replace("#ASSETSCRIPTFIELDS#\n", scriptFields);
        return result;
    }
}
