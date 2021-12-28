using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using CinemaDirector;
using Rewired;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;
//using nn;
using UnityEditor;
using UnityStandardAssets.ImageEffects;

public class StartScreen : MonoBehaviour
{

    public bool loadOneAndSeven;
    public bool newLoading;
    public bool useConsole;
    public GameObject consoleObj;
    [HideInInspector] public GameObject[] fullLevels;

    [Header("MISC")]
    public TPC tpc;
    public TPC tpcFox;
    public TPC tpcBeaver;
    public TPC tpcBush;
    public CameraFollower cam;
    public GameObject camPos;
    public PauseScreen ps;
    public ObjDeactivateManager odm;
    Player input;

    public LoadLevelAdditive firstLevelLoadZone;
    public LoadLevelAdditive seventhLevelLoadZone;

    [HideInInspector] public LowPolyTrigger curLPT;
    public LowPolyTrigger[] lowPolyTriggers;
    public LoadLevelAdditive[] loadLevelAdditives;

    public PlayableDirector firstCutscene;
    int introwatched;

    [HideInInspector] public bool inStart;

    public AtmosphereManager atmosphereManager;

    public BlueBerryIDCreator bbic;
    public string[] levelNames;

    public GraphicRaycaster graphicRaycaster;

    [Header("ALL")]
    public EventSystem es;
    public GameObject mainScreen;
    public GameObject optionsScreen;
    public GameObject creditsScreen;
    bool inMain;
    bool inOptions;
    bool inCredits;
    public GameObject mainFirst;
    public GameObject optionsFirst;
    public GameObject creditsFirst;
    GameObject prevSelected;

    [Header("MAIN")]
    public Text startText;
    public GameObject[] startButtons;
    public GameObject titleText;
    public Image loadIcon;
    public Image loadFS;
    public Animator loadAnim;
    public GameObject arrowIcon;
    TextToTranslate toTranslate;

    float startCamX;
    float startCamY;

    [Header("OPTIONS")]
    public GameObject ixTick;
    public GameObject iyTick;
    public GameObject vibTick;
    bool aaIsOn;
    public GameObject aaTick;
    //    public Slider sensitivity;
    public Slider effectsSlider;
    public Slider musicSlider;
    public AudioMixer[] audioMixers;
    public GameObject returnButton;
    public Text returnText;
    public GameObject ays;
    public GameObject noButton;
    public Text noText;
    public GameObject yesButton;
    public Text yesText;
    public GameObject deleteButton;
    public Text deleteText;
    bool invertX;
    bool invertY;
    bool vibOn;
    //	float camSense;
    float effects;
    float music;
    bool deletingFile;
    public GameObject defaultsAYS;
    public GameObject defaultsNo;
    public GameObject defaultsButton;

    public ActivateItemsMasks[] aiM;
    public ActivateItemsLeafs[] aiL;
    public ActivateBonusAbility[] aiB;

    bool languagesOpen;
    public GameObject languagesList;
    public GameObject languagesFirst;
    public GameObject languageButton;

    [HideInInspector] public OptionsNew optionsNew;
    [HideInInspector] public bool remapperOpen;

    [Header("SOUNDS")]
    public AudioClip highlightSound;
    public AudioClip highlightDownSound;
    public AudioClip selectSound;
    public AudioClip backSound;
    public AudioClip startGameSound;
    public AudioClip toggleOnSound;
    public AudioClip toggleOffSound;
    AudioSource sound1;
    AudioSource sound2;
    AudioSource bgMusic;
    int soundInt;
    public GameObject musicToActivate;

    [Header("LOW POLY LEVELS")]
    public GameObject[] lowPolys;
    public GameObject lowPolyExt;
    public ActivateAtDistanceAndUnloadLevel[] aadul;

    Font originalFont;
    FontStyle originalFontStyle;

    bool notMouseOver = false;

    void Start()
    {
        //    Application.targetFrameRate = 60;

        mainScreen.SetActive(false);
        titleText.SetActive(false);
        loadFS.color = Color.black;

        if (loadOneAndSeven)
        {
         //   SceneManager.LoadSceneAsync(3, LoadSceneMode.Additive);
            //    SceneManager.LoadSceneAsync(9, LoadSceneMode.Additive);

        }

        inStart = true;
        cam.disableControl = true;
        tpc.disableControl = true;
        tpcFox.disableControl = true;
        tpcBeaver.disableControl = true;
        tpcBush.disableControl = true;
        input = ReInput.players.GetPlayer(0);

        AudioSource[] sources = this.gameObject.GetComponents<AudioSource>();
        sound1 = sources[0];
        sound2 = sources[1];
        bgMusic = sources[2];
        soundInt = 0;

        cam.transform.SetParent(camPos.transform);
        cam.transform.localPosition = Vector3.zero;
        cam.transform.localRotation = Quaternion.identity;

        if (!PlayerPrefs.HasKey("LastCheckpoint"))
            PlayerPrefs.SetInt("LastCheckpoint", -1);

        if (!PlayerPrefs.HasKey("MainPlaza7NewBlueBerry"))
        {
            foreach (string s in levelNames)
            {
                int total = 140;
                if (s == "ExternalWorld")
                    total = 100;
                if (s == "Level1.2")
                    total = 80;
                if (s == "Level2")
                    total = 90;
                if (s == "Level3")
                    total = 80;
                if (s == "Level4")
                    total = 80;
                if (s == "Level5")
                    total = 80;
                if (s == "Level6")
                    total = 120;
                if (s == "Level7")
                    total = 80;
                if (s == "Level8")
                    total = 80;
                PlayerPrefs.SetInt(s + "BlueBerryTotal", total);

                string newString = "";
                for (int i = 0; i < total; i++)
                {
                    newString = newString.Insert(0, "0");
                }
                PlayerPrefs.SetString(s + "BlueBerry", newString);
            }
            PlayerPrefs.Save();
        }

        if (!PlayerPrefs.HasKey("BlueBerryTotal"))
        {
            ps.CheckBlues();
            PlayerPrefs.Save();
        }

        if (!PlayerPrefs.HasKey("HalfBlueBerries"))
        {
            if (PlayerPrefs.GetInt("BlueBerryTotal") >= 465)
            {
                PlayerPrefs.SetInt("HalfBlueBerries", 1);
                PlayerPrefs.Save();
                ps.textMain.SetText(15);
            }
        }

        toTranslate = this.gameObject.GetComponent<TextToTranslate>();

        originalFont = startText.font;
        originalFontStyle = startText.fontStyle;
        toTranslate.SetTextElement(startText, null, TextTranslationManager.TextCollection.startMenu, PlayerPrefs.GetInt("IntroWatched", 0), "", false, false, originalFont, originalFontStyle);

        if (PlayerPrefs.GetInt("SteamCheckedLanguage", 0) == 0)
        {
            switch (Application.systemLanguage)
            {
                case SystemLanguage.English:
                    PlayerPrefs.SetInt("Language", 0);
                    break;

                case SystemLanguage.Russian:
                    PlayerPrefs.SetInt("Language", 1);
                    break;

                case SystemLanguage.Spanish:
                    PlayerPrefs.SetInt("Language", 2);
                    break;

                case SystemLanguage.Italian:
                    PlayerPrefs.SetInt("Language", 3);
                    break;

                case SystemLanguage.ChineseSimplified:
                    PlayerPrefs.SetInt("Language", 4);
                    break;

                case SystemLanguage.French:
                    PlayerPrefs.SetInt("Language", 5);
                    break;

                case SystemLanguage.Portuguese:
                    PlayerPrefs.SetInt("Language", 6);
                    break;

                case SystemLanguage.Dutch:
                    PlayerPrefs.SetInt("Language", 7);
                    break;

                case SystemLanguage.German:
                    PlayerPrefs.SetInt("Language", 8);
                    break;

                case SystemLanguage.Japanese:
                    PlayerPrefs.SetInt("Language", 9);
                    break;

                case SystemLanguage.Turkish:
                    PlayerPrefs.SetInt("Language", 10);
                    break;

                case SystemLanguage.Arabic:
                    PlayerPrefs.SetInt("Language", 11);
                    break;

                case SystemLanguage.Polish:
                    PlayerPrefs.SetInt("Language", 12);
                    break;

                case SystemLanguage.Danish:
                    PlayerPrefs.SetInt("Language", 13);
                    break;

                case SystemLanguage.Korean:
                    PlayerPrefs.SetInt("Language", 14);
                    break;

                default:
                    PlayerPrefs.SetInt("Language", 0);
                    break;
            }

            PlayerPrefs.SetInt("SteamCheckedLanguage", 1);
        }
        if (!PlayerPrefs.HasKey("BlueBerryTotal"))
        {
            ps.CheckBlues();
            PlayerPrefs.Save();
            //
#if UNITY_XBOXONE
            DataManager.xOneEventsManager.SaveProgs();
#endif
        }

        toTranslate = this.gameObject.GetComponent<TextToTranslate>();

        originalFont = startText.font;
        originalFontStyle = startText.fontStyle;
        toTranslate.SetTextElement(startText, null, TextTranslationManager.TextCollection.startMenu, PlayerPrefs.GetInt("IntroWatched", 0), "", false, false, originalFont, originalFontStyle);

        StartCoroutine(EndOfStart());
        //
#if UNITY_XBOXONE
        ReUnlockAchievsAfterConnection();
#endif
    }

