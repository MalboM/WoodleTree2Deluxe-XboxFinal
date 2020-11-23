using UnityEngine;
using System.Collections;

public class ActivateAtDistanceAndActivateMMO : MonoBehaviour {

    public GameObject object1;
    // public GameObject object2;
    // public GameObject object3;
    // public GameObject object4;
    public GameObject objecttoactivate;

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "MultiplayerLoading")
        {

            objecttoactivate.SetActive(false);
            object1.SetActive(true);
        //    object2.SetActive(true);
        //    object3.SetActive(true);
         //   object4.SetActive(true);
        }


    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "MultiplayerLoading")
        {
            objecttoactivate.SetActive(true);
            object1.SetActive(false);
         //   object2.SetActive(false);
         //   object3.SetActive(false);
         //   object4.SetActive(false);
        }


    }



}
