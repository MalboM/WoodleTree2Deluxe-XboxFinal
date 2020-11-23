using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoSoundtrackPlay : MonoBehaviour
{
    public AudioSource[] audioSources;

    private void OnEnable()
    {
        StartCoroutine(DelayedPlay());
    }

    IEnumerator DelayedPlay()
    {
        foreach (AudioSource audio in audioSources)
            audio.Stop();
        yield return new WaitForSeconds(1f);

        foreach (AudioSource audio in audioSources)
            audio.PlayDelayed(1f);
    }
}
