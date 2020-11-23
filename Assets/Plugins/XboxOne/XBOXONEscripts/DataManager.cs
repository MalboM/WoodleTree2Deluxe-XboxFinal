using UnityEngine;
using System.Collections;

public class Language
{
    public string key, desc;
    public Language(string _key, string _desc)
    {
        key = _key;
        desc = _desc;
    }
}

public static class DataManager
{
    //
    static public XOneEventsManager xOneEventsManager;

    //tutti i dati del gioco!
    static public bool isAchievInitialized = false;
    static public bool isSuspended = false, isPaused = false;
    static public int lastXONEUserID;
    static public bool isOnSignScreen;
    static public bool userIsLogged;


    //loading errors
    static public bool hadErrorWhileLoading;
    static public bool CheckForLoadingError(object o)
    {
#if UNITY_XBOXONE
        Debug.Log("CHECK FOR ERROR LOADING");
        if (o == null)
        {
            Debug.Log("ERROR WHILE LOADING");
            hadErrorWhileLoading = true;
            return true;
        }
#endif
        return false;
    }

    static public void Start()
    {

    }

    static public void SaveGame()
    {

    }

    static public void LoadGame()
    {

    }


}
