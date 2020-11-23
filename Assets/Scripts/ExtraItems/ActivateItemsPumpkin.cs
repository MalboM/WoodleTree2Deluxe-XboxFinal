using UnityEngine;
using System.Collections;
//using Steamworks;

public class ActivateItemsPumpkin : MonoBehaviour
{


    public GameObject itemonwoodle;
    public GameObject masksecond;
    public GameObject maskthird;
    // Use this for initialization
    void Start()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && other.gameObject.name == "Woodle Character")
        {

            this.GetComponent<AudioSource>().Play();
            itemonwoodle.SetActive(true);
            masksecond.SetActive(false);
            maskthird.SetActive(false);
            //SteamUserStats.SetAchievement("Woodle Mask");
        }


    }
}
