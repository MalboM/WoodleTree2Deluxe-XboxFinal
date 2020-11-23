using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeactivateAndActivate : MonoBehaviour {

    public GameObject objecttoactivate;
    public GameObject objecttodeactivate;
    
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && other.gameObject.name == "Woodle Character" )
        {
            EnterTrigger();
        }
    }

    private void OnTriggerExit(Collider other)
    {

        if (other.tag == "Player" && other.gameObject.name == "Woodle Character")
        {
            ExitTrigger();
        }
    }

    public void EnterTrigger()
    {
        if(objecttoactivate != null && !objecttoactivate.activeInHierarchy)
            objecttoactivate.SetActive(true);
        if (objecttodeactivate != null && objecttodeactivate.activeInHierarchy)
            objecttodeactivate.SetActive(false);
    }

    public void ExitTrigger()
    {

        if (objecttoactivate != null && objecttoactivate.activeInHierarchy)
            objecttoactivate.SetActive(false);
        if (objecttodeactivate != null && !objecttodeactivate.activeInHierarchy)
            objecttodeactivate.SetActive(true);
    }
}
