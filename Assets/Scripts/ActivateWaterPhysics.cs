using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateWaterPhysics : MonoBehaviour {

    TPC tpc;

	void Start () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
            other.gameObject.GetComponent<TPC>().waterPhysics = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
            other.gameObject.GetComponent<TPC>().waterPhysics = false;
    }
}
