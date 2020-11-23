using UnityEngine;
using System.Collections;

public class DeactivateAtDistance : MonoBehaviour {

    public GameObject object1;


    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && other.gameObject.name == "Woodle Character")
        {

            object1.SetActive(false);
    
        }


    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player" && other.gameObject.name == "Woodle Character")
        {

            object1.SetActive(true);
       
        }


    }



}
