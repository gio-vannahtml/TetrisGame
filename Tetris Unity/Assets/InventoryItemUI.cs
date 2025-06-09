using UnityEngine;

public class InventoryItemUI : MonoBehaviour
{
    public enum ItemType { Bombastic, Crusher, Tractor, ColorPopper }
    public ItemType itemType;

    public GridScript gridScript;

    public void UseItem()
    {
        if (gridScript == null)
        {
            Debug.LogWarning("GridScript is not assigned!");
            return;
        }

        switch (itemType)
        {
            case ItemType.Bombastic:
                gridScript.UseBombastic();
                break;
            case ItemType.Crusher:
                gridScript.UseCrusher();
                break;
            case ItemType.Tractor:
                gridScript.UseTractor();
                break;
            case ItemType.ColorPopper:
                gridScript.UseColorPopper();
                break;
        }

        InventoryUI inventory = FindFirstObjectByType<InventoryUI>();
        if (inventory != null)
        {
            inventory.RemoveItem(itemType); // update counter
        }

        Debug.Log($"{itemType} used!");
    }
}