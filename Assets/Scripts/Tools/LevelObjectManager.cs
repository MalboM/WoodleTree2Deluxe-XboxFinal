using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelObjectManager : MonoBehaviour {

    [System.Serializable] public class ParentPrefab { public GameObject parent; public GameObject prefab; }
    public ParentPrefab[] parentPrefabs;

    GameObject curPref;
    int counter;

	void Start () {
        StartCoroutine("CreateObjects");
	}

    IEnumerator CreateObjects()
    {
        foreach(ParentPrefab p in parentPrefabs)
        {
            foreach(Transform t in p.parent.transform)
            {
                if(t.gameObject != p.parent.gameObject)
                {
                    curPref = Instantiate(p.prefab, t) as GameObject;
                    curPref.transform.localPosition = Vector3.zero;
                    curPref.transform.localEulerAngles = Vector3.zero;
                    curPref.transform.localScale = Vector3.one;
                    counter++;
                    if (counter >= 4)
                    {
                        counter = 0;
                        yield return null;
                    }
                }
            }
        }
        yield return null;
    }
}
