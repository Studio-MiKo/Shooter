using UnityEngine;

public class MenuController : MonoBehaviour
{
     public GameObject btnClicked;
     
     public void HideUIElement()
    {
        if (btnClicked != null)
        {
            btnClicked.SetActive(false);
        }
    }
}
