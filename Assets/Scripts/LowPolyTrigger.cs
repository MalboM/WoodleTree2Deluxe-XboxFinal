using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LowPolyTrigger : MonoBehaviour
{

    public GameObject fullObj;
    public GameObject lowPoly;

    public GameObject[] allLowPolyChildren;
    public GameObject[] extLowPolyKeepEnabled;

    public string fullObjName;

    public bool plazaDisable;
    public GameObject fullPlaza;
    public GameObject lowPolyPlaza;

    public StartScreen startScreenScript;
    public PauseScreen pauseScreenScript;

    public ObjDeactivateManager odm;

    [HideInInspector] public bool currentlyInside;

    bool stoppedDeact;

    // Use this for initialization
    void Start()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == PlayerManager.GetMainPlayer().gameObject)
        {
            EnterTrigger();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == PlayerManager.GetMainPlayer().gameObject)
        {
            if (pauseScreenScript.curLPT == this)
            {
                ExitTrigger();
            }
        }
    }

    public void EnterTrigger()
    {
        if (!currentlyInside && !PlayerManager.GetMainPlayer().challengeWarping)
        {
            currentlyInside = true;
            if (fullObj == null)
            {
                foreach (GameObject g in GameObject.FindGameObjectsWithTag("WholeLevel"))
                {
                    if (g.name == fullObjName)
                    {
                        fullObj = g.gameObject;
                    }
                }
            }


            if (lowPoly != null)
            {
                odm.StopCoroutine("WaitToDeactLowExt");
                odm.DeactivateObject(lowPoly, extLowPolyKeepEnabled);
            }

            if (fullObj != null)
            {
                StartCoroutine("WaitToDeactFullExt");
            }
            /*
            foreach (GameObject t in allLowPolyChildren)
            {
                if (t != lowPoly)
                {
                    foreach (GameObject g in extLowPolyKeepEnabled)
                    {
                        if (g == t.gameObject)
                        {
                            t.gameObject.SetActive(true);
                        }
                    }
                }
            }
            */
            if (plazaDisable)
            {
                StopCoroutine("WaitToDeactLow");
                odm.ActivateObject(lowPolyPlaza);
                odm.justActivatedPlazaLow = true;
                StartCoroutine("WaitToDeactFull");
            }
            startScreenScript.curLPT = this;
            pauseScreenScript.curLPT = this;
        }
    }

    public void ExitTrigger()
    {
        if (currentlyInside && !PlayerManager.GetMainPlayer().challengeWarping)
        {
            currentlyInside = false;
            if (fullObj == null)
            {
                foreach (GameObject g in GameObject.FindGameObjectsWithTag("WholeLevel"))
                {
                    if (g.name == fullObjName)
                    {
                        fullObj = g.gameObject;
                    }
                }
            }
            if (fullObj != null)
            {
                StopCoroutine("WaitToDeactFullExt");
                odm.ActivateObject(fullObj);
            }

            if (lowPoly != null)
            {
                odm.StartCoroutine("WaitToDeactLowExt", lowPoly);
            }

            if (plazaDisable)
            {
                StopCoroutine("WaitToDeactFull");
                odm.ActivateObject(fullPlaza);

                if (odm.justActivatedPlazaLow)
                    odm.justActivatedPlazaLow = false;
                StartCoroutine("WaitToDeactLow");
            }
        }
    }

    void Update()
    {
        if(fullPlaza == null)
        {
            if(fullObj == null)
            {
                foreach (GameObject g in GameObject.FindGameObjectsWithTag("WholeLevel"))
                {
                    if (g.name == fullObjName)
                    {
                        fullObj = g.gameObject;
                    }
                }
            }
            if(fullObj != null)
            {
                if (!lowPoly.gameObject.activeSelf && !fullObj.gameObject.activeSelf)
                    fullObj.SetActive(true);
            }
        }
    }

    IEnumerator WaitToDeactFull()
    {
        yield return new WaitForSeconds(2f);
        odm.DeactivateObject(fullPlaza, null);
    }

    IEnumerator WaitToDeactLow()
    {
        yield return new WaitForSeconds(2f);
        if (!odm.justActivatedPlazaLow)
            odm.DeactivateObject(lowPolyPlaza, null);
        else
            odm.justActivatedPlazaLow = false;
    }

    IEnumerator WaitToDeactFullExt()
    {
        yield return new WaitForSeconds(2f);
        odm.DeactivateObject(fullObj, null);
    }
}
