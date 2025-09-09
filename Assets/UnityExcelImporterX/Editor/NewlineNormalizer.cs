using System;
using System.IO;
using System.Text.RegularExpressions;

/// <summary>
/// ���ڶ�д�ļ�ʱ�Զ������з�ͳһ����Ϊ\n�Ĺ�����
/// </summary>
public static class NewlineNormalizer
{
    /// <summary>
    /// ��ȡ�ļ���������ƽ̨�Ļ��з�ͳһת��Ϊ\n
    /// </summary>
    /// <param name="filePath">�ļ�·��</param>
    /// <returns>ͳһ���з�Ϊ\n���ļ�����</returns>
    public static string Read(string filePath)
    {
        // ��ȡԭʼ�ļ�����
        string originalContent = File.ReadAllText(filePath);

        // �����п��ܵĻ��з�����(\r\n, \r, \n)ͳһת��Ϊ\n
        string normalizedContent = originalContent.Replace("\r\n", "\n").Replace("\r", "\n");

        return normalizedContent;
    }

    /// <summary>
    /// ������д���ļ������ֻ��з�Ϊ\n�����Զ�ת��Ϊƽ̨�ض���ʽ��
    /// </summary>
    /// <param name="filePath">�ļ�·��</param>
    /// <param name="content">����\n���з�������</param>
    public static void Write(string filePath, string content)
    {
        // �������е�\n�滻Ϊ��ǰƽ̨���з�
        string platformContent = content.Replace("\n", Environment.NewLine);
        // д���ļ���ʹ��Ĭ�ϱ���
        File.WriteAllText(filePath, platformContent);
    }
}
