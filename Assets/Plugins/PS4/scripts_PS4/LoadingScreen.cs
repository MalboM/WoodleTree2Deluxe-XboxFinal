using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScreen : MonoBehaviour
{
    //
    public GameObject loadObj;


    private void Awake()
    {
        //
        DontDestroyOnLoad(this.gameObject);
#if UNITY_PS4
        PS4Manager.loadingScreen = this;
#endif
    }

    //
    private void OnLevelWasLoaded(int level)
    {
        //
        Display(false);
    }

    private void Start()
    {
        //
        Display(false);
    }

    public void Display(bool on)
    {
        //
        loadObj.SetActive(on);
    }
}
        
