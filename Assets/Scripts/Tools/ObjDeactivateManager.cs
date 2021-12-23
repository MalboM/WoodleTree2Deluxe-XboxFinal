using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjDeactivateManager : MonoBehaviour {

    GameObject[] curGameObjects = new GameObject[16];
    Coroutine[] coroutines = new Coroutine[16];

    [HideInInspector] public bool justActivatedPlazaLow;

    public void ActivateObject(GameObject g)
    {
        int curID = -1;
        for (int a = 0; a < 16; a++)
        {
            if (curGameObjects[a] == g)
            {
                StopCoroutine(coroutines[a]);
                curID = a;
            }
        }
        if (curID == -1)
        {
            for (int a = 0; a < 16 && curID == -1; a++)
            {
                if (curGameObjects[a] == null)
                {
                    curGameObjects[a] = g;
                    curID = a;
                }
            }
        }
        coroutines[curID] = StartCoroutine("Activating", curID);
    }

    IEnumerator Activating(int g)
    {
        //    Debug.Log("ACTIVATING " + curGameObjects[g].name);
        if (curGameObjects[g].transform.childCount > 0)
        {
            foreach (Transform t in curGameObjects[g].transform)
            {
            //    Debug.Log("ACTIVATING " + curGameObjects[g].name + " : " + t.name);
            //    if (t.gameObject != curGameObjects[g])
            //    {
                if (t.childCount > 0)
                {
                    foreach (Transform tr in t)
                    {
                        if (tr.gameObject != t.gameObject)
                        {
                            tr.gameObject.SetActive(true);
                            yield return null;
                        }
                    }
                }
                t.gameObject.SetActive(true);
                yield return null;
            //    }
            }
            curGameObjects[g].gameObject.SetActive(true);
        }
        curGameObjects[g] = null;
        yield return null;
    }

    public void DeactivateObject(GameObject g, GameObject[] keepActive)
    {
        int curID = -1;
        for (int a = 0; a < 16; a++)
        {
            if (curGameObjects[a] == g)
            {
                StopCoroutine(coroutines[a]);
                curID = a;
            }
        }
        if (curID == -1)
        {
            for (int a = 0; a < 16 && curID == -1; a++)
            {
                if (curGameObjects[a] == null)
                {
                    curGameObjects[a] = g;
                    curID = a;
                }
            }
        }
        object[] parms = new object[2] { curID, keepActive };
        coroutines[curID] = StartCoroutine("Deactivating", parms);
    }

    IEnumerator Deactivating(object[] parms)
    {
        int g = (int)parms[0];
        GameObject[] keep = (GameObject[])parms[1];

        bool deactive = true;
        //    Debug.Log("DEACTIVATING " + curGameObjects[g].name);
        if (curGameObjects[g].transform.childCount > 0)
        {
            foreach (Transform t in curGameObjects[g].transform)
            {
                //    Debug.Log("DEACTIVATING " + curGameObjects[g].name + " : " + t.name);
                if (t.gameObject != curGameObjects[g])
                {
                    deactive = true;
                    if (keep != null)
                    {
                        foreach (GameObject go in keep)
                        {
                            if (t.gameObject == go)
                                deactive = false;
                            else
                            {

                            }
                        }
                    }
                    if (deactive)
                    {
                        if (t != null && t.childCount > 0)
                        {
                            foreach (Transform tr in t)
                            {
                                if (t != null && tr != null)
                                {
                                    if (tr.gameObject != t.gameObject)
                                    {
                                        if (curGameObjects[g].name == "External Full")
                                        {
                                            foreach (Transform tra in tr)
                                            {
                                                if (tra.gameObject != tr.gameObject)
                                                {
                                                    tr.gameObject.SetActive(false);
                                                    yield return null;
                                                }

                                            }
                                        }
                                        else
                                        {
                                            tr.gameObject.SetActive(false);
                                            yield return null;
                                        }
                                    }
                                }
                            }
                        }
                        else
                            t.gameObject.SetActive(false);
                    }
                    else
                    {
                        if (!t.gameObject.activeInHierarchy)
                            t.gameObject.SetActive(true);
                    }
                    yield return null;
                }
            }
        }
        curGameObjects[g] = null;
        yield return null;
    }

    IEnumerator WaitToDeactLowExt(GameObject lowPoly)
    {
        yield return new WaitForSeconds(2f);
        DeactivateObject(lowPoly, null);
    }
}
