using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Rewired;
using Rewired.Demos;
using UnityEngine.EventSystems;
using UnityEngine.Rendering.PostProcessing;

public class OptionsNew : MonoBehaviour
{
    public StartScreen startScreen;
    public PauseScreen pauseScreen;
    public ControlRemappingDemo1 controlRemapping;
    TPC tpc;
    EventSystem es;
    public PostProcessLayer ppLayer;
    public PostProcessLayer[] cutscenePPLayers;
    public PostProcessVolume ppVolume;
    public PostProcessVolume[] cutscenePPVolumes;

    public List<GameObject> runToggleTicks;

    public List<GameObject> keyboardToggleTicks;

    public List<GameObject> remapperBGs;

    int currentResolution = 0;
    Resolution curRes;
    public List<Text> resolutionTexts;

    int currentFSMode = 0;
    FullScreenMode screenMode;
    public List<Text> windowTexts;

    int currentVSync = 1;
    public List<Text> vsyncTexts;

    public List<GameObject> lockFPSTicks;
    bool locked;
    
    public List<Slider> objectSliders;

    public List<Slider> distanceSliders;

    BoolParameter bloomOn;
    BoolParameter aoOn;
    BoolParameter dofOn;

    public List<GameObject> bloomTicks;
    public List<GameObject> aoTicks;
    public List<GameObject> dofTicks;

    public List<Text> aaTexts;
    int aaMode = 0;

    public List<RectTransform> optionsContents;
    public List<GameObject> pauseOptions;
    public Slider pauseOptionScroll;
    public List<GameObject> startOptions;
    public Slider startOptionScroll;
    GameObject prevSel;
    float scrollResult;

    public AudioClip applySound;
    public AudioSource applySource;

    bool dontPlaySound;
    int curLanguage;

    [Header("Options GUI", order = 0)]

    [Header("Ivert X", order = 1)]
    [SerializeField] List<GameObject> invertXTicks;

    [Header("Ivert Y")]
    [SerializeField] List<GameObject> invertYTicks;

    [Header("Vibration")]
    [SerializeField] List<GameObject> vibrationTicks;

    [Header("Anti-Aliasing")]
    [SerializeField] List<GameObject> aATicks;

    [Header("V-Sync")]
    [SerializeField] List<GameObject> vSyncTicks;

    [Header("Run by Default")]
    [SerializeField] List<GameObject> runTicks;

    [Header("Bloom")]
    [SerializeField] List<GameObject> bloomTicks2;

    [Header("Ambient Occlusion")]
    [SerializeField] List<GameObject> ambientOcclusionTicks;

    void Start()
    {
        Debug.Log(gameObject);

        tpc = PlayerManager.GetMainPlayer();
        es = pauseScreen.es;
        QualitySettings.SetQualityLevel(0, true);

        startScreen.optionsNew = this;
        pauseScreen.optionsNew = this;

        bloomOn = ppVolume.profile.GetSetting<Bloom>().enabled;
        aoOn = ppVolume.profile.GetSetting<AmbientOcclusion>().enabled;
        dofOn = ppVolume.profile.GetSetting<DepthOfField>().enabled;

        foreach (PostProcessVolume ppv in cutscenePPVolumes)
            ppv.profile = ppVolume.profile;

        InitializeValues();

        controlRemapping.enabled = false;
        
        applySource.clip = applySound;

        curLanguage = PlayerPrefs.GetInt("Language", 0);
    }
    
    void InitializeValues()
    {
        if (PlayerPrefs.GetInt("RunByDefault", 1) == 0)
            tpc.runByDefault = false;
        else
            tpc.runByDefault = true;
        foreach (GameObject g in runToggleTicks)
            g.SetActive(tpc.runByDefault);

        if (PlayerPrefs.GetInt("Player1KeyboardOnly", 0) == 0)
        {
            PlayerManager.singleton.testingWithKeyboard = false;
            KeyboardOnlyOff();
        }
        else
        {
            PlayerManager.singleton.testingWithKeyboard = true;
            KeyboardOnlyOn();
        }
        foreach (GameObject g in keyboardToggleTicks)
            g.SetActive(PlayerManager.singleton.testingWithKeyboard);

        if (PlayerPrefs.GetInt("Bloom", 1) == 0)
            bloomOn.value = false;
        else
            bloomOn.value = true;
        SetBloom();
        if (PlayerPrefs.GetInt("AO", 1) == 0)
            aoOn.value = false;
        else
            aoOn.value = true;
        SetAO();


        //DIsable DoF
        PlayerPrefs.SetInt("DoF", 0);
        if (PlayerPrefs.GetInt("DoF", 0) == 0)
            dofOn.value = false;
        else
            dofOn.value = true;
        SetDoF();

        aaMode = PlayerPrefs.GetInt("AAMode", 1);
        AAApply();

        SetInitialTextValues();

        if (PlayerPrefs.GetInt("LockFPS", 1) == 0)
        {
            locked = false;
            Application.targetFrameRate = 999;
        }
        else
        {
            locked = true;
            Application.targetFrameRate = Screen.currentResolution.refreshRate;
        }
        foreach (GameObject g in lockFPSTicks)
            g.SetActive(locked);
    }

