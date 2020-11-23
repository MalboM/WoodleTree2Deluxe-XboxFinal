using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class HDRumbleMain : MonoBehaviour
{
    [HideInInspector] public static HDRumbleMain singleton;
    
    void Awake()
    {
        if (singleton != null)
        {
            enabled = false;
            Destroy(this);
            return;
        }

        singleton = this;
    }

    public static void PlayVibrationPreset(int playerID, string presetName, float intensityMultiplier, int motorIndex, float duration)
    {
        if (singleton == null)
        {
            Debug.Log("Singleton is null...");
            return;
        }
        else
        {
            if (PlayerPrefs.GetInt("Vibration", 1) == 1)
                singleton._PlayVibrationPreset(playerID, presetName, intensityMultiplier, motorIndex, duration);
        }
    }

    void _PlayVibrationPreset(int playerID, string presetName, float intensityMultiplier, int motorIndex, float duration)
    {
        if (Time.timeScale != 0f && PlayerPrefs.GetInt("Vibration", 1) == 1)
        {
            foreach (Joystick j in ReInput.players.GetPlayer(playerID).controllers.Joysticks)
            {
                if (!j.supportsVibration)
                    continue;
                if (j.vibrationMotorCount > 0)
                    j.SetVibration(motorIndex, intensityMultiplier/2f, duration);
            }
        }
    }
}
