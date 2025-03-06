using UnityEngine;

public class PlayerInventory : MonoBehaviour 
{
    public Inventory inventory = new Inventory();

    public void PickupItem(Item item) 
    {
        Debug.Log($"Предмет добавлен в инвентарь: {item.name}"); // Проверяем, вызывается ли метод

        bool found = false;

        foreach (var invItem in inventory.items)
        {
            if (invItem.name == item.name && invItem.type == item.type)
            {
                invItem.quantity += item.quantity; // Увеличиваем количество
                found = true;
                break;
            }
        }

        if (!found)
        {
            inventory.AddItem(item); // Добавляем новый предмет
        }

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