    public void SetInitialTextValues()
    {
        currentResolution = PlayerPrefs.GetInt("Resolution", Screen.resolutions.Length - 1);
        if (currentResolution >= Screen.resolutions.Length)
        {
            currentResolution = Screen.resolutions.Length - 1;
        }
        SetResolutionValues();

        currentFSMode = PlayerPrefs.GetInt("ScreenMode", 0);
        SetWindowValues();
        dontPlaySound = true;
        WindowApply();

        currentVSync = PlayerPrefs.GetInt("VSync", 1);
        SetVSyncValues();
        dontPlaySound = true;
        VSyncApply();
        
        aaMode = PlayerPrefs.GetInt("AAMode", 1);
        SetAATexts();

        foreach (Slider s in objectSliders)
            s.value = PlayerPrefs.GetInt("ObjectDetails", 4);

        foreach (Slider s in distanceSliders)
            s.value = PlayerPrefs.GetInt("AddedDistance", 4);
    }

    private void Update()
    {
        if (pauseScreen.optionsTab.activeInHierarchy || startScreen.optionsScreen.activeInHierarchy)
        {
            if (PlayerPrefs.GetInt("Language",0) != curLanguage)
            {
                curLanguage = PlayerPrefs.GetInt("Language", 0);
                SetWindowValues();
                SetVSyncValues();
                SetAATexts();
            }

            if (es.currentSelectedGameObject != null)
            {
                if (prevSel != es.currentSelectedGameObject)
                {
                    prevSel = es.currentSelectedGameObject;
                    if (tpc.input.GetAxis("UIV") != 0f && tpc.input.GetAxis("RV") == 0f && es.currentSelectedGameObject != startOptionScroll.gameObject && es.currentSelectedGameObject != pauseOptionScroll.gameObject)
                    {
                        if (pauseScreen.optionsTab.activeInHierarchy)
                        {
                            if (pauseOptions.Contains(es.currentSelectedGameObject))
                            {
                                float minPos = pauseOptions[0].GetComponent<RectTransform>().position.y;
                                float maxPos = pauseOptions[pauseOptions.Count - 1].GetComponent<RectTransform>().position.y;
                                float curPos = es.currentSelectedGameObject.GetComponent<RectTransform>().position.y;
                                scrollResult = (float)Mathf.RoundToInt(Mathf.Lerp(0, 10, (curPos - minPos) / (maxPos - minPos)));
                            }
                        }
                        if (startScreen.optionsScreen.activeInHierarchy)
                        {
                            if (startOptions.Contains(es.currentSelectedGameObject))
                            {
                                float minPos = startOptions[0].GetComponent<RectTransform>().position.y;
                                float maxPos = startOptions[startOptions.Count - 1].GetComponent<RectTransform>().position.y;
                                float curPos = es.currentSelectedGameObject.GetComponent<RectTransform>().position.y;
                                scrollResult = (float)Mathf.RoundToInt(Mathf.Lerp(0, 10, (curPos - minPos) / (maxPos - minPos)));
                            }
                        }
                    }
                 /*   else
                    {
                        if (pauseScreen.optionsTab.activeInHierarchy)
                            Debug.Log(pauseScreen.notMouseOver);
                        scrollResult = -1f;
                    }*/
                }
                else
                {
                    if (tpc.input.GetAxis("MapZoom") != 0f)
                        scrollResult -= tpc.input.GetAxis("MapZoom");
                }
            }
            if (scrollResult != -1f && pauseScreen.optionsTab.activeInHierarchy)
                pauseOptionScroll.value = Mathf.Lerp(pauseOptionScroll.value, scrollResult, 1f/12f);
            if (scrollResult != -1f && startScreen.optionsScreen.activeInHierarchy)
                startOptionScroll.value = Mathf.Lerp(startOptionScroll.value, scrollResult, 1f / 12f);
        }
    }

