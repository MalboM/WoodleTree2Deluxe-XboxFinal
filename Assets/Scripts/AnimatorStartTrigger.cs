using UnityEngine;
using System.Collections;

public class AnimatorStartTrigger : MonoBehaviour
{
    
    Animator animator;
   
    void Awake()
    {

        animator = GetComponent<Animator>();
        
    }


    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Player")
        {

            animator.SetBool("Activated", true);
           
        }
    }
}
