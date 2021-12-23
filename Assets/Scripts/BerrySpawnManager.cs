using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BerrySpawnManager : MonoBehaviour
{

    static BerrySpawnManager singleton;

    public GameObject[] berries;
    Coroutine[] despawnings;
    int berryIterator;

    public GameObject[] bigberries;
    Coroutine[] bigdespawnings;
    int bigberryIterator;
    
    [SerializeField] private AudioClip[] berrySounds = new AudioClip[4];
    [SerializeField] private AudioClip[] blueBerrySounds = new AudioClip[4];
    public AudioClip berryBigSound;

    public AudioSource[] audioSources;
    int soundIterator = 0;
    int berryIt;
    int prevBerryIt;

    void Awake()
    {
        if (singleton != null)
        {
            enabled = false;
            Destroy(this);
            return;
        }
        singleton = this;

        despawnings = new Coroutine[berries.Length];

        bigdespawnings = new Coroutine[bigberries.Length];
        
        for(int au = 0; au < 6; au++)
        {
            audioSources[au].loop = false;
            audioSources[au].playOnAwake = false;
        }
    }

    /*
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
            singleton._SpawnABerry(PlayerManager.GetMainPlayer().transform.position + (Vector3.forward * 2f));
    }
    */

    public static void SpawnABerry(Vector3 position)
    {
        if (singleton == null)
        {
            Debug.LogError("Singleton is null! Aborting.");
            return;
        }

        singleton._SpawnABerry(position);
    }

    void _SpawnABerry(Vector3 position)
    {
        if(despawnings[berryIterator] != null)
            StopCoroutine(despawnings[berryIterator]);
        
        GameObject currentBerry = berries[berryIterator];
        Rigidbody berryRB = currentBerry.GetComponentInChildren<Rigidbody>(true);

        currentBerry.SetActive(true);
        currentBerry.transform.GetChild(0).gameObject.SetActive(true);
        currentBerry.transform.GetChild(0).localScale = Vector3.one;
        currentBerry.transform.GetChild(0).localPosition = Vector3.zero;
        Collectible bCollectible = currentBerry.GetComponentInChildren<Collectible>(true);
        bCollectible.sphereCol.enabled = true;
        bCollectible.canCollect = true;
        bCollectible.movingBerry = false;

        currentBerry.transform.position = position;

        berryRB.velocity = new Vector3(Random.Range(-5f, 5f), 0f, Random.Range(-5f, 5f));
        berryRB.AddForce(Vector3.up * berryRB.mass * 5f, ForceMode.Impulse);

        despawnings[berryIterator] = StartCoroutine("DespawnBerry", berryIterator);

        berryIterator++;
        if (berryIterator >= berries.Length)
            berryIterator = 0;
    }

    IEnumerator DespawnBerry(int index)
    {
        yield return new WaitForSeconds(10f);

        GameObject currentBerry = berries[index];
        Vector3 berryPos = currentBerry.transform.position;
        for (float s = 0f; s <= 20f; s++)
        {
            currentBerry.transform.GetChild(0).localScale = Vector3.Lerp(Vector3.one, Vector3.one * 0.01f, s / 20f);
            yield return null;
        }

        currentBerry.SetActive(false);
    }


    public static void SpawnABigBerry(Vector3 position)
    {
        if (singleton == null)
        {
            Debug.LogError("Singleton is null! Aborting.");
            return;
        }

        singleton._SpawnABigBerry(position);
    }

    void _SpawnABigBerry(Vector3 position)
    {
        if (bigdespawnings[bigberryIterator] != null)
            StopCoroutine(bigdespawnings[bigberryIterator]);

        GameObject currentBerry = bigberries[bigberryIterator];
        Rigidbody berryRB = currentBerry.GetComponentInChildren<Rigidbody>(true);

        currentBerry.SetActive(true);
        currentBerry.transform.GetChild(0).gameObject.SetActive(true);
        currentBerry.transform.GetChild(0).localScale = Vector3.one;
        currentBerry.transform.GetChild(0).localPosition = Vector3.zero;
        currentBerry.GetComponentInChildren<Collectible>(true).sphereCol.enabled = true;

        currentBerry.transform.position = position;

        berryRB.velocity = new Vector3(Random.Range(-5f, 5f), 0f, Random.Range(-5f, 5f));
        berryRB.AddForce(Vector3.up * berryRB.mass * 5f, ForceMode.Impulse);

        bigdespawnings[bigberryIterator] = StartCoroutine("DespawnBigBerry", bigberryIterator);

        bigberryIterator++;
        if (bigberryIterator >= bigberries.Length)
            bigberryIterator = 0;
    }

    public static void PlayBerryNoise(bool bigBerry)
    {
        if (singleton == null)
        {
            Debug.LogError("Singleton is null! Aborting.");
            return;
        }

        singleton._PlayBerryNoise(bigBerry);
    }

    void _PlayBerryNoise(bool bigBerry)
    {
        if (!bigBerry)
        {
            berryIt = Random.Range(0, 4);
            if (berryIt == prevBerryIt)
            {
                berryIt += 1;
                if (berryIt == 4)
                    berryIt = 0;
            }
            prevBerryIt = berryIt;
            audioSources[soundIterator].clip = berrySounds[berryIt];
        }
        else
            audioSources[soundIterator].clip = berryBigSound;
        audioSources[soundIterator].PlayDelayed(0f);
        soundIterator++;
        if (soundIterator > 5)
            soundIterator = 0;
    }

    public static void PlayBlueBerryNoise(int pID)
    {
        if (singleton == null)
        {
            Debug.LogError("Singleton is null! Aborting.");
            return;
        }

        singleton._PlayBlueBerryNoise(pID);
    }

    void _PlayBlueBerryNoise(int pID)
    {
        berryIt = Random.Range(0, 4);
        if (berryIt == prevBerryIt)
        {
            berryIt += 1;
            if (berryIt == 4)
                berryIt = 0;
        }
        HDRumbleMain.PlayVibrationPreset(pID, "K04_FadingPatter1", 1f, 1, 0.2f);
        prevBerryIt = berryIt;
        audioSources[soundIterator].clip = blueBerrySounds[berryIt];
        audioSources[soundIterator].PlayDelayed(0f);
        soundIterator++;
        if (soundIterator > 5)
            soundIterator = 0;
    }

    IEnumerator DespawnBigBerry(int index)
    {
        yield return new WaitForSeconds(10f);

        GameObject currentBerry = bigberries[index];
        Vector3 berryPos = currentBerry.transform.position;
        currentBerry.transform.SetParent(null);
        for (float s = 0f; s <= 20f; s++)
        {
            currentBerry.transform.GetChild(0).localScale = Vector3.Lerp(Vector3.one, Vector3.one * 0.01f, s / 20f);
            yield return null;
        }
        currentBerry.transform.SetParent(this.transform);

        currentBerry.SetActive(false);
    }
}