    void Update()
    {
        if (useConsole && !consoleObj.activeInHierarchy)
            consoleObj.SetActive(true);

        if (inStart)
        {
            if (Cursor.visible && (input.GetAxis("UIH") != 0f || input.GetAxis("UIV") != 0f))
            {
                Cursor.visible = false;
                graphicRaycaster.enabled = false;
            }
            if (!Cursor.visible && input.GetAxis("MouseMove") != 0f)
            {
                Cursor.visible = true;
                graphicRaycaster.enabled = true;
            }

            if (cam.transform.localScale != Vector3.zero)
            {
                cam.transform.localPosition = Vector3.zero;
                cam.transform.localRotation = Quaternion.identity;

                /*
                startCamX = Mathf.Lerp(startCamX, input.GetAxis("RH") * 40f, Time.deltaTime * 0.5f);
                startCamY = Mathf.Lerp(startCamY, -input.GetAxis("RV") * 40f, Time.deltaTime * 0.5f);

                cam.transform.localEulerAngles = new Vector3(startCamY, startCamX, 0f);
                */

                if (es.currentSelectedGameObject == mainFirst && input.GetButtonDown("Start") && !remapperOpen)
                    StartContinue();

                //    cam.transform.RotateAround(cam.transform.right, startCamY);
                //    cam.transform.RotateAround(cam.transform.up, startCamX);
            }
#if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.J))
            {
                for (int ca = 0; ca < CagedNPCManager.GetAmount(); ca++)
                {
                    PlayerPrefs.SetInt("Cage" + ca.ToString(), 0);
                    CagedNPCManager.SpecialCaseCheck(ca);
                }
            }
#endif
            if (inMain)
            {
                toTranslate.SetTextElement(startText, null, TextTranslationManager.TextCollection.startMenu, PlayerPrefs.GetInt("IntroWatched", 0), "", false, false, originalFont, originalFontStyle);

                if (es.currentSelectedGameObject == null)
                    es.SetSelectedGameObject(mainFirst);
            }

            //OPTIONS SELECTION
            if (inOptions)
            {
                /*	if (es.currentSelectedGameObject == returnButton) {
					if (returnText.color != Color.grey)
						returnText.color = Color.grey;
				} else {
					if (returnText.color != Color.white)
						returnText.color = Color.white;
				}
				if (es.currentSelectedGameObject == deleteButton) {
					if (deleteText.color != Color.grey)
						deleteText.color = Color.grey;
				} else {
					if (deleteText.color != Color.white)
						deleteText.color = Color.white;
				}
				if (es.currentSelectedGameObject == yesButton) {
					if (yesText.color != Color.red)
						yesText.color = Color.red;
				} else {
					if (yesText.color != Color.white)
						yesText.color = Color.white;
				}
				if (es.currentSelectedGameObject == noButton) {
					if (noText.color != Color.grey)
						noText.color = Color.grey;
				} else {
					if (noText.color != Color.white)
						noText.color = Color.white;
				}*/
            }

            //NAVIGATION
            if (input.GetButtonDown("Back") && !deletingFile)
            {
                if (inOptions || inCredits)
                {
                    HDRumbleMain.PlayVibrationPreset(0, "K02_Patter2", 1f, 0, 0.2f);
                    GoBack();
                }
            }

            if (input.GetButtonDown("Submit"))
                HDRumbleMain.PlayVibrationPreset(0, "K01_Patter1", 1f, 0, 0.2f);

            //SOUNDS
            if (inStart && prevSelected != es.currentSelectedGameObject && prevSelected != null && es.currentSelectedGameObject != null
                && (input.GetAxis("UIH") != 0f || input.GetAxis("UIV") != 0f))
            {
                notMouseOver = true;
                MouseOverButton(es.currentSelectedGameObject);
            }
        }
    }

    public void GoBack()
    {
        if (inOptions && ays.activeInHierarchy)
            CloseAYS();
        else
        {
            if (inOptions && languagesOpen)
                ToggleLanguagesList();
            else
            {
                if (remapperOpen)
                    optionsNew.CloseButtonRemapper();
                else
                {
                    if (defaultsAYS.activeInHierarchy)
                        CloseDefaultsAYS();
                    else
                        MainX(false);
                }
            }
        }
    }

    public void OpenStart()
    {
        inStart = true;
        cam.disableControl = true;
        tpc.disableControl = true;
        tpcFox.disableControl = true;
        tpcBeaver.disableControl = true;
        tpcBush.disableControl = true;
        this.transform.GetChild(0).gameObject.SetActive(true);

        cam.transform.SetParent(camPos.transform);
        cam.transform.localPosition = Vector3.zero;
        cam.transform.localRotation = Quaternion.identity;

        prevSelected = mainFirst;
        MainX(true);
    }

