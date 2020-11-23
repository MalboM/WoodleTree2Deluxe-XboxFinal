using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeShakeManager : MonoBehaviour
{
    static TreeShakeManager singleton;

    public AudioSource[] audioSources;
    int audioIterator;

    public AudioClip[] treeSounds;

    public GameObject[] treeParticles;
    private ParticleSystem.EmissionModule[] treeEMs;
    int pfxIterator;

    public GameObject[] treeParticlesPink;
    private ParticleSystem.EmissionModule[] treePinkEMs;
    int pfxPinkIterator;

    public GameObject[] treeParticlesYellow;
    private ParticleSystem.EmissionModule[] treeYellowEMs;
    int pfxYellowIterator;

    public GameObject[] treeParticlesSnow;
    private ParticleSystem.EmissionModule[] treeSnowEMs;
    int pfxSnowIterator;

    public enum FoliageColour { green, pink, yellow, snow };
    FoliageColour foliageColour;

    public float volume;

    void Awake()
    {
        if (singleton != null)
        {
            enabled = false;
            Destroy(this);
            return;
        }
        singleton = this;
        
        treeEMs = new ParticleSystem.EmissionModule[treeParticles.Length];
        for(int t = 0; t < treeParticles.Length; t++)
        {
            treeEMs[t] = treeParticles[t].GetComponent<ParticleSystem>().emission;
        }
        treePinkEMs = new ParticleSystem.EmissionModule[treeParticlesPink.Length];
        for (int t = 0; t < treeParticlesPink.Length; t++)
        {
            treePinkEMs[t] = treeParticlesPink[t].GetComponent<ParticleSystem>().emission;
        }
        treeYellowEMs = new ParticleSystem.EmissionModule[treeParticlesYellow.Length];
        for (int t = 0; t < treeParticlesYellow.Length; t++)
        {
            treeYellowEMs[t] = treeParticlesYellow[t].GetComponent<ParticleSystem>().emission;
        }
        treeSnowEMs = new ParticleSystem.EmissionModule[treeParticlesSnow.Length];
        for (int t = 0; t < treeParticlesSnow.Length; t++)
        {
            treeSnowEMs[t] = treeParticlesSnow[t].GetComponent<ParticleSystem>().emission;
        }
    }

    public static void ShakeTree(GameObject tree, MeshRenderer meshRenderer)
    {
        if (singleton == null)
        {
            Debug.LogError("Singleton is null! Aborting.");
            return;
        }
        singleton._ShakeTree(tree, meshRenderer);
    }

    void _ShakeTree(GameObject tree, MeshRenderer meshRenderer)
    {
        StartCoroutine(TreeShake(tree, meshRenderer));
    }
    
    IEnumerator TreeShake(GameObject tree, MeshRenderer meshRenderer)
    {
        audioSources[audioIterator].Stop();
        audioSources[audioIterator].clip = treeSounds[Random.Range(0, treeSounds.Length)];
        audioSources[audioIterator].pitch = Random.Range(1f, 2f);
        audioSources[audioIterator].volume = volume;
        audioSources[audioIterator].gameObject.transform.position = tree.transform.position;
        audioSources[audioIterator].PlayDelayed(0f);
        
        audioIterator++;
        if (audioIterator >= audioSources.Length)
            audioIterator = 0;

        if (tree.transform.parent.name.Contains("Pink") || tree.transform.parent.parent.name.Contains("Pink"))
            foliageColour = FoliageColour.pink;
        else if (tree.transform.parent.name.Contains("Yellow") || tree.transform.parent.parent.name.Contains("Yellow"))
            foliageColour = FoliageColour.yellow;
        else if (tree.transform.parent.name.Contains("Snow") || tree.transform.parent.parent.name.Contains("Snow"))
            foliageColour = FoliageColour.snow;
        else
            foliageColour = FoliageColour.green;


        switch (foliageColour) {

            case FoliageColour.pink:
                    treeParticlesPink[pfxPinkIterator].transform.position = tree.transform.position;
                    treePinkEMs[pfxPinkIterator].enabled = true;
                    treeParticlesPink[pfxPinkIterator].GetComponent<ParticleSystem>().Play();
                    StartCoroutine(DeactivateParticle(treeParticlesPink[pfxPinkIterator], treePinkEMs[pfxPinkIterator]));
                    pfxPinkIterator++;
                    if (pfxPinkIterator >= treeParticlesPink.Length)
                        pfxPinkIterator = 0;
                break;

            case FoliageColour.yellow:
                    treeParticlesYellow[pfxYellowIterator].transform.position = tree.transform.position;
                    treeYellowEMs[pfxYellowIterator].enabled = true;
                    treeParticlesYellow[pfxYellowIterator].GetComponent<ParticleSystem>().Play();
                    StartCoroutine(DeactivateParticle(treeParticlesYellow[pfxYellowIterator], treeYellowEMs[pfxYellowIterator]));
                    pfxYellowIterator++;
                    if (pfxYellowIterator >= treeParticlesYellow.Length)
                        pfxYellowIterator = 0;
                break;

            case FoliageColour.snow:
                    treeParticlesSnow[pfxSnowIterator].transform.position = tree.transform.position;
                    treeSnowEMs[pfxSnowIterator].enabled = true;
                    treeParticlesSnow[pfxSnowIterator].GetComponent<ParticleSystem>().Play();
                    StartCoroutine(DeactivateParticle(treeParticlesSnow[pfxSnowIterator], treeSnowEMs[pfxSnowIterator]));
                    pfxSnowIterator++;
                    if (pfxSnowIterator >= treeParticlesSnow.Length)
                        pfxSnowIterator = 0;
                break;

            case FoliageColour.green:
                    treeParticles[pfxIterator].transform.position = tree.transform.position;
                    treeEMs[pfxIterator].enabled = true;
                    treeParticles[pfxIterator].GetComponent<ParticleSystem>().Play();
                    StartCoroutine(DeactivateParticle(treeParticles[pfxIterator], treeEMs[pfxIterator]));
                    pfxIterator++;
                    if (pfxIterator >= treeParticles.Length)
                        pfxIterator = 0;
                break;
        }

        for (int t = 0; t <= 60; t++)
        {
            meshRenderer.material.SetFloat("_TimeAnimParam", Mathf.Lerp(0f, 1f, t / 60f));
            yield return null;
        }
    }

    public IEnumerator DeactivateParticle(GameObject pObj, ParticleSystem.EmissionModule tempEM)
    {
        yield return new WaitForSeconds(pObj.GetComponent<ParticleSystem>().main.duration + 0.1f);
        tempEM.enabled = false;
    }
}
