using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public PlayerInventory playerInventory; // Ссылка на инвентарь игрока
    public Transform gridContainer; // Ссылка на Grid (контейнер для слотов)
    public GameObject itemSlotPrefab; // Префаб одного слота

    private void Start()
    {
        UpdateUI(); 
    }

    public void UpdateUI()
    {
        foreach (Transform child in gridContainer)
        {
            Destroy(child.gameObject);
        }

        foreach (var item in playerInventory.inventory.items)
        {
            GameObject slot = Instantiate(itemSlotPrefab, gridContainer);

            Image icon = slot.transform.Find("Image").GetComponent<Image>();
            if (icon != null && item.icon != null)
            {
                icon.sprite = item.icon; 
                icon.enabled = true; 
            }
            else
            {
                icon.enabled = false; 
            }

            // Находим компонент Text для отображения количества
            Text quantityText = slot.transform.Find("Quantity").GetComponent<Text>();

            if (quantityText != null)
            {
                if (item.quantity > 1)
                {
                    quantityText.text = item.quantity.ToString(); 
                }
                // else
                // {
                //     quantityText.text = ""; // Если количество = 1, не показываем текст
                // }
            }
        }
    }
}