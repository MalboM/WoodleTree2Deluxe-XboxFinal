using UnityEngine;
using System.Collections;

public class ActivateMMO : MonoBehaviour {

    // Use this for initialization
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            
         //   ((WoodleConnector)uMMO.get.connector).connectClientFromTrigger();
        }

    }
}
