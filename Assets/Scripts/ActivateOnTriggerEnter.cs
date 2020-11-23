using UnityEngine;
using System.Collections;

public class ActivateOnTriggerEnter : MonoBehaviour
{

    public GameObject underwaterplane;

    public AudioReverbFilter filter;

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Underwater")
        {
            underwaterplane.SetActive(true);
            filter.reverbPreset = AudioReverbPreset.User;
            filter.room = -100f;
            filter.roomHF = -2000f;
            filter.roomLF = -3000f;
            filter.decayTime = 3f;
            filter.decayHFRatio = 0.3f;
            filter.reflectionsLevel = -10000f;
            filter.reflectionsDelay = 0.02f;
            filter.reverbLevel = 50f;
            filter.reverbDelay = 0.0756f;
            filter.hfReference = 5000f;
            filter.lfReference = 20f;
            filter.diffusion = 100f;
            filter.density = 100f;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Underwater")
        {
            TurnOffWaterEffect();
        }
    }

    public void TurnOffWaterEffect()
    {
        underwaterplane.SetActive(false);
        filter.reverbPreset = AudioReverbPreset.Off;
    }
}