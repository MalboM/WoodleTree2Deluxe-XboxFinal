
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using UnityEngine.UI;


public class HDRumblePlayer : MonoBehaviour
{
#if UNITY_SWITCH

    public int playerID;

    public int sampleSize;
    public int sampleIterate;
    public float samplesPerSec;
    public float overallIntensity = 0.42f;
    float intensityOverallMultiplier;

    public bool displayDebugLogs;

    public TextAsset[] rumblePresets;
    int iterator;

    public Text displayText;
    public Slider intensitySlider;
    public Text intensityText;
    float curIntensity;

    private nn.hid.NpadId npadId = nn.hid.NpadId.Invalid;
    private nn.hid.NpadStyle npadStyle = nn.hid.NpadStyle.Invalid;
    private NpadState npadState = new NpadState();
    private VibrationValue vibrationValue = VibrationValue.Make();
    private VibrationValue outputValue = VibrationValue.Make();
    private const int vibrationDeviceCountMax = 5;
    private int vibrationDeviceCount = 0;
    private VibrationDeviceHandle[] vibrationDeviceHandles = new VibrationDeviceHandle[vibrationDeviceCountMax];
    private VibrationDeviceInfo[] vibrationDeviceInfos = new VibrationDeviceInfo[vibrationDeviceCountMax];
    private VibrationFileInfo curFileInfo = new VibrationFileInfo();
    private VibrationFileParserContext curFileContext = new VibrationFileParserContext();
    private int sampleA;
    private byte[] curFile;

    [HideInInspector] public float vibrationTimer = 0f;
    
    void Start () {
        iterator = 0;
        
        Npad.Initialize();
        Npad.SetSupportedStyleSet(nn.hid.NpadStyle.Handheld | nn.hid.NpadStyle.JoyDual | nn.hid.NpadStyle.FullKey);
        nn.hid.NpadId[] npadIds = { nn.hid.NpadId.Handheld, nn.hid.NpadId.No1, nn.hid.NpadId.No2, nn.hid.NpadId.No3, nn.hid.NpadId.No4};
        Npad.SetSupportedIdType(npadIds);

        intensityOverallMultiplier = 1f;
    }

    private void Update()
    {
        if (displayText.gameObject.activeInHierarchy)
        {
            displayText.text = rumblePresets[iterator].name;
            intensityText.text = (intensitySlider.value / 100f).ToString("F2");
        }

        if(vibrationTimer > 0f)
        {
            if (UpdatePadState())
            {
                vibrationValue.Clear();

                VibrationFile.RetrieveValue(ref vibrationValue, sampleA, ref curFileContext);
                if ((sampleA % sampleSize) == 0) { sampleA++; } // 200 sample / sec
                sampleA += sampleIterate;
                if (sampleA >= curFileInfo.sampleLength) { sampleA = 0; }

                for (int i = 0; i < vibrationDeviceCount; i++)
                {
                    outputValue.Clear();
                    outputValue = VibrationValue.Make(vibrationValue.amplitudeLow * overallIntensity * intensityOverallMultiplier,
                    vibrationValue.frequencyLow * overallIntensity * intensityOverallMultiplier,
                    vibrationValue.amplitudeHigh * overallIntensity * intensityOverallMultiplier,
                    vibrationValue.frequencyHigh * overallIntensity * intensityOverallMultiplier);
                    curIntensity = intensitySlider.value / 100f;

                    if (displayDebugLogs)
                        Debug.Log(curIntensity);
                    /*
                        outputValue = VibrationValue.Make(vibrationValue.amplitudeLow * curIntensity,
                        vibrationValue.frequencyLow * curIntensity,
                        vibrationValue.amplitudeHigh * curIntensity,
                        vibrationValue.frequencyHigh * curIntensity);
                    */
                    Vibration.SendValue(vibrationDeviceHandles[i], outputValue);
                }
            }

            vibrationTimer -= 1f/60f;

            if(vibrationTimer <= 0f)
            {
                if (UpdatePadState())
                {
                    vibrationValue.Clear();
                    sampleA = 0;
                    for (int i = 0; i < vibrationDeviceCount; i++)
                    {
                        outputValue.Clear();
                        outputValue = VibrationValue.Make(0f, 0f, 0f, 0f);
                        Vibration.SendValue(vibrationDeviceHandles[i], outputValue);
                    }
                }
            }
        }
    }

