using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level5EntranceTrigger : MonoBehaviour
{
    public Collider colliderToActivate;
    public LoadLevelAdditive lla;
    GameObject character;

    [HideInInspector] public bool keepOn;
    [HideInInspector] public bool isInsideThis;

    private void Start()
    {
        character = PlayerManager.GetMainPlayer().gameObject;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == character)
        {
                Debug.Log("IN");
            colliderToActivate.enabled = true;
            isInsideThis = true;
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (!keepOn && other.gameObject == character)
        {
                Debug.Log("OUT");
            colliderToActivate.enabled = false;
            if (lla != null)
                lla.lptCollider.enabled = false;
            isInsideThis = false;
        }
    }
}
