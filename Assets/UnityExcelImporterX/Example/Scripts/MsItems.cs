using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MsItemsEntity_item
{
    public int id;
    /// <summary>
    /// 名字
    /// </summary>
    public string name;
    /// <summary>
    /// 价格
    /// </summary>
    public float price;
    /// <summary>
    /// 是否缩放
    /// </summary>
    public bool isNotForSale;
    /// <summary>
    /// 比率
    /// </summary>
    public float rate;
    /// <summary>
    /// 类别
    /// </summary>
    public MstItemCategoryEnum category;
}

[Serializable]
public class MsItemsEntity_itemEx
{
    public int id;
    /// <summary>
    /// 名字
    /// </summary>
    public string name;
    /// <summary>
    /// 性别
    /// </summary>
    public char sex;
    /// <summary>
    /// 伤害
    /// </summary>
    public float hit;
    /// <summary>
    /// 激活
    /// </summary>
    public bool activate;
    /// <summary>
    /// 时间
    /// </summary>
    public DateTime createTime;
    /// <summary>
    /// 日期
    /// </summary>
    public DateTime createDate;
    /// <summary>
    /// 长得像日期的数字
    /// </summary>
    public long longNum;
    /// <summary>
    /// 经验数组
    /// </summary>
    public List<int> expList;
    /// <summary>
    /// 描述数组
    /// </summary>
    public string[] descriptList;
    /// <summary>
    /// 点
    /// </summary>
    public Vector3 point;
    /// <summary>
    /// Hash
    /// </summary>
    public HashSet<int> hset;
    /// <summary>
    /// 集合
    /// </summary>
    public Dictionary<string,int> dict;
    /// <summary>
    /// 自定义类
    /// </summary>
    public MstItemCustomType customType;
}


[ExcelAsset]
public class MsItems : ScriptableObject
{
    public List<MsItemsEntity_item> item;
    public List<MsItemsEntity_itemEx> itemEx;
}
