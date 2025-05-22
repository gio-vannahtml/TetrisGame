using UnityEngine;
using UnityEngine.UI;


public class ShopManager : MonoBehaviour
{
    public Text currencyText;

    private void Start()
    {
        UpdateCurrencyUI();
    }

    public void BuyItem(int cost, string itemName)
    {
        if (CurrencyManager.Instance.currency >= cost)
        {
            CurrencyManager.Instance.SpendCurrency(cost);
            UpdateCurrencyUI();

            Debug.Log("Purchased: " + itemName);

            // TODO: Apply the item effect here
        }
        else
        {
            Debug.Log("Not enough currency to buy: " + itemName);
        }
    }

    public void UpdateCurrencyUI()
    {
        currencyText.text = CurrencyManager.Instance.currency.ToString();
    }

    public void BuyBombastic()
    {
        BuyItem(300, "Bombastic");
    }

    public void BuyTractor()
    {
        BuyItem(400, "The Tractor");
    }

    public void BuyCrasher()
    {
        BuyItem(500, "The Crasher");
    }

    public void BuyColorPopper()
    {
        BuyItem(600, "Color Popper");
    }
}