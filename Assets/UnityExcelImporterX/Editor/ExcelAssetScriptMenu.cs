using System;
using System.IO;
using UnityEditor;
using static TextTemplate;

public class ExcelAssetScriptMenu
{
    private const string ScriptTemplateName = "ExcelAssetScriptTemplete.cs.txt";

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

            // 生成脚本
            CreateScript(assetPath, savePath);
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

                // 生成脚本
                CreateScript(path, savePath);
            }
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

    private static void CreateScript(string assetPath, string savePath)
    {
        // 读取Excel文件
        ExcelStruct excelStruct = ExcelAssetHelper.GetExcelStruct(assetPath);
        if (excelStruct.Sheets.Count == 0)
        {
            return;
        }

        string templateFilePath = GetScriptTemplatePath();
        DictParams dictParams = ExcelAssetScriptParams.PrepareTemplateParams(excelStruct);
        TextTemplate template = new();
        template.Load(templateFilePath);
        string scriptContent = template.Build(dictParams);
        NewlineNormalizer.Write(savePath, scriptContent);
    }

    private static string GetScriptTemplatePath()
    {
        string currentDirectory = Directory.GetCurrentDirectory();
        string[] filePath = Directory.GetFiles(currentDirectory, ScriptTemplateName, SearchOption.AllDirectories);
        if (filePath.Length == 0)
        {
            throw new Exception("Script template not found.");
        }
        return filePath[0];
    }
}
