using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class crouchEnemy : MonoBehaviour
{
    void OnTriggerEnter(Collider other) {

        Debug.Log(other.tag);

        if(other.tag== "enemy"){

            

            Animator animator;

            animator = other.GetComponent<Animator>();

            animator.SetBool("agachado", true); 
        
        
        }      
    }
    void OnTriggerExit(Collider other) {

       if(other.tag== "enemy"){

        Animator animator;

        animator = other.GetComponent<Animator>();

        animator.SetBool("agachado", false); 
        
        }      
    }

}
