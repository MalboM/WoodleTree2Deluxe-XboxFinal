using UnityEngine;
using System.Collections;

public class AchievementTutorial : MonoBehaviour
{
    public int achievementID;


    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (achievementID == 0)
            {
#if !UNITY_EDITOR
                if (SteamManager.Initialized) {                
                SteamUserStats.SetAchievement("The Basics");
                SteamUserStats.StoreStats();
                }
#endif
            }

            if (achievementID == 1)
            {
#if !UNITY_EDITOR
                if (SteamManager.Initialized) {                
                SteamUserStats.SetAchievement("To The Top");
                SteamUserStats.StoreStats();
                }
#endif
            }
        }
    }
}
