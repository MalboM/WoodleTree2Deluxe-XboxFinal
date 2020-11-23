using UnityEngine;
using System.Collections;
//using Steamworks;

public class AchievementTutorial : MonoBehaviour
{

 
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
        //    SteamUserStats.SetAchievement("Tutorial Complete!");

        }

        
    }



}
