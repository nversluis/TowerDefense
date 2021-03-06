﻿using UnityEngine;
using System.Collections;

public class PlayerAnimator : MonoBehaviour {

    protected Animator animator;


    void Start()
    {

        animator = GetComponent<Animator>();

    }

    void Update()
    {

		if (PlayerController.isDancing) {
			animator.SetBool ("Dancing", true);
			animator.SetBool ("Idle", false);
			animator.SetBool ("AttackingSword1", false);
			animator.SetBool ("AttackingSword2", false);
			animator.SetBool ("AttackingSword3", false);
			animator.SetBool ("MagicAttack", false);
			animator.SetBool ("Jumping", false);
		} else {
			animator.SetBool ("Dancing", false);
			if (PlayerController.idle) {
				animator.SetBool ("Idle", true);

			} else {
				animator.SetBool ("Idle", false);
			}

			if (PlayerController.moving) {
				animator.SetBool ("Walking", true);

			} else {
				animator.SetBool ("Walking", false);
			}

			if (PlayerController.attackingSword1) {
				animator.SetBool ("AttackingSword1", true);
			} else {
				animator.SetBool ("AttackingSword1", false);
			}

			if (PlayerController.attackingSword2) {
				animator.SetBool ("AttackingSword2", true);
			} else {
				animator.SetBool ("AttackingSword2", false);
			}

			if (PlayerController.attackingSword3) {
				animator.SetBool ("AttackingSword3", true);
			} else {
				animator.SetBool ("AttackingSword3", false);
			}




			if (PlayerController.attackMagic1) {
				animator.SetBool ("MagicAttack", true);
			} else {
				animator.SetBool ("MagicAttack", false);
				if (PlayerController.jumping) {
					animator.SetBool ("Jumping", true);
				} else {
					animator.SetBool ("Jumping", false);
				}
			}

		}

		/*
		if (PlayerController.moving  && !(PlayerController.attackingSword1 || PlayerController.attackingSword2 || PlayerController.attackingSword3))
        {
            animator.SetBool("Walking", true);

        }
        else
        {
            animator.SetBool("Walking", false);
        }

		if (PlayerController.moving && (PlayerController.attackingSword1 || PlayerController.attackingSword2 || PlayerController.attackingSword3)) {
			if (!(animator.GetBool ("AttackingSword1") || animator.GetBool ("AttackingSword2"))) {
				animator.SetBool ("AttWalk", true);
			}
			//animator.SetBool("AttackingSword1", false);
			//animator.SetBool("AttackingSword2", false);
		} else {
			animator.SetBool ("AttWalk", false);
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

		}


        if (PlayerController.idle)
        {
            animator.SetBool("Idle", true);

        }
        else
        {
            animator.SetBool("Idle", false);
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
            //animator.SetBool("AttackingMagic2", false);
        }

		if (PlayerController.jumping) {
			animator.SetBool ("Jumping", true);
		}
		else
		{
			animator.SetBool ("Jumping", false);
		}
		*/
    }
}
