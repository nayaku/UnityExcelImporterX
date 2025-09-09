using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;



public class SheetStruct
{
    public string SheetName;
    public List<SheetField> Fields = new();
}

public class ExcelAssetScriptMenu
{
    const string ScriptTemplateName = "ExcelAssetScriptTemplete.cs.txt";

    [MenuItem("Assets/Create/ExcelAssetScript", false)]
    private static void CreateScript()
    {
        // 选中文件
        var selectedAssets = Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.Assets);
        var selectedAsset = selectedAssets[0];
        var assetPath = AssetDatabase.GetAssetPath(selectedAsset);
        var assetName = Path.GetFileName(assetPath);
        var assetDirectory = Path.GetDirectoryName(assetPath);

        // 选择保存路径
        var newScriptName = Path.ChangeExtension(assetName, "cs");
        var savePath = EditorUtility.SaveFilePanel("Save ExcelAssetScript", assetDirectory, newScriptName, "cs");
        if (string.IsNullOrEmpty(savePath))
            return;

        // 生成脚本
        CreateScript(assetPath, savePath);

        // 刷新资源
        AssetDatabase.Refresh();
    }

    [MenuItem("Assets/Create/ExcelAssetScript", true)]
    private static bool CreateScriptValidation()
    {
        var selectedAssets = Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.Assets);
        if (selectedAssets.Length != 1) return false;
        var path = AssetDatabase.GetAssetPath(selectedAssets[0]);
        return Path.GetExtension(path) == ".xls" || Path.GetExtension(path) == ".xlsx";
    }

    private static void CreateScript(string assetPath, string savePath)
    {
        // 读取Excel文件
        var sheetStructs = GetSheetStruct(assetPath);
        if (sheetStructs.Count == 0) return;
        var assetName = Path.GetFileNameWithoutExtension(assetPath);
        var scriptContent = BuildScriptContent(assetName, sheetStructs);
        NewlineNormalizer.Write(savePath, scriptContent);
    }

    private static List<SheetStruct> GetSheetStruct(string excelPath)
    {
        var sheetStructs = new List<SheetStruct>();
        using var stream = File.Open(excelPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        IWorkbook book = null;
        if (Path.GetExtension(excelPath) == ".xls") book = new HSSFWorkbook(stream);
        else book = new XSSFWorkbook(stream);
        for (var i = 0; i < book.NumberOfSheets; i++)
        {
            var sheet = book.GetSheetAt(i);
            var sheetfields = ExcelAssetHelper.GetFieldFromSheetHeader(sheet);
            if (sheetfields == null || sheetfields.Count == 0) continue;

            var sheetStruct = new SheetStruct
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
        var currentDirectory = Directory.GetCurrentDirectory();
        var filePath = Directory.GetFiles(currentDirectory, ScriptTemplateName, SearchOption.AllDirectories);
        if (filePath.Length == 0) throw new Exception("Script template not found.");

        var templateString = NewlineNormalizer.Read(filePath[0]);
        return templateString;
    }

    private static string BuildScriptEnityContent(string entityTemplateString, string excelName, string sheetName,
        List<SheetField> Fields)
    {
        var enityClassName = excelName + "Entity";
        if (!string.IsNullOrEmpty(sheetName))
        {
            enityClassName += "_" + sheetName;
        }
        entityTemplateString = entityTemplateString.Replace("#ASSETENITYNAME#", enityClassName);
        var fields = "";
        foreach (var field in Fields)
        {
            if (!string.IsNullOrEmpty(field.FieldComment))
            {
                fields += $"\t/// <summary>\n\t/// {field.FieldComment}\n\t/// </summary>\n";
            }
            fields += $"\tpublic {field.FieldType} {field.FieldName};\n";
        }
        entityTemplateString = entityTemplateString.Replace("#ASSETENITYFIELDS#", fields);
        entityTemplateString += "\n";
        return entityTemplateString;
    }

    private static string BuildScriptFields(string excelName, List<SheetStruct> sheetStructs)
    {
        var scriptFieldContent = "";
        // 工作表为1个时，不区分工作表名称
        if (sheetStructs.Count == 1)
        {
            var enityClassName = excelName + "Entity";
            scriptFieldContent = $"public List<{enityClassName}> {sheetStructs[0].SheetName};";
        }
        else
        {
            foreach (var sheetStruct in sheetStructs)
            {
                var enityClassName = excelName + "Entity" + "_" + sheetStruct.SheetName;
                scriptFieldContent += $"public List<{enityClassName}> {sheetStruct.SheetName};\n";
            }
        }
        return scriptFieldContent;
    }

    private static string BuildScriptContent(string excelName, List<SheetStruct> sheetStructs)
    {
        var templateString = GetScriptTemplate();
        var entityTemplateStringMatch = Regex.Match(templateString,
            "#BEGINASSETENITYNAMEDEFINE#(.*?)#ENDASSETENITYNAMEDEFINE#", RegexOptions.Singleline);
        if (!entityTemplateStringMatch.Success) throw new Exception("Script template format error.");
        var entityTemplateString = entityTemplateStringMatch.Groups[1].Value.Trim();
        // 工作表为1个时，不区分工作表名称
        if (sheetStructs.Count == 1)
        {
            entityTemplateString = BuildScriptEnityContent(entityTemplateString, excelName, "",
                sheetStructs[0].Fields);
        }
        else
        {
            var entityStrings = "";
            foreach (var sheetStruct in sheetStructs)
            {
                entityStrings += BuildScriptEnityContent(entityTemplateString, excelName, sheetStruct.SheetName,
                    sheetStruct.Fields);
            }
            entityTemplateString = entityStrings;
        }
        var scriptFields = BuildScriptFields(excelName, sheetStructs);
        var result = templateString.Replace(entityTemplateStringMatch.Value, entityTemplateString);
        result = result.Replace("#ASSETSCRIPTNAME#", excelName);
        result = result.Replace("#ASSETSCRIPTFIELDS#", scriptFields);
        return result;
    }
}
