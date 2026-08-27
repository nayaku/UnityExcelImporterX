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
    /// 价格1
    /// </summary>
    public float price;
    /// <summary>
    /// 价格2
    /// </summary>
    public float price2;
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
    /// 
    ///   这个是一个多行注释
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
    /// 日期
    /// </summary>
    public DateTime createDate;
    /// <summary>
    /// 长得像日期数字
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
    public Dictionary<string, int> dict;
    /// <summary>
    /// 自定义类
    /// </summary>
    public MstItemCustomType customType;
}

[ExcelAsset]
public class MsItems : ScriptableObject, ISerializationCallbackReceiver
{
    public List<MsItemsEntity_item> item = new();
    public Dictionary<(int id, string name), MsItemsEntity_item> itemDict;
    public List<MsItemsEntity_itemEx> itemEx = new();
    public Dictionary<int, MsItemsEntity_itemEx> itemExDict;

    public void OnBeforeSerialize()
    {
        // Implement any logic needed before serialization
    }

    public void OnAfterDeserialize()
    {
        // Implement any logic needed after deserialization
        itemDict = new();
        foreach (MsItemsEntity_item item in item)
        {
            var key = (
                item.id,
                item.name
            );
            if (itemDict.ContainsKey(key))
            {
                Debug.LogError($"Duplicate key found in itemDict (script: MsItems): {key}. Each key must be unique.");
                continue; // Skip adding this item to the dictionary
            }
            itemDict[key] = item;
        }
        itemExDict = new();
        foreach (MsItemsEntity_itemEx item in itemEx)
        {
            var key = item.id;
            if (itemExDict.ContainsKey(key))
            {
                Debug.LogError($"Duplicate key found in itemExDict (script: MsItems): {key}. Each key must be unique.");
                continue; // Skip adding this item to the dictionary
            }
            itemExDict[key] = item;
        }
    }
}
