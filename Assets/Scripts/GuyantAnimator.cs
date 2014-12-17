using UnityEngine;
using System.Collections;

public class GuyantAnimator : MonoBehaviour {

        protected Animator animator;
        EnemyScript enemyScript;

        public bool walking;
	// Use this for initialization
    void Start()
    {

        animator = GetComponent<Animator>();
        enemyScript = GetComponent<EnemyScript>();
    }

	// Update is called once per frame
    void Update()
    {
        walking = enemyScript.walking;
        if (enemyScript.walking)
        {
            animator.SetBool("Walking", true);
        }
        else
        {
            animator.SetBool("Walking", false);
        }
    }
}
