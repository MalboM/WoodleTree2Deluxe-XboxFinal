using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySoundManager : MonoBehaviour {

    static EnemySoundManager singleton;

    AudioSource[] ouchSources;
    AudioSource curOuchSource;
    public GameObject ouchSourcesObj;
    public AudioClip[] ouchSounds;
    public AudioClip[] bigOuchSounds;
    int ouchIterator = 0;

    void Awake()
    {
        if (singleton != null)
        {
            enabled = false;
            Destroy(this);
            return;
        }
        singleton = this;

        ouchSources = ouchSourcesObj.GetComponents<AudioSource>();

    }

    public static void PlayOuchSound()
    {
        if (singleton == null)
        {
            Debug.LogError("Singleton is null! Aborting.");
            return;
        }

        singleton._PlayOuchSound();
    }

    void _PlayOuchSound()
    {
        curOuchSource = ouchSources[ouchIterator];
        curOuchSource.Stop();

        curOuchSource.clip = ouchSounds[Random.Range(0, ouchSounds.Length)];
        curOuchSource.PlayDelayed(0f);

        ouchIterator++;
        if (ouchIterator >= ouchSources.Length)
            ouchIterator = 0;
    }

    public static void PlayBigOuchSound()
    {
        if (singleton == null)
        {
            Debug.LogError("Singleton is null! Aborting.");
            return;
        }

        singleton._PlayBigOuchSound();
    }

    void _PlayBigOuchSound()
    {
        curOuchSource = ouchSources[ouchIterator];
        curOuchSource.Stop();

        curOuchSource.clip = bigOuchSounds[Random.Range(0, bigOuchSounds.Length)];
        curOuchSource.PlayDelayed(0f);

        ouchIterator++;
        if (ouchIterator >= ouchSources.Length)
            ouchIterator = 0;
    }
}