    public void IncreaseIterator()
    {
        iterator++;
        if (iterator >= rumblePresets.Length)
            iterator = 0;
    }

    public void DecreaseIterator()
    {
        iterator--;
        if (iterator < 0)
            iterator = rumblePresets.Length-1;
    }

    public void PlayVibration()
    {
        vibrationValue.Clear();
        curFile = rumblePresets[iterator].bytes;
        Result result;
        result = VibrationFile.Parse(ref curFileInfo, ref curFileContext, curFile, curFile.Length);
        Debug.Assert(result.IsSuccess());
        sampleA = 0;

        vibrationTimer = (curFileInfo.sampleLength - 1) / samplesPerSec;
    }

    public void PlayVibrationPreset(string presetName, float intensityMultiplier)
    {
        if (intensityMultiplier == 0f)
            intensityMultiplier = 1f;
        intensityOverallMultiplier = intensityMultiplier;

        int toUse = -1;
        for(int it = 0; (it < rumblePresets.Length && toUse == -1); it++)
        {
            if (rumblePresets[it].name == presetName)
                toUse = it;
        }

        if (toUse != -1)
        {
            vibrationValue.Clear();
            curFile = rumblePresets[toUse].bytes;
            Result result;
            result = VibrationFile.Parse(ref curFileInfo, ref curFileContext, curFile, curFile.Length);
            Debug.Assert(result.IsSuccess());
            sampleA = 0;
        }

        vibrationTimer = (curFileInfo.sampleLength - 1) / samplesPerSec;

        if (displayDebugLogs)
        {
            if (toUse != -1)
                Debug.Log("PLAYING: " + rumblePresets[toUse].name);
            else
                Debug.Log("Preset: " + presetName + "... was not found!");
        }
    }

    private void GetVibrationDevice(nn.hid.NpadId id, nn.hid.NpadStyle style)
    {
        vibrationValue.Clear();
        for (int i = 0; i < vibrationDeviceCount; i++)
        {
            Vibration.SendValue(vibrationDeviceHandles[i], vibrationValue);
        }

        vibrationDeviceCount = Vibration.GetDeviceHandles(
            vibrationDeviceHandles, vibrationDeviceCountMax, id, style);

        for (int i = 0; i < vibrationDeviceCount; i++)
        {
            Vibration.InitializeDevice(vibrationDeviceHandles[i]);
            Vibration.GetDeviceInfo(ref vibrationDeviceInfos[i], vibrationDeviceHandles[i]);
        }
    }

