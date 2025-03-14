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
    }

    public void UpdateHealthBar(int currentHealth)
    {
        if (healthBar != null)
        {
            // Устанавливаем значение слайдера в диапазоне [0, 1]
            healthBar.value = Mathf.Clamp01(currentHealth / 100f);

            // Дополнительно: можно добавить визуальные эффекты при низком здоровье
            if (currentHealth <= 25)
            {
                healthBar.fillRect.GetComponent<Image>().color = Color.red; // Красный цвет при низком здоровье
            }
            else if (currentHealth <= 50)
            {
                healthBar.fillRect.GetComponent<Image>().color = Color.yellow; // Желтый цвет при среднем здоровье
            }
            else
            {
                healthBar.fillRect.GetComponent<Image>().color = Color.green; // Зеленый цвет при высоком здоровье
            }
        }
    }
}