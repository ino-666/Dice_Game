using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class ShopManager : MonoBehaviour
{
    [SerializeField] private DiceManager diceManager;
    [SerializeField] private TextAsset shopJson;

    private Dictionary<string, ShopItemData> itemDict;

    private void Awake()
    {
        LoadShopData();
    }

    void LoadShopData()
    {
        var list = JsonUtility.FromJson<ShopItemList>(shopJson.text);
        itemDict = list.items.ToDictionary(x => x.id, x => x);
    }

    public bool TryPurchase(string itemId)
    {
        if (!itemDict.ContainsKey(itemId))
        {
            Debug.Log("そのIDは存在しない");
            return false;
        }
    
        var item = itemDict[itemId];
    
        // スコア足りる？
        if (diceManager.GetScore() < item.cost)
        {
            Debug.Log("スコア不足");
            return false;
        }
    
        // スコア消費
        diceManager.SpendScore(item.cost);
    
        // 効果適用
        ApplyEffect(item);
    
        Debug.Log(item.itemName + " を購入！");
        return true;
    }

    void ApplyEffect(ShopItemData item)
    {
        switch (item.type)
        {
            case "DiceCount":
                diceManager.SetDiceCount(item.intValue);
                break;

            case "ScoreMultiplier":
                diceManager.SetScoreMultiplier(item.floatValue);
                break;
        }
    }
}