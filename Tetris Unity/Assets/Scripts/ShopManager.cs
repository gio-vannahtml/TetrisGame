using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    public int playerCurrency = 1000;
    public Text currencyText;

    public void BuyItem(int cost, string itemName)
    {
        if (playerCurrency >= cost)
        {
            playerCurrency -= cost;
            UpdateCurrencyUI();
            Debug.Log("Purchased: " + itemName);

            // Add your logic for giving the item here, e.g.:
            // Inventory.Add(itemName);
        }
        else
        {
            Debug.Log("Not enough currency!");
        }
    }

    void UpdateCurrencyUI()
    {
        currencyText.text = playerCurrency.ToString();
    }

    public void BuyBombastic()
    {
        BuyItem(300, "Bombastic");
    }

    public void BuyCrusher()
    {
        BuyItem(400, "The Crusher");
    }

    public void BuyTractor()
    {
        BuyItem(500, "The Tractor");
    } 
    
    public void BuyColorPopper()
    {
    BuyItem(600, "Color Popper");
    } 
}