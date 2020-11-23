using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueBerryIDCreator : MonoBehaviour
{
    [HideInInspector]
    public List<Transform> berries = new List<Transform>();
    
    public ChunkDistanceChecker chunkDistanceChecker;

    void OnEnable()
    {
        int counter = 0;
        bool checking = false;
        bool collected = false;
        berries.Clear();
        string totalString = "";
        Debug.Log("BERRIES - HAS KEY: " + PlayerPrefs.HasKey(this.gameObject.scene.name + "BlueBerry0"));
        foreach (Transform t in this.GetComponentsInChildren<Transform>(true))
        {
            if (t.transform.parent == this.transform)
            {
                checking = false;
                collected = false;
                foreach (Transform t1 in t.GetComponentsInChildren<Transform>(true))
                {
                    if (t1.GetComponentInChildren<BerryCollect>() != null)
                        t1.GetComponentInChildren<BerryCollect>().id = counter;
                    //if (!checking && PlayerPrefs.HasKey("MainPlaza7NewBlueBerry"))
                    if (!checking)
                    {
                   //     if (PlayerPrefs.GetString(this.gameObject.scene.name + "BlueBerry")[counter].ToString() == "1")
                        if(PlayerPrefs.GetInt(this.gameObject.scene.name + "BlueBerry" + counter.ToString()) == 1)
                            collected = true;
                    }
                    checking = true;
                    if (collected)
                        t1.gameObject.SetActive(false);
                }
                berries.Add(t);
                totalString = totalString.Insert(totalString.Length, (PlayerPrefs.GetInt(this.gameObject.scene.name + "BlueBerry" + counter.ToString())).ToString());
                counter++;
            }
        }
        Debug.Log("* BERRIES TOTAL: " + totalString);
        Debug.Log("* SCENE NAME: " + this.gameObject.scene.name);
    }

    public void ReEnableBerries()
    {
        StartCoroutine(SlowReEnable());
    }

    IEnumerator SlowReEnable()
    {
        int iterator = 0;
        foreach (Transform t in this.GetComponentsInChildren<Transform>(true))
        {
            if (t.transform.parent == this.transform)
            {
                if (chunkDistanceChecker && !t.gameObject.activeInHierarchy && chunkDistanceChecker.ShouldBeActivated(t.gameObject))
                    t.gameObject.SetActive(true);
                foreach (Transform t1 in t.GetComponentsInChildren<Transform>(true))
                {
                    if (t1 != t)
                    {
                        if (!t1.gameObject.activeInHierarchy)
                        {
                            t1.gameObject.SetActive(true);
                            iterator++;
                        }
                        if (iterator >= 10)
                        {
                            iterator = 0;
                            yield return null;
                        }
                    }
                }
            }
        }
    }


}
