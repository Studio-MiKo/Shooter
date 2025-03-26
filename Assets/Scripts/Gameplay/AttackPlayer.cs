using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPlayer : MonoBehaviour
{
    Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();  
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            animator.SetBool("atack", true);
        } 
    }

    // Добавьте эту функцию и вызовите её через Animation Event в конце анимации
    public void ResetAttack()
    {
        animator.SetBool("atack", false);
    }
}
