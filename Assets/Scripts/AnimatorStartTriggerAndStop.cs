using UnityEngine;
using System.Collections;

public class AnimatorStartTriggerAndStop : MonoBehaviour
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
            StartCoroutine(Bounce());
        }
    }

    IEnumerator Bounce()
    {
        yield return new WaitForSeconds(0.1f);
        animator.SetBool("Activated", false);
    }
   
}
