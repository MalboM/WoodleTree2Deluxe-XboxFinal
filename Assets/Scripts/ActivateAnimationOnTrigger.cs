using UnityEngine;
using System.Collections;

public class ActivateAnimationOnTrigger : MonoBehaviour
{

    public Animator animator;
    public string booleanactivated;
    public int layerint;
    bool activated;
    [HideInInspector] public bool canUseSpline;

    void OnEnable()
    {
        if(animator == null)
            animator = GetComponent<Animator>();

        if (activated && animator.GetBool(booleanactivated) == false)
            ActivatePlant();
    }

    private void OnDisable()
    {
        if (activated && animator.GetBool(booleanactivated) == true)
            animator.SetBool(booleanactivated, false);
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == layerint)
            ActivatePlant();
    }

    void ActivatePlant()
    {
        activated = true;
        animator.SetBool(booleanactivated, true);
        StartCoroutine(WaitToUseSpline());
    }

    IEnumerator WaitToUseSpline()
    {
        yield return new WaitForSeconds(3f);
        canUseSpline = true;
    }
}
