using UnityEngine;
using System.Collections;

public class EnableStatue : MonoBehaviour {

    public int intro2watched;
    public GameObject statue;

    // Use this for initialization
    void Start () {

        intro2watched = PlayerPrefs.GetInt("Intro2Watched", 0);

        if (intro2watched == 1)
            statue.gameObject.SetActive(true);

    }

   
}
