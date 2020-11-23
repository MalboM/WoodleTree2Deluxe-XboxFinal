using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BerryManagerDistance : MonoBehaviour {

    GameObject[] berries;
    GameObject woodle;
    bool[] collectedBerries;
    

    Vector3 woodlePos;
    TPC tpc;

	void Start () {
        foreach(GameObject go in GameObject.FindGameObjectsWithTag("Player"))
        {
            if (go.name == "Woodle Character")
                woodle = go;
        }
        tpc = woodle.GetComponent<TPC>();

        berries = new GameObject[this.transform.childCount];
        collectedBerries = new bool[berries.Length];
        int b = 0;
        foreach(Transform t in transform)
        {
            berries[b] = t.gameObject;
            collectedBerries[b] = false;
            b++;
        }
	}

    void OnEnable()
    {
        StartCoroutine("CheckDistance");
    }

    IEnumerator CheckDistance()
    {
        yield return null;
        yield return null;

        woodlePos = woodle.transform.position;
        for(int c = 0; c < berries.Length; c++)
        {
            if (collectedBerries[c] == false && berries[c] != null && Vector3.Distance(woodlePos, berries[c].transform.position) < 1f)
            {
                collectedBerries[c] = true;
                CollectBerry(berries[c]);
            }
            else
            {
                if (berries[c] == null)
                    Debug.Log(c + " GONE " + this.transform.root);
            }
        }

        StartCoroutine("CheckDistance");
    }

    void CollectBerry(GameObject berry)
    {
        tpc.berryCount += 1;
        BerrySpawnManager.PlayBerryNoise(false);
        if (berry.transform.Find("BlobShadowProjector") != null)
            berry.transform.Find("BlobShadowProjector").gameObject.SetActive(false);
        StartCoroutine(MoveBerry(berry));
    }

    IEnumerator MoveBerry(GameObject berry)
    {
        int berryIt = 0;
        Vector3 origPos = berry.transform.position;
        GameObject bagpack = tpc.dayPack;
        while (berryIt < 40)
        {
            berry.transform.position = Vector3.Slerp(origPos, woodle.transform.position + (bagpack.transform.localPosition.z * (woodle.transform.forward * 1.6f)), (berryIt / 40f));
            berry.transform.localScale = Vector3.one * ((40f - berryIt) / 40f);
            berryIt++;
            yield return null;
        }

     //   if (this.transform.parent.gameObject.GetComponent<ActivateAtDistanceNew2>() != null)
     //       Destroy(this.transform.parent.gameObject.GetComponent<ActivateAtDistanceNew2>());

        berry.gameObject.SetActive(false);
    }
}
