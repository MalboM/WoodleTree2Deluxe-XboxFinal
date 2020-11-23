using UnityEngine;
using System.Collections;

public class ActivateAtDistanceAndUnloadLevel : MonoBehaviour {

    
    public GameObject leveltoactivate;

    public GameObject leveltounload;

    public string level;
    /*
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && other.gameObject.name == "Woodle Character")
        {
            OnEnter();
        }


    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player" && other.gameObject.name == "Woodle Character")
        {
            OnExit();
        }
    }

    public void OnEnter()
    {
        if (leveltounload != null)
            leveltounload.SetActive(true);
        if (leveltoactivate !=null)
            leveltoactivate.SetActive(false);
    }

    public void OnExit()
    {
        leveltounload = GameObject.Find(level);
        if (leveltoactivate != null)
            leveltoactivate.SetActive(true);
        if (leveltounload != null)
            leveltounload.SetActive(false);
    }*/
}
