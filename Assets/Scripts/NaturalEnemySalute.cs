using UnityEngine;
using System.Collections;

public class NaturalEnemySalute : MonoBehaviour {

    public GameObject animatorobject;
    public bool enter;
    Animator animator;
    public Transform woodle;

    void Awake()
    {

        animator = animatorobject.GetComponent<Animator>();
        woodle = PlayerManager.GetMainPlayer().transform;

    }

        void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 10)

        {


            enter = true;
            //animatorobject.transform.LookAt(woodle);
            animator.SetBool("Salute", true);
        }
    }


    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 10)

        {


            enter = false;
            animator.SetBool("Salute", false);
        }
    }
}
