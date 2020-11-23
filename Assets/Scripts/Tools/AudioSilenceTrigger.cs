using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioSilenceTrigger : MonoBehaviour {

    float actualValue;
    bool inSilenceZone;
    PauseScreen ps;

    private void Start()
    {
        ps = GameObject.FindWithTag("Pause").GetComponent<PauseScreen>();
    }

    private void Update()
    {
        if (inSilenceZone)
        {
            foreach (AudioMixer am in ps.audioMixers) {
                am.GetFloat("musicVol", out actualValue);
                if (actualValue != -80f)
                    am.SetFloat("musicVol", -80f);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == PlayerManager.GetMainPlayer().gameObject)
        {
            StopCoroutine("BreakSilence");
            StartCoroutine("InitiateSilence");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == PlayerManager.GetMainPlayer().gameObject)
        {
            StopCoroutine("InitiateSilence");
            StartCoroutine("BreakSilence");
        }
    }

    IEnumerator InitiateSilence()
    {
        for (float i = 0f; i <= 1f; i += (1f / 200f))
        {
            foreach (AudioMixer am in ps.audioMixers)
            {
                am.GetFloat("musicVol", out actualValue);
                am.SetFloat("musicVol", Mathf.Lerp((-80f + (PlayerPrefs.GetFloat("musicVolume", 8f) * 10f)), -80f, i));
            }
            yield return null;
        }
        inSilenceZone = true;
    }

    IEnumerator BreakSilence()
    {
        inSilenceZone = false;
        for (float i = 0f; i <= 1f; i += (1f / 200f))
        {
            foreach (AudioMixer am in ps.audioMixers)
            {
                am.SetFloat("musicVol", Mathf.Lerp(-80f, (-80f + (PlayerPrefs.GetFloat("musicVolume", 8f) * 10f)), i));
            }
            yield return null;
        }
    }
}
