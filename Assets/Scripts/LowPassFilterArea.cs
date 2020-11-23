using UnityEngine;
using System.Collections;

public class LowPassFilterArea : MonoBehaviour {


    private GameObject gamecamera;

    // Use this for initialization
    void Start () {

       
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && other.gameObject.name == "Woodle Character")
        {
            gamecamera = GameObject.Find("Main Camera");
            gamecamera.GetComponent<LowPass>().cutoff = 1800.0f;

        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player" && other.gameObject.name == "Woodle Character")
        {

            gamecamera.GetComponent<LowPass>().cutoff = 20000.0f;

        }
    }

}
