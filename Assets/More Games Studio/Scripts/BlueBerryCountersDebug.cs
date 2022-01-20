using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

#if UNITY_XBOXONE
using ConsoleUtils;
#endif

public class BlueBerryCountersDebug : MonoBehaviour
{

    [Header("UI Text")]
    [SerializeField] Text _text;

    private void Awake()
    {
        if (!_text)
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

        //AGGIORNIAMO IL TESTO IN MODO CHE PRENDA I VALORI DEI COUNTER PREFS
        _text.text = "Blue Berries Total: " + GetBlueBerriesTotal().ToString() + "\n" +
                     "Blue Berries All: " + GetAllBlueBerries().ToString() + "\n" +
                     "Blue Berries Half: " + GetHalfBlueBerries().ToString() + "\n" +
                     "Blue Berries: " + GetBlueBerries().ToString();

        Debug.Log("--- BLUE BERRY DEBUG TEXT UPDATED ---");
    }

    int GetBlueBerriesTotal()
    {
        int blueBerryTotal = PlayerPrefs.GetInt("BlueBerryTotal");
        return blueBerryTotal;
    }

    int GetBlueBerries()
    {
        int blueBerries = PlayerPrefs.GetInt("BlueBerries");
        return blueBerries;
    }


    int GetAllBlueBerries()
    {
        int allBlueBerries = PlayerPrefs.GetInt("AllBlueBerries");
        return allBlueBerries;
    }

    int GetHalfBlueBerries()
    {
        int halfBlueBerries = PlayerPrefs.GetInt("HalfBlueBerries");
        return halfBlueBerries;
    }

}
