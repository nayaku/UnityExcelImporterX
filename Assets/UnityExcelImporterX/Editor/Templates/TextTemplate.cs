using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class TextTemplate
{
    private const string BeginTag = "BEGIN";
    private const string EndTag = "END";

    private string _template;
    private VBlock _block;

    private abstract class VNode
    {
        public string Name;
        public Range Range;
    }
    private class VVar : VNode
    {

    }
    private class VBlock : VNode
    {
        public Range EndRange;
        public List<VNode> Children = new();
    }

    public interface ITemplateParams
    {

    }
    public class DictParams : ITemplateParams
    {
        public Dictionary<string, ITemplateParams> Params { get; }

        public DictParams(Dictionary<string, ITemplateParams> @params)
        {
            Params = @params;
        }
    }
    public class StringParams : ITemplateParams
    {
        public string Param { get; }

        public StringParams(string param)
        {
            Param = param;
        }
    }
    public class ListParams : ITemplateParams
    {
        public List<DictParams> Params { get; }

        public ListParams(List<DictParams> @params)
        {
            Params = @params;
        }
    }

    /// <summary>
    /// 加载模板文件
    /// </summary>
    /// <param name="filePath"></param>
    public void Load(string filePath)
    {
        _template = NewlineNormalizer.Read(filePath);
        Parse();
    }

    private void Parse()
    {
        VBlock block = new()
        {
            Range = new Range(0, 0),
        };
        int startIndex = -1;
        Stack<VBlock> blockStack = new();
        blockStack.Push(block);
        int lineIndex = 0;
        int colIndex = 0;
        for (int index = 0; index < _template.Length; index++)
        {
            // 计算行列号
            if (_template[index] == '\n')
            {
                lineIndex++;
                colIndex = 0;
            }
            else
            {
                colIndex++;
            }

            if (_template[index] != '#')
            {
                continue;
            }
            // 如果当前字符是#，并且startIndex为-1，说明遇到了一个新的标签的开始
            if (startIndex == -1)
            {
                startIndex = index;
                continue;
            }

            // 如果当前字符是#，并且startIndex不为-1，说明遇到了一个标签的结束
            VBlock curBlock = blockStack.Peek();
            ReadOnlySpan<char> tagBuff = _template.AsSpan(startIndex + 1, index - startIndex - 1); // 获取标签内容，去掉前后的#号
            // 如果标签内容是以BEGIN开头的，说明是一个块的开始
            if (tagBuff.StartsWith(BeginTag, StringComparison.Ordinal))
            {
                string name = tagBuff[BeginTag.Length..].ToString(); // 获取块的名称，去掉BEGIN
                VBlock newBlock = new()
                {
                    Name = name,
                };
                // Block的Range额外包含了换行符
                if (index + 1 < _template.Length && _template[index + 1] == '\n')
                {
                    newBlock.Range = new Range(startIndex, index + 2);
                }
                else
                {
                    newBlock.Range = new Range(startIndex, index + 1);
                }
                curBlock.Children.Add(newBlock);
                blockStack.Push(newBlock);
            }
            else if (tagBuff.StartsWith(EndTag, StringComparison.Ordinal))
            {
                string name = tagBuff[EndTag.Length..].ToString(); // 获取块的名称，去掉BEGIN
                if (name != curBlock.Name)
                {
                    throw new Exception($"Mismatched block end tag: {name} in line {lineIndex + 1}, column {colIndex + 1}, expected: {curBlock.Name}");
                }
                // Block的Range额外包含了换行符
                if (index + 1 < _template.Length &&
                    _template[index + 1] == '\n' &&
                    _template[curBlock.Range.End.Value - 1] == '\n')
                {
                    curBlock.EndRange = new Range(startIndex, index + 2);
                }
                else
                {
                    curBlock.EndRange = new Range(startIndex, index + 1);
                }
                blockStack.Pop();
            }
            // 为标签内容
            else
            {
                VVar newVar = new()
                {
                    Name = tagBuff.ToString(),
                    Range = new Range(startIndex, index + 1)
                };
                curBlock.Children.Add(newVar);
            }
            startIndex = -1;
        }
        // 标签没有闭合
        if (startIndex != -1)
        {
            Debug.LogError($"Unmatched # in template at line {lineIndex + 1}, column {colIndex + 1}, expected closing #, ignoring...");
        }
        // 如果栈中还有元素，说明有块没有闭合
        if (blockStack.Count > 1)
        {
            Debug.LogError($"Unmatched block in template at line {lineIndex + 1}, column {colIndex + 1}, expected closing block, ignoring...");
        }
        // 默认EOF是万能的结束符，所有未闭合的块都以EOF结束
        while (blockStack.Count > 0)
        {
            VBlock unclosedBlock = blockStack.Pop();
            unclosedBlock.EndRange = new Range(_template.Length, _template.Length);
        }
        _block = block; 
    }

    public string Build(DictParams templateParams)
    {
        StringBuilder sb = new(_template.Length * 4);
        VBlock block = _block;
        if (_block == null)
        {
            throw new Exception("Template not loaded");
        }

        List<DictParams> paramStack = new() { templateParams };
        List<VBlock> blockStack = new() { block };
        BuildBlock(ref sb, ref paramStack, ref blockStack);
        string result = sb.ToString();
        return result;
    }

    private static bool TryGetParam(List<DictParams> paramStack, string name, out ITemplateParams param)
    {
        for (int i = paramStack.Count - 1; i >= 0; i--)
        {
            if (paramStack[i].Params.TryGetValue(name, out param))
            {
                return true;
            }
        }
        param = null;
        return false;
    }

    private static string AppendInBlockString(List<VBlock> blockStack, string errorMsg)
    {
        List<string> blockNames = blockStack.Skip(1).Select(b => b.Name).ToList();
        if (blockNames.Count > 0)
        {
            errorMsg += $" in block: {string.Join(" < ", blockNames)}";
        }
        // 默认为空字符串
        errorMsg += ", replace with empty string";
        return errorMsg;
    }

    private void BuildBlock(ref StringBuilder sb, ref List<DictParams> paramStack, ref List<VBlock> blockStack)
    {
        VBlock block = blockStack.Last();
        Range startRange = block.Range;
        foreach (var child in block.Children)
        {
            Range curRange = child.Range;
            sb.Append(_template[startRange.End..curRange.Start]);
            // 更新起始范围
            if (child is VBlock curBlock)
            {
                startRange = curBlock.EndRange;
            }
            else
            {
                startRange = curRange;
            }

            // 找不到
            if (!TryGetParam(paramStack, child.Name, out ITemplateParams param))
            {
                //string errorMsg = $"Missing parameter: {child.Name}";
                //errorMsg = AppendInBlockString(blockStack, errorMsg);
                //Debug.LogWarning(errorMsg);
                continue;
            }
            if (param == null)
            {
                // 如果参数为null，直接跳过
                continue;
            }

            // 处理块
            if (child is VBlock vBlock)
            {
                if (param is StringParams)
                {
                    string errorMsg = $"Parameter type mismatch for block: {child.Name}, expected DictParams or ListParams, got {param.GetType().Name}";
                    errorMsg = AppendInBlockString(blockStack, errorMsg);
                    Debug.LogWarning(errorMsg);
                    continue;
                }
                else
                {
                    blockStack.Add(vBlock);
                    List<DictParams> dictParams;
                    if (param is DictParams paramDict)
                    {
                        dictParams = new() { paramDict };
                    }
                    else
                    {
                        dictParams = ((ListParams)param).Params;
                    }
                    foreach (DictParams item in dictParams)
                    {
                        paramStack.Add(item);
                        BuildBlock(ref sb, ref paramStack, ref blockStack);
                        paramStack.RemoveAt(paramStack.Count - 1);
                    }
                    blockStack.RemoveAt(blockStack.Count - 1);
                }
            }
            // 处理变量
            else if (child is VVar)
            {
                if (param is StringParams paramString)
                {
                    sb.Append(paramString.Param);
                }
                else
                {
                    string errorMsg = $"Parameter type mismatch for variable: {child.Name}, expected StringParams, got {param.GetType().Name}";
                    errorMsg = AppendInBlockString(blockStack, errorMsg);
                    Debug.LogWarning(errorMsg);
                }
            }
        }
        // 添加最后一段文本
        sb.Append(_template[startRange.End..block.EndRange.Start]);
    }
}
