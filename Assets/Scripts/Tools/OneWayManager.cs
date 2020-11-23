using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OneWayManager : MonoBehaviour {

    [HideInInspector] public bool currentlyChecking;

    private void Start()
    {
        CheckOneWays();
    }

    public void CheckOneWays()
    {
        foreach(GameObject g in GameObject.FindGameObjectsWithTag("OneWay"))
        {
            foreach (Collider c in g.GetComponentsInChildren<Collider>(true))
            {
            //    Debug.Log(c.name);
                if (c.tag == "OneWay") {
                    //    if (g.GetComponent<Collider>() != null)
                    //    {
                    for (int i = 0; i < 4; i++)
                    {
                        if (PlayerManager.GetPlayer(i) != null)
                        {
                            Physics.IgnoreCollision(c, PlayerManager.GetPlayer(i).GetComponent<Collider>(), true);
                        }
                    }
                }
            //    }
            }
        }
        currentlyChecking = false;
    }
}
