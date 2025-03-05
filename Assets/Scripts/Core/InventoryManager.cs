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
    }
}