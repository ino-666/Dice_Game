using System;
using System.Collections.Generic;

[Serializable]
public class ShopItemData
{
    public string id;
    public string itemName;
    public string type;
    public int cost;
    public int intValue;
    public float floatValue;
}

[Serializable]
public class ShopItemList
{
    public List<ShopItemData> items;
}