using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TestDataEntity_NoKey
{
    /// <summary>
    /// 编号
    /// </summary>
    public int id;
    /// <summary>
    /// 名称：包含中文与 Unicode
    /// </summary>
    public string name;
    /// <summary>
    /// 是否启用
    /// </summary>
    public bool enabled;
    /// <summary>
    /// 分数
    /// </summary>
    public double score;
    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime createdAt;
    /// <summary>
    /// JSON 整数列表
    /// </summary>
    public List<int> tags;
    /// <summary>
    /// Unity 三维向量
    /// </summary>
    public Vector3 position;
    /// <summary>
    /// JSON 字典
    /// </summary>
    public Dictionary<string, int> metadata;
}

[Serializable]
public class TestDataEntity_SingleKey
{
    /// <summary>
    /// 编号
    /// </summary>
    public int id;
    /// <summary>
    /// 名称：包含中文与 Unicode
    /// </summary>
    public string name;
    /// <summary>
    /// 是否启用
    /// </summary>
    public bool enabled;
    /// <summary>
    /// 分数
    /// </summary>
    public double score;
    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime createdAt;
    /// <summary>
    /// JSON 整数列表
    /// </summary>
    public List<int> tags;
    /// <summary>
    /// Unity 三维向量
    /// </summary>
    public Vector3 position;
    /// <summary>
    /// JSON 字典
    /// </summary>
    public Dictionary<string, int> metadata;
}

[Serializable]
public class TestDataEntity_MultiKey
{
    /// <summary>
    /// 编号
    /// </summary>
    public int id;
    /// <summary>
    /// 名称：包含中文与 Unicode
    /// </summary>
    public string name;
    /// <summary>
    /// 是否启用
    /// </summary>
    public bool enabled;
    /// <summary>
    /// 分数
    /// </summary>
    public double score;
    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime createdAt;
    /// <summary>
    /// JSON 整数列表
    /// </summary>
    public List<int> tags;
    /// <summary>
    /// Unity 三维向量
    /// </summary>
    public Vector3 position;
    /// <summary>
    /// JSON 字典
    /// </summary>
    public Dictionary<string, int> metadata;
}

