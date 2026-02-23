using UnityEngine;

public class ShopButton : MonoBehaviour
{
    [SerializeField] private string itemId;
    [SerializeField] private ShopManager shopManager;

    public void OnClick()
    {
        shopManager.TryPurchase(itemId);
    }
}