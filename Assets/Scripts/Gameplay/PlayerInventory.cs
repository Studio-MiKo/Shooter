using UnityEngine;

public class PlayerInventory : MonoBehaviour 
{
    public Inventory inventory = new Inventory();

    public void PickupItem(Item item) 
    {
        inventory.AddItem(item);
        // bool found = false;

        // foreach (var invItem in inventory.items)
        // {
        //     if (invItem.name == item.name && invItem.type == item.type)
        //     {
        //         invItem.quantity += item.quantity; 
        //         found = true;
        //         break;
        //     }
        // }

        // if (!found)
        // {
        //     inventory.AddItem(item); 
        // }

        // UpdateUI(); // Обновляем UI инвентаря
    }

    // private void UpdateUI()
    // {
    //     GameObject uiManager = FindObjectOfType<InventoryUI>().gameObject;
    //     if (uiManager != null)
    //     {
    //         uiManager.GetComponent<InventoryUI>().UpdateUI();
    //     }
    // }
}