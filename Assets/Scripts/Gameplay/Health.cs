using UnityEngine;

public class Health : MonoBehaviour
{
    public QuickBarManager quickBarManager;
    public float maxHealth = 100f; 
    public float currentHealth; 

    private void Start()
    {
        currentHealth = maxHealth;
        quickBarManager = FindObjectOfType<QuickBarManager>();
        if (quickBarManager == null)
        {
            Debug.LogWarning("QuickBarManager не найден!");
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        UpdateHealthUI();

        // Если здоровье <=0, уничтожаем объект
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void UpdateHealthUI()
    {
        if (quickBarManager != null)
        {
            quickBarManager.UpdateHealthBar((int)currentHealth);
        }
    }

    // Метод смерти (например, уничтожение объекта)
    private void Die()
    {
        Destroy(gameObject);
    }
}