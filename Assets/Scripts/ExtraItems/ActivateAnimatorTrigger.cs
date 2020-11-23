using UnityEngine;
using System.Collections;

public class ActivateAnimatorTrigger : MonoBehaviour
{

    public Animator animator;
    public string booleanactivated;
    public int layerint;

    void Awake()
    {

        animator = GetComponent<Animator>();

    }


    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 14)


            animator.SetBool("Activated", true);

        
    }
}
