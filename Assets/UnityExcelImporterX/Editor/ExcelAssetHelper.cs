using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class SheetField
{
    public string FieldName;
    public string FieldType;
    public string FieldComment;
}

public static class ExcelAssetHelper
{
    public static List<SheetField> GetFieldFromSheetHeader(ISheet sheet)
    {
        var headerRow = sheet.GetRow(0);
        var typeRow = sheet.GetRow(1);
        var commentRow = sheet.GetRow(2);
        if (headerRow == null || typeRow == null)
            return null;
        var sheetFields = new List<SheetField>();
        for (var j = 0; j < headerRow.LastCellNum; j++)
        {
            var nameCell = headerRow.GetCell(j);
            var typeCell = typeRow.GetCell(j);
            var commentCell = commentRow?.GetCell(j);

            if (nameCell == null || typeCell == null) break;
            // 注释列跳过
            if ((nameCell.CellType == CellType.String && nameCell.StringCellValue.StartsWith("#")) ||
                (typeCell.CellType == CellType.String && typeCell.StringCellValue.StartsWith("#"))) continue;
            // 空白列视为结束
            if (nameCell.CellType == CellType.Blank || typeCell.CellType == CellType.Blank) break;
            var field = new SheetField
            {
                FieldName = nameCell.StringCellValue,
                FieldType = typeCell.StringCellValue,
                FieldComment = commentCell?.StringCellValue ?? ""
            };
            sheetFields.Add(field);
        }
        return sheetFields;
    }
}

