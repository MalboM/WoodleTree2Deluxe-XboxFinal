using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueBerryIDCreator : MonoBehaviour
{
    [HideInInspector] public List<Transform> berries = new List<Transform>();
    [HideInInspector] public List<BerryCollect> berryCollects = new List<BerryCollect>();
    [HideInInspector] public bool allCollected;

    public bool collectAll;

    public ChunkDistanceChecker chunkDistanceChecker;

    void OnEnable()
    {
        int counter = 0;
        bool checking = false;
        bool collected = false;
        allCollected = true;
        berries.Clear();

        int amountCollected = 0;
        Debug.LogError("YAN: BlueBerryIDCreator in " + this.gameObject.scene.name + ". HasKey(MainPlaza7NewBlueBerry)? " + PlayerPrefs.HasKey("MainPlaza7NewBlueBerry"));
        foreach (Transform t in this.GetComponentsInChildren<Transform>(true))
        {
            if (t.transform.parent == this.transform)
            {
                checking = false;
                collected = false;
                foreach (Transform t1 in t.GetComponentsInChildren<Transform>(true))
                {
                    if (!checking && PlayerPrefs.HasKey("MainPlaza7NewBlueBerry"))
                    {
                        if (PlayerPrefs.GetString(this.gameObject.scene.name + "BlueBerry" + counter).Contains("1"))
                        {
                            PlayerPrefs.SetString(this.gameObject.scene.name + "BlueBerry" + counter, "1");
                            collected = true;
                            amountCollected++;
                        }
                    }

                    if (t1.GetComponentInChildren<BerryCollect>() != null)
                    {
                        t1.GetComponentInChildren<BerryCollect>().id = counter;

                        if (collected)
                            t1.GetComponentInChildren<BerryCollect>().collected = true;
                        berryCollects.Add(t1.GetComponentInChildren<BerryCollect>());
                    }

                    checking = true;
                    if (collected)
                        t1.gameObject.SetActive(false);
                    else
                        allCollected = false;
                }
                berries.Add(t);
                counter++;
            }
        }
        Debug.LogError("YAN: BlueBerryIDCreator in " + this.gameObject.scene.name + ". BlueBerries collected: " + amountCollected.ToString());
    }

    private void Update()
    {
        //I DO NOT UNDERSTAND WHAT DOES THIS DO
        if (collectAll)
        {
            collectAll = false;

            string completeString = "";
            foreach (Transform t in berries)
            {
                completeString = completeString + "1";
                foreach (Transform t1 in t.GetComponentsInChildren<Transform>(true))
                    t1.gameObject.SetActive(false);
            }

            PlayerPrefs.SetString(this.gameObject.scene.name + "BlueBerry", completeString);
        }
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