    public void ToggleRunDefault()
    {
        tpc.runByDefault = !tpc.runByDefault;

        if (tpc.runByDefault)
            PlayerPrefs.SetInt("RunByDefault", 1);
        else
            PlayerPrefs.SetInt("RunByDefault", 0);

        foreach (GameObject g in runToggleTicks)
            g.SetActive(tpc.runByDefault);
    }

    public void ToggleKeyboard()
    {
        PlayerManager.singleton.testingWithKeyboard = !PlayerManager.singleton.testingWithKeyboard;

        if (PlayerManager.singleton.testingWithKeyboard)
        {
            PlayerPrefs.SetInt("Player1KeyboardOnly", 1);
            KeyboardOnlyOn();
        }
        else
        {
            PlayerPrefs.SetInt("Player1KeyboardOnly", 0);
            KeyboardOnlyOff();
        }

        foreach (GameObject g in keyboardToggleTicks)
            g.SetActive(PlayerManager.singleton.testingWithKeyboard);
    }

    void KeyboardOnlyOn()
    {
        ReInput.players.GetPlayer(0).controllers.RemoveController(ControllerType.Joystick, 0);
        bool foundOne = false;
        for (int p = 1; p < ReInput.players.playerCount; p++)
        {
            if (!foundOne && ReInput.players.GetPlayer(p).controllers.joystickCount == 0)
            {
                ReInput.players.GetPlayer(p).controllers.AddController(ReInput.controllers.GetController(ControllerType.Joystick, 0), true);
                foundOne = true;
            }
        }
    }

    void KeyboardOnlyOff()
    {
        ReInput.players.GetPlayer(0).controllers.AddController(ReInput.controllers.GetController(ControllerType.Joystick, 0), true);
    }

    public void ToggleLockFPS()
    {
        locked = !locked;
        if (!locked)
        {
            PlayerPrefs.SetInt("LockFPS", 0);
            Application.targetFrameRate = 999;
        }
        else
        {
            PlayerPrefs.SetInt("LockFPS", 1);
            Application.targetFrameRate = Screen.currentResolution.refreshRate;
        }

        foreach (GameObject g in lockFPSTicks)
            g.SetActive(locked);
    }

    public void OpenButtonRemapper()
    {
        startScreen.remapperOpen = true;
        pauseScreen.remapperOpen = true;
        controlRemapping.enabled = true;
        controlRemapping.Open();
        foreach (GameObject g in remapperBGs)
            g.SetActive(true);
    }

    public void CloseButtonRemapper()
    {
        pauseScreen.remapperOpen = false;
        startScreen.remapperOpen = false;
        controlRemapping.Close();
        controlRemapping.enabled = false;
        foreach (GameObject g in remapperBGs)
            g.SetActive(false);
    }

    public void ResolutionLeft()
    {
        currentResolution--;
        if (currentResolution < 0)
            currentResolution = Screen.resolutions.Length - 1;

        SetResolutionValues();
    }

    public void ResolutionRight()
    {
        currentResolution++;
        if (currentResolution >= Screen.resolutions.Length)
            currentResolution = 0;

        SetResolutionValues();
    }

    void SetResolutionValues()
    {
        curRes = Screen.resolutions[currentResolution];
        foreach (Text t in resolutionTexts)
            t.text = curRes.width + " x " + curRes.height + " " + curRes.refreshRate + "Hz";

        ResolutionApply();
    }

    public void ResolutionApply()
    {
        PlayerPrefs.SetInt("Resolution", currentResolution);
        PlayerPrefs.SetInt("ScreenMode", currentFSMode);
        Screen.SetResolution(curRes.width, curRes.height, screenMode);
        if(locked)
            Application.targetFrameRate = Screen.currentResolution.refreshRate;
        PlayApplySound();
    }

    public void WindowLeft()
    {
        currentFSMode--;
        if (currentFSMode < 0)
            currentFSMode = 2;

        SetWindowValues();
    }

    public void WindowRight()
    {
        currentFSMode++;
        if (currentFSMode > 2)
            currentFSMode = 0;

        SetWindowValues();
    }

