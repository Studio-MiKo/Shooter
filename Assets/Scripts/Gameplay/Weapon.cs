using UnityEngine;

// Абстрактный класс оружия
public abstract class Weapon : MonoBehaviour
{
    // Общие свойства для всех видов оружия
    [SerializeField] protected float damage = 10f;         // Урон
    [SerializeField] protected float attackSpeed = 1f;     // Скорость атаки (атаки в секунду)
    [SerializeField] protected float range = 1f;           // Дальность действия

    // Время до следующей атаки (для контроля скорости)
    protected float nextAttackTime = 0f;

    // Метод для получения урона (доступен для дочерних классов)
    public float GetDamage()
    {
        return damage;
    }

    // Абстрактный метод атаки — каждый тип оружия реализует его по-своему
    public abstract void Attack();

    // Общий метод проверки, можно ли атаковать
    public bool CanAttack()
    {
        return Time.time >= nextAttackTime;
    }

    // Устанавливаем время следующей атаки
    public void UpdateAttackTime()
    {
        nextAttackTime = Time.time + (1f / attackSpeed);
    }
}