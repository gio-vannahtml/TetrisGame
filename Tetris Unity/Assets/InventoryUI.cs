using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryUI : MonoBehaviour
{
    [Serializable]
    public class InventorySlot
    {
        public Button button;
        public TextMeshProUGUI counterText;
        public InventoryItemUI.ItemType itemType;
    }

    [Header("Optional: Total Inventory Count")]
    public TextMeshProUGUI inventoryCountText;

    [Header("Each slot with button + counter + type")]
    public List<InventorySlot> itemSlots = new List<InventorySlot>();

    private Dictionary<InventoryItemUI.ItemType, int> itemCounts = new Dictionary<InventoryItemUI.ItemType, int>();

    public void AddItem(Sprite icon, Action onClick, InventoryItemUI.ItemType itemType)
    {
        InventorySlot slot = itemSlots.Find(s => s.itemType == itemType);
        if (slot == null)
        {
            Debug.LogWarning($"No slot found for item type: {itemType}");
            return;
        }

        if (icon != null)
        {
            var iconImage = slot.button.transform.Find("Icon")?.GetComponent<Image>();
            if (iconImage != null)
            {
                iconImage.sprite = icon;
                iconImage.enabled = true;
            }
        }

        slot.button.onClick.RemoveAllListeners();
        slot.button.onClick.AddListener(() => onClick.Invoke());

        if (!itemCounts.ContainsKey(itemType))
            itemCounts[itemType] = 0;

        itemCounts[itemType]++;
        slot.counterText.text = itemCounts[itemType].ToString();

        UpdateTotalCountUI();
    }

    public void RemoveItem(InventoryItemUI.ItemType itemType)
    {
        if (!itemCounts.ContainsKey(itemType)) return;

        itemCounts[itemType] = Mathf.Max(0, itemCounts[itemType] - 1);

        InventorySlot slot = itemSlots.Find(s => s.itemType == itemType);
        if (slot != null && slot.counterText != null)
        {
            slot.counterText.text = itemCounts[itemType].ToString();
        }

        UpdateTotalCountUI();
    }

    private void UpdateTotalCountUI()
    {
        if (inventoryCountText == null) return;

        int total = 0;
        foreach (var val in itemCounts.Values)
            total += val;

        inventoryCountText.text = $"Items: {total}";
    }

    public void ClearAllSlots()
    {
        foreach (var slot in itemSlots)
        {
            Image iconImage = slot.button.transform.Find("Icon")?.GetComponent<Image>();
            if (iconImage != null)
            {
                iconImage.sprite = null;
                iconImage.enabled = false;
            }

            slot.button.onClick.RemoveAllListeners();
            if (slot.counterText != null)
                slot.counterText.text = "0";
        }

        itemCounts.Clear();
        UpdateTotalCountUI();
    }
}