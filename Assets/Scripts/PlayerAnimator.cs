using UnityEngine;
using System.Collections;

public class PlayerAnimator : MonoBehaviour {

    protected Animator animator;


    void Start(){

        animator = GetComponent<Animator>();

}

    void Update()
    {
        Debug.Log(PlayerController.moving);
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