    void SetWindowValues()
    {
        if (currentFSMode == 0)
        {
            screenMode = FullScreenMode.FullScreenWindow;
            foreach (Text t in windowTexts)
                t.text = TextTranslationManager.GetText(TextTranslationManager.TextCollection.startMenu, 19, curLanguage);
        }
        if (currentFSMode == 1)
        {
            screenMode = FullScreenMode.MaximizedWindow;
            foreach (Text t in windowTexts)
                t.text = TextTranslationManager.GetText(TextTranslationManager.TextCollection.startMenu, 20, curLanguage);
        }
        if (currentFSMode == 2)
        {
            screenMode = FullScreenMode.Windowed;
            foreach (Text t in windowTexts)
                t.text = TextTranslationManager.GetText(TextTranslationManager.TextCollection.startMenu, 21, curLanguage);
        }

        WindowApply();
    }

    public void WindowApply()
    {
    //    PlayerPrefs.SetInt("ScreenMode", currentFSMode);
    //    Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, screenMode);
        ResolutionApply();
    }

    public void VSyncLeft()
    {
        currentVSync--;
        if (currentVSync < 0)
            currentVSync = 2;

        SetVSyncValues();
    }

    public void VSyncRight()
    {
        currentVSync++;
        if (currentVSync > 2)
            currentVSync = 0;

        SetVSyncValues();
    }

    void SetVSyncValues()
    {
        if (currentVSync == 0)
        {
            foreach (Text t in vsyncTexts)
                t.text = TextTranslationManager.GetText(TextTranslationManager.TextCollection.startMenu, 22, curLanguage);
        }
        if (currentVSync == 1)
        {
            foreach (Text t in vsyncTexts)
                t.text = TextTranslationManager.GetText(TextTranslationManager.TextCollection.startMenu, 23, curLanguage);
        }
        if (currentVSync == 2)
        {
            foreach (Text t in vsyncTexts)
                t.text = TextTranslationManager.GetText(TextTranslationManager.TextCollection.startMenu, 24, curLanguage);
        }

        VSyncApply();
    }

    public void VSyncApply()
    {
        PlayerPrefs.SetInt("VSync", currentVSync);
        QualitySettings.vSyncCount = currentVSync;
        PlayApplySound();
    }

    void PlayApplySound()
    {/*
        if (!dontPlaySound)
        {
            applySource.Stop();
            applySource.PlayDelayed(0f);
        }
        else
            dontPlaySound = false;*/
    }

    public void ObjectSlider(Slider slider)
    {
        PlayerPrefs.SetInt("ObjectDetails", Mathf.RoundToInt(slider.value));
    }

    public void DistanceSlider(Slider slider)
    {
        PlayerPrefs.SetInt("AddedDistance", Mathf.RoundToInt(slider.value));
    }

    public void AALeft()
    {
        aaMode--;
        if (aaMode < 0)
            aaMode = 3;

        SetAATexts();
    }

    public void AARight()
    {
        aaMode++;
        if (aaMode > 3)
            aaMode = 0;

        SetAATexts();
    }

    void SetAATexts()
    {
        if (aaMode == 0)
        {
            foreach (Text t in aaTexts)
                t.text = TextTranslationManager.GetText(TextTranslationManager.TextCollection.startMenu, 25, curLanguage);
        }
        if (aaMode == 1)
        {
            foreach (Text t in aaTexts)
                t.text = "FXAA";
        }
        if (aaMode == 2)
        {
            foreach (Text t in aaTexts)
                t.text = "SMAA";
        }
        if (aaMode == 3)
        {
            foreach (Text t in aaTexts)
                t.text = "TAA";
        }

        AAApply();
    }

    public void AAApply()
    {
        PlayerPrefs.SetInt("AAMode", aaMode);

        if (aaMode == 0)
        {
            ppLayer.antialiasingMode = PostProcessLayer.Antialiasing.None;
            foreach (PostProcessLayer ppl in cutscenePPLayers)
                ppl.antialiasingMode = PostProcessLayer.Antialiasing.None;
        }
        if (aaMode == 1)
        {
            ppLayer.antialiasingMode = PostProcessLayer.Antialiasing.FastApproximateAntialiasing;
            foreach (PostProcessLayer ppl in cutscenePPLayers)
                ppl.antialiasingMode = PostProcessLayer.Antialiasing.FastApproximateAntialiasing;
        }
        if (aaMode == 2)
        {
            ppLayer.antialiasingMode = PostProcessLayer.Antialiasing.SubpixelMorphologicalAntialiasing;
            foreach (PostProcessLayer ppl in cutscenePPLayers)
                ppl.antialiasingMode = PostProcessLayer.Antialiasing.SubpixelMorphologicalAntialiasing;
        }
        if (aaMode == 3)
        {
            ppLayer.antialiasingMode = PostProcessLayer.Antialiasing.TemporalAntialiasing;
            foreach (PostProcessLayer ppl in cutscenePPLayers)
                ppl.antialiasingMode = PostProcessLayer.Antialiasing.TemporalAntialiasing;
        }
    }

