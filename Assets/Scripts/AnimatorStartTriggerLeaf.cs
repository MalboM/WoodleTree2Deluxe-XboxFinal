using UnityEngine;
using System.Collections;

public class AnimatorStartTriggerLeaf : MonoBehaviour
{
    
    Animator animator;
   
    void Awake()
    {

        animator = GetComponent<Animator>();
        
    }



    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 14)

        {

            if (other.gameObject.GetComponent<AttackSettings>().activeAttack == true)
            {
                animator.SetBool("Activated", true);
               
            }
        }
    }
}
