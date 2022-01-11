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
    //public ControlRemappingDemo1 controlRemapping;
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

    //public List<GameObject> bloomTicks;
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
    [SerializeField] public bool isXInverted;
    [SerializeField] List<GameObject> invertXTicks;

    [Header("Ivert Y")]
    [SerializeField] public bool isYInverted;
    [SerializeField] List<GameObject> invertYTicks;

    [Header("Vibration")]
    [SerializeField] public bool isVibrationsActive;
    [SerializeField] List<GameObject> vibrationTicks;

    [Header("Anti-Aliasing")]
    [SerializeField] public bool isAnitAliasingActive;
    [SerializeField] List<GameObject> anitAlisingTicks;

    [Header("V-Sync")]
    [SerializeField] public bool isVSyncActive;
    [SerializeField] List<GameObject> vSyncTicks;

    [Header("Run by Default")]
    [SerializeField] public bool isRunByDefaultActive;
    [SerializeField] List<GameObject> runByDefaultTicks;

    [Header("Bloom")]
    [SerializeField] public bool isBloomActive;
    [SerializeField] List<GameObject> bloomTicks;

    [Header("Ambient Occlusion")]
    [SerializeField] public bool isAmbientOcclusionActive;
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

        //controlRemapping.enabled = false;
        
        applySource.clip = applySound;

        curLanguage = PlayerPrefs.GetInt("Language", 0);
    }
    
    void InitializeValues()
    {
        //WE GET THE PREFS VALUE AND CONVERT THEM IN TO BOOLS
        UpdateOptionsBoolWithPlayerPrefs();
        //WE UPDATE EACH UI ELEMENT USING THOSE BOOLS
        UpdateOptionsUIToggles();
        //WE APPLAY THE VALUE OF THOSE BOOLS IN THE OPTIONS
        ApplyOptions();


        PlayerManager.singleton.testingWithKeyboard = false;
        KeyboardOnlyOff();
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
        PlayerPrefs.SetInt("RunByDefault", 0);
        PlayerPrefs.SetInt("Bloom", 1);
        PlayerPrefs.SetInt("AO", 1);
        PlayerPrefs.SetInt("AAMode", 1);
        PlayerPrefs.SetInt("VSync", 1);

        //THIS
        InitializeValues();

        //START
        startScreen.InitializeValues();

        //PAUSE
        pauseScreen.InitializeValues();

       

        //CLOSE AYS OVERLAYS
        if (pauseScreen.optionsTab.activeInHierarchy)
            pauseScreen.CloseDefaultsAYS();
        if (startScreen.optionsScreen.activeInHierarchy)
            startScreen.CloseDefaultsAYS();
    }



    #region PUBLIC METHODS FOR OPTIONS
    public void ToggleInvertX()
    {
        isXInverted = !isXInverted;

        UpdateOptionsPlayerPrefsAndUIAndApply();
    }

    public void ToggleInvertY()
    {
        isYInverted = !isYInverted;

        UpdateOptionsPlayerPrefsAndUIAndApply();
    }

    public void ToggleVibrations()
    {
        isVibrationsActive = !isVibrationsActive;

        UpdateOptionsPlayerPrefsAndUIAndApply();
    }

    public void ToggleAntiAliasing()
    {
        isAnitAliasingActive = !isAnitAliasingActive;

        UpdateOptionsPlayerPrefsAndUIAndApply();
    }

    public void ToggleVsync()
    {
        isVSyncActive = !isVSyncActive;

        UpdateOptionsPlayerPrefsAndUIAndApply();
    }

    public void ToggleRunByDefault()
    {
        isRunByDefaultActive = !isRunByDefaultActive;

        UpdateOptionsPlayerPrefsAndUIAndApply();
    }

    public void ToggleBloom()
    {
        isBloomActive = !isBloomActive;

        UpdateOptionsPlayerPrefsAndUIAndApply();
    }

    public void ToggleAmbientOcclusion()
    {
        isAmbientOcclusionActive = !isAmbientOcclusionActive;

        UpdateOptionsPlayerPrefsAndUIAndApply();
    }
    #endregion

    #region UI - PLAYERPREFS - OPTIONS UPDATES
    void UpdateOptionsPlayerPrefsAndUIAndApply()
    {
        UpdateOptionsUIToggles();

        ApplyOptions();

        UpdateOptionsPlayerPrefsWithBool();
    }

    void UpdateOptionsUIToggles()
    {
        foreach (GameObject tick in invertXTicks)
        {
            tick.SetActive(isXInverted);
        }

        foreach (GameObject tick in invertYTicks)
        {
            tick.SetActive(isYInverted);
        }

        foreach (GameObject tick in vibrationTicks)
        {
            tick.SetActive(isVibrationsActive);
        }

        foreach (GameObject tick in anitAlisingTicks)
        {
            tick.SetActive(isAnitAliasingActive);
        }

        foreach (GameObject tick in vSyncTicks)
        {
            tick.SetActive(isVSyncActive);
        }

        foreach (GameObject tick in runByDefaultTicks)
        {
            tick.SetActive(isRunByDefaultActive);
        }

        foreach (GameObject tick in bloomTicks)
        {
            tick.SetActive(isBloomActive);
        }

        foreach (GameObject tick in ambientOcclusionTicks)
        {
            tick.SetActive(isAmbientOcclusionActive);
        }
    }

    void UpdateOptionsPlayerPrefsWithBool()
    {
        PlayerPrefs.SetInt("InvertX", isXInverted.GetHashCode());
        PlayerPrefs.SetInt("InvertY", isYInverted.GetHashCode());
        PlayerPrefs.SetInt("Vibration", isVibrationsActive.GetHashCode());
        PlayerPrefs.SetInt("AAMode", isAnitAliasingActive.GetHashCode());
        PlayerPrefs.SetInt("VSync", isVSyncActive.GetHashCode());
        PlayerPrefs.SetInt("RunByDefault", isRunByDefaultActive.GetHashCode()); ;
        PlayerPrefs.SetInt("Bloom", isBloomActive.GetHashCode());
        PlayerPrefs.SetInt("AO", isAmbientOcclusionActive.GetHashCode());


        PlayerPrefs.SetFloat("musicVolume", 8f);
        PlayerPrefs.SetFloat("effectsVolume", 8f);
        PlayerPrefs.SetInt("Sensitivity", 4);
    }

    void UpdateOptionsBoolWithPlayerPrefs()
    {
        //WE GET THE VALUES FROM THE PLAYERPREFS
        isXInverted = PlayerPrefs.GetInt("InvertX") > 0;
        isYInverted = PlayerPrefs.GetInt("InvertY") > 0;
        isVibrationsActive = PlayerPrefs.GetInt("Vibration") > 0;
        isAnitAliasingActive = PlayerPrefs.GetInt("AAMode") > 0;
        isVSyncActive = PlayerPrefs.GetInt("VSync") > 0;
        isRunByDefaultActive = PlayerPrefs.GetInt("RunByDefault") > 0;
        isBloomActive = PlayerPrefs.GetInt("Bloom") > 0;
        isAmbientOcclusionActive = PlayerPrefs.GetInt("AO") > 0;
    }
    #endregion

    #region APPLY OPTIONS
    void ApplyOptions()
    {
        ApplyBloom();
        ApplyAmbientOcclusion();
        ApplyAntiAliasing();
        ApplyRunByDefault();
        ApplyVSync();
    }

    void ApplyBloom()
    {
        BoolParameter boolParameter = new BoolParameter();

        boolParameter.value = isBloomActive;

        ppVolume.profile.GetSetting<Bloom>().enabled = boolParameter;

        foreach (PostProcessVolume ppv in cutscenePPVolumes)
            ppv.profile.GetSetting<Bloom>().enabled = boolParameter;
    }
    
    void ApplyAmbientOcclusion()
    {
        BoolParameter boolParameter = new BoolParameter();

        boolParameter.value = isAmbientOcclusionActive;

        ppVolume.profile.GetSetting<AmbientOcclusion>().enabled = boolParameter;

        foreach (PostProcessVolume ppv in cutscenePPVolumes)
            ppv.profile.GetSetting<AmbientOcclusion>().enabled = boolParameter;
    }
    
    void ApplyAntiAliasing()
    {
        if (isAnitAliasingActive)
        {
             ppLayer.antialiasingMode = PostProcessLayer.Antialiasing.FastApproximateAntialiasing;

            foreach (PostProcessLayer ppl in cutscenePPLayers)
                ppl.antialiasingMode = PostProcessLayer.Antialiasing.FastApproximateAntialiasing;
        }
        else
        {
            ppLayer.antialiasingMode = PostProcessLayer.Antialiasing.None;

            foreach (PostProcessLayer ppl in cutscenePPLayers)
                ppl.antialiasingMode = PostProcessLayer.Antialiasing.None;
        }
    }

    void ApplyRunByDefault()
    {
        tpc.runByDefault = isRunByDefaultActive;
    }

    void ApplyVSync()
    {
        if(QualitySettings.vSyncCount != isVSyncActive.GetHashCode())
        {
            QualitySettings.vSyncCount = isVSyncActive.GetHashCode();
            PlayApplySound();
        }
    }
    #endregion


    private void Update()
    {
        if (pauseScreen.optionsTab.activeInHierarchy || startScreen.optionsScreen.activeInHierarchy)
        {
            if (PlayerPrefs.GetInt("Language",0) != curLanguage)
            {
                curLanguage = PlayerPrefs.GetInt("Language", 0);
                //SetWindowValues();
                //SetVSyncValues();
                //SetAATexts();
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

    public void SetInitialTextValues()
    {
        /*
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
        */
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


    /* public void ObjectSlider(Slider slider)
     {
         PlayerPrefs.SetInt("ObjectDetails", Mathf.RoundToInt(slider.value));
     }

     public void DistanceSlider(Slider slider)
     {
         PlayerPrefs.SetInt("AddedDistance", Mathf.RoundToInt(slider.value));
     }
 */

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
