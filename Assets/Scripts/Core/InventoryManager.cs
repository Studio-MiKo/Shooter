using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public GameObject fullInventoryUI; 

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            fullInventoryUI.SetActive(!fullInventoryUI.activeSelf);
        }

        if (fullInventoryUI.activeSelf){
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            InventoryUI inventoryUI = FindObjectOfType<InventoryUI>();
            if (inventoryUI != null)
            {
                inventoryUI.UpdateUI();
            }
        }
        else{
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}