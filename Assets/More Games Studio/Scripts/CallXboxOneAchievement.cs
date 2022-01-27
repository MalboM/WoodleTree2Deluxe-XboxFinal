using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallXboxOneAchievement : MonoBehaviour
{
    [SerializeField] XONEACHIEVS xboxOneAchievement = XONEACHIEVS.NONE;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.LogError("PLAYER ENTERED CALL ACHIEVEMENT");
            XONEAchievements.SubmitAchievement((int)xboxOneAchievement);
        }
    }
}
