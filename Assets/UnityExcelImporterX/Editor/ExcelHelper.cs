public class ExcelHelper
{
    /// <summary>
    /// 将列索引转换为Excel列名（例如：1 -> A, 2 -> B, 27 -> AA）
    /// </summary>
    /// <param name="columnIndex"></param>
    /// <returns></returns>
    public static string ColIndexToName(int columnIndex)
    {
        string columnName = "";
        while (columnIndex > 0)
        {
            int remainder = (columnIndex - 1) % 26;
            columnName = (char)(remainder + 'A') + columnName;
            columnIndex = (columnIndex - 1) / 26;
        }
        return columnName;
    }

    public static int ColNameToIndex(string columnName)
    {
        int columnIndex = 0;
        columnName = columnName.ToUpper();
        for (int i = 0; i < columnName.Length; i++)
        {
            columnIndex *= 26;
            columnIndex += (columnName[i] - 'A' + 1);
        }
        return columnIndex;
    }
}

