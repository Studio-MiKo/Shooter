using UnityEngine;

public class Health : MonoBehaviour
{
    public float maxHealth = 100f; 
    public float currentHealth; 

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        // Если здоровье <=0, уничтожаем объект
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // Метод смерти (например, уничтожение объекта)
    private void Die()
    {
        Destroy(gameObject);
    }
}