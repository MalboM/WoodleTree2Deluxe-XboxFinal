using UnityEngine;
using System.Collections;

public class ActivateAtDistanceExtenalWorldNoColliders : MonoBehaviour {

    public GameObject ExternalWorldNoColliders;
    public GameObject ExternalWorld;

    public GameObject loadingareaexternalworld;
    public GameObject loadingareadarkzone;
    public GameObject logsexternalworld;
    public GameObject meshcolliders;

    public bool activated;

    void Update()
    {

       if(loadingareaexternalworld.activeInHierarchy == false) {
            if(activated == false) {
                ExternalWorld = GameObject.Find("WorldExternalCollidersBlocks");
                loadingareadarkzone = GameObject.Find("DarkZoneExternalWorld");
                logsexternalworld = GameObject.Find("LogsExternalWorld");
                meshcolliders = GameObject.Find("MeshCollidersExternalWorld");
                activated = true;
            }

       }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && other.gameObject.name == "Woodle Character")
        {

           // 

            ExternalWorldNoColliders.SetActive(true);
            if (ExternalWorld != null)
                ExternalWorld.SetActive(false);
            if(loadingareadarkzone != null)
                loadingareadarkzone.SetActive(false);
            if (logsexternalworld != null)
                logsexternalworld.SetActive(false);
            if (meshcolliders != null)
                meshcolliders.SetActive(false);
        }


    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player" && other.gameObject.name == "Woodle Character")
        {

            ExternalWorldNoColliders.SetActive(false);
            if(ExternalWorld != null)
                ExternalWorld.SetActive(true);
            if (loadingareadarkzone != null)
                loadingareadarkzone.SetActive(true);
            if (logsexternalworld != null)
                logsexternalworld.SetActive(true);
            if (meshcolliders != null)
                meshcolliders.SetActive(true);
        }


    }



}
