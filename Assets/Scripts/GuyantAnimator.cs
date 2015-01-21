using UnityEngine;
using System.Collections;

public class GuyantAnimator : MonoBehaviour {

    protected Animator animator;
    EnemyResources enemyResources;

	// Use this for initialization
    void Start()
    {

        animator = GetComponent<Animator>();
        enemyResources = GetComponent<EnemyResources>();

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
