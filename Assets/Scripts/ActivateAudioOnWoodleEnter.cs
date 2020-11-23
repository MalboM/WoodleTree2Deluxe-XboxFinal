using UnityEngine;
using System.Collections;

public class ActivateAudioOnWoodleEnter : MonoBehaviour
{

   
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player" && other.gameObject.name == "Woodle Character")
        {
            GetComponent<AudioSource>().Play();
        }


    }
}