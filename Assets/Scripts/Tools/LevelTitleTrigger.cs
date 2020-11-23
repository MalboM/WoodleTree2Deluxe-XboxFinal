using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTitleTrigger : MonoBehaviour
{
    public string levelName;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == PlayerManager.GetMainPlayer().gameObject)
        {
            PlayerManager.GetMainPlayer().ps.ShowLevelTitle(levelName);
        }
    }
}
