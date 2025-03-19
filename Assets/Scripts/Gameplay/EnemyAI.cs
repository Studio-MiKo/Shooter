using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
     public float chaseDistance = 10f; // Радиус обнаружения игрока
    public float attackDistance = 1.5f; // Радиус атаки
    public float attackDamage = 10f; // Урон от атаки
    public float attackCooldown = 1f; // Задержка между атаками

    private Animator animator;
    private GameObject player;
    private NavMeshAgent agent;
    private Health health; // Система здоровья
    private bool canAttack = true; // Флаг возможности атаки

    void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindWithTag("Player");
        health = GetComponent<Health>();

        // Инициализация скорости
        animator.SetFloat("speed", 0f);
        animator.SetBool("isAttack", false);
        agent.isStopped = true;
    }

     void Update()
    {
         if (health != null && health.currentHealth <= 0)
        {
            Die();
            return;
        }

        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.transform.position);

        if (distance <= chaseDistance)
        {
            if (distance <= attackDistance)
            {
                animator.SetBool("isAttack", true);
                animator.SetFloat("speed", 0f);
                agent.isStopped = true;

                Attack();
            }
            else
            {
                animator.SetBool("isAttack", false);
                animator.SetFloat("speed", 1f);
                agent.isStopped = false;
                agent.SetDestination(player.transform.position);
            }
        }
        else
        {
            animator.SetBool("isAttack", false);
            animator.SetFloat("speed", 0f);
            agent.isStopped = true;
        }
    }

    private void Attack()
    {
        if (!canAttack)
        {
            return;
        }

        Health playerHealth = player.GetComponent<Health>();
        
        if (playerHealth == null)
        {
            Debug.LogError("Компонент Health не найден на игроке!");
            return;
        }

        if (playerHealth != null)
        {
            Debug.Log($"Наносится урон игроку: {attackDamage} единиц.");
            playerHealth.TakeDamage(attackDamage);
        }

        Debug.Log($"Текущее здоровье игрока: {playerHealth.currentHealth:F2}");
        canAttack = false;
        Invoke("ResetAttack", attackCooldown);
    }

    private void ResetAttack()
    {
        Debug.Log("Атака готова к использованию.");
        canAttack = true;
    }

    private void Die()
    {
        // Здесь можно вызвать анимацию смерти
        // animator.SetBool("isDead", true); 

        // После завершения анимации смерти уничтожаем объект
        Destroy(gameObject);
    }
}