    public void StartContinue()
    {
        if (inStart)
        {
            inStart = false;

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            foreach (EventTrigger et in mainFirst.transform.parent.gameObject.GetComponentsInChildren<EventTrigger>(true))
                Destroy(et);

            cam.transform.SetParent(null);
            PlaySound(startGameSound);
            HDRumbleMain.PlayVibrationPreset(0, "P05_DampedFm2", 1.3f, 0, 0.2f);
            bgMusic.Stop();

            this.transform.GetChild(0).GetComponent<Animator>().Play("StartScreenFade", 0);

            if (loadOneAndSeven)
            {
            /*    foreach (GameObject g in GameObject.FindGameObjectsWithTag("WholeLevel"))
                {
                    if (g.name == "Level1.2")
                    {
                        firstLevelLoadZone.fullSceneObject = g.gameObject;
                        odm.DeactivateObject(g.gameObject, null);
                    }
                }*/
            }


            if (PlayerPrefs.GetInt("IntroWatched", 0) == 0)
            {
                StartCoroutine(LoadIt(-1, -1));
            }
            else
            {
                Time.timeScale = 0f;
                int whichCheckpoint = PlayerPrefs.GetInt("LastCheckpoint", 0);
                if (whichCheckpoint != -1)
                {
                    int checkForScene = PlayerPrefs.GetInt("Checkpoint" + whichCheckpoint + "Scene", 0);
                //    if ((!loadOneAndSeven && checkForScene != 2) || (loadOneAndSeven && (checkForScene != 1 && checkForScene != 4)) || whichCheckpoint == 2)
                    {
                        ps.cantPause = true;
                        StartCoroutine(LoadIt(whichCheckpoint, checkForScene));
                    }
                //    else
                 //       StartCoroutine(LoadIt(whichCheckpoint, -1));
                }
            }
        }
    }

    public void InitializeValues()
    {
        if (PlayerPrefs.GetInt("InvertX", 0) == 0)
        {
            invertX = false;
        }
        else
            invertX = true;
        cam.freeInvertXAxis = invertX;
        ixTick.SetActive(invertX);

        if (PlayerPrefs.GetInt("InvertY", 0) == 0)
            invertY = false;
        else
            invertY = true;
        cam.freeInvertYAxis = invertY;
        iyTick.SetActive(invertY);

        if (PlayerPrefs.GetInt("Vibration", 1) == 0)
            vibOn = false;
        else
            vibOn = true;
        //		camSense = cam.freeRotateSpeed;

        music = PlayerPrefs.GetFloat("musicVolume", 8f);
        effects = PlayerPrefs.GetFloat("effectsVolume", 8f);

        /*
        if (PlayerPrefs.GetInt("AA", 1) == 0)
            aaIsOn = false;
        else
            aaIsOn = true;
        foreach (Antialiasing aa in ps.antialiasing)
            aa.enabled = aaIsOn;
        */

        if (invertX)
            ixTick.SetActive(true);
        else
            ixTick.SetActive(false);

        if (invertY)
            iyTick.SetActive(true);
        else
            iyTick.SetActive(false);

        if (vibOn)
            vibTick.SetActive(true);
        else
            vibTick.SetActive(false);

        if (aaIsOn)
            aaTick.SetActive(true);
        else
            aaTick.SetActive(false);

        //    sensitivity.value = camSense;

        musicSlider.value = music;
        effectsSlider.value = effects;
    }

    void OpenMain()
    {
        inMain = true;
        mainScreen.SetActive(true);
    }

    void CloseMain()
    {
        inMain = false;
        mainScreen.SetActive(false);
    }

    void OpenOptions()
    {
        inOptions = true;
        InitializeValues();
        optionsNew.SetInitialTextValues();
        optionsNew.ResetOptionsScroll();
        optionsScreen.SetActive(true);
    }

    void CloseOptions()
    {
        if (inOptions)
            PlayerPrefs.Save();
        inOptions = false;
        optionsScreen.SetActive(false);

#if UNITY_XBOXONE && !UNITY_EDITOR
            DataManager.xOneEventsManager.SaveProgs();
#endif
    }

    void OpenCredits()
    {
        inCredits = true;
        creditsScreen.SetActive(true);
    }

    void CloseCredits()
    {
        inCredits = false;
        creditsScreen.SetActive(false);
    }

    public void MainX(bool firstTime)
    {
        CloseOptions();
        CloseCredits();
        OpenMain();
        if (firstTime)
            mainScreen.SetActive(false);
        es.SetSelectedGameObject(mainFirst);
        prevSelected = es.currentSelectedGameObject;
        MainButtonSelect();
        if (!firstTime)
            PlaySound(backSound);
    }

    public void Options()
    {
        CloseMain();
        CloseCredits();
        OpenOptions();
        es.SetSelectedGameObject(optionsFirst);
        prevSelected = es.currentSelectedGameObject;
        PlaySound(selectSound);
    }

    public void Credits()
    {
        CloseOptions();
        CloseMain();
        OpenCredits();
        es.SetSelectedGameObject(creditsFirst);
        prevSelected = es.currentSelectedGameObject;
        PlaySound(selectSound);
    }

    public void MouseOverButton(GameObject selected)
    {
        if (selected != null)
        {
            if (notMouseOver || Cursor.visible)
            {
                notMouseOver = false;
                es.SetSelectedGameObject(selected);
                if (prevSelected.gameObject.transform.position.y < selected.transform.position.y)
                    PlaySound(highlightDownSound);
                else
                    PlaySound(highlightSound);
                //    Vibrate(0.05f, 0.2f);
                prevSelected = selected;

                if (inMain)
                    MainButtonSelect();
            }
        }
    }

    void MainButtonSelect()
    {
        arrowIcon.transform.position = new Vector3(arrowIcon.transform.position.x, es.currentSelectedGameObject.transform.position.y, arrowIcon.transform.position.z);
        arrowIcon.transform.localPosition = new Vector3(Mathf.Clamp((-25f * (float)es.currentSelectedGameObject.GetComponentInChildren<Text>().text.Length), -2000f, -200f), arrowIcon.transform.localPosition.y, arrowIcon.transform.localPosition.z);
        foreach (GameObject g in startButtons)
        {
            if (g == es.currentSelectedGameObject)
                g.transform.GetChild(0).GetComponent<Text>().color = Color.white;
            else
                g.transform.GetChild(0).GetComponent<Text>().color = Color.gray;
        }
    }

    public void QuitGame()
    {
        PlayerPrefs.Save();
        if (!Application.isEditor)
            System.Diagnostics.Process.GetCurrentProcess().Kill();
    }

    //OPTIONS
    public void InvertX()
    {
        invertX = !invertX;
        if (invertX)
        {
            ixTick.SetActive(true);
            PlaySound(toggleOnSound);
            PlayerPrefs.SetInt("InvertX", 1);
        }
        else
        {
            ixTick.SetActive(false);
            PlaySound(toggleOffSound);
            PlayerPrefs.SetInt("InvertX", 0);
        }
        cam.freeInvertXAxis = invertX;
    }

    public void InvertY()
    {
        invertY = !invertY;
        if (invertY)
        {
            iyTick.SetActive(true);
            PlaySound(toggleOnSound);
            PlayerPrefs.SetInt("InvertY", 1);
        }
        else
        {
            iyTick.SetActive(false);
            PlaySound(toggleOffSound);
            PlayerPrefs.SetInt("InvertY", 0);
        }
        cam.freeInvertYAxis = invertY;
        PlaySound(selectSound);
    }

    public void Vibration()
    {
        vibOn = !vibOn;
        if (vibOn)
        {
            vibTick.SetActive(true);
            PlaySound(toggleOnSound);
        }
        else
        {
            vibTick.SetActive(false);
            PlaySound(toggleOffSound);
        }
        if (vibOn)
            PlayerPrefs.SetInt("Vibration", 1);
        else
            PlayerPrefs.SetInt("Vibration", 0);
        PlaySound(selectSound);
    }

