using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

#if UNITY_XBOXONE
using ConsoleUtils;
#endif


public class CheckBlueBerryPref : MonoBehaviour
{
    [Header("UI Text")]
    [SerializeField] Text _text;

    [Header("Berry Value")]
    [SerializeField] int _berryID = 0;
    [SerializeField] string _berryLevelName = "MainPlaza7New";


    void Awake()
    {
        if (_text == null)
            _text = GetComponentInChildren<Text>();
    }

    // Start is called before the first frame update
    void Start()
    {
        UpdateText();
    }

    // Update is called once per frame
    void Update()
    {
        if (XboxOneInput.GetKeyDown(XboxOneKeyCode.GamepadButtonY))
            UpdateText();
    }

    void UpdateText()
    {
        Debug.Log("--- UPDATING BLUE BERRY DEBUG TEXT ---");

        _text.text = "Checking: " + GetBlueBerryPref() + "\n"
                   + "Does berry Pref exisit: " + DoesBlueBerryPrefExisit() + "\n\n"
                   + "Value: " + GetBlueBerryPrefContent();


        Debug.Log("--- BLUE BERRY DEBUG TEXT UPDATED ---");
    }

    string GetBlueBerryPref()
    {
        string blueBerryPref = _berryLevelName + "BlueBerry" + _berryID;

        return blueBerryPref;
    }

    bool DoesBlueBerryPrefExisit()
    {
        bool doesBlueBerryPrefExist = PlayerPrefs.HasKey(GetBlueBerryPref());

        return doesBlueBerryPrefExist;
    }

    string GetBlueBerryPrefContent()
    {
        string blueBerryPrefContent = PlayerPrefs.GetString(GetBlueBerryPref());

        return blueBerryPrefContent;
    }
}