[Serializable]
public class TestDataEntity_WideData
{
    /// <summary>
    /// 宽表字段 1
    /// 第二行备注：<>&"'
    /// </summary>
    public string field001;
    /// <summary>
    /// 宽表字段 2
    /// 第二行备注：<>&"'
    /// </summary>
    public bool field002;
    /// <summary>
    /// 宽表字段 3
    /// 第二行备注：<>&"'
    /// </summary>
    public double field003;
    /// <summary>
    /// 宽表字段 4
    /// 第二行备注：<>&"'
    /// </summary>
    public List<int> field004;
    /// <summary>
    /// 宽表字段 5
    /// 第二行备注：<>&"'
    /// </summary>
    public int field005;
    /// <summary>
    /// 宽表字段 6
    /// 第二行备注：<>&"'
    /// </summary>
    public string field006;
    /// <summary>
    /// 宽表字段 7
    /// 第二行备注：<>&"'
    /// </summary>
    public bool field007;
    /// <summary>
    /// 宽表字段 8
    /// 第二行备注：<>&"'
    /// </summary>
    public double field008;
    /// <summary>
    /// 宽表字段 9
    /// 第二行备注：<>&"'
    /// </summary>
    public List<int> field009;
    /// <summary>
    /// 宽表字段 10
    /// 第二行备注：<>&"'
    /// </summary>
    public int field010;
    /// <summary>
    /// 宽表字段 11
    /// 第二行备注：<>&"'
    /// </summary>
    public string field011;
    /// <summary>
    /// 宽表字段 12
    /// 第二行备注：<>&"'
    /// </summary>
    public bool field012;
    /// <summary>
    /// 宽表字段 13
    /// 第二行备注：<>&"'
    /// </summary>
    public double field013;
    /// <summary>
    /// 宽表字段 14
    /// 第二行备注：<>&"'
    /// </summary>
    public List<int> field014;
    /// <summary>
    /// 宽表字段 15
    /// 第二行备注：<>&"'
    /// </summary>
    public int field015;
    /// <summary>
    /// 宽表字段 16
    /// 第二行备注：<>&"'
    /// </summary>
    public string field016;
    /// <summary>
    /// 宽表字段 17
    /// 第二行备注：<>&"'
    /// </summary>
    public bool field017;
    /// <summary>
    /// 宽表字段 18
    /// 第二行备注：<>&"'
    /// </summary>
    public double field018;
    /// <summary>
    /// 宽表字段 19
    /// 第二行备注：<>&"'
    /// </summary>
    public List<int> field019;
    /// <summary>
    /// 宽表字段 20
    /// 第二行备注：<>&"'
    /// </summary>
    public int field020;
    /// <summary>
    /// 宽表字段 21
    /// 第二行备注：<>&"'
    /// </summary>
    public string field021;
    /// <summary>
    /// 宽表字段 22
    /// 第二行备注：<>&"'
    /// </summary>
    public bool field022;
    /// <summary>
    /// 宽表字段 23
    /// 第二行备注：<>&"'
    /// </summary>
    public double field023;
    /// <summary>
    /// 宽表字段 24
    /// 第二行备注：<>&"'
    /// </summary>
    public List<int> field024;
    /// <summary>
    /// 宽表字段 25
    /// 第二行备注：<>&"'
    /// </summary>
    public int field025;
    /// <summary>
    /// 宽表字段 26
    /// 第二行备注：<>&"'
    /// </summary>
    public string field026;
    /// <summary>
    /// 宽表字段 27
    /// 第二行备注：<>&"'
    /// </summary>
    public bool field027;
    /// <summary>
    /// 宽表字段 28
    /// 第二行备注：<>&"'
    /// </summary>
    public double field028;
    /// <summary>
    /// 宽表字段 29
    /// 第二行备注：<>&"'
    /// </summary>
    public List<int> field029;
    /// <summary>
    /// 宽表字段 30
    /// 第二行备注：<>&"'
    /// </summary>
    public int field030;
    /// <summary>
    /// 宽表字段 31
    /// 第二行备注：<>&"'
    /// </summary>
    public string field031;
    /// <summary>
    /// 宽表字段 32
    /// 第二行备注：<>&"'
    /// </summary>
    public bool field032;
    /// <summary>
    /// 宽表字段 33
    /// 第二行备注：<>&"'
    /// </summary>
    public double field033;
    /// <summary>
    /// 宽表字段 34
    /// 第二行备注：<>&"'
    /// </summary>
    public List<int> field034;
    /// <summary>
    /// 宽表字段 35
    /// 第二行备注：<>&"'
    /// </summary>
    public int field035;
    /// <summary>
    /// 宽表字段 36
    /// 第二行备注：<>&"'
    /// </summary>
    public string field036;
    /// <summary>
    /// 宽表字段 37
    /// 第二行备注：<>&"'
    /// </summary>
    public bool field037;
    /// <summary>
    /// 宽表字段 38
    /// 第二行备注：<>&"'
    /// </summary>
    public double field038;
    /// <summary>
    /// 宽表字段 39
    /// 第二行备注：<>&"'
    /// </summary>
    public List<int> field039;
    /// <summary>
    /// 宽表字段 40
    /// 第二行备注：<>&"'
    /// </summary>
    public int field040;
    /// <summary>
    /// 宽表字段 41
    /// 第二行备注：<>&"'
    /// </summary>
    public string field041;
    /// <summary>
    /// 宽表字段 42
    /// 第二行备注：<>&"'
    /// </summary>
    public bool field042;
    /// <summary>
    /// 宽表字段 43
    /// 第二行备注：<>&"'
    /// </summary>
    public double field043;
    /// <summary>
    /// 宽表字段 44
    /// 第二行备注：<>&"'
    /// </summary>
    public List<int> field044;
    /// <summary>
    /// 宽表字段 45
    /// 第二行备注：<>&"'
    /// </summary>
    public int field045;
    /// <summary>
    /// 宽表字段 46
    /// 第二行备注：<>&"'
    /// </summary>
    public string field046;
    /// <summary>
    /// 宽表字段 47
    /// 第二行备注：<>&"'
    /// </summary>
    public bool field047;
    /// <summary>
    /// 宽表字段 48
    /// 第二行备注：<>&"'
    /// </summary>
    public double field048;
    /// <summary>
    /// 宽表字段 49
    /// 第二行备注：<>&"'
    /// </summary>
    public List<int> field049;
    /// <summary>
    /// 宽表字段 50
    /// 第二行备注：<>&"'
    /// </summary>
    public int field050;
    /// <summary>
    /// 宽表字段 51
    /// 第二行备注：<>&"'
    /// </summary>
    public string field051;
    /// <summary>
    /// 宽表字段 52
    /// 第二行备注：<>&"'
    /// </summary>
    public bool field052;
    /// <summary>
    /// 宽表字段 53
    /// 第二行备注：<>&"'
    /// </summary>
    public double field053;
    /// <summary>
    /// 宽表字段 54
    /// 第二行备注：<>&"'
    /// </summary>
    public List<int> field054;
    /// <summary>
    /// 宽表字段 55
    /// 第二行备注：<>&"'
    /// </summary>
    public int field055;
    /// <summary>
    /// 宽表字段 56
    /// 第二行备注：<>&"'
    /// </summary>
    public string field056;
    /// <summary>
    /// 宽表字段 57
    /// 第二行备注：<>&"'
    /// </summary>
    public bool field057;
    /// <summary>
    /// 宽表字段 58
    /// 第二行备注：<>&"'
    /// </summary>
    public double field058;
    /// <summary>
    /// 宽表字段 59
    /// 第二行备注：<>&"'
    /// </summary>
    public List<int> field059;
    /// <summary>
    /// 宽表字段 60
    /// 第二行备注：<>&"'
    /// </summary>
    public int field060;
    /// <summary>
    /// 宽表字段 61
    /// 第二行备注：<>&"'
    /// </summary>
    public string field061;
    /// <summary>
    /// 宽表字段 62
    /// 第二行备注：<>&"'
    /// </summary>
    public bool field062;
    /// <summary>
    /// 宽表字段 63
    /// 第二行备注：<>&"'
    /// </summary>
    public double field063;
    /// <summary>
    /// 宽表字段 64
    /// 第二行备注：<>&"'
    /// </summary>
    public List<int> field064;
    /// <summary>
    /// 宽表字段 65
    /// 第二行备注：<>&"'
    /// </summary>
    public int field065;
    /// <summary>
    /// 宽表字段 66
    /// 第二行备注：<>&"'
    /// </summary>
    public string field066;
    /// <summary>
    /// 宽表字段 67
    /// 第二行备注：<>&"'
    /// </summary>
    public bool field067;
    /// <summary>
    /// 宽表字段 68
    /// 第二行备注：<>&"'
    /// </summary>
    public double field068;
    /// <summary>
    /// 宽表字段 69
    /// 第二行备注：<>&"'
    /// </summary>
    public List<int> field069;
    /// <summary>
    /// 宽表字段 70
    /// 第二行备注：<>&"'
    /// </summary>
    public int field070;
    /// <summary>
    /// 宽表字段 71
    /// 第二行备注：<>&"'
    /// </summary>
    public string field071;
    /// <summary>
    /// 宽表字段 72
    /// 第二行备注：<>&"'
    /// </summary>
    public bool field072;
    /// <summary>
    /// 宽表字段 73
    /// 第二行备注：<>&"'
    /// </summary>
    public double field073;
    /// <summary>
    /// 宽表字段 74
    /// 第二行备注：<>&"'
    /// </summary>
    public List<int> field074;
    /// <summary>
    /// 宽表字段 75
    /// 第二行备注：<>&"'
    /// </summary>
    public int field075;
    /// <summary>
    /// 宽表字段 76
    /// 第二行备注：<>&"'
    /// </summary>
    public string field076;
    /// <summary>
    /// 宽表字段 77
    /// 第二行备注：<>&"'
    /// </summary>
    public bool field077;
    /// <summary>
    /// 宽表字段 78
    /// 第二行备注：<>&"'
    /// </summary>
    public double field078;
    /// <summary>
    /// 宽表字段 79
    /// 第二行备注：<>&"'
    /// </summary>
    public List<int> field079;
    /// <summary>
    /// 宽表字段 80
    /// 第二行备注：<>&"'
    /// </summary>
    public int field080;
    /// <summary>
    /// 宽表字段 81
    /// 第二行备注：<>&"'
    /// </summary>
    public string field081;
    /// <summary>
    /// 宽表字段 82
    /// 第二行备注：<>&"'
    /// </summary>
    public bool field082;
    /// <summary>
    /// 宽表字段 83
    /// 第二行备注：<>&"'
    /// </summary>
    public double field083;
    /// <summary>
    /// 宽表字段 84
    /// 第二行备注：<>&"'
    /// </summary>
    public List<int> field084;
    /// <summary>
    /// 宽表字段 85
    /// 第二行备注：<>&"'
    /// </summary>
    public int field085;
    /// <summary>
    /// 宽表字段 86
    /// 第二行备注：<>&"'
    /// </summary>
    public string field086;
    /// <summary>
    /// 宽表字段 87
    /// 第二行备注：<>&"'
    /// </summary>
    public bool field087;
    /// <summary>
    /// 宽表字段 88
    /// 第二行备注：<>&"'
    /// </summary>
    public double field088;
    /// <summary>
    /// 宽表字段 89
    /// 第二行备注：<>&"'
    /// </summary>
    public List<int> field089;
    /// <summary>
    /// 宽表字段 90
    /// 第二行备注：<>&"'
    /// </summary>
    public int field090;
    /// <summary>
    /// 宽表字段 91
    /// 第二行备注：<>&"'
    /// </summary>
    public string field091;
    /// <summary>
    /// 宽表字段 92
    /// 第二行备注：<>&"'
    /// </summary>
    public bool field092;
    /// <summary>
    /// 宽表字段 93
    /// 第二行备注：<>&"'
    /// </summary>
    public double field093;
    /// <summary>
    /// 宽表字段 94
    /// 第二行备注：<>&"'
    /// </summary>
    public List<int> field094;
    /// <summary>
    /// 宽表字段 95
    /// 第二行备注：<>&"'
    /// </summary>
    public int field095;
    /// <summary>
    /// 宽表字段 96
    /// 第二行备注：<>&"'
    /// </summary>
    public string field096;
    /// <summary>
    /// 宽表字段 97
    /// 第二行备注：<>&"'
    /// </summary>
    public bool field097;
    /// <summary>
    /// 宽表字段 98
    /// 第二行备注：<>&"'
    /// </summary>
    public double field098;
    /// <summary>
    /// 宽表字段 99
    /// 第二行备注：<>&"'
    /// </summary>
    public List<int> field099;
    /// <summary>
    /// 宽表字段 100
    /// 第二行备注：<>&"'
    /// </summary>
    public int field100;
}

