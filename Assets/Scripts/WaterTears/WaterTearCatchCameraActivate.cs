using UnityEngine;
using System.Collections;

public class WaterTearCatchCameraActivate : MonoBehaviour
{

    public string cameratofind;

    public GameObject watertear;
    public int watertearactivated;
    public string vasetofind;

    void OnEnable() {
        if (PlayerPrefs.GetInt(vasetofind) > 0)
            DeactivateTear();
        else
            ActivateTear();
    }

    private void Start()
    {
        if (PlayerPrefs.GetInt(vasetofind) > 0)
        {
            DeactivateTear();
        }
        else
        {
            ActivateTear();
        }
    }

    void OnTriggerEnter(Collider other) 
    {

		if (other.gameObject == PlayerManager.GetMainPlayer().gameObject && !PlayerManager.GetMainPlayer().inButtonCutscene && PlayerPrefs.GetInt(vasetofind) < 1) 
        {

            this.GetComponent<AudioSource>().Play();

            this.gameObject.GetComponentInChildren<VibrateInRange>(true).CollectedTear();

            DeactivateTear();

            WaterTearManager.ShowCutscene(this);
        }
    }

    public void ActivateTear()
    {
        if(watertear != null && !watertear.activeInHierarchy)
        {
            watertear.SetActive(true);
        }
    }

    public void DeactivateTear()
    {
        if (watertear != null && watertear.activeInHierarchy)
        {
            watertear.SetActive(false);
            //Posso fare un check aggiuntivo per verificare che la lacrima sia stata presa?
            WaterTearManager.UpdateTears();
        }
    }

}