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

            }

            if (achievementID == 1)
            {

            }
        }
    }
}