    public void BloomToggle()
    {
        bloomOn.value = !bloomOn.value;
        if (bloomOn.value)
            PlayerPrefs.SetInt("Bloom", 1);
        else
            PlayerPrefs.SetInt("Bloom", 0);
        SetBloom();
    }

    void SetBloom()
    {
        ppVolume.profile.GetSetting<Bloom>().enabled = bloomOn;
        foreach (PostProcessVolume ppv in cutscenePPVolumes)
            ppv.profile.GetSetting<Bloom>().enabled = bloomOn;
        foreach (GameObject g in bloomTicks)
            g.SetActive(bloomOn.value);
    }

    public void AOToggle()
    {
        aoOn.value = !aoOn.value;
        if (aoOn.value)
            PlayerPrefs.SetInt("AO", 1);
        else
            PlayerPrefs.SetInt("AO", 0);
        SetAO();
    }

    void SetAO()
    {
        ppVolume.profile.GetSetting<AmbientOcclusion>().enabled = aoOn;
        foreach (PostProcessVolume ppv in cutscenePPVolumes)
            ppv.profile.GetSetting<AmbientOcclusion>().enabled = aoOn;
        foreach (GameObject g in aoTicks)
            g.SetActive(aoOn.value);
    }

    public void DoFToggle()
    {
        dofOn.value = !dofOn.value;
        if (dofOn.value)
            PlayerPrefs.SetInt("DoF", 1);
        else
            PlayerPrefs.SetInt("DoF", 0);
        SetDoF();
    }

    void SetDoF()
    {
        ppVolume.profile.GetSetting<DepthOfField>().enabled = dofOn;
        foreach (PostProcessVolume ppv in cutscenePPVolumes)
            ppv.profile.GetSetting<DepthOfField>().enabled = dofOn;
        foreach (GameObject g in dofTicks)
            g.SetActive(dofOn.value);
    }
    
    public void RestoreDefaults()
    {
        //VALUES
        PlayerPrefs.SetInt("InvertX", 0);
        PlayerPrefs.SetInt("InvertY", 0);
        PlayerPrefs.SetInt("Vibration", 1);
        PlayerPrefs.SetFloat("musicVolume", 8f);
        PlayerPrefs.SetFloat("effectsVolume", 8f);
        PlayerPrefs.SetInt("Sensitivity", 4);
        PlayerPrefs.SetInt("RunByDefault", 1);
        PlayerPrefs.SetInt("Player1KeyboardOnly", 0);
        PlayerPrefs.SetInt("Bloom", 1);
        PlayerPrefs.SetInt("AO", 1);
        PlayerPrefs.SetInt("DoF", 0);
        PlayerPrefs.SetInt("AAMode", 1);
        PlayerPrefs.SetInt("LockFPS", 1);
        PlayerPrefs.SetInt("VSync", 1);
        PlayerPrefs.SetInt("ObjectDetails", 4);
        PlayerPrefs.SetInt("AddedDistance", 4);

        //START
        startScreen.InitializeValues();

        //PAUSE
        pauseScreen.InitializeValues();

        //THIS
        InitializeValues();

        //CLOSE AYS OVERLAYS
        if (pauseScreen.optionsTab.activeInHierarchy)
            pauseScreen.CloseDefaultsAYS();
        if (startScreen.optionsScreen.activeInHierarchy)
            startScreen.CloseDefaultsAYS();
    }

    public void OptionsScroll(Slider slider)
    {
        foreach(RectTransform rt in optionsContents)
        {
            rt.anchoredPosition = new Vector2(0f, Mathf.Lerp(0f, 1200f, slider.value/10f));
        }
        if (es.currentSelectedGameObject != null && es.currentSelectedGameObject == slider.gameObject)
            scrollResult = slider.value;
    }

    public void ResetOptionsScroll()
    {
        scrollResult = 0f;
        pauseOptionScroll.value = 0f;
        foreach (RectTransform rt in optionsContents)
            rt.anchoredPosition = Vector2.zero;
        //    startOptionScroll.value = 0f;
    }
}
