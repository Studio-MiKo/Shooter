using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public float moveSpeed = 2f; 
    public float attackRange = 2f; 
    public float attackDamage = 10f; 
    public float attackCooldown = 1f; // Задержка между атаками

    private Transform player; 
    private Health enemyHealth; 
    private bool canAttack = true; // Флаг для атаки

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        enemyHealth = GetComponent<Health>();
    }

    private void Update()
    {
        if (player == null) return;

        Vector3 position = transform.position;
        position.y = 0;
        transform.position = position;

        MoveTowardsPlayer();

        if (Vector3.Distance(transform.position, player.position) <= attackRange && canAttack)
        {
            Attack();
        }
    }

    private void MoveTowardsPlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        transform.position += direction * moveSpeed * Time.deltaTime;
    }

    private void Attack()
    {
        Health playerHealth = player.GetComponent<Health>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(attackDamage);
        }

        canAttack = false;
        Invoke("ResetAttack", attackCooldown);
    }

    private void ResetAttack()
    {
        canAttack = true;
    }
}