[Serializable]
public class TestDataEntity_HugeRows
{
    /// <summary>
    /// 大数据行字段 1
    /// </summary>
    public int id;
    /// <summary>
    /// 大数据行字段 2
    /// </summary>
    public string code;
    /// <summary>
    /// 大数据行字段 3
    /// </summary>
    public string title;
    /// <summary>
    /// 大数据行字段 4
    /// </summary>
    public bool active;
    /// <summary>
    /// 大数据行字段 5
    /// </summary>
    public long count;
    /// <summary>
    /// 大数据行字段 6
    /// </summary>
    public double ratio;
    /// <summary>
    /// 大数据行字段 7
    /// </summary>
    public DateTime created;
    /// <summary>
    /// 大数据行字段 8
    /// </summary>
    public List<int> values;
    /// <summary>
    /// 大数据行字段 9
    /// </summary>
    public Vector3 point;
    /// <summary>
    /// 大数据行字段 10
    /// </summary>
    public Dictionary<string, int> dict;
    /// <summary>
    /// 大数据行字段 11
    /// </summary>
    public string memo;
    /// <summary>
    /// 大数据行字段 12
    /// </summary>
    public string tail;
}

[Serializable]
public class TestDataEntity_ComplexTypes
{
    /// <summary>
    /// 复杂类型 1
    /// </summary>
    public int id;
    /// <summary>
    /// 复杂类型 2
    /// </summary>
    public byte tiny;
    /// <summary>
    /// 复杂类型 3
    /// </summary>
    public short small;
    /// <summary>
    /// 复杂类型 4
    /// </summary>
    public int normal;
    /// <summary>
    /// 复杂类型 5
    /// </summary>
    public long big;
    /// <summary>
    /// 复杂类型 6
    /// </summary>
    public uint unsigned;
    /// <summary>
    /// 复杂类型 7
    /// </summary>
    public float single;
    /// <summary>
    /// 复杂类型 8
    /// </summary>
    public double real;
    /// <summary>
    /// 复杂类型 9
    /// </summary>
    public bool flag;
    /// <summary>
    /// 复杂类型 10
    /// </summary>
    public int? nullableValue;
    /// <summary>
    /// 复杂类型 11
    /// </summary>
    public DateTime when;
    /// <summary>
    /// 复杂类型 12
    /// </summary>
    public List<int> numbers;
    /// <summary>
    /// 复杂类型 13
    /// </summary>
    public string[] words;
    /// <summary>
    /// 复杂类型 14
    /// </summary>
    public Vector2 vec2;
    /// <summary>
    /// 复杂类型 15
    /// </summary>
    public Vector3 vec3;
    /// <summary>
    /// 复杂类型 16
    /// </summary>
    public Dictionary<string, int> map;
    /// <summary>
    /// 复杂类型 17
    /// </summary>
    public string escaped;
    /// <summary>
    /// 复杂类型 18
    /// </summary>
    public string emptyText;
}

