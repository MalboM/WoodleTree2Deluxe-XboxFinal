using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCCage : MonoBehaviour
{
    public CageObject cageObject;

    public void CageHit()
    {
        cageObject.CageBroken();
        this.gameObject.GetComponent<Collider>().enabled = false;
        this.gameObject.GetComponent<Animator>().SetBool("Activated", true);
        if (!this.gameObject.activeSelf)
            this.gameObject.SetActive(true);
        if (this.gameObject.activeSelf && this.gameObject.activeInHierarchy)
            StartCoroutine(WaitToDeactivate());
    }

    IEnumerator WaitToDeactivate()
    {
        yield return new WaitForSeconds(4.5f);
        this.gameObject.transform.parent.gameObject.SetActive(false);
    }
}
