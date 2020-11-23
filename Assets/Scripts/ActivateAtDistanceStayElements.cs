using UnityEngine;
using System.Collections;

public class ActivateAtDistanceStayElements : MonoBehaviour {

    public GameObject flowerbooster;

    public GameObject blackmug;

    void Start()
    {
        if (blackmug != null) {
            blackmug.SetActive(false);

        }

        if (flowerbooster != null)
        {
            flowerbooster.SetActive(false);

        }
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && other.gameObject.name == "Woodle Character")
        {
            if(blackmug != null)
                blackmug.SetActive(true);
        
        }


        if (other.tag == "Player" && other.gameObject.name == "Woodle Character")
        {
            if (flowerbooster != null)
                flowerbooster.SetActive(true);

        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player" && other.gameObject.name == "Woodle Character")
        {
            if (blackmug != null)
                blackmug.SetActive(false);
         
        }


        if (other.tag == "Player" && other.gameObject.name == "Woodle Character")
        {
            if (flowerbooster != null)
                flowerbooster.SetActive(false);

        }


    }



}