    public void AAToggle()
    {
        if (!aaIsOn)
        {
            aaIsOn = true;
            aaTick.SetActive(true);
            PlaySound(toggleOnSound);
            foreach (Antialiasing aa in ps.antialiasing)
                aa.enabled = true;
            PlayerPrefs.SetInt("AA", 1);
        }
        else
        {
            aaIsOn = false;
            aaTick.SetActive(false);
            PlaySound(toggleOffSound);
            foreach (Antialiasing aa in ps.antialiasing)
                aa.enabled = false;
            PlayerPrefs.SetInt("AA", 0);
        }
    }

    /*   public void Sensitivity(){
           camSense = sensitivity.value;
           cam.freeRotateSpeed = camSense;
           PlaySound (highlightSound);
       }*/

    public void SetEffects()
    {
        PlayerPrefs.SetFloat("effectsVolume", effectsSlider.value);
        foreach (AudioMixer am in audioMixers)
            am.SetFloat("effectsVol", -80f + ((PlayerPrefs.GetFloat("effectsVolume", 8f)) * 10f));
        PlaySound(highlightSound);
    }

    public void SetMusic()
    {
        PlayerPrefs.SetFloat("musicVolume", musicSlider.value);
        foreach (AudioMixer am in audioMixers)
            am.SetFloat("musicVol", -80f + ((PlayerPrefs.GetFloat("musicVolume", 8f)) * 10f));
        PlaySound(highlightSound);
    }

    public void OpenAYS()
    {
        ays.SetActive(true);
        es.SetSelectedGameObject(noButton.gameObject);
    }

    public void CloseAYS()
    {
        ays.SetActive(false);
        es.SetSelectedGameObject(deleteButton.gameObject);
    }

    public void DeleteFile()
    {
        if (!deletingFile)
        {
            deletingFile = true;
            StartCoroutine(DeleteCoRo());
        }
    }

    public void ToggleLanguagesList()
    {
        if (!languagesOpen)
        {
            languagesOpen = true;
            languagesList.SetActive(true);
            es.SetSelectedGameObject(languagesFirst);
        }
        else
        {
            languagesOpen = false;
            languagesList.SetActive(false);
            es.SetSelectedGameObject(languageButton);
        }
    }

    public void SetLanguage(int languageInt)
    {
        PlayerPrefs.SetInt("Language", languageInt);
        ToggleLanguagesList();
    }

    IEnumerator DeleteCoRo()
    {
        deleteButton.gameObject.SetActive(false);
        noButton.gameObject.SetActive(false);

        /*
        int vibInt = PlayerPrefs.GetInt("Vibration", 1);
        int aaInt = PlayerPrefs.GetInt("AA", 1);
        float musicFT = PlayerPrefs.GetFloat("musicVolume", 8f);
        float effectsFT = PlayerPrefs.GetFloat("effectsVolume", 8f);
        */

        PlayerPrefs.SetInt("Berries", 0);
        PlayerPrefs.SetInt("BlueBerries", 0);

        foreach (string s in levelNames)
        {
            string newString = "";
            for (int bb = 0; bb < PlayerPrefs.GetInt(s + "BlueBerryTotal"); bb++)
                newString = newString.Insert(0, "0");
            PlayerPrefs.SetString(s + "BlueBerry", newString);
        }

        yield return null;

        bbic.ReEnableBerries();
        foreach (GameObject g in GameObject.FindGameObjectsWithTag("ToActivate"))
        {
            if (g.gameObject.scene.name == "Level1.2")
            {
                GameObject curBB = g.gameObject.transform.Find("BerriesBlue").gameObject;
                if (curBB.GetComponent<BlueBerryIDCreator>() != null)
                    curBB.GetComponent<BlueBerryIDCreator>().ReEnableBerries();
            }
        }
        PlayerPrefs.SetInt("TotalRedBerries", 0);
        PlayerPrefs.SetInt("AllBlueBerries", 0);
        PlayerPrefs.SetInt("BlueBerryTotal", 0);
        PlayerPrefs.SetInt("HalfBlueBerries", 0);

        //TEARS
        for (int lvl = 1; lvl <= 8; lvl++)
        {
            for (int tearNo = 1; tearNo <= 3; tearNo++)
                PlayerPrefs.SetInt("Vase" + tearNo.ToString() + "Level" + lvl.ToString(), 0);
            PlayerPrefs.SetInt("Played" + lvl.ToString() + "TreeCompletion", 0);
        }

        //CHECKPOINTS
        for (int cp = 1; cp <= 37; cp++)
        {
            PlayerPrefs.SetInt("Checkpoint" + cp.ToString(), 0);
            //    PlayerPrefs.SetInt("Checkpoint" + cp.ToString() + "Scene", 0);
        }
        PlayerPrefs.SetInt("Checkpoint0", 1);
        PlayerPrefs.SetInt("Checkpoint0Scene", 2);

        //SHOPS
        for (int xyz = 0; xyz < 25; xyz++)
        {
            PlayerPrefs.SetInt("UsingItem" + xyz.ToString(), 0);
            PlayerPrefs.SetInt("PaidForItem" + xyz.ToString(), 0);
        }

        //BUTTONS
        for (int ab = 0; ab <= 124; ab++)
            PlayerPrefs.SetInt("Button" + ab.ToString(), 0);

        //PROMPTS
        PlayerPrefs.SetInt("FirstCollectable", 0);
        PlayerPrefs.SetInt("FirstBorder", 0);

        //COLLECTABLES
        PlayerPrefs.SetInt("marmellade1", 0);
        PlayerPrefs.SetInt("marmellade2", 0);
        PlayerPrefs.SetInt("marmellade3", 0);
        PlayerPrefs.SetInt("plantjar", 0);
        PlayerPrefs.SetInt("plantjar2", 0);
        PlayerPrefs.SetInt("book1", 0);
        PlayerPrefs.SetInt("book2", 0);
        PlayerPrefs.SetInt("book3", 0);
        PlayerPrefs.SetInt("paint", 0);
        PlayerPrefs.SetInt("paint2", 0);
        PlayerPrefs.SetInt("gameboy", 0);
        PlayerPrefs.SetInt("bell", 0);
        PlayerPrefs.SetInt("heater", 0);
        PlayerPrefs.SetInt("globe", 0);
        PlayerPrefs.SetInt("cupbear", 0);
        PlayerPrefs.SetInt("compass", 0);
        PlayerPrefs.SetInt("carpet", 0);
        PlayerPrefs.SetInt("candle", 0);
        PlayerPrefs.SetInt("statue1", 0);
        PlayerPrefs.SetInt("statue2", 0);
        PlayerPrefs.SetInt("statue3", 0);
        PlayerPrefs.SetInt("mask1", 0);
        PlayerPrefs.SetInt("mask2", 0);
        PlayerPrefs.SetInt("mask3", 0);
        PlayerPrefs.SetInt("map", 0);
        PlayerPrefs.SetInt("jukebox", 0);
        PlayerPrefs.SetInt("inbox", 0);
        PlayerPrefs.SetInt("CollectablesTotal", 0);
        GameObject.Find("Collectables").GetComponent<ActivateItemsExtra>().DeactivateAllInHouse();

        //CAGES
        for (int ca = 0; ca < CagedNPCManager.GetAmount(); ca++)
        {
            PlayerPrefs.SetInt("Cage" + ca.ToString(), 0);
            CagedNPCManager.SpecialCaseCheck(ca);
        }

        //CHALLENGES
        for (int ch = 0; ch <= 11; ch++)
        {
            PlayerPrefs.SetInt("Challenge" + ch.ToString() + "Found", 0);
        }
        ps.UpdateChallengePortalIcons();
        PlayerPrefs.SetInt("AllFlowers", 0);

        //CUTSCENES
        PlayerPrefs.SetInt("IntroWatched", 0);
        PlayerPrefs.SetInt("Intro2Watched", 0);
        PlayerPrefs.SetInt("SeenLogo", 0);
        PlayerPrefs.SetInt("First3Tears", 0);
        PlayerPrefs.SetInt("FinalBossDefeated", 0);

        //TEXT PROMPTS
        PlayerPrefs.SetInt("FirstRedBerry", 0);
        PlayerPrefs.SetInt("FirstBlueBerry", 0);
        PlayerPrefs.SetInt("FirstCheckpoint", 0);
        PlayerPrefs.SetInt("FirstTear", 0);
        PlayerPrefs.SetInt("GotHurt", 0);
        PlayerPrefs.SetInt("WrongLeafBlock", 0);
        PlayerPrefs.SetInt("WrongLeafBlockBlack", 0);
        PlayerPrefs.SetInt("AllTears", 0);

        /*PlayerPrefs.SetInt("Vibration", vibInt);
          PlayerPrefs.SetFloat("musicVolume", musicFT);
          PlayerPrefs.SetFloat("effectsVolume", effectsFT);
          PlayerPrefs.SetInt("AA", aaInt);*/

        PlayerPrefs.SetInt("Checkpoint0", 1);
        PlayerPrefs.SetInt("Checkpoint0Scene", 2);

        //ACHIEVEMENTS
        PlayerPrefs.SetInt("DarkKilled", 0);
        PlayerPrefs.SetInt("NaturalKilled", 0);
        PlayerPrefs.SetInt("BoughtItems", 0);
        PlayerPrefs.SetInt("BoughtPowers", 0);
        PlayerPrefs.GetInt("PTree26", 0);
        PlayerPrefs.GetInt("PTree27", 0);
        PlayerPrefs.GetInt("PTree28", 0);
        PlayerPrefs.GetInt("PTree29", 0);

        tpc.berryCount = 0;
        tpc.blueberryCount = 0;

        bbic.ReEnableBerries();
        foreach (GameObject g in GameObject.FindGameObjectsWithTag("ToActivate"))
        {
            if (g.gameObject.scene.name == "Level1.2")
            {
                GameObject curBB = g.gameObject.transform.Find("BerriesBlue").gameObject;
                if (curBB.GetComponent<BlueBerryIDCreator>() != null)
                    curBB.GetComponent<BlueBerryIDCreator>().ReEnableBerries();
            }
        }
        foreach (ActivateItemsMasks a in aiM)
            a.UntouchThis();
        foreach (ActivateItemsLeafs a in aiL)
            a.UnTouchThis();
        foreach (ActivateBonusAbility a in aiB)
            a.UnTouchThis();
        ItemPromptManager.UpdateAllStatuses();
        WaterTearManager.AtivateAllTears();
        WaterTearManager.UpdateTears();

        PlayerPrefs.Save();

        deletingFile = false;

        deleteButton.gameObject.SetActive(true);
        noButton.gameObject.SetActive(true);

#if UNITY_XBOXONE && !UNITY_EDITOR
        DataManager.xOneEventsManager.SaveProgs();
#endif
        CloseAYS();
    }


