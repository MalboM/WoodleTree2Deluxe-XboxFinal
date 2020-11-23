using UnityEngine;
using System.Collections;

public class AnimatorStartTriggerTriggered : MonoBehaviour
{
    
    Animator animator;
   
    void Awake()
    {

        animator = GetComponent<Animator>();
        
    }


    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {

            animator.SetBool("Activated", true);
           
        }
    }
}