    private bool UpdatePadState()
    {
        nn.hid.NpadStyle handheldStyle = Npad.GetStyleSet(nn.hid.NpadId.Handheld);
        NpadState handheldState = new NpadState();
        nn.hid.NpadStyle no1Style = Npad.GetStyleSet(nn.hid.NpadId.No1);
        NpadState no1State = new NpadState();
        if (playerID == 0)
        {
            if (handheldStyle != nn.hid.NpadStyle.None)
            {
                Npad.GetState(ref handheldState, nn.hid.NpadId.Handheld, handheldStyle);
                if (handheldState.buttons != nn.hid.NpadButton.None)
                {
                    if ((npadId != nn.hid.NpadId.Handheld) || (npadStyle != handheldStyle))
                    {
                        this.GetVibrationDevice(nn.hid.NpadId.Handheld, handheldStyle);
                    }
                    npadId = nn.hid.NpadId.Handheld;
                    npadStyle = handheldStyle;
                    npadState = handheldState;
                    return true;
                }
            }

            if (no1Style != nn.hid.NpadStyle.None)
            {
                Npad.GetState(ref no1State, nn.hid.NpadId.No1, no1Style);
                if (no1State.buttons != nn.hid.NpadButton.None)
                {
                    if ((npadId != nn.hid.NpadId.No1) || (npadStyle != no1Style))
                    {
                        this.GetVibrationDevice(nn.hid.NpadId.No1, no1Style);
                    }
                    npadId = nn.hid.NpadId.No1;
                    npadStyle = no1Style;
                    npadState = no1State;
                    return true;
                }
            }
        }

        nn.hid.NpadStyle no2Style = Npad.GetStyleSet(nn.hid.NpadId.No2);
        NpadState no2State = new NpadState();
        if (playerID == 1)
        {
            if (no2Style != nn.hid.NpadStyle.None)
            {
                Npad.GetState(ref no2State, nn.hid.NpadId.No2, no2Style);
                if (no2State.buttons != nn.hid.NpadButton.None)
                {
                    if ((npadId != nn.hid.NpadId.No2) || (npadStyle != no2Style))
                    {
                        this.GetVibrationDevice(nn.hid.NpadId.No2, no2Style);
                    }
                    npadId = nn.hid.NpadId.No2;
                    npadStyle = no2Style;
                    npadState = no2State;
                    return true;
                }
            }
        }

        nn.hid.NpadStyle no3Style = Npad.GetStyleSet(nn.hid.NpadId.No3);
        NpadState no3State = new NpadState();
        if (playerID == 2) { 
            if (no3Style != nn.hid.NpadStyle.None)
            {
                Npad.GetState(ref no3State, nn.hid.NpadId.No3, no3Style);
                if (no3State.buttons != nn.hid.NpadButton.None)
                {
                    if ((npadId != nn.hid.NpadId.No3) || (npadStyle != no3Style))
                    {
                        this.GetVibrationDevice(nn.hid.NpadId.No3, no3Style);
                    }
                    npadId = nn.hid.NpadId.No3;
                    npadStyle = no3Style;
                    npadState = no3State;
                    return true;
                }
            }
        }

        nn.hid.NpadStyle no4Style = Npad.GetStyleSet(nn.hid.NpadId.No4);
        NpadState no4State = new NpadState();
        if (playerID == 3)
        {
            if (no4Style != nn.hid.NpadStyle.None)
            {
                Npad.GetState(ref no4State, nn.hid.NpadId.No4, no4Style);
                if (no4State.buttons != nn.hid.NpadButton.None)
                {
                    if ((npadId != nn.hid.NpadId.No4) || (npadStyle != no4Style))
                    {
                        this.GetVibrationDevice(nn.hid.NpadId.No4, no4Style);
                    }
                    npadId = nn.hid.NpadId.No4;
                    npadStyle = no4Style;
                    npadState = no4State;
                    return true;
                }
            }
        }

        if (playerID == 0 && (npadId == nn.hid.NpadId.Handheld) && (handheldStyle != nn.hid.NpadStyle.None))
        {
            npadId = nn.hid.NpadId.Handheld;
            npadStyle = handheldStyle;
            npadState = handheldState;
        }
        else if (playerID == 0 && (npadId == nn.hid.NpadId.No1) && (no1Style != nn.hid.NpadStyle.None))
        {
            npadId = nn.hid.NpadId.No1;
            npadStyle = no1Style;
            npadState = no1State;
        }
        else if (playerID == 1 && (npadId == nn.hid.NpadId.No2) && (no2Style != nn.hid.NpadStyle.None))
        {
            npadId = nn.hid.NpadId.No2;
            npadStyle = no2Style;
            npadState = no2State;
        }
        else if (playerID == 2 && (npadId == nn.hid.NpadId.No3) && (no3Style != nn.hid.NpadStyle.None))
        {
            npadId = nn.hid.NpadId.No3;
            npadStyle = no3Style;
            npadState = no3State;
        }
        else if (playerID == 3 && (npadId == nn.hid.NpadId.No4) && (no4Style != nn.hid.NpadStyle.None))
        {
            npadId = nn.hid.NpadId.No4;
            npadStyle = no4Style;
            npadState = no4State;
        }
        else
        {
            npadId = nn.hid.NpadId.Invalid;
            npadStyle = nn.hid.NpadStyle.Invalid;
            npadState.Clear();
            return false;
        }
        return true;
    }
#endif
}
