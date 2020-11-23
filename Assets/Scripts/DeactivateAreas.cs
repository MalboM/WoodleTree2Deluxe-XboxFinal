using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeactivateAreas : MonoBehaviour {

    public GameObject mainplaza;
    public GameObject mainplazalow;

    public GameObject externalworld;
    public GameObject externalworldlow;

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && other.gameObject.name == "Woodle Character")
        {
            if (mainplaza == null)
                mainplaza = FindFullLevel("MainPlazaFather");
            if (mainplaza != null)
                mainplaza.SetActive(false);
            if (externalworld == null)
                externalworld = FindFullLevel("External Full");
            if (externalworld != null)
                externalworld.SetActive(false);

            if (mainplazalow == null)
                mainplazalow = FindFullLevel("LevelMainPlazaNoCollidersCombinedFinal");
            if (mainplazalow != null)
                mainplazalow.SetActive(true);
            if (externalworldlow == null)
                externalworldlow = FindFullLevel("");
            if (externalworldlow != null)
                externalworldlow.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {

        if (other.tag == "Player" && other.gameObject.name == "Woodle Character")
        {
            if (mainplaza == null)
                mainplaza = FindFullLevel("MainPlazaFather");
            if (mainplaza != null)
                mainplaza.SetActive(true);
            if (externalworld == null)
                externalworld = FindFullLevel("External Full");
            if (externalworld != null)
                externalworld.SetActive(true);

            if (mainplazalow == null)
                mainplazalow = FindFullLevel("LevelMainPlazaNoCollidersCombinedFinal");
            if (mainplazalow != null)
                mainplazalow.SetActive(false);
            if (externalworldlow == null)
                externalworldlow = FindFullLevel("");
            if (externalworldlow != null)
                externalworldlow.SetActive(false);
        }
    }



    GameObject FindFullLevel(string objName)
    {
        foreach (GameObject g in GameObject.FindGameObjectsWithTag("WholeLevel"))
        {
            if (g.name == objName)
                return g.gameObject;
        }
        return null;
    }
}
