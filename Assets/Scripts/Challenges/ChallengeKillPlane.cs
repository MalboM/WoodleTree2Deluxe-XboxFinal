using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChallengeKillPlane : MonoBehaviour
{
    public ChallengePortal portal;
    PauseScreen ps;
    TPC mainChara;

    void Start()
    {
        mainChara = PlayerManager.GetMainPlayer();
    }

    void Update()
    {
        if (mainChara.transform.position.y < this.transform.position.y)
            ResetCharacter(mainChara);

    }

    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Player")
            ResetCharacter(col.gameObject.GetComponent<TPC>());
    }

    void ResetCharacter(TPC chara)
    {
        if (!chara.challengeReset)
        {
            StartCoroutine(portal.ResetCharacter(chara));
        }
    }
}
