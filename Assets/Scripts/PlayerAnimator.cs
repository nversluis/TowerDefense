using UnityEngine;
using System.Collections;

public class PlayerAnimator : MonoBehaviour {

    protected Animator animator;


    void Start(){

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
    }
}
