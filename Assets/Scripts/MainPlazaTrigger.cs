using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainPlazaTrigger : MonoBehaviour {

    public GameObject mainPlaza;
    public GameObject lowPlaza;
    public int howMany;

	void Start () {
        howMany = 0;
	}

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player" && col.gameObject.name == "Woodle Character")
        {
            if(howMany == 0)
                EnterThis();
            howMany++;
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "Player" && col.gameObject.name == "Woodle Character")
        {
            howMany--;
            if(howMany == 0)
                ExitThis();
        }
    }

    public void EnterThis()
    {
        mainPlaza.gameObject.SetActive(true);
        lowPlaza.gameObject.SetActive(false);
    }

    public void ExitThis()
    {
        mainPlaza.gameObject.SetActive(false);
        lowPlaza.gameObject.SetActive(true);
    }
}