    //SOUNDS
    void PlaySound(AudioClip c)
    {
        if (Time.timeSinceLevelLoad > 0.5f)
        {
            if (soundInt == 0)
            {
                sound1.Stop();
                sound1.clip = c;
                sound1.PlayDelayed(0f);
            }
            else
            {
                sound2.Stop();
                sound2.clip = c;
                sound2.PlayDelayed(0f);
            }
            soundInt++;
            if (soundInt > 1)
                soundInt = 0;
        }
    }

    IEnumerator FadeIn()
    {
        /*
        loadAnim.SetBool("Loading", true);
        yield return new WaitForSeconds(1.1f);
        if (loadOneAndSeven)
        {
            AsyncOperation async1 = new AsyncOperation();
            async1 = SceneManager.LoadSceneAsync(3, LoadSceneMode.Additive);

            while (!async1.isDone)
            {
                loadIcon.fillAmount = Mathf.Lerp(loadIcon.fillAmount, async1.progress / 2f, 0.8f);
                yield return null;
            }
            AsyncOperation async2 = new AsyncOperation();
            async2 = SceneManager.LoadSceneAsync(9, LoadSceneMode.Additive);

            while (!async2.isDone)
            {
                loadIcon.fillAmount = Mathf.Lerp(loadIcon.fillAmount, 0.5f + (async2.progress / 2f), 0.8f);
                yield return null;
            }
            
            foreach (GameObject g in GameObject.FindGameObjectsWithTag("WholeLevel"))
            {
                if (g.name == "Level1.2")
                {
                    firstLevelLoadZone.fullSceneObject = g.gameObject;
                    g.gameObject.SetActive(false);
                }
                if(g.name == "Level7")
                {
                    seventhLevelLoadZone.fullSceneObject = g.gameObject;
                    g.gameObject.SetActive(false);
                }
            }
        }
        loadAnim.SetBool("Loading", false);
        */
        /*
        yield return null;
        yield return null;
        yield return null;
        yield return null;
        yield return null;
        yield return null;
        yield return null;
        yield return null;
        yield return null;

        if (loadOneAndSeven)
        {

            foreach (GameObject g in GameObject.FindGameObjectsWithTag("WholeLevel"))
            {
                if (g.name == "Level1.2")
                {
                    firstLevelLoadZone.fullSceneObject = g.gameObject;
                    g.gameObject.SetActive(false);
                }
                if (g.name == "Level7")
                {
                    seventhLevelLoadZone.fullSceneObject = g.gameObject;
                    g.gameObject.SetActive(false);
                }
            }
        }
        */
        mainScreen.SetActive(true);
        titleText.SetActive(true);
        for (int f = 59; f >= 0; f--)
        {
            AudioListener.volume = Mathf.Lerp(1f, 0f, (f * 1f) / 60f);
            loadFS.color = Color.Lerp(Color.clear, Color.black, (f * 1f) / 60f);
            yield return null;
        }
    }

