using UnityEngine;
using UnityEngine.UI;

public class QuickBarManager : MonoBehaviour
{
    public Image weaponIcon; 
    public Text ammoCount; 
    public Slider healthBar; 

    public void UpdateQuickBar(Item currentWeapon, int health)
    {
        if (currentWeapon != null && weaponIcon != null)
        {
            weaponIcon.sprite = currentWeapon.icon;
            weaponIcon.enabled = true;
        }
        else
        {
            weaponIcon.enabled = false;
        }

        if (ammoCount != null)
        {
            ammoCount.text = currentWeapon?.quantity.ToString() ?? "0";
        }

        if (healthBar != null)
        {
            healthBar.value = Mathf.Clamp(health, 0, healthBar.maxValue);
        }
    }
}