using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportFromTo : MonoBehaviour {

    public GameObject teleportTo;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
            other.transform.position = teleportTo.transform.position + (teleportTo.transform.forward * 2f);
    }
}
