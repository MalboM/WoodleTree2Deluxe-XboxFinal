using UnityEngine;
using System.Collections;

public class NaturalEnemyAttacking : MonoBehaviour {

    public GameObject animatorobject;
    public bool enter;
    Animator animator;
    

    void Awake()
    {

        animator = animatorobject.GetComponent<Animator>();
    }

        void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 10)

        {


            enter = true;
            animator.SetFloat("Stop", 0.1f);
            animator.SetFloat("Speed", 0.5f);
            animator.SetBool("Attack", true);
        }
    }


    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 10)

        {


            enter = false;
            animator.SetFloat("Stop", 1.0f);
            animator.SetFloat("Speed", 0.3f);
            animator.SetBool("Attack", false);
        }
    }
}
