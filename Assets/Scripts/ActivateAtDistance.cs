using UnityEngine;
using System.Collections;

public class ActivateAtDistance : MonoBehaviour {

    public GameObject object1;


    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && other.gameObject.name == "Woodle Character")
        {
            if(object1 != null)
                object1.SetActive(true);
      
        }


    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player" && other.gameObject.name == "Woodle Character")
        {
            if (object1 != null)
                object1.SetActive(false);
         
        }


    }



}
