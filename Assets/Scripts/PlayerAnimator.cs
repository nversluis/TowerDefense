using UnityEngine;
using System.Collections;

public class PlayerAnimator : MonoBehaviour {

    protected Animator animator;


    void Start()
    {

        animator = GetComponent<Animator>();

    }

    void Update()
    {
        if (PlayerController.moving)
        {
            animator.SetBool("Walking", true);

        }
        else
        {
            animator.SetBool("Walking", false);
        }

        if (PlayerController.idle)
        {
            animator.SetBool("Idle", true);

        }
        else
        {
            animator.SetBool("Idle", false);
        }

        if (PlayerController.attackingSword1)
        {
            animator.SetBool("AttackingSword1", true);
        }

        else
        {
            animator.SetBool("AttackingSword1", false);
        }


        if (PlayerController.attackingSword2)
        {
            animator.SetBool("AttackingSword2", true);
        }

        else
        {
            animator.SetBool("AttackingSword2", false);
        }

        if (PlayerController.attackingSword3)
        {
            animator.SetBool("AttackingSword3", true);
        }

        else
        {
            animator.SetBool("AttackingSword3", false);
        }

        if (PlayerController.attackMagic1)
        {
            animator.SetBool("AttackingMagic1", true);
        }

        else
        {
            animator.SetBool("AttackingMagic1", false);
        }

        if (PlayerController.attackMagic2)
        {
            animator.SetBool("AttackingMagic2", true);
        }

        else
        {
            animator.SetBool("AttackingMagic2", false);
        }
    }
}
