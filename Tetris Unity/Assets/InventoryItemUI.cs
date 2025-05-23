using UnityEngine;

public class InventoryItemUI : MonoBehaviour
{
    public enum ItemType { Bombastic, Crusher, Tractor, ColorPopper }
    public ItemType itemType;

    public GridScript gridScript;

    public void UseItem()
    {
        switch (itemType)
        {
            case ItemType.Bombastic:
                gridScript.UseBombastic();
                break;
            case ItemType.Crusher:
                gridScript.UseCrusher();
                break;
            case ItemType.Tractor:
                gridScript.UseTractor(); // 💡 This is the only line for Tractor
                break;
            case ItemType.ColorPopper:
                gridScript.UseColorPopper();
                break;
        }

        Debug.Log($"{itemType} used!");
        Destroy(gameObject); // Optional: remove item after use
    }
}