[Serializable]
public class TestDataEntity_FormulaCells
{
    /// <summary>
    /// key
    /// </summary>
    public int id;
    /// <summary>
    /// 输入 A
    /// </summary>
    public double a;
    /// <summary>
    /// 输入 B
    /// </summary>
    public double b;
    /// <summary>
    /// A+B
    /// </summary>
    public double sum;
    /// <summary>
    /// A*B
    /// </summary>
    public double product;
    /// <summary>
    /// 文本公式
    /// </summary>
    public string textFormula;
    /// <summary>
    /// 布尔公式
    /// </summary>
    public bool boolFormula;
    /// <summary>
    /// 跨表公式
    /// </summary>
    public double crossSheet;
}

[Serializable]
public class TestDataEntity_CommentRows
{
    /// <summary>
    /// 编号
    /// </summary>
    public int id;
    /// <summary>
    /// 值
    /// </summary>
    public string value;
    /// <summary>
    /// 备注
    /// </summary>
    public string memo;
}

[Serializable]
public class TestDataEntity_CommentCols
{
    /// <summary>
    /// 编号
    /// </summary>
    public int id;
    /// <summary>
    /// 有效值
    /// </summary>
    public string value;
}

[Serializable]
public class TestDataEntity_BlankRowStop
{
    /// <summary>
    /// 编号
    /// </summary>
    public int id;
    /// <summary>
    /// 值
    /// </summary>
    public string value;
    /// <summary>
    /// 尾
    /// </summary>
    public string tail;
}

