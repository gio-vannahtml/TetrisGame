using UnityEngine;
using UnityEngine.UI;


public class ShopManager : MonoBehaviour
{
    public static ShopManager Instance { get; private set; }

    public TMPro.TextMeshProUGUI currencyText;
    public TMPro.TextMeshProUGUI combosText;
    public TMPro.TextMeshProUGUI upgradeMessageText;

    public TMPro.TextMeshProUGUI linesClearedCurrencyText;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject); // Optional: prevent duplicates
            return;
        }

    }

    private void Start()
    {
        CurrencyManager.Instance.OnCurrencyChanged += UpdateCurrencyUI;

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

    public void BuyUpgradeWithCombos(int cost, string upgradeName)
    {
        if (CurrencyManager.Instance.combos >= cost)
        {
            CurrencyManager.Instance.combos -= cost;
            Debug.Log("Upgrade purchased with combos: " + upgradeName);
            UpdateCurrencyUI(); // Refresh UI
        }
        else
        {   
            Debug.Log("Not enough combos to buy: " + upgradeName);
            if (upgradeMessageText != null)
                upgradeMessageText.text = "Not enough combos to buy: " + upgradeName;
        }      
    }

    private void OnEnable()
    {
        if (CurrencyManager.Instance != null)
            CurrencyManager.Instance.OnCurrencyChanged += UpdateCurrencyUI;
        UpdateCurrencyUI();
        UpdateLinesClearedUI();
    }

    public void UpdateCurrencyUI()
    {
        if (currencyText != null)
            currencyText.text = "" + CurrencyManager.Instance.currency;

        if (combosText != null)
            combosText.text = CurrencyManager.Instance.combos.ToString();

    }

    public void UpdateLinesClearedUI()
    {
        if (linesClearedCurrencyText != null)
        {
            linesClearedCurrencyText.text = GameManager.Instance.GetLinesCleared().ToString();
        }
    }

    private void OnDisable()
    {
        CurrencyManager.Instance.OnCurrencyChanged -= UpdateCurrencyUI;
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

    public void BuyBombasticUpgrade()
    {
        BuyUpgradeWithCombos(300, "Bombastic");
    }

    public void BuyCrusherUpgrade()
    {
        BuyUpgradeWithCombos(300, "The Crasher");
    }

    public void BuyTractorUpgrade()
    {
        BuyUpgradeWithCombos(300, "The Tractor");
    }
    
    public void BuyColorPUpgrade()
    {
        BuyUpgradeWithCombos(300, "Color Popper");
    }
}