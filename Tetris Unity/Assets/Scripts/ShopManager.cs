using UnityEngine;
using UnityEngine.UI;


public class ShopManager : MonoBehaviour
{
    public static ShopManager Instance { get; private set; }

    public TMPro.TextMeshProUGUI currencyText;
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


    private void OnEnable()
    {
        if (CurrencyManager.Instance != null)
            CurrencyManager.Instance.OnCurrencyChanged += UpdateCurrencyUI;
        UpdateCurrencyUI();
        UpdateLinesClearedUI();
    }

    public void UpdateCurrencyUI()
    {
        currencyText.text = CurrencyManager.Instance.currency.ToString();
    }

    public void UpdateLinesClearedUI()
    {
        if (GameManager.Instance != null)
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
    
    public void BuyComboUpgrade()
    {
        int cost = 10; // Number of lines required
        if (GameManager.Instance.SpendLines(cost))
        {
            Debug.Log("Upgrade bought with lines!");
            // Apply the upgrade logic here...
            UpdateLinesClearedUI();
        }
        else
        {
            Debug.Log("Not enough lines cleared!");
        }
    }

}