using UnityEngine;
using System.Collections;

public class ActivateAnimationOnTriggerAndStop : MonoBehaviour
{

    public Animator animator;
    //public string booleanactivated;
    public int layerint;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == layerint && !other.isTrigger)
        {
            if(animator == null)
                animator = GetComponent<Animator>();
            animator.SetBool("Activated", true);
            StartCoroutine(Stop());
        }
    }

    IEnumerator Stop()
    {
        yield return new WaitForSeconds(0.1f);
        animator.SetBool("Activated", false);
    }

}
