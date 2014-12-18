using UnityEngine;
using System.Collections;

public class GuyantAnimator : MonoBehaviour {

    protected Animator animator;
    EnemyScript enemyScript;
    EnemyHealth enemyHealth;

	// Use this for initialization
    void Start()
    {

        animator = GetComponent<Animator>();
        enemyScript = GetComponent<EnemyScript>();
        enemyHealth = GetComponent<EnemyHealth>();

    }

	// Update is called once per frame
    void Update()
    {
        if (enemyScript.walking)
        {
            animator.SetBool("Walking", true);
        }
        else
        {
            animator.SetBool("Walking", false);
        }

        if (enemyScript.attacking)
        {
            animator.SetBool("Attack", true);
        }
        else
        {
            animator.SetBool("Attack", false);
        }

        if (enemyHealth.isDead)
        {
            animator.SetBool("Dead", true);
        }
        else
        {
            animator.SetBool("Dead", false);
        }
    }
}
