using UnityEngine;
using System.Collections;

public class ActivateAnimationOnTriggerAndStop : MonoBehaviour
{

    public Animator animator;
    //public string booleanactivated;
    public int layerint;

    void Awake()
    {

        animator = GetComponent<Animator>();

    }


    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == layerint && !other.isTrigger)
        {
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
