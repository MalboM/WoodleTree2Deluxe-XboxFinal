using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChallengeKillPlane : MonoBehaviour
{
    public ChallengePortal portal;
    PauseScreen ps;

    void OnTriggerEnter(Collider col)
    {
        if(col.tag == "Player")
        {
            StartCoroutine(portal.ResetCharacter(col.gameObject.GetComponent<TPC>()));
        }
    }
}
