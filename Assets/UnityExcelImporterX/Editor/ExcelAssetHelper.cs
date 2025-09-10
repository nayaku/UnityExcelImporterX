using NPOI.SS.UserModel;
using System.Collections.Generic;

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
        IRow headerRow = sheet.GetRow(0);
        IRow typeRow = sheet.GetRow(1);
        IRow commentRow = sheet.GetRow(2);
        if (headerRow == null || typeRow == null)
        {
            return null;
        }

        List<SheetField> sheetFields = new();
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
                continue;
            }
            // 空白列视为结束
            if (nameCell.CellType == CellType.Blank || typeCell.CellType == CellType.Blank)
            {
                break;
            }

            SheetField field = new()
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