[Serializable]
public class TestDataEntity_BlankColStop
{
    /// <summary>
    /// 编号
    /// </summary>
    public int id;
    /// <summary>
    /// 值
    /// </summary>
    public string value;
    /// <summary>
    /// 空白前
    /// </summary>
    public string gap;
}

[Serializable]
public class TestDataEntity_WhitespaceStop
{
    /// <summary>
    /// 编号
    /// </summary>
    public string id;
    /// <summary>
    /// 值
    /// </summary>
    public string value;
}

[Serializable]
public class TestDataEntity_HeaderOnly
{
    /// <summary>
    /// 编号
    /// </summary>
    public int id;
    /// <summary>
    /// 名称
    /// </summary>
    public string name;
}

[Serializable]
public class TestDataEntity_SparseCells
{
    /// <summary>
    /// 编号
    /// </summary>
    public int id;
    /// <summary>
    /// A
    /// </summary>
    public string a;
    /// <summary>
    /// B
    /// </summary>
    public int b;
    /// <summary>
    /// C
    /// </summary>
    public bool c;
    /// <summary>
    /// D
    /// </summary>
    public string d;
}

[Serializable]
public class TestDataEntity_DuplicateKeys
{
    /// <summary>
    /// 重复键
    /// </summary>
    public int id;
    /// <summary>
    /// 名称
    /// </summary>
    public string name;
}

[Serializable]
public class TestDataEntity_BoundaryValues
{
    /// <summary>
    /// key
    /// </summary>
    public int id;
    /// <summary>
    /// 整数边界
    /// </summary>
    public long integer;
    /// <summary>
    /// 浮点边界
    /// </summary>
    public double real;
    /// <summary>
    /// 布尔
    /// </summary>
    public bool truth;
    /// <summary>
    /// 长文本
    /// </summary>
    public string text;
    /// <summary>
    /// 日期
    /// </summary>
    public DateTime date;
    /// <summary>
    /// JSON
    /// </summary>
    public List<int> json;
}

[Serializable]
public class TestDataEntity_NullableTypes
{
    /// <summary>
    /// Nullable 空单元格应为 null
    /// </summary>
    public int id;
    /// <summary>
    /// Nullable 空单元格应为 null
    /// </summary>
    public int? intValue;
    /// <summary>
    /// Nullable 空单元格应为 null
    /// </summary>
    public long? longValue;
    /// <summary>
    /// Nullable 空单元格应为 null
    /// </summary>
    public float? floatValue;
    /// <summary>
    /// Nullable 空单元格应为 null
    /// </summary>
    public double? doubleValue;
    /// <summary>
    /// Nullable 空单元格应为 null
    /// </summary>
    public bool? boolValue;
    /// <summary>
    /// Nullable 空单元格应为 null
    /// </summary>
    public DateTime? dateValue;
    /// <summary>
    /// Nullable 空单元格应为 null
    /// </summary>
    public MstItemCategoryEnum? enumValue;
    /// <summary>
    /// Nullable 空单元格应为 null
    /// </summary>
    public string textValue;
}