    [HideInInspector] public bool isLoading;
    IEnumerator LoadIt(int whichCheckpoint, int sceneToGoTo)
    {
        isLoading = true;
        Debug.Log("LOADING: " + whichCheckpoint + " @ " + sceneToGoTo);
    //    UnityEditor.EditorApplication.isPaused = true;
        for (int lol = 1; lol <= 60; lol++)
            yield return null;

        Debug.LogError("A");

        loadAnim.SetBool("Loading", true);
        loadIcon.fillAmount = 0f;

        if (tpc != null && tpc.GetComponentInParent<OneWayManager>() != null)
            tpc.GetComponentInParent<OneWayManager>().currentlyChecking = true;

        Debug.LogError("B");


        for (int f = 1; f <= 60; f++)
        {
            AudioListener.volume = Mathf.Lerp(1f, 0f, (f * 1f) / 60f);
            loadFS.color = Color.Lerp(Color.clear, Color.black, (f * 1f) / 60f);
            yield return null;
        }
        this.transform.GetChild(0).gameObject.SetActive(false);

        cam.transform.localEulerAngles = new Vector3(0f, 180f, 0f);

        Debug.LogError("C");


        if (newLoading)
        {
            Debug.LogError("D");

            /*    float xf = 0f;
                for (int x = 2; x <= 10; x++)
                {
                    xf = x * 1f;
                    AsyncOperation async1 = new AsyncOperation();
                    async1 = SceneManager.LoadSceneAsync(x, LoadSceneMode.Additive);

                    while (!async1.isDone)
                    {
                        loadIcon.fillAmount = Mathf.Lerp(loadIcon.fillAmount, ((xf - 2f) / 9f) + (async1.progress / 9f), 0.8f);
                        yield return null;
                    }
                }

                yield return new WaitForEndOfFrame();
                fullLevels = GameObject.FindGameObjectsWithTag("WholeLevel");
                foreach (GameObject g in fullLevels)
                {
                    if (g.scene.buildIndex != sceneToGoTo)
                        g.SetActive(false);
                }*/
        }
        else
        {
            Debug.LogError("E");

            /*    if (sceneToGoTo > 4 && SceneManager.GetSceneByBuildIndex(4))
                {
                    AsyncOperation async0 = new AsyncOperation();
                    async0 = SceneManager.UnloadSceneAsync(4);

                    while (!async0.isDone)
                        yield return null;
                }*/
            yield return null;

       //     if (whichCheckpoint >= 2 || sceneToGoTo == -1)
            {
                AsyncOperation async1 = new AsyncOperation();
                async1 = SceneManager.LoadSceneAsync(3, LoadSceneMode.Additive);

                while (!async1.isDone)
                {
                    if (whichCheckpoint > 2)
                        loadIcon.fillAmount = Mathf.Lerp(loadIcon.fillAmount, async1.progress / 2f, 0.8f);
                    else
                        loadIcon.fillAmount = Mathf.Lerp(loadIcon.fillAmount, async1.progress, 0.8f);
                    yield return null;
                }

                Debug.LogError("F");

            }
            //    Debug.Log(sceneToGoTo);
            if (sceneToGoTo > 3)
            {
                AsyncOperation async2 = new AsyncOperation();
                async2 = SceneManager.LoadSceneAsync(sceneToGoTo, LoadSceneMode.Additive);

                while (!async2.isDone)
                {
                    loadIcon.fillAmount = Mathf.Lerp(loadIcon.fillAmount, 0.5f + (async2.progress / 2f), 0.8f);
                    yield return null;
                }
                lowPolys[sceneToGoTo - 4].SetActive(false);
            }

            Debug.LogError("G");

        }


        if (curLPT != null)
        {
            yield return null;
            yield return null;
            yield return null;
            curLPT.EnterTriggerForced();
        }

        Debug.LogError("H");


        if (whichCheckpoint == 0 || whichCheckpoint == 1)
        {
            lowPolyTriggers[0].currentlyInside = false;
            lowPolyTriggers[0].EnterTriggerForced();
        }

        Debug.LogError("I");


        //     lowPolyTriggers[0].enabled = false;

        if (whichCheckpoint == 3 || whichCheckpoint == 4 || whichCheckpoint == 5)
            lowPolyTriggers[1].EnterTriggerForced();
        if (whichCheckpoint == 7 || whichCheckpoint == 8)
            lowPolyTriggers[2].EnterTriggerForced();
        if (whichCheckpoint == 10 || whichCheckpoint == 11)
            lowPolyTriggers[3].EnterTriggerForced();
        if (whichCheckpoint == 13 || whichCheckpoint == 14)
            lowPolyTriggers[4].EnterTriggerForced();
        if (whichCheckpoint == 16 || whichCheckpoint == 17 || whichCheckpoint == 37)
        {
            if (atmosphereManager.curLevel != "Level5")
            {
                atmosphereManager.EnterTrigger(2);
                atmosphereManager.curLevel = "Level5";
            }
            lowPolyTriggers[5].EnterTriggerForced();
        }
        if (whichCheckpoint == 18 || whichCheckpoint == 19 || whichCheckpoint == 20 || whichCheckpoint == 27)
            lowPolyTriggers[6].EnterTriggerForced();
        if (whichCheckpoint == 22 || whichCheckpoint == 23)
            lowPolyTriggers[7].EnterTriggerForced();
        if (whichCheckpoint == 24 || whichCheckpoint == 25 || whichCheckpoint == 26)
            lowPolyTriggers[8].EnterTriggerForced();

        if (whichCheckpoint != 16 && whichCheckpoint != 17 && whichCheckpoint != 37 && atmosphereManager.curLevel == "Level5")
        {
            atmosphereManager.ExitTrigger();
            atmosphereManager.curLevel = "";
        }

        Debug.LogError("L");


        if (PlayerPrefs.GetInt("IntroWatched", 0) == 1)
        {
            Debug.LogError("M");

            Time.timeScale = 1f;
            tpc.gameObject.transform.position = new Vector3(PlayerPrefs.GetFloat("Checkpoint" + whichCheckpoint + "X", 0f),
                PlayerPrefs.GetFloat("Checkpoint" + whichCheckpoint + "Y", 0f), PlayerPrefs.GetFloat("Checkpoint" + whichCheckpoint + "Z", 0f));
            tpcFox.gameObject.transform.position = tpc.gameObject.transform.position + (Vector3.up * 4f);
            tpcBeaver.gameObject.transform.position = tpc.gameObject.transform.position + (Vector3.up * 4f);
            tpcBush.gameObject.transform.position = tpc.gameObject.transform.position + (Vector3.up * 4f);
            cam.disableControl = false;
            tpc.rb.isKinematic = true;

            if (PlayerPrefs.GetInt("Checkpoint" + whichCheckpoint.ToString() + "Scene", 0) > 3)
            {
                Debug.LogError("N");

                LoadLevelAdditive lla = loadLevelAdditives[PlayerPrefs.GetInt("Checkpoint" + whichCheckpoint.ToString() + "Scene", 0) - 4];
                if (lla.lptCollider != null)
                    lla.lptCollider.enabled = true;

                Debug.LogError("N-1");

                lla.FindFullLevel();
                if (lla.fullSceneObject != null)
                    odm.ActivateObject(lla.fullSceneObject);

                Debug.LogError("N-2");

                if (lla.entTrig)
                    lla.entTrig.colliderToActivate.enabled = true;

                Debug.LogError("N-3");

                lowPolys[PlayerPrefs.GetInt("Checkpoint" + whichCheckpoint.ToString() + "Scene", 0) - 4].SetActive(false);
            }
            /*
            foreach(GameObject g in GameObject.FindGameObjectsWithTag("ToActivate"))
            {
                if(g.transform.parent != null && g.transform.parent.name == 
                    SceneManager.GetSceneByBuildIndex(PlayerPrefs.GetInt("Checkpoint" + whichCheckpoint.ToString() + "Scene")).name)
                {
                    for(int c = 0; c < g.transform.childCount; c++)
                        g.transform.GetChild(c).gameObject.SetActive(true);

                    for (int c = 0; c < g.transform.childCount; c++)
                        g.transform.GetChild(c).gameObject.SetActive(true);
                }
            }
            */

            //    yield return new WaitForSeconds(5f);
            Debug.LogError("N-4");

            yield return new WaitForSeconds(1f);
            tpc.GetComponentInParent<OneWayManager>().CheckOneWays();

            Debug.LogError("N-5");

            while (!CheckpointLoaded())
                yield return null;

            Debug.LogError("O");

            tpc.rb.isKinematic = false;

            loadAnim.SetBool("Loading", false);
            tpc.transform.forward = cam.transform.forward;
            for (int f = 59; f >= 0; f--)
            {
                if (f == 30)
                    EndLoading(whichCheckpoint);
                AudioListener.volume = Mathf.Lerp(1f, 0f, (f * 1f) / 60f);
                loadFS.color = Color.Lerp(Color.clear, Color.black, (f * 1f) / 60f);
                yield return null;
            }


            Debug.LogError("P");

        }
        else
        {
            Debug.LogError("Q");


            loadAnim.SetBool("Loading", false);
            for (int f = 59; f >= 0; f--)
            {
                AudioListener.volume = Mathf.Lerp(1f, 0f, (f * 1f) / 60f);
                loadFS.color = Color.Lerp(Color.white, Color.black, (f * 1f) / 60f);
                yield return null;
            }

            Debug.LogError("R");


            Time.timeScale = 1f;
            loadFS.color = Color.clear;
            PlayerPrefs.SetInt("IntroWatched", 1);
            PlayerPrefs.SetInt("LastCheckpoint", 0);
            //    PlayerPrefs.Save();
            ps.StartCutscene(firstCutscene, Vector3.one);
            WaterTearManager.UpdateTears();

            yield return new WaitForSeconds(1f);
            tpc.GetComponentInParent<OneWayManager>().CheckOneWays();

            Debug.LogError("S");

        }


        if (whichCheckpoint >= 4 && whichCheckpoint <= 5)
            tpc.ps.ShowLevelTitle("Level1.2");
        if (sceneToGoTo == 5)
            tpc.ps.ShowLevelTitle("Level2");
        if (sceneToGoTo == 6)
            tpc.ps.ShowLevelTitle("Level3");
        if (sceneToGoTo == 7)
            tpc.ps.ShowLevelTitle("Level4");
        if (sceneToGoTo == 8)
            tpc.ps.ShowLevelTitle("Level5");
        if (sceneToGoTo == 9)
            tpc.ps.ShowLevelTitle("Level6");
        if (sceneToGoTo == 10)
            tpc.ps.ShowLevelTitle("Level7");
        if (sceneToGoTo == 11)
            tpc.ps.ShowLevelTitle("Level8");

        /*
        lowPolyTriggers[0].enabled = true;
        if(whichCheckpoint != 2 && whichCheckpoint != 6 && whichCheckpoint != 12 && whichCheckpoint != 15 && whichCheckpoint != 21)
            lowPolyExt.SetActive(true);
        */
        isLoading = false;

        Debug.LogError("T");

    }

