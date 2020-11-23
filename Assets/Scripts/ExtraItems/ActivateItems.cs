using UnityEngine;
using System.Collections;

public class ActivateItemsMask : MonoBehaviour {

  
    public GameObject itemonwoodle;
    public GameObject masksecond;
    public GameObject maskthird;
    // Use this for initialization
    void Start () {
	    
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && other.gameObject.name == "Woodle Character")
        {

            itemonwoodle.SetActive(true);
            masksecond.SetActive(false);
            maskthird.SetActive(false);
        }


    }
}