[Serializable]
public class TestDataEntity_EmptyCellDefaults
{
    /// <summary>
    /// 空单元格应使用字段默认值
    /// </summary>
    public int id;
    /// <summary>
    /// 空单元格应使用字段默认值
    /// </summary>
    public int intValue;
    /// <summary>
    /// 空单元格应使用字段默认值
    /// </summary>
    public long longValue;
    /// <summary>
    /// 空单元格应使用字段默认值
    /// </summary>
    public float floatValue;
    /// <summary>
    /// 空单元格应使用字段默认值
    /// </summary>
    public double doubleValue;
    /// <summary>
    /// 空单元格应使用字段默认值
    /// </summary>
    public bool boolValue;
    /// <summary>
    /// 空单元格应使用字段默认值
    /// </summary>
    public string textValue;
    /// <summary>
    /// 空单元格应使用字段默认值
    /// </summary>
    public DateTime dateValue;
    /// <summary>
    /// 空单元格应使用字段默认值
    /// </summary>
    public List<int> listValue;
    /// <summary>
    /// 空单元格应使用字段默认值
    /// </summary>
    public int? nullableValue;
}

[Serializable]
public class TestDataEntity_ManyNotes
{
    /// <summary>
    /// 单行备注
    /// </summary>
    public int id;
    /// <summary>
    /// 普通备注
    /// </summary>
    public string singleLine;
    /// <summary>
    /// 第一行`n第二行`n第三行
    /// </summary>
    public string multiLine;
    /// <summary>
    /// XML: <tag> & value > 0; ///
    /// </summary>
    public string xmlChars;
    /// <summary>
    /// 中文、日本語、한글、emoji 😀
    /// </summary>
    public string unicode;
    /// <summary>
    /// 超长备注：
    /// </summary>
    public string veryLong;
}

[ExcelAsset]
public class TestData : ScriptableObject, ISerializationCallbackReceiver
{
    public List<TestDataEntity_NoKey> NoKey = new();
    public List<TestDataEntity_SingleKey> SingleKey = new();
    public Dictionary<int, TestDataEntity_SingleKey> SingleKeyDict;
    public List<TestDataEntity_MultiKey> MultiKey = new();
    public Dictionary<(int id, string name), TestDataEntity_MultiKey> MultiKeyDict;
    public List<TestDataEntity_WideData> WideData = new();
    public Dictionary<string, TestDataEntity_WideData> WideDataDict;
    public List<TestDataEntity_HugeRows> HugeRows = new();
    public Dictionary<int, TestDataEntity_HugeRows> HugeRowsDict;
    public List<TestDataEntity_ComplexTypes> ComplexTypes = new();
    public Dictionary<int, TestDataEntity_ComplexTypes> ComplexTypesDict;
    public List<TestDataEntity_FormulaCells> FormulaCells = new();
    public Dictionary<int, TestDataEntity_FormulaCells> FormulaCellsDict;
    public List<TestDataEntity_CommentRows> CommentRows = new();
    public Dictionary<int, TestDataEntity_CommentRows> CommentRowsDict;
    public List<TestDataEntity_CommentCols> CommentCols = new();
    public Dictionary<int, TestDataEntity_CommentCols> CommentColsDict;
    public List<TestDataEntity_BlankRowStop> BlankRowStop = new();
    public Dictionary<int, TestDataEntity_BlankRowStop> BlankRowStopDict;
    public List<TestDataEntity_BlankColStop> BlankColStop = new();
    public Dictionary<int, TestDataEntity_BlankColStop> BlankColStopDict;
    public List<TestDataEntity_WhitespaceStop> WhitespaceStop = new();
    public Dictionary<string, TestDataEntity_WhitespaceStop> WhitespaceStopDict;
    public List<TestDataEntity_HeaderOnly> HeaderOnly = new();
    public Dictionary<int, TestDataEntity_HeaderOnly> HeaderOnlyDict;
    public List<TestDataEntity_SparseCells> SparseCells = new();
    public Dictionary<int, TestDataEntity_SparseCells> SparseCellsDict;
    public List<TestDataEntity_DuplicateKeys> DuplicateKeys = new();
    public Dictionary<int, TestDataEntity_DuplicateKeys> DuplicateKeysDict;
    public List<TestDataEntity_BoundaryValues> BoundaryValues = new();
    public Dictionary<int, TestDataEntity_BoundaryValues> BoundaryValuesDict;
    public List<TestDataEntity_NullableTypes> NullableTypes = new();
    public Dictionary<int, TestDataEntity_NullableTypes> NullableTypesDict;
    public List<TestDataEntity_EmptyCellDefaults> EmptyCellDefaults = new();
    public Dictionary<int, TestDataEntity_EmptyCellDefaults> EmptyCellDefaultsDict;
    public List<TestDataEntity_ManyNotes> ManyNotes = new();
    public Dictionary<int, TestDataEntity_ManyNotes> ManyNotesDict;

