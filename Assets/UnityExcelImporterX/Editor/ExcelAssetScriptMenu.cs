using Microsoft.CSharp;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using static TextTemplate;

public class ExcelAssetScriptMenu
{
    private const string SCRIPT_TEMPLATE_NAME = "ExcelAssetScriptTemplete.cs.txt";
    private static readonly CSharpCodeProvider _provider = new();

    [MenuItem("Assets/Create/ExcelAssetScript", false)]
    public static void CreateScript()
    {
        // 选中文件
        UnityEngine.Object[] selectedAssets =
            Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.Assets);
        UnityEngine.Object selectedAsset = selectedAssets[0];
        string assetPath = AssetDatabase.GetAssetPath(selectedAsset);
        string assetName = Path.GetFileName(assetPath);
        string assetDirectory = Path.GetDirectoryName(assetPath);
        List<string> savePathList = new(selectedAssets.Length);
        if (selectedAssets.Length == 1)
        {
            // 选择保存路径
            string newScriptName = Path.ChangeExtension(assetName, "cs");
            string savePath =
                EditorUtility.SaveFilePanel("Save ExcelAssetScript", assetDirectory, newScriptName, "cs");
            if (string.IsNullOrEmpty(savePath))
            {
                return;
            }
            savePathList.Add(savePath);
        }
        else
        {
            string saveDirectory =
                EditorUtility.OpenFolderPanel("Save ExcelAssetScripts", assetDirectory, "");
            if (string.IsNullOrEmpty(saveDirectory))
            {
                return;
            }
            foreach (UnityEngine.Object obj in selectedAssets)
            {
                // 选择保存文件夹
                string path = AssetDatabase.GetAssetPath(obj);
                string name = Path.GetFileNameWithoutExtension(path);
                string savePath = Path.Combine(saveDirectory, name + ".cs");
                savePathList.Add(savePath);
            }
        }

        TextTemplate template = GetTextTemplate();
        foreach (string savePath in savePathList)
        {
            CreateScript(template, assetPath, savePath);
        }

        // 刷新资源
        AssetDatabase.Refresh();
    }

    [MenuItem("Assets/Create/ExcelAssetScript", true)]
    public static bool CreateScriptValidation()
    {
        UnityEngine.Object[] selectedAssets =
            Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.Assets);
        if (selectedAssets.Length == 0)
        {
            return false;
        }
        foreach (UnityEngine.Object obj in selectedAssets)
        {
            if (obj == null)
            {
                return false;
            }
            string path = AssetDatabase.GetAssetPath(selectedAssets[0]);
            if (string.IsNullOrEmpty(path))
            {
                return false;
            }
            if (!Path.GetExtension(path).Equals(".xls", StringComparison.OrdinalIgnoreCase) &&
                !Path.GetExtension(path).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }
        }
        return true;
    }

    private static void CreateScript(TextTemplate template, string assetPath, string savePath)
    {
        try
        {
            CreateScriptWithoutException(template, assetPath, savePath);
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error generating script for '{assetPath}': {ex.Message}");
        }
    }

    private static void CreateScriptWithoutException(TextTemplate template, string assetPath, string savePath)
    {
        // 读取Excel文件
        ExcelStruct excelStruct = ExcelAssetHelper.GetExcelStruct(assetPath);
        if (excelStruct.Sheets.Count == 0)
        {
            return;
        }
        // 验证Excel文件名、Sheet名和字段名是否合法
        if (!ValidateExcelScriptFieldName(excelStruct, out string errorMessage))
        {
            errorMessage += "Name must start with a letter or underscore and contain only letters, digits, and underscores.";
            throw new Exception(errorMessage);
        }

        DictParams dictParams = ExcelAssetScriptParams.PrepareTemplateParams(excelStruct);
        string scriptContent = template.Build(dictParams);
        NewlineNormalizer.Write(savePath, scriptContent);
    }

    private static TextTemplate GetTextTemplate()
    {
        string currentDirectory = Directory.GetCurrentDirectory();
        string[] filePath = Directory.GetFiles(currentDirectory, SCRIPT_TEMPLATE_NAME, SearchOption.AllDirectories);
        if (filePath.Length == 0)
        {
            throw new Exception("Script template not found.");
        }
        string scriptTemplatePath = filePath[0];
        string templateContent = NewlineNormalizer.Read(scriptTemplatePath);
        return new TextTemplate(templateContent);
    }

    private static bool ValidateExcelScriptFieldName(ExcelStruct excelStruct, out string errorMsg)
    {
        errorMsg = string.Empty;
        if (!_provider.IsValidIdentifier(excelStruct.ExcelName))
        {
            errorMsg = $"Invalid C# identifier excel name '{excelStruct.ExcelName}'.";
            return false;
        }
        foreach (SheetStruct sheet in excelStruct.Sheets)
        {
            if (!_provider.IsValidIdentifier(sheet.SheetName))
            {
                errorMsg = $"Invalid C# identifier sheet name '{sheet.SheetName}' in excel '{excelStruct.ExcelName}'.";
                return false;
            }
            foreach (SheetField field in sheet.Fields)
            {
                if (field == null)
                    continue;
                if (!_provider.IsValidIdentifier(field.FieldName))
                {
                    errorMsg = $"Invalid C# identifier field name '{field.FieldName}' in sheet '{sheet.SheetName}' of excel '{excelStruct.ExcelName}'.";
                    return false;
                }
            }
        }
        return true;
    }
}
