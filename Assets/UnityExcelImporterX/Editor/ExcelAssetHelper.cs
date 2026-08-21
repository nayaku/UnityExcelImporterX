using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;

public class SheetField
{
    public string FieldName;
    public string FieldType;
    public string FieldComment;
    public bool IsKey;
}
public class ExcelStruct
{
    public string ExcelName;
    public bool HasKeyField; // 是否有主键字段
    public List<SheetStruct> Sheets;
}

public class SheetStruct
{
    public string SheetName;
    public bool HasKeyField; // 是否有主键字段
    public List<SheetField> Fields;
}

public static class ExcelAssetHelper
{
    /// <summary>
    /// 从Excel表头获取字段信息
    /// </summary>
    /// <param name="sheet">Excel工作表</param>
    /// <returns>字段信息列表</returns>
    public static (List<SheetField>, bool) GetFieldFromSheetHeader(ISheet sheet)
    {
        IRow headerRow = sheet.GetRow(0);
        IRow typeRow = sheet.GetRow(1);
        IRow commentRow = sheet.GetRow(2);
        if (headerRow == null || typeRow == null)
        {
            return (null, false);
        }

        List<SheetField> sheetFields = new();
        HashSet<string> fieldSet = new();
        bool hasKeyField = false;
        for (int j = 0; j < headerRow.LastCellNum; j++)
        {
            ICell nameCell = headerRow.GetCell(j);
            ICell typeCell = typeRow.GetCell(j);
            ICell commentCell = commentRow?.GetCell(j);

            if (nameCell == null || typeCell == null)
            {
                break;
            }
            // 注释列跳过
            if ((nameCell.CellType == CellType.String && nameCell.StringCellValue.StartsWith("#")) ||
                (typeCell.CellType == CellType.String && typeCell.StringCellValue.StartsWith("#")))
            {
                sheetFields.Add(null);
                continue;
            }
            // 空白列视为结束
            if (nameCell.CellType == CellType.Blank || typeCell.CellType == CellType.Blank)
            {
                break;
            }
            // 检查字段名和字段类型是否为字符串类型
            if (nameCell.CellType != CellType.String || typeCell.CellType != CellType.String)
            {
                throw new Exception($"Invalid cell type in sheet '{sheet.SheetName}' at column " +
                    $"{CellReference.ConvertNumToColString(j)}({j + 1}). " +
                    $"Expected string type for field name and field type.");
            }
            string nameValue = nameCell.StringCellValue.Trim();
            string fieldType = typeCell.StringCellValue.Trim();
            // 空白字符列视为结束
            if (nameValue.Length == 0 || fieldType.Length == 0)
            {
                break;
            }
            string[] nameItem = nameCell.StringCellValue.Split(',', StringSplitOptions.RemoveEmptyEntries);
            string fieldName = nameItem[0].Trim();
            // 检查字段名是否重复
            if (!fieldSet.Add(fieldName))
            {
                throw new Exception($"Duplicate field name '{fieldName}' in sheet '{sheet.SheetName}' " +
                    $"at column {CellReference.ConvertNumToColString(j)}({j + 1}).");
            }
            // 检查是否为主键字段
            bool isKey = nameItem.Length > 1 && nameItem[1].Trim().ToLower() == "key";
            if (isKey)
            {
                hasKeyField = true;
            }

            string fieldComment = commentCell?.ToString()?.Trim() ?? "";

            SheetField field = new()
            {
                FieldName = fieldName,
                FieldType = fieldType,
                FieldComment = fieldComment,
                IsKey = isKey
            };
            sheetFields.Add(field);
        }
        return (sheetFields, hasKeyField);
    }

    public static ExcelStruct GetExcelStruct(string excelPath)
    {
        List<SheetStruct> sheetStructs = new();
        using FileStream stream = File.Open(excelPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        IWorkbook book = Path.GetExtension(excelPath).Equals(".xls", StringComparison.OrdinalIgnoreCase) ?
            new HSSFWorkbook(stream) : new XSSFWorkbook(stream);
        bool anyHasKeyField = false;
        HashSet<string> sheetNameSet = new();
        for (int i = 0; i < book.NumberOfSheets; i++)
        {
            ISheet sheet = book.GetSheetAt(i);
            string sheetName = sheet.SheetName.Trim();
            if (sheetName.Length == 0 || sheetName.StartsWith("#", StringComparison.Ordinal))
            {
                continue;
            }
            (List<SheetField> sheetfields, bool hasKeyField) = GetFieldFromSheetHeader(sheet);
            if (sheetfields == null || sheetfields.Count(f => f != null) == 0)
            {
                continue;
            }
            // 如果有主键字段，则标记ExcelStruct.HasKeyField为true
            if (hasKeyField)
            {
                anyHasKeyField = true;
            }
            // 检查表名是否重复
            if (!sheetNameSet.Add(sheetName))
            {
                throw new Exception($"Duplicate sheet name '{sheetName}'.");
            }

            SheetStruct sheetStruct = new()
            {
                SheetName = sheetName,
                Fields = sheetfields,
                HasKeyField = hasKeyField
            };
            sheetStructs.Add(sheetStruct);
        }
        ExcelStruct excelStruct = new()
        {
            ExcelName = Path.GetFileNameWithoutExtension(excelPath),
            Sheets = sheetStructs,
            HasKeyField = anyHasKeyField
        };
        return excelStruct;
    }
}