    public void OnBeforeSerialize()
    {
        // Implement any logic needed before serialization
    }

    public void OnAfterDeserialize()
    {
        // Implement any logic needed after deserialization
        SingleKeyDict = new();
        foreach (TestDataEntity_SingleKey item in SingleKey)
        {
            var key = item.id;
            if (SingleKeyDict.ContainsKey(key))
            {
                Debug.LogError($"Duplicate key found in SingleKeyDict (script: TestData): {key}. Each key must be unique.");
                continue; // Skip adding this item to the dictionary
            }
            SingleKeyDict[key] = item;
        }
        MultiKeyDict = new();
        foreach (TestDataEntity_MultiKey item in MultiKey)
        {
            var key = (
                item.id,
                item.name
            );
            if (MultiKeyDict.ContainsKey(key))
            {
                Debug.LogError($"Duplicate key found in MultiKeyDict (script: TestData): {key}. Each key must be unique.");
                continue; // Skip adding this item to the dictionary
            }
            MultiKeyDict[key] = item;
        }
        WideDataDict = new();
        foreach (TestDataEntity_WideData item in WideData)
        {
            var key = item.field001;
            if (WideDataDict.ContainsKey(key))
            {
                Debug.LogError($"Duplicate key found in WideDataDict (script: TestData): {key}. Each key must be unique.");
                continue; // Skip adding this item to the dictionary
            }
            WideDataDict[key] = item;
        }
        HugeRowsDict = new();
        foreach (TestDataEntity_HugeRows item in HugeRows)
        {
            var key = item.id;
            if (HugeRowsDict.ContainsKey(key))
            {
                Debug.LogError($"Duplicate key found in HugeRowsDict (script: TestData): {key}. Each key must be unique.");
                continue; // Skip adding this item to the dictionary
            }
            HugeRowsDict[key] = item;
        }
        ComplexTypesDict = new();
        foreach (TestDataEntity_ComplexTypes item in ComplexTypes)
        {
            var key = item.id;
            if (ComplexTypesDict.ContainsKey(key))
            {
                Debug.LogError($"Duplicate key found in ComplexTypesDict (script: TestData): {key}. Each key must be unique.");
                continue; // Skip adding this item to the dictionary
            }
            ComplexTypesDict[key] = item;
        }
        FormulaCellsDict = new();
        foreach (TestDataEntity_FormulaCells item in FormulaCells)
        {
            var key = item.id;
            if (FormulaCellsDict.ContainsKey(key))
            {
                Debug.LogError($"Duplicate key found in FormulaCellsDict (script: TestData): {key}. Each key must be unique.");
                continue; // Skip adding this item to the dictionary
            }
            FormulaCellsDict[key] = item;
        }
        CommentRowsDict = new();
        foreach (TestDataEntity_CommentRows item in CommentRows)
        {
            var key = item.id;
            if (CommentRowsDict.ContainsKey(key))
            {
                Debug.LogError($"Duplicate key found in CommentRowsDict (script: TestData): {key}. Each key must be unique.");
                continue; // Skip adding this item to the dictionary
            }
            CommentRowsDict[key] = item;
        }
        CommentColsDict = new();
        foreach (TestDataEntity_CommentCols item in CommentCols)
        {
            var key = item.id;
            if (CommentColsDict.ContainsKey(key))
            {
                Debug.LogError($"Duplicate key found in CommentColsDict (script: TestData): {key}. Each key must be unique.");
                continue; // Skip adding this item to the dictionary
            }
            CommentColsDict[key] = item;
        }
        BlankRowStopDict = new();
        foreach (TestDataEntity_BlankRowStop item in BlankRowStop)
        {
            var key = item.id;
            if (BlankRowStopDict.ContainsKey(key))
            {
                Debug.LogError($"Duplicate key found in BlankRowStopDict (script: TestData): {key}. Each key must be unique.");
                continue; // Skip adding this item to the dictionary
            }
            BlankRowStopDict[key] = item;
        }
        BlankColStopDict = new();
        foreach (TestDataEntity_BlankColStop item in BlankColStop)
        {
            var key = item.id;
            if (BlankColStopDict.ContainsKey(key))
            {
                Debug.LogError($"Duplicate key found in BlankColStopDict (script: TestData): {key}. Each key must be unique.");
                continue; // Skip adding this item to the dictionary
            }
            BlankColStopDict[key] = item;
        }
        WhitespaceStopDict = new();
        foreach (TestDataEntity_WhitespaceStop item in WhitespaceStop)
        {
            var key = item.id;
            if (WhitespaceStopDict.ContainsKey(key))
            {
                Debug.LogError($"Duplicate key found in WhitespaceStopDict (script: TestData): {key}. Each key must be unique.");
                continue; // Skip adding this item to the dictionary
            }
            WhitespaceStopDict[key] = item;
        }
        HeaderOnlyDict = new();
        foreach (TestDataEntity_HeaderOnly item in HeaderOnly)
        {
            var key = item.id;
            if (HeaderOnlyDict.ContainsKey(key))
            {
                Debug.LogError($"Duplicate key found in HeaderOnlyDict (script: TestData): {key}. Each key must be unique.");
                continue; // Skip adding this item to the dictionary
            }
            HeaderOnlyDict[key] = item;
        }
        SparseCellsDict = new();
        foreach (TestDataEntity_SparseCells item in SparseCells)
        {
            var key = item.id;
            if (SparseCellsDict.ContainsKey(key))
            {
                Debug.LogError($"Duplicate key found in SparseCellsDict (script: TestData): {key}. Each key must be unique.");
                continue; // Skip adding this item to the dictionary
            }
            SparseCellsDict[key] = item;
        }
        DuplicateKeysDict = new();
        foreach (TestDataEntity_DuplicateKeys item in DuplicateKeys)
        {
            var key = item.id;
            if (DuplicateKeysDict.ContainsKey(key))
            {
                Debug.LogError($"Duplicate key found in DuplicateKeysDict (script: TestData): {key}. Each key must be unique.");
                continue; // Skip adding this item to the dictionary
            }
            DuplicateKeysDict[key] = item;
        }
        BoundaryValuesDict = new();
        foreach (TestDataEntity_BoundaryValues item in BoundaryValues)
        {
            var key = item.id;
            if (BoundaryValuesDict.ContainsKey(key))
            {
                Debug.LogError($"Duplicate key found in BoundaryValuesDict (script: TestData): {key}. Each key must be unique.");
                continue; // Skip adding this item to the dictionary
            }
            BoundaryValuesDict[key] = item;
        }
        NullableTypesDict = new();
        foreach (TestDataEntity_NullableTypes item in NullableTypes)
        {
            var key = item.id;
            if (NullableTypesDict.ContainsKey(key))
            {
                Debug.LogError($"Duplicate key found in NullableTypesDict (script: TestData): {key}. Each key must be unique.");
                continue; // Skip adding this item to the dictionary
            }
            NullableTypesDict[key] = item;
        }
        EmptyCellDefaultsDict = new();
        foreach (TestDataEntity_EmptyCellDefaults item in EmptyCellDefaults)
        {
            var key = item.id;
            if (EmptyCellDefaultsDict.ContainsKey(key))
            {
                Debug.LogError($"Duplicate key found in EmptyCellDefaultsDict (script: TestData): {key}. Each key must be unique.");
                continue; // Skip adding this item to the dictionary
            }
            EmptyCellDefaultsDict[key] = item;
        }
        ManyNotesDict = new();
        foreach (TestDataEntity_ManyNotes item in ManyNotes)
        {
            var key = item.id;
            if (ManyNotesDict.ContainsKey(key))
            {
                Debug.LogError($"Duplicate key found in ManyNotesDict (script: TestData): {key}. Each key must be unique.");
                continue; // Skip adding this item to the dictionary
            }
            ManyNotesDict[key] = item;
        }
    }
}