    private bool CheckpointLoaded()
    {
        foreach (Checkpoint c in Resources.FindObjectsOfTypeAll<Checkpoint>())
        {
            if (c.checkpointID == PlayerPrefs.GetInt("LastCheckpoint", 0))
                return true;
        }
        return false;
    }/*
    private bool CheckpointLoaded()
    {
        foreach (GameObject g in GameObject.FindGameObjectsWithTag("Checkpoint"))
        {
            if (g.GetComponent<Checkpoint>().checkpointID == PlayerPrefs.GetInt("LastCheckpoint", 0))
                return true;
        }
        return false;
    }*/

    IEnumerator EndOfStart()
    {
        bool foundOldData = false;
        foreach (string s in levelNames)
        {
            if (!foundOldData)
            {
                for (int i = 0; i < PlayerPrefs.GetInt(s + "BlueBerryTotal"); i++)
                {
                    if (!foundOldData && PlayerPrefs.HasKey(s + "BlueBerry" + i.ToString()))
                        foundOldData = true;
                }
            }
        }

        if (foundOldData)
        {
            int totalBB = 0;
            int counter = 0;
            foreach (string s in levelNames)
            {
                string newString = "";
                for (int i = 0; i < PlayerPrefs.GetInt(s + "BlueBerryTotal"); i++)
                {
                    newString = newString.Insert(newString.Length, PlayerPrefs.GetInt(s + "BlueBerry" + i.ToString(), 0).ToString());

                    if (PlayerPrefs.GetInt(s + "BlueBerry" + i.ToString()) == 1)
                        totalBB++;

                    PlayerPrefs.DeleteKey(s + "BlueBerry" + i.ToString());

                    counter++;

                    if (counter % 10 == 0)
                        yield return null;
                }
                PlayerPrefs.SetString(s + "BlueBerry", newString);
            }
            PlayerPrefs.SetInt("BlueBerries", totalBB);
            tpc.blueberryText.text = totalBB.ToString();
            PlayerPrefs.Save();
        }

        yield return null;

        InitializeValues();
        MainX(true);

        SetCheckpointScenes();

        StartCoroutine(FadeIn());
    }

    void EndLoading(int whichCheckpoint)
    {
        tpc.inCutscene = false;
        tpcFox.inCutscene = false;
        tpcBeaver.inCutscene = false;
        tpcBush.inCutscene = false;
        tpc.disableControl = false;
        tpcFox.disableControl = false;
        tpcBeaver.disableControl = false;
        tpcBush.disableControl = false;
        ps.cantPause = false;
    }

    public void OpenDefaultsAYS()
    {
        defaultsAYS.SetActive(true);
        es.SetSelectedGameObject(defaultsNo);
    }

    public void CloseDefaultsAYS()
    {
        defaultsAYS.SetActive(false);
        es.SetSelectedGameObject(defaultsButton);
    }

    public void SetCheckpointScenes()
    {
        //PL
        PlayerPrefs.SetInt("Checkpoint0Scene", 2);
        PlayerPrefs.SetInt("Checkpoint1Scene", 2);
        PlayerPrefs.SetInt("Checkpoint2Scene", 2);

        //1
        PlayerPrefs.SetInt("Checkpoint3Scene", 4);
        PlayerPrefs.SetInt("Checkpoint4Scene", 4);
        PlayerPrefs.SetInt("Checkpoint5Scene", 4);

        //2
        PlayerPrefs.SetInt("Checkpoint6Scene", 5);
        PlayerPrefs.SetInt("Checkpoint7Scene", 5);
        PlayerPrefs.SetInt("Checkpoint8Scene", 5);

        PlayerPrefs.SetInt("Checkpoint32Scene", 5);
        PlayerPrefs.SetInt("Checkpoint33Scene", 5);

        //3
        PlayerPrefs.SetInt("Checkpoint9Scene", 6);
        PlayerPrefs.SetInt("Checkpoint10Scene", 6);
        PlayerPrefs.SetInt("Checkpoint11Scene", 6);

        PlayerPrefs.SetInt("Checkpoint36Scene", 6);

        //4
        PlayerPrefs.SetInt("Checkpoint12Scene", 7);
        PlayerPrefs.SetInt("Checkpoint13Scene", 7);
        PlayerPrefs.SetInt("Checkpoint14Scene", 7);

        PlayerPrefs.SetInt("Checkpoint30Scene", 7);
        PlayerPrefs.SetInt("Checkpoint31Scene", 7);

        //5
        PlayerPrefs.SetInt("Checkpoint15Scene", 8);
        PlayerPrefs.SetInt("Checkpoint16Scene", 8);
        PlayerPrefs.SetInt("Checkpoint17Scene", 8);

        PlayerPrefs.SetInt("Checkpoint37Scene", 8);

        //6
        PlayerPrefs.SetInt("Checkpoint18Scene", 9);
        PlayerPrefs.SetInt("Checkpoint19Scene", 9);
        PlayerPrefs.SetInt("Checkpoint20Scene", 9);

        PlayerPrefs.SetInt("Checkpoint27Scene", 9);

        //7
        PlayerPrefs.SetInt("Checkpoint21Scene", 10);
        PlayerPrefs.SetInt("Checkpoint22Scene", 10);
        PlayerPrefs.SetInt("Checkpoint23Scene", 10);

        PlayerPrefs.SetInt("Checkpoint28Scene", 10);
        PlayerPrefs.SetInt("Checkpoint29Scene", 10);

        //8
        PlayerPrefs.SetInt("Checkpoint24Scene", 11);
        PlayerPrefs.SetInt("Checkpoint25Scene", 11);
        PlayerPrefs.SetInt("Checkpoint26Scene", 11);

        PlayerPrefs.SetInt("Checkpoint34Scene", 11);
        PlayerPrefs.SetInt("Checkpoint35Scene", 11);

        //    PlayerPrefs.Save();
    }

