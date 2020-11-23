using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlazaDisabler : MonoBehaviour
{
    public GameObject plazaFull;
    public GameObject plazaLow;

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == PlayerManager.GetMainPlayer().gameObject)
        {
            if (plazaFull != null)
                plazaFull.SetActive(true);

            if (plazaLow != null)
            {
                plazaLow.SetActive(false);
                foreach (Transform t in plazaLow.GetComponentsInChildren<Transform>(true))
                    t.gameObject.SetActive(false);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject == PlayerManager.GetMainPlayer().gameObject)
        {
            if (plazaFull != null)
                plazaFull.SetActive(false);

            if (plazaLow != null)
            {
                plazaLow.SetActive(true);
                foreach (Transform t in plazaLow.GetComponentsInChildren<Transform>(true))
                    t.gameObject.SetActive(true);
            }
        }
    }
}
