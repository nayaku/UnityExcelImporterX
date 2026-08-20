using System;
using System.Collections.Generic;
using System.Linq;
using static TextTemplate;


public class ExcelAssetScriptParams
{
    private static ListParams PrepareAssetEntityFiledCommentMutiLineParams(string[] commentLines)
    {
        List<DictParams> commentLinesParamsList = new();
        foreach (string line in commentLines)
        {
            Dictionary<string, ITemplateParams> commentParams = new()
            {
                ["ASSETENTITYFIELDCOMMENT"] = new StringParams(line)
            };
            commentLinesParamsList.Add(new DictParams(commentParams));
        }
        return new ListParams(commentLinesParamsList);
    }

    private static DictParams PrepareAssetEntityFieldCommentParams(string fieldComment)
    {
        Dictionary<string, ITemplateParams> commentParams = new();
        string[] fieldCommentLines = fieldComment.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);
        commentParams["ASSETENTITYFIELDCOMMENTMUTILINE"] = PrepareAssetEntityFiledCommentMutiLineParams(fieldCommentLines);
        return new DictParams(commentParams);
    }

    private static ListParams PrepareAssetEntityFieldsParams(List<SheetField> fields)
    {
        List<DictParams> fieldParamsList = new();
        foreach (SheetField field in fields)
        {
            if (field == null)
            {
                continue;
            }
            Dictionary<string, ITemplateParams> fieldParams = new();
            if (!string.IsNullOrEmpty(field.FieldComment))
            {
                fieldParams["ASSETENTITYFIELDCOMMENT"] = PrepareAssetEntityFieldCommentParams(field.FieldComment);
            }
            fieldParams["ASSETFIELDTYPE"] = new StringParams(field.FieldType);
            fieldParams["ASSETFIELDNAME"] = new StringParams(field.FieldName);
            fieldParamsList.Add(new DictParams(fieldParams));
        }
        return new ListParams(fieldParamsList);
    }

    private static ListParams PrepareAssetEntityParams(string excelName, List<SheetStruct> sheetStructs)
    {
        List<DictParams> entityParamsList = new();
        foreach (SheetStruct sheet in sheetStructs)
        {
            Dictionary<string, ITemplateParams> entityParams = new();
            string enityClassName = excelName + "Entity";
            // 工作表不为1个时，需要区分工作表名称
            if (sheetStructs.Count > 1)
            {
                enityClassName += "_" + sheet.SheetName;
            }
            entityParams["ASSETENTITYNAME"] = new StringParams(enityClassName);
            entityParams["ASSETENTITYFIELDS"] = PrepareAssetEntityFieldsParams(sheet.Fields);
            entityParamsList.Add(new DictParams(entityParams));
        }
        return new ListParams(entityParamsList);
    }

    private static ListParams PrepareAssetKeyFieldsParams(SheetField[] keyFields)
    {
        List<DictParams> keyFieldParamsList = new();
        for (int i = 0; i < keyFields.Length; i++)
        {
            SheetField keyField = keyFields[i];
            Dictionary<string, ITemplateParams> keyFieldParams = new()
            {
                ["ASSETKEYFIELDTYPE"] = new StringParams(keyField.FieldType),
                ["ASSETKEYFIELDNAME"] = new StringParams(keyField.FieldName)
            };
            if (i != keyFields.Length - 1)
            {
                keyFieldParams["ASSETKEYFIELDSEPARATOR"] = new StringParams(", ");
            }
            keyFieldParamsList.Add(new DictParams(keyFieldParams));
        }
        return new ListParams(keyFieldParamsList);
    }

    private static ListParams PrepareAssetFieldsParams(string excelName, List<SheetStruct> sheets)
    {
        List<DictParams> fieldParamsList = new();
        foreach (SheetStruct sheet in sheets)
        {
            Dictionary<string, ITemplateParams> fieldParams = new();
            string enityClassName = excelName + "Entity";
            // 工作表不为1个时，需要区分工作表名称
            if (sheets.Count > 1)
            {
                enityClassName += "_" + sheet.SheetName;
            }
            fieldParams["ASSETENTITYNAME"] = new StringParams(enityClassName);
            fieldParams["ASSETENTITYLISTNAME"] = new StringParams(sheet.SheetName);
            fieldParamsList.Add(new DictParams(fieldParams));
            // 如果有主键，则生成字典字段
            if (sheet.HasKeyField)
            {
                SheetField[] keyFields = sheet.Fields.Where(f => f != null && f.IsKey).ToArray();
                fieldParams["ASSETENTITYDICTNAME"] = new StringParams(sheet.SheetName + "Dict");
                fieldParams["ASSETKEYFIELDNAME"] = new StringParams(keyFields[0].FieldName);
                if (keyFields.Length == 1)
                {
                    Dictionary<string, ITemplateParams> keyFieldParams = new()
                    {
                        ["ASSETKEYFIELDTYPE"] = new StringParams(keyFields[0].FieldType),
                    };
                    fieldParams["ASSETSINGLEKEYFIELDS"] = new DictParams(keyFieldParams);
                }
                else
                {
                    Dictionary<string, ITemplateParams> keyFieldsParams = new()
                    {
                        ["ASSETKEYFIELDS"] = PrepareAssetKeyFieldsParams(keyFields)
                    };
                    fieldParams["ASSETMUTIKEYFIELDS"] = new DictParams(keyFieldsParams);
                }
            }
        }
        return new ListParams(fieldParamsList);
    }

    private static ListParams PrepareAssetKeyFieldNamesParams(SheetField[] keyFields)
    {
        List<DictParams> keyFieldNamesParamsList = new();
        for (int i = 0; i < keyFields.Length; i++)
        {
            SheetField keyField = keyFields[i];
            Dictionary<string, ITemplateParams> keyFieldNameParams = new()
            {
                ["ASSETKEYFIELDNAME"] = new StringParams(keyField.FieldName)
            };
            if (i != keyFields.Length - 1)
            {
                keyFieldNameParams["ASSETKEYFIELDSEPARATOR"] = new StringParams(",");
            }
            keyFieldNamesParamsList.Add(new DictParams(keyFieldNameParams));
        }
        return new ListParams(keyFieldNamesParamsList);
    }

    private static ListParams PrepareOnAfterDeserializeParams(string excelName, List<SheetStruct> sheetStructs)
    {
        List<DictParams> onAfterDeserializeParamsList = new();
        foreach (SheetStruct sheet in sheetStructs)
        {
            if (!sheet.HasKeyField)
                continue;

            Dictionary<string, ITemplateParams> onAfterDeserializeParams = new();
            string enityClassName = excelName + "Entity";
            // 工作表不为1个时，需要区分工作表名称
            if (sheetStructs.Count > 1)
            {
                enityClassName += "_" + sheet.SheetName;
            }
            onAfterDeserializeParams["ASSETENTITYDICTNAME"] = new StringParams(sheet.SheetName + "Dict");
            onAfterDeserializeParams["ASSETENTITYNAME"] = new StringParams(enityClassName);
            onAfterDeserializeParams["ASSETENTITYLISTNAME"] = new StringParams(sheet.SheetName);
            SheetField[] keyFields = sheet.Fields.Where(f => f != null && f.IsKey).ToArray();
            if (keyFields.Length == 1)
            {
                Dictionary<string, ITemplateParams> singleKeyFieldParams = new()
                {
                    ["ASSETKEYFIELDNAME"] = new StringParams(keyFields[0].FieldName)
                };
                onAfterDeserializeParams["ASSETSINGLEKEYFIELDS"] = new DictParams(singleKeyFieldParams);
            }
            else
            {
                Dictionary<string, ITemplateParams> mutiKeyFieldsParams = new()
                {
                    ["ASSETKEYFIELDNAMES"] = PrepareAssetKeyFieldNamesParams(keyFields)
                };
                onAfterDeserializeParams["ASSETMUTIKEYFIELDS"] = new DictParams(mutiKeyFieldsParams);
            }
            onAfterDeserializeParamsList.Add(new DictParams(onAfterDeserializeParams));
        }
        return new ListParams(onAfterDeserializeParamsList);
    }

    private static DictParams PrepareAssetMethodsParams(string excelName, List<SheetStruct> sheetStructs)
    {
        Dictionary<string, ITemplateParams> assetMethodsParams = new()
        {
            ["ONAFTERDESERIALIZE"] = PrepareOnAfterDeserializeParams(excelName, sheetStructs)
        };
        return new DictParams(assetMethodsParams);
    }

    private static DictParams PrepareAssetParams(ExcelStruct excelStruct)
    {
        Dictionary<string, ITemplateParams> assetParams = new()
        {
            ["ASSETNAME"] = new StringParams(excelStruct.ExcelName),
            ["ASSETFIELDS"] = PrepareAssetFieldsParams(excelStruct.ExcelName, excelStruct.Sheets)
        };
        if (excelStruct.HasKeyField)
        {
            assetParams["ASSETWITHKEY"] = new DictParams(new Dictionary<string, ITemplateParams>());
            assetParams["ASSETMETHODS"] = PrepareAssetMethodsParams(excelStruct.ExcelName, excelStruct.Sheets);
        }
        return new DictParams(assetParams);
    }

    public static DictParams PrepareTemplateParams(ExcelStruct excelStruct)
    {
        Dictionary<string, ITemplateParams> templateParams = new()
        {
            ["ASSETENTITY"] = PrepareAssetEntityParams(excelStruct.ExcelName, excelStruct.Sheets),
            ["ASSET"] = PrepareAssetParams(excelStruct)
        };
        return new DictParams(templateParams);
    }
}
