using UnityEngine;
using System.Collections;

public class ActivateAtDistanceAndActivate : MonoBehaviour {

    public GameObject object1;
    public GameObject objecttoactivate;

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && other.gameObject.name == "Woodle Character")
        {
            if(objecttoactivate != null)
                objecttoactivate.SetActive(false);
            if (object1 != null)
                object1.SetActive(true);
        }


    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player" && other.gameObject.name == "Woodle Character")
        {
            if (objecttoactivate != null)
                objecttoactivate.SetActive(true);
            if (object1 != null)
                object1.SetActive(false);
        }


    }



}
