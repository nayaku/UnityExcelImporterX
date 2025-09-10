using System;
using System.IO;

/// <summary>
/// 用于读写文件时自动将换行符统一处理为\n的工具类
/// </summary>
public static class NewlineNormalizer
{
    /// <summary>
    /// 读取文件并将所有平台的换行符统一转换为\n
    /// </summary>
    /// <param name="filePath">文件路径</param>
    /// <returns>统一换行符为\n的文件内容</returns>
    public static string Read(string filePath)
    {
        // 读取原始文件内容
        string originalContent = File.ReadAllText(filePath);

        // 将所有可能的换行符序列(\r\n, \r, \n)统一转换为\n
        string normalizedContent = originalContent.Replace("\r\n", "\n").Replace("\r", "\n");

        return normalizedContent;
    }

    /// <summary>
    /// 将内容写入文件，保持换行符为\n（不自动转换为平台特定格式）
    /// </summary>
    /// <param name="filePath">文件路径</param>
    /// <param name="content">包含\n换行符的内容</param>
    public static void Write(string filePath, string content)
    {
        // 将内容中的\n替换为当前平台换行符
        string platformContent = content.Replace("\n", Environment.NewLine);
        // 写入文件，使用默认编码
        File.WriteAllText(filePath, platformContent);
    }
}