    void ReUnlockAchievsAfterConnection()
    {
#if UNITY_XBOXONE
        // the basics
        if (PlayerPrefs.GetInt("FirstCheckpoint", 0) == 1)
            XONEAchievements.SubmitAchievement((int)XONEACHIEVS.THE_BASICS);
        if (PlayerPrefs.GetInt("First3Tears", 0) == 1)
            XONEAchievements.SubmitAchievement((int)XONEACHIEVS.FIRST_TREE_SAGE_RESTORED);

        int totalRBCount = PlayerPrefs.GetInt("FirstCheckpoint", 0);
        if (totalRBCount >= 100)
            XONEAchievements.SubmitAchievement((int)XONEACHIEVS.RED_BERRIES_LOVER);
        //
        if (totalRBCount >= 1000)
            XONEAchievements.SubmitAchievement((int)XONEACHIEVS.RED_BERRIES_PARADE);
        //
        if (totalRBCount >= 3000)
            XONEAchievements.SubmitAchievement((int)XONEACHIEVS.RED_BERRIES_CHAMPION);
        //
        int totalBBCount = PlayerPrefs.GetInt("BlueBerryTotal", 0);
        if (totalBBCount >= 100)
            XONEAchievements.SubmitAchievement((int)XONEACHIEVS.RED_BERRIES_LOVER);
        //
        if (totalBBCount >= 100)
            XONEAchievements.SubmitAchievement((int)XONEACHIEVS.BLUE_BERRIES_LOVER);
        //
        if (totalBBCount >= 930)
            XONEAchievements.SubmitAchievement((int)XONEACHIEVS.BLUE_BERRIES_CHAMPION);
        //
        int counted = PlayerPrefs.GetInt("marmellade1", 0) +
       PlayerPrefs.GetInt("marmellade2", 0) +
       PlayerPrefs.GetInt("marmellade3", 0) +
       PlayerPrefs.GetInt("plantjar", 0) +
       PlayerPrefs.GetInt("plantjar2", 0) +
       PlayerPrefs.GetInt("book1", 0) +
       PlayerPrefs.GetInt("book2", 0) +
       PlayerPrefs.GetInt("book3", 0) +
       PlayerPrefs.GetInt("paint", 0) +
       PlayerPrefs.GetInt("paint2", 0) +
       PlayerPrefs.GetInt("gameboy", 0) +
       PlayerPrefs.GetInt("bell", 0) +
       PlayerPrefs.GetInt("heater", 0) +
       PlayerPrefs.GetInt("globe", 0) +
       PlayerPrefs.GetInt("cupbear", 0) +
       PlayerPrefs.GetInt("compass", 0) +
       PlayerPrefs.GetInt("carpet", 0) +
       PlayerPrefs.GetInt("candle", 0) +
       PlayerPrefs.GetInt("statue1", 0) +
       PlayerPrefs.GetInt("statue2", 0) +
       PlayerPrefs.GetInt("statue3", 0) +
       PlayerPrefs.GetInt("mask1", 0) +
       PlayerPrefs.GetInt("mask2", 0) +
       PlayerPrefs.GetInt("mask3", 0) +
       PlayerPrefs.GetInt("map", 0) +
       PlayerPrefs.GetInt("jukebox", 0) +
       PlayerPrefs.GetInt("inbox", 0);

        if (counted >= 27)
             XONEAchievements.SubmitAchievement((int)XONEACHIEVS.BEAUTIFUL_WOODLE_HOUSE);

        if (counted >= 11)
            XONEAchievements.SubmitAchievement((int)XONEACHIEVS.WONDERFUL_WOODLE_HOUSE);

        if (PlayerPrefs.GetInt("FinalBossDefeated", 0) == 1)
            XONEAchievements.SubmitAchievement((int)XONEACHIEVS.WOODLE_SAVIOR);

        //
        int countedPrefs = 0;
        for (int i = 0; i < 4; i++)
            countedPrefs += PlayerPrefs.GetInt("Cage" + i.ToString(), 0);
        if (countedPrefs >= 4)
            XONEAchievements.SubmitAchievement((int)XONEACHIEVS.MUSIC_LOVER);

        if (PlayerPrefs.GetInt("AllFlowers", 0) == 1)
            XONEAchievements.SubmitAchievement((int)XONEACHIEVS.AWAKING_FROM_A_LONG_SLEEP);

        if (PlayerPrefs.GetInt("PaidItemsCount", 0) >= 3)
            XONEAchievements.SubmitAchievement((int)XONEACHIEVS.GO_SHOPPING);

        if (PlayerPrefs.GetInt("PaidItemsCount", 0) >= 17)
            XONEAchievements.SubmitAchievement((int)XONEACHIEVS.GO_SHOPPING_FOR_EVERYTHING);

        if (PlayerPrefs.GetInt("WoodleFriends", 0) == 1)
            XONEAchievements.SubmitAchievement((int)XONEACHIEVS.WOODLE_FRIENDS);

        //
        int normalKilledEnemies;
        if (PlayerPrefs.HasKey("NormalEnemiesKilledCount"))
        {
            normalKilledEnemies = PlayerPrefs.GetInt("NormalEnemiesKilledCount");
            //
            if (normalKilledEnemies >= 100)
                XONEAchievements.SubmitAchievement((int)XONEACHIEVS.WOODLE_WARRIOR);
        }

        int darkKilledEnemies;
        if (PlayerPrefs.HasKey("DarkEnemiesKilledCount"))
        {
            darkKilledEnemies = PlayerPrefs.GetInt("DarkEnemiesKilledCount");
            //
            if (darkKilledEnemies >= 100)
                XONEAchievements.SubmitAchievement((int)XONEACHIEVS.WOODLE_AVENGER);
        }

        if (PlayerPrefs.GetInt("IsThere", 0) == 1)
            XONEAchievements.SubmitAchievement((int)XONEACHIEVS.IS_THERE_ANYONE);

        if (PlayerPrefs.GetInt("Top", 0) == 1)
            XONEAchievements.SubmitAchievement((int)XONEACHIEVS.TO_THE_TOP);

        if (PlayerPrefs.GetInt("PowerUpsBought", 0) >= 4)
            XONEAchievements.SubmitAchievement((int)XONEACHIEVS.WOODLE_POWER);
#endif
    }
}