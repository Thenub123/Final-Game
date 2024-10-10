using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class deathTransition : MonoBehaviour
{
    public Movement movement;

    public Animator animator;
    void Update()
    {
        if(movement.isDead == true) {
            animator.SetBool("isDead", true);
        } else {
            animator.SetBool("isDead", false);
        }
        
    }
}
