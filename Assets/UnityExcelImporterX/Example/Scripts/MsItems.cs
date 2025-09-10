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
public class MsItemsEntity_equip
{
    public int id;
    /// <summary>
    /// 名字
    /// </summary>
    public string name;
    /// <summary>
    /// 伤害
    /// </summary>
    public float hit;

}

[ExcelAsset]
public class MsItems : ScriptableObject
{
    public List<MsItemsEntity_item> item;
    public List<MsItemsEntity_equip> equip;

}
