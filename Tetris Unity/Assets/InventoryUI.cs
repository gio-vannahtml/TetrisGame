using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [Header("Assign your 4 item buttons (top to bottom):")]
    public List<Button> itemSlots = new List<Button>();

    private int currentSlot = 0;

    /// <summary>
    /// Adds an item to the next available slot.
    /// </summary>
    public void AddItem(Sprite icon, Action onClick)
    {
        if (currentSlot >= itemSlots.Count)
        {
            Debug.LogWarning("No more inventory slots available.");
            return;
        }

        Button slot = itemSlots[currentSlot];

        // Set icon
        Image iconImage = slot.transform.Find("Icon")?.GetComponent<Image>();
        if (iconImage != null)
        {
            iconImage.sprite = icon;
            iconImage.enabled = true;
        }

        // Set click behavior
        slot.onClick.RemoveAllListeners();
        slot.onClick.AddListener(() => onClick.Invoke());

        currentSlot++;
    }

    /// <summary>
    /// Clears all item slots.
    /// </summary>
    public void ClearAllSlots()
    {
        foreach (var slot in itemSlots)
        {
            Image iconImage = slot.transform.Find("Icon")?.GetComponent<Image>();
            if (iconImage != null)
            {
                iconImage.sprite = null;
                iconImage.enabled = false;
            }

            slot.onClick.RemoveAllListeners();
        }

        currentSlot = 0;
    }
}