using UnityEngine;
using System.Collections;

public class GwarfAnimator : MonoBehaviour
{

    protected Animator animator;
    EnemyResources enemyResources;
    EnemyHealth enemyHealth;

    // Use this for initialization
    void Start()
    {

        animator = GetComponent<Animator>();
        enemyResources = GetComponent<EnemyResources>();
        enemyHealth = GetComponent<EnemyHealth>();

    }

    // Update is called once per frame
    void Update()
    {
        if (enemyResources.walking)
        {
            animator.SetBool("Walking", true);
        }
        else
        {
            animator.SetBool("Walking", false);
        }

        if (enemyResources.attacking)
        {
            animator.SetBool("Attack", true);
        }
        else
        {
            animator.SetBool("Attack", false);
        }

        if (enemyResources.isDead)
        {
            animator.SetBool("Dead", true);
        }
        else
        {
            animator.SetBool("Dead", false);
        }
    }
}
