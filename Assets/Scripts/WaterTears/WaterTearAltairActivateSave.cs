using UnityEngine;
using System.Collections;

public class WaterTearAltairActivateSave : MonoBehaviour
{

    public GameObject waterteartoactivate;

    void Start()
    {
        CheckActiveState();
    }

    public void CheckActiveState()
    {
        if (PlayerPrefs.GetInt(this.gameObject.name) == 0 && PlayerPrefs.GetInt("IntroWatched", 0) == 1)
        {
            if (waterteartoactivate.activeInHierarchy)
                waterteartoactivate.SetActive(false);
        }
        else
        {
            if (!waterteartoactivate.activeInHierarchy)
                waterteartoactivate.SetActive(true);
        }
    }
}
