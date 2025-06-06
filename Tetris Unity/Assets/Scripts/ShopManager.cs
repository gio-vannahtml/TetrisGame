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

            // 🧠 Add the item to inventory
            switch (itemName)
            {
                case "Bombastic":
                    AddItemToInventory(InventoryItemUI.ItemType.Bombastic);
                    break;
                case "The Tractor":
                    AddItemToInventory(InventoryItemUI.ItemType.Tractor);
                    break;
                case "The Crasher":
                    AddItemToInventory(InventoryItemUI.ItemType.Crusher);
                    break;
                case "Color Popper":
                    AddItemToInventory(InventoryItemUI.ItemType.ColorPopper);
                    break;
            }
        }
        else
        {
            Debug.Log("Not enough currency to buy: " + itemName);
        }
    }

    private void AddItemToInventory(InventoryItemUI.ItemType type)
    {
        InventoryUI inventory = FindFirstObjectByType<InventoryUI>();
        if (inventory != null)
        {
            inventory.AddItem(null, () =>
            {
                GridScript grid = FindFirstObjectByType<GridScript>();
                switch (type)
                {
                    case InventoryItemUI.ItemType.Bombastic:
                        grid.UseBombastic();
                        break;
                    case InventoryItemUI.ItemType.Crusher:
                        grid.UseCrusher();
                        break;
                    case InventoryItemUI.ItemType.Tractor:
                        grid.UseTractor();
                        break;
                    case InventoryItemUI.ItemType.ColorPopper:
                        grid.UseColorPopper();
                        break;
                }
            });
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
    
    public void ResetAllProgress()
    {
        CurrencyManager.Instance.ResetRun();
        CurrencyManager.Instance.ResetCombos();
        FindFirstObjectByType<InventoryUI>()?.ClearAllSlots();
    }

}