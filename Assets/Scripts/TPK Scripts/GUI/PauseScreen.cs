using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using Rewired;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;

//using nn.hid;
using Beffio.Dithering;
using System.Linq;
using UnityStandardAssets.ImageEffects;

public class PauseScreen : MonoBehaviour
{

    public bool enableDebugTab;
    double cutTime;

    public Vector2 mapscale;

    public GameObject woodle;
    public GameObject foxChara;
    public GameObject beaverChara;
    public GameObject bushChara;

    [HideInInspector] public TextToTranslate textToTranslate;

    [Header("MISC")]
    public Animator skipCSAnim;
    public DisableControlForCutscenes cutsceneControl;
    PlayableDirector currentCutscene;
    Vector3 cutsceneEndPos;

    public EventSystem es;
    TPC tpc;
    [HideInInspector] public TPC[] multiTPC = new TPC[3];
    Player input;
    public CameraFollower cam;
    public GameObject main;
    [HideInInspector] public bool inPause;
    public StartScreen sS;
    GameObject prevSelected;
    bool justOpenedMap;
    public Telescope telescope;
    public TextTriggerMain textMain;
    public ObjDeactivateManager odm;
    public Stylizer stylizerCS;
    public AtmosphereManager atmosphereManager;
    public ActivateOnTriggerEnter underwaterCameraTrigger;

    [HideInInspector] public LowPolyTrigger curLPT;
    public LowPolyTrigger[] lowPolyTriggers;

    public Animator foxAnim;
    public Animator beaverAnim;
    public Animator bushAnim;

    public Animator challengeWarpAnim;

    public Animator racePromptAnim;
    public Animator raceCountdownAnim;
    public Image whiteFade;
    public Animator raceResultsAnim;
    public Image raceResultsIcon;
    public Animator raceQuitAnim;
    public Text raceRewardText;

    public GraphicRaycaster graphicRaycaster;

    public GameObject quitAYS;
    public GameObject quitFirst;
    public GameObject quitSelector;

    [Header("TABS")]
    public GameObject itemsTab;
    public GameObject mapTab;
    public GameObject optionsTab;
    public GameObject debugTab;
    public GameObject[] selectPos;
    public GameObject selectIcon;
    public GameObject[] dots;
    public GameObject dotWhite;
    public Text titleText;
    bool inItems;
    bool inMap;
    bool inOptions;
    bool inDebug;

    public Animator checkpointAnim;

    [Header("DEBUG")]
    public GameObject firstDebug;
    public Slider clippin;
    public Text clipText;
    public GameObject actTick;
    [HideInInspector] public bool deactivatedAll;
    GameObject[] toactives = new GameObject[10];
    [HideInInspector] public bool[] deactToActs = new bool[10];
    public GameObject[] toActivateTicks = new GameObject[10];
    public GameObject debugCPTick;
    bool aaIsOn;
    public GameObject aaTick;

    bool dguiIsOn;
    public GameObject dguiTick;
    public GameObject fpsObj;
    bool expandIsOn;
    public GameObject expandTick;

    bool playThreeTears;
    bool playAllTears;

    public Dropdown levelDD;
    public Dropdown toActDD;
    bool initCheck;
    bool[] deactivatedToActObj;
    public GameObject toActDDTick;
    public GameObject npcFlowers;

    [Header("ITEMS")]
    public Text berries;
    public Text blueBerries;
    public Text waterTears;
    public Text collectables;
    [HideInInspector] public int currentBBCount;
    public GameObject bbFinderIcon;

    [Header("MAP")]
    public GameObject firstMap;
    public GameObject[] mapPos;
    public GameObject mapIcon;
    public GameObject mapAYS;
    public GameObject mapYesButton;
    GameObject curWarp;
    public GameObject woodleIcon;
    public GameObject woodleIconRot;
    public RectTransform mapRect;
    public GameObject waterTearsParent;
    public Text[] blueCounters;
    public GameObject externalLowPoly;
    public GameObject[] lowerPolyLevels = new GameObject[8];
    public GameObject plazaMain;
    public GameObject plazaLow;
    public ActivateAtDistanceAndUnloadLevel[] aadul;
    Vector3 woodlePos;
    GameObject curES;
    public Text mapMarkerText;
    string mapText;
    [Range(0f, 1f)] public float mapScaleSpeed;
    public GameObject[] altScaleObjs;
    float curMapScale;
    [HideInInspector] public bool warping;
    public Image[] challengesIcons;
    public Color challengesFoundColour;
    public Color challengescClearedColour;
    public GameObject mapAYSSelection;

    [Header("OPTIONS")]
    public GameObject firstOptions;
    public GameObject ixTick;
    public GameObject iyTick;
    public GameObject vibTick;
    public GameObject styTick;
    [HideInInspector] public bool stying;

    int currentLevelID;

    bool dPlzExt;
    GameObject fullExt;

    public Slider sensitivity;
    public Slider effectsSlider;
    public Slider musicSlider;
    public AudioMixer[] audioMixers;
    public GameObject returnButton;
    public Text returnText;
    public GameObject yesButton;
    public GameObject noButton;
    public Text yesText;
    public Text noText;
    public Image loadIcon;
    public Image loadFS;
    public Animator loadAnim;
    public Dropdown qualityT;
    bool invertX;
    bool invertY;
    bool vibOn;
    float effects;
    [HideInInspector] public float music;
    float clip;

    public Button aaLButton;
    public Button aaRButton;
    public Button aaAButton;
    public Button filterButton;

    bool languagesOpen;
    public GameObject languagesList;
    public GameObject languagesFirst;
    public GameObject languageButton;

    [HideInInspector] public OptionsNew optionsNew;
    [HideInInspector] public bool remapperOpen;

    public GameObject defaultsAYS;
    public GameObject defaultsNo;
    public GameObject defaultsButton;

    [Header("SOUNDS")]
    public AudioClip highlightSound;
    public AudioClip highlightDownSound;
    public AudioClip selectSound;
    public AudioClip tabRSound;
    public AudioClip tabLSound;
    public AudioClip warpSound;
    public AudioClip openSound;
    public AudioClip closeSound;
    public AudioClip exitSound;
    public AudioClip toggleOnSound;
    public AudioClip toggleOffSound;
    AudioSource sound1;
    AudioSource sound2;
    int soundInt;

    [HideInInspector] public bool cantPause;
    [HideInInspector] public bool inLoad;

    [HideInInspector] public int tearCount;

    [Header("LEVEL TITLES")]
    public Animator levelTitlesAnim;
    public Text levelTitlesText;
    int currentLevelTitle;

    Font originalMapFont;
    Font titlesFont;
    Font levelTitlesFont;

    FontStyle originalMapFontStyle;
    FontStyle titlesFontStyle;
    FontStyle levelTitlesFontStyle;

    [HideInInspector] public bool notMouseOver = false;

    bool quitin;

    void Start()
    {
        if (!PlayerPrefs.HasKey("Checkpoint37"))
        {
            for (int cp = 0; cp <= 37; cp++)
                PlayerPrefs.SetInt("Checkpoint" + cp.ToString(), 0);
            //    PlayerPrefs.Save();
        }

        originalMapFont = mapMarkerText.font;
        titlesFont = titleText.font;
        levelTitlesFont = levelTitlesText.font;

        originalMapFontStyle = mapMarkerText.fontStyle;
        titlesFontStyle = titleText.fontStyle;
        levelTitlesFontStyle = levelTitlesText.fontStyle;

        AudioSource[] sources = this.gameObject.GetComponents<AudioSource>();
        sound1 = sources[0];
        sound2 = sources[1];
        soundInt = 0;
        stying = false;
        deactivatedAll = false;
        main.SetActive(false);

        toActDD.ClearOptions();
        toActDD.options.Add(new Dropdown.OptionData() { text = "NOTHING TO SELECT" });

        sS.consoleObj.SetActive(false);
        fpsObj.SetActive(false);

        textToTranslate = this.gameObject.GetComponent<TextToTranslate>();
        currentLevelTitle = -1;
    }

    private void Awake()
    {
        ReInput.ControllerDisconnectedEvent += OnControllerDisconnected;
    }

    void OnControllerDisconnected(ControllerStatusChangedEventArgs args)
    {
        if (!sS.inStart && !inPause)
        {
            inPause = true;
            OpenPauseScreen(false);
        }
    }

    void Update()
    {
        if (tpc == null)
        {
            if (woodle == null)
            {
                foreach (GameObject g in GameObject.FindGameObjectsWithTag("Player"))
                {
                    if (g.name == "Woodle Character")
                        woodle = g;
                }
            }
            tpc = woodle.gameObject.GetComponent<TPC>();

            multiTPC[0] = PlayerManager.GetPlayer(1);
            multiTPC[1] = PlayerManager.GetPlayer(2);
            multiTPC[2] = PlayerManager.GetPlayer(3);

            tpc.ps = this;
            input = ReInput.players.GetPlayer(0);
            InitializeValues();
        }
        else
        {
            if (!quitin)
            {
                if (currentCutscene != null)
                {
                    if (currentCutscene != WaterTearManager.GetFinalCutscene() && input.GetAnyButtonDown())
                        SkipCutscene(currentCutscene);
#if UNITY_EDITOR
                    if (Input.GetKeyDown(KeyCode.J))
                    {
                        currentCutscene.time = cutTime;
                    }
                    if (Input.GetKeyDown(KeyCode.K))
                    {
                        cutTime = currentCutscene.time;
                    }
#endif
                }
                else
                {
                    if (!sS.inStart)
                    {
                        if (fullExt == null)
                        {
                            foreach (GameObject g in GameObject.FindGameObjectsWithTag("WholeLevel"))
                            {
                                if (g.name == "External Full")
                                    fullExt = g.gameObject;
                            }
                        }
                    }

                    if (!sS.inStart && (input.GetButtonDown("Start") || input.GetButtonDown("Items") || (inPause && input.GetButtonDown("Back"))) && !tpc.defeated && !tpc.anim.GetCurrentAnimatorStateInfo(0).IsName("Defeat") && !tpc.inCutscene && !cantPause && !tpc.challengeWarping && !telescope.usingTelescope && !telescope.transitioning && !ItemPromptManager.IsDisplaying())
                    {
                        if (inPause && (levelDD.transform.childCount > 3 || toActDD.transform.childCount > 3))
                        {
                            if (levelDD.transform.childCount > 3)
                                levelDD.Hide();
                            if (toActDD.transform.childCount > 3)
                                toActDD.Hide();
                        }
                        else
                        {
                            if ((input.GetButtonDown("Start") || input.GetButtonDown("Items")) && !languagesOpen && !remapperOpen && !defaultsAYS.activeInHierarchy)
                            {
                                inPause = !inPause;
                                if (inPause)
                                    OpenPauseScreen(input.GetButtonDown("Items"));
                                else
                                    ClosePauseScreen(true);
                            }
                        }
                    }

                    if (npcFlowers.activeSelf && PlayerPrefs.GetInt("AllFlowers", 0) == 1)
                        npcFlowers.SetActive(false);
                    if (!npcFlowers.activeSelf && PlayerPrefs.GetInt("AllFlowers", 0) == 0)
                        npcFlowers.SetActive(true);

                    if (inPause)
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

                        //NAVIGATION
                        if (input.GetButtonDown("GUIL") && !languagesOpen && !remapperOpen && !quitAYS.activeInHierarchy && !defaultsAYS.activeInHierarchy)
                        {
                            MenuLeftPage();
                        }
                        if (input.GetButtonDown("GUIR") && !languagesOpen && !remapperOpen && !quitAYS.activeInHierarchy && !defaultsAYS.activeInHierarchy)
                        {
                            MenuRightPage();
                        }
                        if (input.GetButtonDown("Run") && !quitAYS.activeInHierarchy && !languagesOpen && !remapperOpen && !defaultsAYS.activeInHierarchy)
                            QuitGameAYS();
                        if (input.GetButtonDown("Back") && (!Input.GetKeyDown(KeyCode.Escape) || (quitAYS.activeInHierarchy || languagesOpen || remapperOpen || (defaultsAYS.activeInHierarchy))))
                        {
                            GoBack();
                        }

                        if (input.GetButtonDown("Submit"))
                            HDRumbleMain.PlayVibrationPreset(0, "K01_Patter1", 1f, 0, 0.2f);

                        //MAP MOVEMENT
                        if (inMap)
                        {
                            textToTranslate.SetTextElement(titleText, null, TextTranslationManager.TextCollection.pause, 0, "", false, true, titlesFont, titlesFontStyle);

                            if (es.currentSelectedGameObject != null && es.currentSelectedGameObject.transform.parent.name == "Checkpoints")
                            {
                                if (curES != null && es.currentSelectedGameObject != curES && !mapAYS.activeInHierarchy)
                                {
                                    curES = es.currentSelectedGameObject;
                                    if (curES != null)
                                    {
                                        mapIcon.transform.SetParent(curES.transform);
                                        DisplayMarkerText(int.Parse(curES.name));
                                        //	PlaySound (highlightSound);
                                    }
                                }
                                if (mapIcon.transform.localPosition.y >= 5000f)
                                    mapIcon.transform.localPosition = Vector3.zero;
                                else
                                    mapIcon.transform.localPosition = Vector3.Lerp(mapIcon.transform.localPosition, Vector3.zero, 0.2f);
                            }
                            else
                                mapIcon.transform.localPosition = Vector3.up * 10000f;

                            if (input.GetAxis("MapZoom") != 0f)
                            {

                                if (Input.GetAxis("Mouse ScrollWheel") != 0f)
                                    curMapScale = Mathf.Clamp(curMapScale + (input.GetAxis("MapZoom") * 10f * mapScaleSpeed), 1f, 2f);
                                else
                                    curMapScale = Mathf.Clamp(curMapScale + (input.GetAxis("MapZoom") * mapScaleSpeed), 1f, 2f);

                                mapRect.transform.localScale = Vector3.one * curMapScale;

                                foreach (GameObject g in altScaleObjs)
                                    g.transform.localScale = Vector3.one / curMapScale;
                            }

                            if (curES != null)
                            {

                                //LERP THIS POSITION
                                mapRect.anchoredPosition = Vector2.Lerp(mapRect.anchoredPosition, new Vector2(Mathf.Lerp(0f, -curES.GetComponent<RectTransform>().anchoredPosition.x, curMapScale - 1f), Mathf.Lerp(0f, -curES.GetComponent<RectTransform>().anchoredPosition.y, curMapScale - 1f)), 0.2f);

                                //    mapRect.anchoredPosition = new Vector2(Mathf.Clamp(mapRect.anchoredPosition.x, (mapRect.rect.width / 2f) * (curMapScale - 1f), mapRect.rect.width - ((mapRect.rect.width / 2f) * (curMapScale - 1f))),
                                //        Mathf.Clamp(mapRect.anchoredPosition.y, (mapRect.rect.height / 2f) * (curMapScale - 1f), mapRect.rect.height - ((mapRect.rect.height / 2f) * (curMapScale - 1f))));
                            }

                            if (mapAYS.activeInHierarchy && es.currentSelectedGameObject != null)
                            {
                                mapAYSSelection.transform.position = es.currentSelectedGameObject.transform.position;
                            }

                            if (quitAYS.activeInHierarchy && es.currentSelectedGameObject != null)
                            {
                                quitSelector.transform.position = es.currentSelectedGameObject.transform.position;
                            }
                        }

                        //ITEMS
                        if (inItems)
                            textToTranslate.SetTextElement(titleText, null, TextTranslationManager.TextCollection.pause, 1, "", false, true, titlesFont, titlesFontStyle);

                        //OPTIONS SELECTION
                        if (inOptions)
                        {
                            textToTranslate.SetTextElement(titleText, null, TextTranslationManager.TextCollection.common, 0, "", false, true, titlesFont, titlesFontStyle);
                            /*	if (es.currentSelectedGameObject == returnButton) {
                                    if (returnText.color != Color.grey)
                                        returnText.color = Color.grey;
                                } else {
                                    if (returnText.color != Color.white)
                                        returnText.color = Color.white;
                                }
                                if (es.currentSelectedGameObject == yesButton) {
                                    if (yesText.color != Color.grey)
                                        yesText.color = Color.grey;
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

                        //SOUNDS
                        if (es.currentSelectedGameObject != null && prevSelected != es.currentSelectedGameObject)
                        {
                            notMouseOver = true;
                            MouseOverButton(es.currentSelectedGameObject);
                        }
                    }
                }
            }
        }
    }

    public void GoBack()
    {
        HDRumbleMain.PlayVibrationPreset(0, "K02_Patter2", 1f, 0, 0.2f);
        if (inPause && (levelDD.transform.childCount > 3 || toActDD.transform.childCount > 3))
        {
            if (levelDD.transform.childCount > 3)
                levelDD.Hide();
            if (toActDD.transform.childCount > 3)
                toActDD.Hide();
        }
        else
        {
            if (quitAYS.activeInHierarchy)
            {
                quitAYS.SetActive(false);
                if (inMap)
                    es.SetSelectedGameObject(firstMap);
                if (inOptions)
                    es.SetSelectedGameObject(firstOptions);
            }
            else
            {
                if (mapAYS.activeInHierarchy)
                    CancelWarp();
                else
                {
                    if (languagesOpen)
                        ToggleLanguagesList();
                    else
                    {
                        if (remapperOpen)
                            Debug.Log("");
                        else
                        {
                            if (defaultsAYS.activeInHierarchy)
                                CloseDefaultsAYS();
                            else
                            {
                                inPause = false;
                                ClosePauseScreen(true);
                            }
                        }
                    }
                }
            }
        }
    }

    public void MenuLeftPage()
    {
        PlaySound(tabLSound);
        if (enableDebugTab && inDebug)
        {
            CloseDebugTab();
            OpenOptionsTab();
        }
        else
        {
            if (inMap)
            {
                CloseMapTab();
                OpenItemsTab();
            }
            else
            {
                if (inItems)
                {
                    CloseItemsTab();
                    if (enableDebugTab)
                        OpenDebugTab();
                    else
                        OpenOptionsTab();
                }
                else
                {
                    if (inOptions)
                    {
                        if (levelDD.transform.childCount > 3 || toActDD.transform.childCount > 3)
                        {
                            if (levelDD.transform.childCount > 3)
                                levelDD.Hide();
                            if (toActDD.transform.childCount > 3)
                                toActDD.Hide();
                        }
                        else
                        {
                            CloseOptionsTab();
                            OpenMapTab();
                        }
                    }
                }
            }
        }
    }

    public void MenuRightPage()
    {
        PlaySound(tabRSound);
        if (enableDebugTab && inDebug)
        {
            CloseDebugTab();
            OpenItemsTab();
        }
        else
        {
            if (inMap)
            {
                CloseMapTab();
                OpenOptionsTab();
            }
            else
            {
                if (inItems)
                {
                    CloseItemsTab();
                    OpenMapTab();
                }
                else
                {
                    if (inOptions)
                    {
                        if (levelDD.transform.childCount > 3 || toActDD.transform.childCount > 3)
                        {
                            if (levelDD.transform.childCount > 3)
                                levelDD.Hide();
                            if (toActDD.transform.childCount > 3)
                                toActDD.Hide();
                        }
                        else
                        {
                            CloseOptionsTab();
                            if (enableDebugTab)
                                OpenDebugTab();
                            else
                                OpenItemsTab();
                        }
                    }
                }
            }
        }
    }

    public void MouseOverButton(GameObject selected)
    {
        if (selected != null)
        {
            if (notMouseOver || Cursor.visible)
            {
                notMouseOver = false;
                es.SetSelectedGameObject(selected);
                if (!justOpenedMap)
                {
                    if (prevSelected != null &&
                        prevSelected.gameObject.transform.position.y < selected.transform.position.y)
                        PlaySound(highlightDownSound);
                    else
                        PlaySound(highlightSound);
                }
                else
                    justOpenedMap = false;
                prevSelected = selected;
            }
        }
    }

    public void InitializeValues()
    {
        if (cam != null)
        {
            if (PlayerPrefs.GetInt("InvertX", 0) == 0)
                invertX = false;
            else
                invertX = true;
            cam.freeInvertXAxis = invertX;
            ixTick.SetActive(invertX);

            if (PlayerPrefs.GetInt("InvertY", 0) == 0)
                invertY = false;
            else
                invertY = true;
            cam.freeInvertYAxis = invertY;
            iyTick.SetActive(invertX);

        //    sensitivity.value = (float)PlayerPrefs.GetInt("Sensitivity", 4);
         //   cam.freeRotateSpeed = sensitivity.value;

            clip = 40f;
        }

        if (PlayerPrefs.GetInt("Vibration", 1) == 0)
            vibOn = false;
        else
            vibOn = true;

        music = PlayerPrefs.GetFloat("musicVolume", 8f);
        effects = PlayerPrefs.GetFloat("effectsVolume", 8f);

        /*
        if (PlayerPrefs.GetInt("AA", 1) == 0)
            aaIsOn = false;
        else
            aaIsOn = true;
        foreach (Antialiasing aa in antialiasing)
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

     /*   if (stying)
            styTick.SetActive(true);
        else
            styTick.SetActive(false);*/

        if (deactivatedAll)
            actTick.SetActive(true);
        else
            actTick.SetActive(false);

        if (aaIsOn)
            aaTick.SetActive(true);
        else
            aaTick.SetActive(false);

        if (dguiIsOn)
            dguiTick.SetActive(true);
        else
            dguiTick.SetActive(false);

        if (expandIsOn)
            expandTick.SetActive(true);
        else
            expandTick.SetActive(false);



        clippin.value = Mathf.RoundToInt(clip);
        clipText.text = (clip * 10f).ToString();
        musicSlider.value = music;
        effectsSlider.value = effects;
    }

    public void OpenPauseScreen(bool goToItems)
    {

        Cursor.lockState = CursorLockMode.None;
        graphicRaycaster.enabled = true;

        Time.timeScale = 0f;
        foreach (AudioMixer am in audioMixers)
            am.SetFloat("musicVol", -80f + ((PlayerPrefs.GetFloat("musicVolume", 8f)) * 10f));
        PlaySound(openSound);
        inPause = true;
        HDRumbleMain.PlayVibrationPreset(0, "D12_DoubleThump1", 1f, 0, 0.2f);
        main.SetActive(true);
        tpc.disableControl = true;
        foreach (TPC mtpc in multiTPC)
            mtpc.disableControl = true;
        cam.disableControl = true;
        if (goToItems)
            OpenItemsTab();
        else
            OpenMapTab();
        languagesOpen = false;
    }

    public void ClosePauseScreen(bool playSound)
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Time.timeScale = 1f;
        foreach (AudioMixer am in audioMixers)
            am.SetFloat("musicVol", -80f + (PlayerPrefs.GetFloat("musicVolume", 8f) * 10f));
        if (playSound)
            PlaySound(closeSound);
        inPause = false;
        quitAYS.SetActive(false);
        StartCoroutine("PauseDelay");
    }

    public void OpenMapTab()
    {
        justOpenedMap = true;
        CloseItemsTab();
        CloseOptionsTab();
        CloseDebugTab();
        mapTab.SetActive(true);
        dotWhite.transform.position = dots[1].transform.position;
        inMap = true;
        DisplayMarkerText(0);
        //	selectIcon.transform.SetParent (selectPos [1].transform);
        //	selectIcon.transform.localPosition = Vector3.zero;
        curES = firstMap;
        if (curES != null)
        {
            mapIcon.transform.SetParent(curES.transform);
            mapIcon.transform.localPosition = Vector3.zero;
        }
        else
            mapIcon.transform.localPosition = Vector3.one * 10000f;
        prevSelected = es.currentSelectedGameObject;
        CheckCheckpoints();
        es.SetSelectedGameObject(firstMap);
        CheckBlues();
        CheckTears();
        mapRect.localScale = Vector3.one;
        mapRect.anchoredPosition = Vector2.zero;

        curMapScale = 1f;

        woodlePos = tpc.gameObject.transform.position;
        if (tpc.challengePortal == null)
        {
            woodleIcon.transform.localPosition = new Vector3((Mathf.Lerp(-mapRect.rect.width / 2f, mapRect.rect.width / 2f, 1f - ((woodlePos.x + (mapscale.x / 2f)) / mapscale.x))),
                Mathf.Lerp(-mapRect.rect.height / 2f, mapRect.rect.height / 2f, 1f - ((woodlePos.z + (mapscale.y / 2f)) / mapscale.y)), 0f);
            woodleIconRot.transform.localEulerAngles = new Vector3(0f, 180f, tpc.gameObject.transform.localEulerAngles.y + 180f);
        }
        else
        {
            woodleIcon.transform.localPosition = new Vector3(-mapRect.rect.width, -mapRect.rect.height, 0f);
        }

        foreach (GameObject g in altScaleObjs)
            g.transform.localScale = Vector3.one;

        UpdateChallengePortalIcons();
    }

    public void CloseMapTab()
    {
        mapTab.SetActive(false);
        mapAYS.SetActive(false);
        inMap = false;
    }

    public void OpenItemsTab()
    {
        CloseMapTab();
        CloseOptionsTab();
        CloseDebugTab();
        itemsTab.SetActive(true);
        dotWhite.transform.position = dots[0].transform.position;
        inItems = true;
        //	selectIcon.transform.SetParent (selectPos [0].transform);
        //	selectIcon.transform.localPosition = Vector3.zero;

        if (PlayerPrefs.GetInt("HalfBlueBerries", 0) == 1)
            bbFinderIcon.SetActive(true);
        else
            bbFinderIcon.SetActive(false);

        SetBerries();
        SetBlueBerries();
        SetWaterTears();
        SetCollectables();
    }

    public void CloseItemsTab()
    {
        itemsTab.SetActive(false);
        inItems = false;
    }

    public void OpenOptionsTab()
    {
        CloseItemsTab();
        CloseMapTab();
        CloseDebugTab();
        optionsTab.SetActive(true);
        optionsNew.SetInitialTextValues();
        optionsNew.ResetOptionsScroll();
        dotWhite.transform.position = dots[2].transform.position;
        inOptions = true;

        /*
        if (PlayerPrefs.GetInt("FinalBossDefeated", 0) == 0)
        {
            if(filterButton != null)
            filterButton.transform.parent.gameObject.SetActive(false);

            Navigation nav = sensitivity.navigation;
            nav.mode = Navigation.Mode.Explicit;
            nav.selectOnUp = aaLButton;
            sensitivity.navigation = nav;

            Navigation nav2 = aaLButton.navigation;
            nav2.mode = Navigation.Mode.Explicit;
            nav2.selectOnDown = sensitivity;
            aaLButton.navigation = nav2;

            Navigation nav3 = aaRButton.navigation;
            nav3.mode = Navigation.Mode.Explicit;
            nav3.selectOnDown = sensitivity;
            aaRButton.navigation = nav3;

            Navigation nav4 = aaAButton.navigation;
            nav4.mode = Navigation.Mode.Explicit;
            nav4.selectOnDown = sensitivity;
            aaAButton.navigation = nav4;
        }
        else
        {
            if (filterButton != null)
                filterButton.transform.parent.gameObject.SetActive(true);

            Navigation nav = sensitivity.navigation;
            nav.mode = Navigation.Mode.Explicit;
            nav.selectOnUp = filterButton;
            sensitivity.navigation = nav;

            Navigation nav2 = aaLButton.navigation;
            nav2.mode = Navigation.Mode.Explicit;
            nav2.selectOnDown = filterButton;
            aaLButton.navigation = nav2;

            Navigation nav3 = aaRButton.navigation;
            nav3.mode = Navigation.Mode.Explicit;
            nav3.selectOnDown = filterButton;
            aaRButton.navigation = nav3;

            Navigation nav4 = aaAButton.navigation;
            nav4.mode = Navigation.Mode.Explicit;
            nav4.selectOnDown = filterButton;
            aaAButton.navigation = nav4;
        }*/

        InitializeValues();
        es.SetSelectedGameObject(firstOptions);
        prevSelected = es.currentSelectedGameObject;
    }

    public void CloseOptionsTab()
    {
        if (inOptions)
            PlayerPrefs.Save();
        inOptions = false;
        optionsTab.SetActive(false);
    }

    public void OpenDebugTab()
    {
        CloseOptionsTab();
        CloseItemsTab();
        CloseMapTab();

        inDebug = true;
        debugTab.SetActive(true);

        titleText.text = "DEBUG";
        dotWhite.transform.position = dots[2].transform.position + (Vector3.right * 76.3f);

        CheckToActivates();
        currentLevelID = 0;
        CompileList();

        InitializeValues();
        es.SetSelectedGameObject(firstDebug);
        prevSelected = es.currentSelectedGameObject;
    }

    public void CloseDebugTab()
    {
        inDebug = false;
        debugTab.SetActive(false);
    }

    public void QuitGameAYS()
    {
    /*    quitAYS.SetActive(true);
        es.SetSelectedGameObject(quitFirst);
        quitSelector.transform.position = es.currentSelectedGameObject.transform.position;*/
    }

    public void QuitGame()
    {
        PlayerPrefs.Save();
        quitin = true;
        if (!Application.isEditor)
            System.Diagnostics.Process.GetCurrentProcess().Kill();
    }

    //ITEMS
    public void SetBerries()
    {
        berries.text = PlayerPrefs.GetInt("Berries", 0).ToString();
    }

    public void SetBlueBerries()
    {
        int amount = PlayerPrefs.GetInt("BlueBerryTotal");

        if (amount < 10)
        {
            blueBerries.text = "00" + amount.ToString();
        }
        else
        {
            if (amount < 100)
            {
                blueBerries.text = "0" + amount.ToString();
            }
            else
            {
                blueBerries.text = amount.ToString();
            }
        }
    }

    bool tearCheckB;
    public void SetWaterTears()
    {
        int tearsCollected = 0;
        for (int lvl = 1; lvl <= 8; lvl++)
        {
            for (int tearNo = 1; tearNo <= 3; tearNo++)
                tearsCollected += PlayerPrefs.GetInt("Vase" + tearNo.ToString() + "Level" + lvl.ToString(), 0);
        }

        if(tearsCollected == 24 && !tearCheckB)
        {
            tearCheckB = true;
#if UNITY_XBOXONE
            XONEAchievements.SubmitAchievement((int)XONEACHIEVS.ALL_TREE_SAGE_RESTORED);
#endif
        }

        if (tearsCollected < 10)
            waterTears.text = "0" + tearsCollected.ToString();
        else
            waterTears.text = tearsCollected.ToString();
    }

    public void SetCollectables()
    {
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
        /*
        if (counted >= 27 && !collectableAllTrophy)
        {
            //
            collectableAllTrophy = true;
#if UNITY_PS4
            //
            // check trophy collectable items
            PS4Manager.ps4TrophyManager.UnlockTrophy((int)PS4_TROPHIES.COLLECT_ALL_OF_THE_COLLECTIBLE);
#endif


#if UNITY_XBOXONE
            //
            // check trophy musicians if all cages are 1 countedPrefs = 4
            XONEAchievements.SubmitAchievement((int)XONEACHIEVS.BEAUTIFUL_WOODLE_HOUSE);
#endif

        }

        if (counted >= 11 && !collectableHalfTrophy)
        {
            //
            collectableHalfTrophy = true;
#if UNITY_PS4
            //
            // check trophy collectable items
            PS4Manager.ps4TrophyManager.UnlockTrophy((int)PS4_TROPHIES.COLLECT_HALF_OF_THE_COLLECTIBLE);
#endif

#if UNITY_XBOXONE
            //
            // check trophy musicians if all cages are 1 countedPrefs = 4
            XONEAchievements.SubmitAchievement((int)XONEACHIEVS.WONDERFUL_WOODLE_HOUSE);
#endif
        }*/

        if (counted < 10)
            collectables.text = "0" + counted.ToString();
        else
            collectables.text = counted.ToString();
    }

    //MAPS
    public void SelectWarp()
    {
        curWarp = es.currentSelectedGameObject;

        mapAYS.SetActive(true);
        es.SetSelectedGameObject(mapYesButton);
    }

    public void ConfirmWarp()
    {
        mapAYS.SetActive(false);
        Warp(int.Parse(curWarp.name));
        PlaySound(warpSound);
    }

    public void CancelWarp()
    {
        mapAYS.SetActive(false);
        es.SetSelectedGameObject(curWarp);
    }

    public void Warp(int whichCheckpoint)
    {
        warping = true;
        bool extsceneLoaded = false;
        bool sceneLoaded = false;
        int checkForScene = PlayerPrefs.GetInt("Checkpoint" + whichCheckpoint.ToString() + "Scene", 0);
        if (checkForScene == 0)
        {
            sS.SetCheckpointScenes();
            checkForScene = PlayerPrefs.GetInt("Checkpoint" + whichCheckpoint.ToString() + "Scene", 0);
        }

        TPCSettings(tpc);
        foreach (TPC mtpc in multiTPC)
        {
            if (mtpc.gameObject.activeInHierarchy)
                TPCSettings(mtpc);
        }

        underwaterCameraTrigger.TurnOffWaterEffect();
        foreach (Scene s in SceneManager.GetAllScenes())
        {
            if (s.buildIndex == 2)
                extsceneLoaded = true;
            if (s.buildIndex == checkForScene)
                sceneLoaded = true;
        }
        if ((!extsceneLoaded && whichCheckpoint >= 2) || !sceneLoaded)
        {
            ClosePauseScreen(false);
            tpc.disableControl = true;
            foreach (TPC mtpc in multiTPC)
                mtpc.disableControl = true;
            cam.disableControl = true;

            if (!sceneLoaded && tpc.beingReset)
            {
                while (atmosphereManager.triggerCount > 0)
                    atmosphereManager.ExitTrigger();
                atmosphereManager.curLevel = "";
            }

            StartCoroutine(LoadIt(checkForScene, whichCheckpoint, extsceneLoaded, sceneLoaded));
        }
        else
        {
            ClosePauseScreen(false);
            tpc.disableControl = true;
            foreach (TPC mtpc in multiTPC)
                mtpc.disableControl = true;
            cam.disableControl = true;
            StartCoroutine(ShortLoad(whichCheckpoint));
        }

        if (checkForScene == 8)
        {
            sS.loadLevelAdditives[4].entTrig.colliderToActivate.enabled = true;
            sS.loadLevelAdditives[4].lptCollider.enabled = true;
        }
    }

    void TPCSettings(TPC toChange)
    {
        toChange.transform.SetParent(toChange.initialParent);
        if (toChange.isBoosting)
            toChange.isBoosting = false;
        if (toChange.waterPhysics)
            toChange.waterPhysics = false;
        if (toChange.slowMovement || toChange.superslowMovement)
        {
            toChange.slowMovement = false;
            toChange.slowCount = 0;
            toChange.superslowMovement = false;
            toChange.quickSandCount = 0;
        }
        toChange.anim.Play("Idle", 0);
    }

    void CheckCheckpoints()
    {
        int c = 0;
        firstMap = null;
        foreach (GameObject g in mapPos)
        {
            if (c == 0 || PlayerPrefs.GetInt("Checkpoint" + c) == 1)
            {
                g.SetActive(true);
                if (firstMap == null)
                    firstMap = g;
            }
            else
                g.SetActive(false);
            c++;
        }
    }

    void CheckpointsActivate()
    {
        foreach (GameObject g in mapPos)
            g.SetActive(true);
    }

    public void CheckBlues()
    {
        currentBBCount = 0;

        int lvlCount = 0;

        PlayerPrefs.SetInt("BlueBerryTotal", 0);

        foreach (string s in sS.levelNames)
        {
            currentBBCount = 0;

            for (int bb = 0; bb < PlayerPrefs.GetInt(s + "BlueBerryTotal"); bb++)
            {
                if (PlayerPrefs.GetString(s + "BlueBerry" + bb.ToString()).Contains("1"))
                    currentBBCount++;
            }

            blueCounters[lvlCount].text = "";

            if (currentBBCount < 10)
            {
                if (lvlCount == 0 || lvlCount == 1 || lvlCount == 7)
                    blueCounters[lvlCount].text = "00";
                else
                    blueCounters[lvlCount].text = "0";
            }
            else
            {
                if (currentBBCount < 100)
                {
                    if (lvlCount == 0 || lvlCount == 1 || lvlCount == 7)
                        blueCounters[lvlCount].text = "0";
                }
            }

            blueCounters[lvlCount].text += currentBBCount.ToString();

            lvlCount++;

            int finalBlueBerryTotal = PlayerPrefs.GetInt("BlueBerryTotal") + currentBBCount;

            PlayerPrefs.SetInt("BlueBerryTotal", finalBlueBerryTotal);

        }

    }

    bool tearCheckA;
    public void CheckTears()
    {
        tearCount = 0;

        if (PlayerPrefs.GetInt("Vase1Level1", 0) == 0)
            waterTearsParent.transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
        else
        {
            tearCount++;
            waterTearsParent.transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
        }
        if (PlayerPrefs.GetInt("Vase2Level1", 0) == 0)
            waterTearsParent.transform.GetChild(1).GetChild(0).gameObject.SetActive(false);
        else
        {
            tearCount++;
            waterTearsParent.transform.GetChild(1).GetChild(0).gameObject.SetActive(true);
        }
        if (PlayerPrefs.GetInt("Vase3Level1", 0) == 0)
            waterTearsParent.transform.GetChild(2).GetChild(0).gameObject.SetActive(false);
        else
        {
            tearCount++;
            waterTearsParent.transform.GetChild(2).GetChild(0).gameObject.SetActive(true);
        }

        if (PlayerPrefs.GetInt("Vase1Level2", 0) == 0)
            waterTearsParent.transform.GetChild(3).GetChild(0).gameObject.SetActive(false);
        else
        {
            tearCount++;
            waterTearsParent.transform.GetChild(3).GetChild(0).gameObject.SetActive(true);
        }
        if (PlayerPrefs.GetInt("Vase2Level2", 0) == 0)
            waterTearsParent.transform.GetChild(4).GetChild(0).gameObject.SetActive(false);
        else
        {
            tearCount++;
            waterTearsParent.transform.GetChild(4).GetChild(0).gameObject.SetActive(true);
        }
        if (PlayerPrefs.GetInt("Vase3Level2", 0) == 0)
            waterTearsParent.transform.GetChild(5).GetChild(0).gameObject.SetActive(false);
        else
        {
            tearCount++;
            waterTearsParent.transform.GetChild(5).GetChild(0).gameObject.SetActive(true);
        }

        if (PlayerPrefs.GetInt("Vase1Level3", 0) == 0)
            waterTearsParent.transform.GetChild(6).GetChild(0).gameObject.SetActive(false);
        else
        {
            tearCount++;
            waterTearsParent.transform.GetChild(6).GetChild(0).gameObject.SetActive(true);
        }
        if (PlayerPrefs.GetInt("Vase2Level3", 0) == 0)
            waterTearsParent.transform.GetChild(7).GetChild(0).gameObject.SetActive(false);
        else
        {
            tearCount++;
            waterTearsParent.transform.GetChild(7).GetChild(0).gameObject.SetActive(true);
        }
        if (PlayerPrefs.GetInt("Vase3Level3", 0) == 0)
            waterTearsParent.transform.GetChild(8).GetChild(0).gameObject.SetActive(false);
        else
        {
            tearCount++;
            waterTearsParent.transform.GetChild(8).GetChild(0).gameObject.SetActive(true);
        }

        if (PlayerPrefs.GetInt("Vase1Level4", 0) == 0)
            waterTearsParent.transform.GetChild(9).GetChild(0).gameObject.SetActive(false);
        else
        {
            tearCount++;
            waterTearsParent.transform.GetChild(9).GetChild(0).gameObject.SetActive(true);
        }
        if (PlayerPrefs.GetInt("Vase2Level4", 0) == 0)
            waterTearsParent.transform.GetChild(10).GetChild(0).gameObject.SetActive(false);
        else
        {
            tearCount++;
            waterTearsParent.transform.GetChild(10).GetChild(0).gameObject.SetActive(true);
        }
        if (PlayerPrefs.GetInt("Vase3Level4", 0) == 0)
            waterTearsParent.transform.GetChild(11).GetChild(0).gameObject.SetActive(false);
        else
        {
            tearCount++;
            waterTearsParent.transform.GetChild(11).GetChild(0).gameObject.SetActive(true);
        }

        if (PlayerPrefs.GetInt("Vase1Level5", 0) == 0)
            waterTearsParent.transform.GetChild(12).GetChild(0).gameObject.SetActive(false);
        else
        {
            tearCount++;
            waterTearsParent.transform.GetChild(12).GetChild(0).gameObject.SetActive(true);
        }
        if (PlayerPrefs.GetInt("Vase2Level5", 0) == 0)
            waterTearsParent.transform.GetChild(13).GetChild(0).gameObject.SetActive(false);
        else
        {
            tearCount++;
            waterTearsParent.transform.GetChild(13).GetChild(0).gameObject.SetActive(true);
        }
        if (PlayerPrefs.GetInt("Vase3Level5", 0) == 0)
            waterTearsParent.transform.GetChild(14).GetChild(0).gameObject.SetActive(false);
        else
        {
            tearCount++;
            waterTearsParent.transform.GetChild(14).GetChild(0).gameObject.SetActive(true);
        }

        if (PlayerPrefs.GetInt("Vase1Level6", 0) == 0)
            waterTearsParent.transform.GetChild(15).GetChild(0).gameObject.SetActive(false);
        else
        {
            tearCount++;
            waterTearsParent.transform.GetChild(15).GetChild(0).gameObject.SetActive(true);
        }
        if (PlayerPrefs.GetInt("Vase2Level6", 0) == 0)
            waterTearsParent.transform.GetChild(16).GetChild(0).gameObject.SetActive(false);
        else
        {
            tearCount++;
            waterTearsParent.transform.GetChild(16).GetChild(0).gameObject.SetActive(true);
        }
        if (PlayerPrefs.GetInt("Vase3Level6", 0) == 0)
            waterTearsParent.transform.GetChild(17).GetChild(0).gameObject.SetActive(false);
        else
        {
            tearCount++;
            waterTearsParent.transform.GetChild(17).GetChild(0).gameObject.SetActive(true);
        }

        if (PlayerPrefs.GetInt("Vase1Level7", 0) == 0)
            waterTearsParent.transform.GetChild(18).GetChild(0).gameObject.SetActive(false);
        else
        {
            tearCount++;
            waterTearsParent.transform.GetChild(18).GetChild(0).gameObject.SetActive(true);
        }
        if (PlayerPrefs.GetInt("Vase2Level7", 0) == 0)
            waterTearsParent.transform.GetChild(19).GetChild(0).gameObject.SetActive(false);
        else
        {
            tearCount++;
            waterTearsParent.transform.GetChild(19).GetChild(0).gameObject.SetActive(true);
        }
        if (PlayerPrefs.GetInt("Vase3Level7", 0) == 0)
            waterTearsParent.transform.GetChild(20).GetChild(0).gameObject.SetActive(false);
        else
        {
            tearCount++;
            waterTearsParent.transform.GetChild(20).GetChild(0).gameObject.SetActive(true);
        }

        if (PlayerPrefs.GetInt("Vase1Level8", 0) == 0)
            waterTearsParent.transform.GetChild(21).GetChild(0).gameObject.SetActive(false);
        else
        {
            tearCount++;
            waterTearsParent.transform.GetChild(21).GetChild(0).gameObject.SetActive(true);
        }
        if (PlayerPrefs.GetInt("Vase2Level8", 0) == 0)
            waterTearsParent.transform.GetChild(22).GetChild(0).gameObject.SetActive(false);
        else
        {
            tearCount++;
            waterTearsParent.transform.GetChild(22).GetChild(0).gameObject.SetActive(true);
        }
        if (PlayerPrefs.GetInt("Vase3Level8", 0) == 0)
            waterTearsParent.transform.GetChild(23).GetChild(0).gameObject.SetActive(false);
        else
        {
            tearCount++;
            waterTearsParent.transform.GetChild(23).GetChild(0).gameObject.SetActive(true);
        }

        if(tearCount == 24 && !tearCheckA)
        {
            tearCheckA = true;
#if UNITY_XBOXONE
            XONEAchievements.SubmitAchievement((int)XONEACHIEVS.ALL_TREE_SAGE_RESTORED);
#endif
        }
    }

    public void UpdateChallengePortalIcons()
    {
        for (int ch = 0; ch <= 11; ch++)
        {
            if (PlayerPrefs.GetInt("Challenge" + ch.ToString() + "Found", 0) == 0)
                challengesIcons[ch].gameObject.SetActive(false);
            else
            {
                challengesIcons[ch].gameObject.SetActive(true);

                if (PlayerPrefs.GetInt("Cage" + (ch + 4).ToString(), 0) == 0)
                    challengesIcons[ch].color = challengesFoundColour;
                else
                    challengesIcons[ch].color = challengescClearedColour;
            }
        }
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
    }

    public void Sensitivity()
    {
        
        cam.freeRotateSpeed = sensitivity.value;
        PlayerPrefs.SetInt("Sensitivity", Mathf.RoundToInt(sensitivity.value));
        PlaySound(highlightSound);
    }

    public void ClippingSet()
    {
        clip = clippin.value * 10f;
        cam.GetComponent<Camera>().farClipPlane = clip;
        // 200 = 0.01 | 400 = 0.005 | 100 = 0.02
        RenderSettings.fogEndDistance = clip;
        PlaySound(highlightSound);
        clipText.text = clip.ToString();
    }

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
        es.SetSelectedGameObject(yesButton);
        PlaySound(selectSound);
    }

    public void CloseAYS()
    {
        es.SetSelectedGameObject(returnButton);
        PlaySound(selectSound);
    }

    public void ExitToMenu()
    {
        PlaySound(exitSound);
        ClosePauseScreen(false);

        if (SceneManager.GetAllScenes().Length > 1)
        {
            foreach (Scene s in SceneManager.GetAllScenes())
            {
                if (s.buildIndex != 1)
                {
                    //    Debug.Log("PS Exit: " + s.buildIndex);
                    SceneManager.UnloadSceneAsync(s.buildIndex);
                }
            }
        }
        StartCoroutine(LoadIt(1, -1, false, false));
    }

    public void QualityChange()
    {
        QualitySettings.SetQualityLevel(qualityT.value, true);
    }

    public void StylizerToggle()
    {
        if (!stying)
        {
            stying = true;
            styTick.SetActive(true);
            stylizerCS.enabled = true;
        }
        else
        {
            stying = false;
            styTick.SetActive(false);
            stylizerCS.enabled = false;
        }
    }


    #region NEW OPTIONS METHODS
    public void ToogleInvertX()
    {
        optionsNew.ToggleInvertX();
    }
    public void ToogleInvertY()
    {
        optionsNew.ToggleInvertY();
    }
    public void ToogleVibrations()
    {
        optionsNew.ToggleVibrations();
    }
    public void ToggleAntiAlising()
    {
        optionsNew.ToggleAntiAliasing();
    }
    public void ToggleVSync()
    {
        optionsNew.ToggleVsync();
    }
    public void ToogleRunByDefault()
    {
        optionsNew.ToggleRunByDefault();
    }
    public void ToggleBloom()
    {
        optionsNew.ToggleBloom();
    }
    public void ToggleAmbientOcclusion()
    {
        optionsNew.ToggleAmbientOcclusion();
    }
    public void ResetOptionsToDefault()
    {
        optionsNew.RestoreDefaults();
    }
    #endregion

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

    public void DGUIToggle()
    {
        if (!dguiIsOn)
        {
            dguiIsOn = true;
            dguiTick.SetActive(true);
            sS.consoleObj.SetActive(true);
            fpsObj.SetActive(true);
        }
        else
        {
            dguiIsOn = false;
            dguiTick.SetActive(false);
            fpsObj.SetActive(false);

            expandIsOn = true;
            ExpandToggle();

            sS.consoleObj.SetActive(false);
        }
    }

    public void ExpandToggle()
    {/*
        if (!expandIsOn)
        {
            if (dguiIsOn)
            {
                expandIsOn = true;
                expandTick.SetActive(true);
                popupManager.Hide();
                popupManager.debugManager.Show();
            }
        }
        else
        {
            expandIsOn = false;
            expandTick.SetActive(false);
            popupManager.Show();
            popupManager.debugManager.Hide();
        }*/
    }

    public void CollectThreeTears()
    {
        for (int tearNo = 1; tearNo <= 3; tearNo++)
            PlayerPrefs.SetInt("Vase" + tearNo.ToString() + "Level1", 1);
        PlayerPrefs.SetInt("Played1TreeCompletion", 1);
        PlayerPrefs.SetInt("First3Tears", 1);

        CheckTears();
        WaterTearManager.UpdateTears();

        PlayerPrefs.SetInt("Intro2Watched", 1);
        playThreeTears = true;
    }

    public void CollectAllTears()
    {
        PlayerPrefs.SetInt("Played1TreeCompletion", 1);
        PlayerPrefs.SetInt("First3Tears", 1);

        for (int lvl = 1; lvl <= 8; lvl++)
        {
            for (int tearNo = 1; tearNo <= 3; tearNo++)
                PlayerPrefs.SetInt("Vase" + tearNo.ToString() + "Level" + lvl.ToString(), 1);
            PlayerPrefs.SetInt("Played" + lvl.ToString() + "TreeCompletion", 1);
        }

        CheckTears();
        WaterTearManager.UpdateTears();

        PlayerPrefs.SetInt("Intro2Watched", 1);
        playAllTears = true;
    }
    public void CollectAllTearsButOne()
    {
        PlayerPrefs.SetInt("Played1TreeCompletion", 1);
        PlayerPrefs.SetInt("First3Tears", 1);
        PlayerPrefs.SetInt("Intro2Watched", 1);

        for (int lvl = 1; lvl <= 8; lvl++)
        {
            for (int tearNo = 1; tearNo <= 3; tearNo++)
            {
                if (lvl != 4 || tearNo != 1)
                    PlayerPrefs.SetInt("Vase" + tearNo.ToString() + "Level" + lvl.ToString(), 1);
            }
            if (lvl != 4)
                PlayerPrefs.SetInt("Played" + lvl.ToString() + "TreeCompletion", 1);
        }

        CheckTears();
        WaterTearManager.UpdateTears();

    }

    public void DeactivateThemAll()
    {
        deactivatedAll = !deactivatedAll;

        actTick.SetActive(deactivatedAll);

        CheckToActivates();

        for (int x = 0; x < 10; x++)
        {
            if (toactives[x] != null)
            {
                deactToActs[x] = deactivatedAll;
                toActivateTicks[x].SetActive(deactivatedAll);
                toactives[x].SetActive(!deactivatedAll);
            }
        }
    }

    public void DeactivateOthers(int id)
    {
        CheckToActivates();

        if (toactives[id] != null)
        {
            deactToActs[id] = !deactToActs[id];
            toActivateTicks[id].SetActive(deactToActs[id]);
            toactives[id].SetActive(!deactToActs[id]);
        }
    }

    void CheckToActivates()
    {
        foreach (GameObject g in GameObject.FindGameObjectsWithTag("ToActivate"))
        {
            if (toactives[0] == null && g.transform.root.name == "ToActivate")
                toactives[0] = g;
            if (toactives[1] == null && g.transform.root.name == "External Full")
                toactives[1] = g;
            if (toactives[2] == null && g.transform.root.name == "Level1.2")
                toactives[2] = g;
            if (toactives[3] == null && g.transform.root.name == "Level2")
                toactives[3] = g;
            if (toactives[4] == null && g.transform.root.name == "Level3")
                toactives[4] = g;
            if (toactives[5] == null && g.transform.root.name == "Level4")
                toactives[5] = g;
            if (toactives[6] == null && g.transform.root.name == "Level5")
                toactives[6] = g;
            if (toactives[7] == null && g.transform.root.name == "Level6")
                toactives[7] = g;
            if (toactives[8] == null && g.transform.root.name == "Level7")
                toactives[8] = g;
            if (toactives[9] == null && g.transform.root.name == "Level8")
                toactives[9] = g;
        }
    }

    void DeactivateExtPlz()
    {
        dPlzExt = !dPlzExt;
        if (dPlzExt)
        {
            if (plazaMain != null)
                odm.DeactivateObject(plazaMain, null);
            if (fullExt != null)
                odm.DeactivateObject(fullExt, null);
        }
        else
        {
            if (plazaMain != null)
                odm.ActivateObject(plazaMain);
            if (fullExt != null)
                odm.ActivateObject(fullExt);
        }
    }

    public void DeactivateSelected()
    {
        int id = -1;
        for (int i = 0; i < toactives[currentLevelID].transform.childCount; i++)
        {
            if (toactives[currentLevelID].transform.GetChild(i).name == toActDD.captionText.text)
                id = i;
        }
        if (id != -1)
        {
            deactivatedToActObj[id] = !deactivatedToActObj[id];
            toactives[currentLevelID].transform.GetChild(id).gameObject.SetActive(deactivatedToActObj[id]);
        }
        SetDeactToActObjTick();
    }

    public void SetCurrentSelectedLevel()
    {
        if (toactives[levelDD.value] == null)
            levelDD.value = currentLevelID;
        else
        {
            currentLevelID = levelDD.value;
            CompileList();
            SetDeactToActObjTick();
        }
    }

    void CompileList()
    {
        if (toactives[currentLevelID] != null)
        {
            toActDD.ClearOptions();
            deactivatedToActObj = new bool[toactives[currentLevelID].transform.childCount];
            for (int i = 0; i < toactives[currentLevelID].transform.childCount; i++)
            {
                toActDD.options.Add(new Dropdown.OptionData() { text = toactives[currentLevelID].transform.GetChild(i).name });
                deactivatedToActObj[i] = toactives[currentLevelID].transform.GetChild(i).gameObject.activeInHierarchy;
            }
        }
    }

    public void SetDeactToActObjTick()
    {
        if (toactives[currentLevelID] != null)
        {
            for (int i = 0; i < toactives[currentLevelID].transform.childCount; i++)
            {
                if (toactives[currentLevelID].transform.GetChild(i).name == toActDD.captionText.text)
                {
                    if (toactives[currentLevelID].transform.GetChild(i).gameObject.activeInHierarchy)
                        toActDDTick.SetActive(false);
                    else
                        toActDDTick.SetActive(true);
                }
            }
        }
    }

    public void AddBerries()
    {
        PlayerPrefs.SetInt("Berries", tpc.berryCount + 999999);
        tpc.berryCount = PlayerPrefs.GetInt("Berries", 0);
        PlayerPrefs.SetInt("BlueBerries", tpc.blueberryCount + 9999);
        tpc.blueberryCount = PlayerPrefs.GetInt("BlueBerries", 0);
    }

    public void ToggleWhiteFade(bool fadeIn)
    {
        StartCoroutine(WhiteFade(fadeIn));
    }

    IEnumerator WhiteFade(bool fadeIn)
    {
        Color startColor = new Color(0f, 0f, 0f, 0f);
        Color endColor = Color.white;
        if (!fadeIn)
        {
            endColor = new Color(0f, 0f, 0f, 0f);
            startColor = Color.white;
        }

        for (float f = 0f; f < 1f; f += Time.deltaTime)
        {
            whiteFade.color = Color.Lerp(startColor, endColor, f);
            yield return null;
        }
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

    public void ActivateAllCheckpoints()
    {
        debugCPTick.SetActive(true);
        for (int cp = 0; cp <= 37; cp++)
            PlayerPrefs.SetInt("Checkpoint" + cp.ToString(), 1);
        //    PlayerPrefs.Save();
        sS.SetCheckpointScenes();
    }

    IEnumerator ShortLoad(int whichCheckpoint)
    {
        Debug.Log("PAUSE LOADING: " +  whichCheckpoint);
        cantPause = true;
        loadAnim.SetBool("Loading", true);
        loadIcon.fillAmount = 0;

        if (tpc != null && tpc.GetComponentInParent<OneWayManager>() != null)
            tpc.GetComponentInParent<OneWayManager>().currentlyChecking = true;

        if (cam.stationaryMode1 || cam.stationaryMode2)
            cam.ExitCameraCut();
        Time.timeScale = 0f;
        for (int s = 0; s <= 60; s++)
        {
            AudioListener.volume = Mathf.Lerp(1f, 0f, (s * 1f) / 60f);
            loadFS.color = Color.Lerp(Color.clear, Color.black, (s * 1f) / 60f);
            loadIcon.fillAmount = Mathf.Lerp(loadIcon.fillAmount, (s * 1f) / 180f, 0.8f);
            yield return null;
        }

        int countLoaded = SceneManager.sceneCount;
        for (int i = 0; i < countLoaded; i++)
        {
            if (SceneManager.GetSceneAt(i).name.Contains("Challenges"))
            {
                tpc.challengePortal = null;

                AsyncOperation async = new AsyncOperation();
                async = SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(i));

                while (!async.isDone)
                    yield return null;

                if (!fullExt.activeInHierarchy)
                    fullExt.SetActive(true);
            }
        }


        foreach (GameObject g in sS.fullLevels)
        {
            if (g.scene.buildIndex == PlayerPrefs.GetInt("Checkpoint" + whichCheckpoint + "Scene", 0))
                g.SetActive(true);
        }
        foreach (GameObject g in lowerPolyLevels)
        {
            if (!g.activeInHierarchy)
                g.SetActive(true);
        }
        if (PlayerPrefs.GetInt("Checkpoint" + whichCheckpoint + "Scene", 0) - 4 >= 0)
        {
            Debug.Log("LOW POLY TOGGLE: " + PlayerPrefs.GetInt("Checkpoint" + whichCheckpoint + "Scene", 0) + " > " + lowerPolyLevels[PlayerPrefs.GetInt("Checkpoint" + whichCheckpoint + "Scene", 0) - 4].gameObject.name);
            lowerPolyLevels[PlayerPrefs.GetInt("Checkpoint" + whichCheckpoint + "Scene", 0) - 4].gameObject.SetActive(false);
        }

        Time.timeScale = 1f;
        cam.disableControl = false;

        yield return null;
        foreach (GameObject g in GameObject.FindGameObjectsWithTag("Checkpoint"))
        {
            if (g.GetComponent<Checkpoint>().checkpointID == whichCheckpoint)
                g.GetComponent<Checkpoint>().ActivateCheckpoint();
        }

        yield return null;

        PlayerPrefs.SetInt("LastCheckpoint", whichCheckpoint);
        //    PlayerPrefs.Save();

        if (whichCheckpoint <= 2)
        {
            if (plazaMain != null)
                odm.ActivateObject(plazaMain);
            if (plazaLow != null)
                odm.DeactivateObject(plazaLow, null);
        }

        if (curLPT != null)
            curLPT.ExitTriggerForced();

        yield return null;

        if (atmosphereManager.curAtmoID == 10 || (whichCheckpoint != 16 && whichCheckpoint != 17 && whichCheckpoint != 37 && atmosphereManager.curLevel == "Level5"))
        {
            while (atmosphereManager.triggerCount > 0)
                atmosphereManager.ExitTrigger();
            atmosphereManager.curLevel = "";
        }

        //PLAZA
        if (whichCheckpoint == 0 || whichCheckpoint == 1)
            lowPolyTriggers[0].EnterTriggerForced();

        //1
        if (whichCheckpoint == 3 || whichCheckpoint == 4 || whichCheckpoint == 5)
            lowPolyTriggers[1].EnterTriggerForced();

        //2
        if (whichCheckpoint == 7 || whichCheckpoint == 8 || whichCheckpoint == 33)
            lowPolyTriggers[2].EnterTriggerForced();

        //3
        if (whichCheckpoint == 10 || whichCheckpoint == 11)
            lowPolyTriggers[3].EnterTriggerForced();

        //4
        if (whichCheckpoint == 13 || whichCheckpoint == 14 || whichCheckpoint == 30 || whichCheckpoint == 31)
            lowPolyTriggers[4].EnterTriggerForced();

        //5
        if (whichCheckpoint == 16 || whichCheckpoint == 17 || whichCheckpoint == 37)
        {
            if (atmosphereManager.curLevel != "Level5")
            {
                atmosphereManager.EnterTrigger(2);
                atmosphereManager.curLevel = "Level5";
            }
            lowPolyTriggers[5].EnterTriggerForced();
        }

        //6
        if (whichCheckpoint == 18 || whichCheckpoint == 19 || whichCheckpoint == 20 || whichCheckpoint == 27)
            lowPolyTriggers[6].EnterTriggerForced();

        //7
        if (whichCheckpoint == 22 || whichCheckpoint == 23 || whichCheckpoint == 28 || whichCheckpoint == 29)
            lowPolyTriggers[7].EnterTriggerForced();

        //9
        if (whichCheckpoint == 24 || whichCheckpoint == 25 || whichCheckpoint == 26 || whichCheckpoint == 34 || whichCheckpoint == 35)
            lowPolyTriggers[8].EnterTriggerForced();
        
        for (int s = 60; s <= 120; s++)
        {
            loadIcon.fillAmount = Mathf.Lerp(loadIcon.fillAmount, (s * 1f) / 180f, 0.8f);
            yield return null;
        }

        if (tpc.gameObject.GetComponentInParent<OneWayManager>() != null)
            tpc.gameObject.GetComponentInParent<OneWayManager>().CheckOneWays();
        
        PositionMultiCharacters();
        
        yield return null;

        tpc.disableControl = true;
        tpc.ExitWaterFS();
        tpc.ExitRiverForce();
        foreach (TPC mtpc in multiTPC)
        {
            mtpc.disableControl = true;
            mtpc.ExitWaterFS();
            mtpc.ExitRiverForce();
        }
        
        yield return null;

        tpc.rb.velocity = Vector3.zero;

        //    yield return new WaitForSeconds(3f);
        
        while (!CheckpointLoaded())
            yield return null;

        Vector3 newPos = new Vector3(checkpointToSpawnAt.checkpointposition.transform.position.x,
            checkpointToSpawnAt.transform.position.y + 1f,
            checkpointToSpawnAt.checkpointposition.transform.position.z);
        tpc.gameObject.transform.position = newPos;

        yield return new WaitForSeconds(5f);

        tpc.anim.enabled = true;
        for (int s = 120; s <= 180; s++)
        {
            //	tpc.gameObject.transform.position = newPos;
            AudioListener.volume = Mathf.Lerp(0f, 1f, ((s - 120) * 1f) / 60f);
            loadFS.color = Color.Lerp(Color.black, Color.clear, ((s - 120) * 1f) / 60f);
            loadIcon.fillAmount = Mathf.Lerp(loadIcon.fillAmount, (s * 1f) / 180f, 0.8f);
            yield return null;
        }
        loadAnim.SetBool("Loading", false);
        
        while (tpc.beingReset)
            yield return null;

        tpc.disableControl = false;
        foreach (TPC mtpc in multiTPC)
            mtpc.disableControl = false;
        tpc.rb.velocity = Vector3.zero;

        foreach (AudioMixer am in audioMixers)
            am.SetFloat("musicVol", -80f + (PlayerPrefs.GetFloat("musicVolume", 8f) * 10f));

        cantPause = false;
        warping = false;
    }

    IEnumerator LoadIt(int whichLevel, int whichCheckpoint, bool extSceneLoaded, bool sceneLoaded)
    {
        Debug.Log("PAUSE LOADING: " + whichLevel + " @ " + whichCheckpoint +" | " + extSceneLoaded +" | "+ sceneLoaded);
        if (!cantPause)
        {
            cantPause = true;
            loadAnim.SetBool("Loading", true);
            loadIcon.fillAmount = 0f;

            if (tpc != null && tpc.GetComponentInParent<OneWayManager>() != null)
                tpc.GetComponentInParent<OneWayManager>().currentlyChecking = true;

            if (cam.stationaryMode1 || cam.stationaryMode2)
            {
                cam.ExitCameraCut();
                GetComponent<Camera>().transform.eulerAngles = new Vector3(GetComponent<Camera>().transform.eulerAngles.x, 0f, GetComponent<Camera>().transform.eulerAngles.z);
            }

            Time.timeScale = 0f;

            for (int f = 1; f <= 60; f++)
            {
                AudioListener.volume = Mathf.Lerp(1f, 0f, (f * 1f) / 60f);
                loadFS.color = Color.Lerp(Color.clear, Color.black, (f * 1f) / 60f);
                yield return null;
            }

            int countLoaded = SceneManager.sceneCount;
            for (int i = 0; i < countLoaded; i++)
            {
                if (SceneManager.GetSceneAt(i).name.Contains("Challenges"))
                {
                    tpc.challengePortal = null;

                    AsyncOperation async = new AsyncOperation();
                    async = SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(i));

                    while (!async.isDone)
                        yield return null;

                    if (!fullExt.activeInHierarchy)
                        fullExt.SetActive(true);
                }
            }

            if (whichCheckpoint == -1)
            {
                while (loadFS.color != Color.black)
                    yield return null;

                AsyncOperation async = new AsyncOperation();
                async = SceneManager.LoadSceneAsync(2, LoadSceneMode.Single);

                async.allowSceneActivation = false;
                while (!async.isDone)
                {
                    loadIcon.fillAmount = Mathf.Lerp(loadIcon.fillAmount, async.progress, 0.8f);
                    if (async.progress >= 0.9f)
                    {
                        Time.timeScale = 1f;
                        async.allowSceneActivation = true;
                    }
                    yield return null;
                }
            }
            else
            {
                float totalFill = 1f;
                if (!extSceneLoaded && !sceneLoaded)
                    totalFill = 2f;

                if (!extSceneLoaded)
                {
                    AsyncOperation async = new AsyncOperation();
                    async = SceneManager.LoadSceneAsync(3, LoadSceneMode.Additive);

                    while (!async.isDone)
                    {
                        loadIcon.fillAmount = Mathf.Lerp(loadIcon.fillAmount, async.progress / totalFill, 0.8f);
                        yield return null;
                    }

                    odm.DeactivateObject(externalLowPoly, null);
                }

                if (!sceneLoaded)
                {
                    int checkedScene = -1;
                    checkedScene = CheckForLevel(whichLevel);

                    if (checkedScene > -1)
                    {
                        AsyncOperation async = new AsyncOperation();
                        async = SceneManager.UnloadSceneAsync(checkedScene);

                        while (!async.isDone)
                        {
                            loadIcon.fillAmount = Mathf.Lerp(loadIcon.fillAmount, async.progress, 0.85f) / 2f;
                            yield return null;
                        }

                        lowerPolyLevels[checkedScene - 4].SetActive(true);
                        foreach (Transform t in lowerPolyLevels[checkedScene - 4].GetComponentsInChildren<Transform>(true))
                            t.gameObject.SetActive(true);

                        AsyncOperation async2 = new AsyncOperation();
                        async2 = SceneManager.LoadSceneAsync(whichLevel, LoadSceneMode.Additive);

                        while (!async2.isDone)
                        {
                            loadIcon.fillAmount = (Mathf.Lerp(loadIcon.fillAmount, async2.progress, 0.85f) / 2f) + 0.5f;
                            yield return null;
                        }
                    }
                    else
                    {
                        AsyncOperation async = new AsyncOperation();
                        async = SceneManager.LoadSceneAsync(whichLevel, LoadSceneMode.Additive);
                        while (!async.isDone)
                        {
                            if (totalFill == 2f)
                                loadIcon.fillAmount = Mathf.Lerp(loadIcon.fillAmount, 0.5f + (async.progress / totalFill), 0.8f);
                            else
                                loadIcon.fillAmount = Mathf.Lerp(loadIcon.fillAmount, async.progress, 0.8f);
                            yield return null;
                        }
                    }
                }

                yield return null;
                foreach (GameObject g in GameObject.FindGameObjectsWithTag("Checkpoint"))
                {
                    if (g.GetComponent<Checkpoint>().checkpointID == whichCheckpoint)
                        g.GetComponent<Checkpoint>().ActivateCheckpoint();
                }
                yield return null;
                PlayerPrefs.SetInt("LastCheckpoint", whichCheckpoint);
                //    PlayerPrefs.Save();

                tpc.gameObject.GetComponentInParent<OneWayManager>().CheckOneWays();

                if (curLPT != null)
                    curLPT.ExitTriggerForced();

                for (float f = 0f; f < 2.1f; f += (1 / 60f))
                    yield return null;

                foreach (LowPolyTrigger lpt in lowPolyTriggers)
                    lpt.currentlyInside = false;

                if (whichCheckpoint != 16 && whichCheckpoint != 17 && whichCheckpoint != 37 && atmosphereManager.curLevel == "Level5")
                {
                    while (atmosphereManager.triggerCount > 0)
                        atmosphereManager.ExitTrigger();
                    atmosphereManager.curLevel = "";
                }

                //PLAZA
                if (whichCheckpoint == 0 || whichCheckpoint == 1)
                    lowPolyTriggers[0].EnterTriggerForced();

                //1
                if (whichCheckpoint == 3 || whichCheckpoint == 4 || whichCheckpoint == 5)
                    lowPolyTriggers[1].EnterTriggerForced();

                //2
                if (whichCheckpoint == 7 || whichCheckpoint == 8 || whichCheckpoint == 33)
                    lowPolyTriggers[2].EnterTriggerForced();

                //3
                if (whichCheckpoint == 10 || whichCheckpoint == 11)
                    lowPolyTriggers[3].EnterTriggerForced();

                //4
                if (whichCheckpoint == 13 || whichCheckpoint == 14 || whichCheckpoint == 30 || whichCheckpoint == 31)
                    lowPolyTriggers[4].EnterTriggerForced();

                //5
                if (whichCheckpoint == 16 || whichCheckpoint == 17 || whichCheckpoint == 37)
                {
                    if (atmosphereManager.curLevel != "Level5")
                    {
                        atmosphereManager.EnterTrigger(2);
                        atmosphereManager.curLevel = "Level5";
                    }
                    lowPolyTriggers[5].EnterTriggerForced();
                }

                //6
                if (whichCheckpoint == 18 || whichCheckpoint == 19 || whichCheckpoint == 20 || whichCheckpoint == 27)
                    lowPolyTriggers[6].EnterTriggerForced();

                //7
                if (whichCheckpoint == 22 || whichCheckpoint == 23 || whichCheckpoint == 28 || whichCheckpoint == 29)
                    lowPolyTriggers[7].EnterTriggerForced();

                //9
                if (whichCheckpoint == 24 || whichCheckpoint == 25 || whichCheckpoint == 26 || whichCheckpoint == 34 || whichCheckpoint == 35)
                    lowPolyTriggers[8].EnterTriggerForced();


                Time.timeScale = 1f;
                cam.disableControl = false;

                //	yield return new WaitForSeconds (5f);
                while (!CheckpointLoaded())
                    yield return null;
                
                Vector3 newPos = new Vector3(checkpointToSpawnAt.checkpointposition.transform.position.x, 
                    checkpointToSpawnAt.transform.position.y + 1f, 
                    checkpointToSpawnAt.checkpointposition.transform.position.z);
                tpc.gameObject.transform.position = newPos;
                yield return new WaitForSeconds(5f);

                tpc.anim.enabled = true;

                if (whichLevel > 2)
                    odm.DeactivateObject(lowerPolyLevels[whichLevel - 3].gameObject, null);
                loadAnim.SetBool("Loading", false);


                for (int f = 59; f >= 0; f--)
                {
                    tpc.gameObject.transform.position = newPos;
                    AudioListener.volume = Mathf.Lerp(1f, 0f, (f * 1f) / 60f);
                    loadFS.color = Color.Lerp(Color.clear, Color.black, (f * 1f) / 60f);
                    yield return null;
                }
                PositionMultiCharacters();

                while (tpc.beingReset)
                    yield return null;

                tpc.disableControl = false;
                tpc.ExitWaterFS();
                tpc.ExitRiverForce();
                foreach (TPC mtpc in multiTPC)
                {
                    mtpc.disableControl = false;
                    mtpc.ExitWaterFS();
                    mtpc.ExitRiverForce();
                }
                tpc.rb.velocity = Vector3.zero;
                cantPause = false;

                if (whichLevel == 3)
                    tpc.ps.ShowLevelTitle("Level1.2");
                if (whichLevel == 4)
                    tpc.ps.ShowLevelTitle("Level2");
                if (whichLevel == 5)
                    tpc.ps.ShowLevelTitle("Level3");
                if (whichLevel == 6)
                    tpc.ps.ShowLevelTitle("Level4");
                if (whichLevel == 7)
                    tpc.ps.ShowLevelTitle("Level5");
                if (whichLevel == 8)
                    tpc.ps.ShowLevelTitle("Level6");
                if (whichLevel == 9)
                    tpc.ps.ShowLevelTitle("Level7");
                if (whichLevel == 10)
                    tpc.ps.ShowLevelTitle("Level8");
            }

            foreach (AudioMixer am in audioMixers)
                am.SetFloat("musicVol", -80f + (PlayerPrefs.GetFloat("musicVolume", 8f) * 10f));

            warping = false;
        }
        //    else
        //        Debug.Log("DOUBLE WARP AVOIDED FOR " + whichLevel +" "+ whichCheckpoint +" "+ extSceneLoaded +" "+ sceneLoaded);
    }

    Checkpoint checkpointToSpawnAt;
    private bool CheckpointLoaded()
    {
        foreach (Checkpoint c in Resources.FindObjectsOfTypeAll<Checkpoint>())
        {
            if (c.checkpointID == PlayerPrefs.GetInt("LastCheckpoint", 0))
            {
                checkpointToSpawnAt = c;
                return true;
            }
        }
        return false;
    }

    private int CheckForLevel(int check)
    {  //3 - 10
        int c = 2;
        while (c <= 9)
        {
            int d = check + c;
            if (d > 11)
                d -= 8;
            if (SceneManager.GetSceneByBuildIndex(d).isLoaded)
                return d;
            c++;
        }
        return -1;
    }

    public void PositionMultiCharacters()
    {
        foreach (TPC mtpc in multiTPC)
            mtpc.transform.position = woodle.transform.position + (Vector3.up * 2f);
    }

    IEnumerator PauseDelay()
    {
        yield return null;
        CloseItemsTab();
        CloseMapTab();
        CloseOptionsTab();
        tpc.disableControl = false;
        foreach (TPC mtpc in multiTPC)
            mtpc.disableControl = false;
        cam.disableControl = false;
        main.SetActive(false);

        if (playThreeTears)
        {
            if (playAllTears)
                playAllTears = false;
            playThreeTears = false;
            WaterTearManager.FirstCutscene();
        }
        if (playAllTears)
        {
            playAllTears = false;
            WaterTearManager.AllTearsCutscene();
        }
    }

    void DisplayMarkerText(int cp)
    {
        //PLAZA
        if (cp == 0 || cp == 1 || cp == 2)
            mapText = TextTranslationManager.GetText(TextTranslationManager.TextCollection.mapLevels, 0, PlayerPrefs.GetInt("Language", 0)).ToUpper() + "\n";
        if (cp == 0)
            mapText += TextTranslationManager.GetText(TextTranslationManager.TextCollection.mapLevels, 2, PlayerPrefs.GetInt("Language", 0));
        if (cp == 1)
            mapText += TextTranslationManager.GetText(TextTranslationManager.TextCollection.mapLevels, 3, PlayerPrefs.GetInt("Language", 0));
        if (cp == 2)
            mapText += TextTranslationManager.GetText(TextTranslationManager.TextCollection.mapLevels, 4, PlayerPrefs.GetInt("Language", 0));


        //lEVEL 1
        if (cp == 3 || cp == 4 || cp == 5)
            mapText = TextTranslationManager.GetText(TextTranslationManager.TextCollection.mapLevels, 1, PlayerPrefs.GetInt("Language", 0)).ToUpper() + " 1\n";
        if (cp == 3)
            mapText += TextTranslationManager.GetText(TextTranslationManager.TextCollection.mapLevels, 5, PlayerPrefs.GetInt("Language", 0));
        if (cp == 4)
            mapText += TextTranslationManager.GetText(TextTranslationManager.TextCollection.mapLevels, 6, PlayerPrefs.GetInt("Language", 0));
        if (cp == 5)
            mapText += TextTranslationManager.GetText(TextTranslationManager.TextCollection.mapLevels, 7, PlayerPrefs.GetInt("Language", 0));

        //lEVEL 2
        if (cp == 6 || cp == 7 || cp == 8 || cp == 32 || cp == 33)
            mapText = TextTranslationManager.GetText(TextTranslationManager.TextCollection.mapLevels, 1, PlayerPrefs.GetInt("Language", 0)).ToUpper() + " 2\n";
        if (cp == 6)
            mapText += TextTranslationManager.GetText(TextTranslationManager.TextCollection.mapLevels, 8, PlayerPrefs.GetInt("Language", 0));
        if (cp == 7)
            mapText += TextTranslationManager.GetText(TextTranslationManager.TextCollection.mapLevels, 9, PlayerPrefs.GetInt("Language", 0));
        if (cp == 8)
            mapText += TextTranslationManager.GetText(TextTranslationManager.TextCollection.mapLevels, 10, PlayerPrefs.GetInt("Language", 0));
        if (cp == 32)
            mapText += TextTranslationManager.GetText(TextTranslationManager.TextCollection.mapLevels, 11, PlayerPrefs.GetInt("Language", 0));
        if (cp == 33)
            mapText += TextTranslationManager.GetText(TextTranslationManager.TextCollection.mapLevels, 12, PlayerPrefs.GetInt("Language", 0));

        //lEVEL 3
        if (cp == 9 || cp == 10 || cp == 11 || cp == 36)
            mapText = TextTranslationManager.GetText(TextTranslationManager.TextCollection.mapLevels, 1, PlayerPrefs.GetInt("Language", 0)).ToUpper() + " 3\n";
        if (cp == 9)
            mapText += TextTranslationManager.GetText(TextTranslationManager.TextCollection.mapLevels, 13, PlayerPrefs.GetInt("Language", 0));
        if (cp == 10)
            mapText += TextTranslationManager.GetText(TextTranslationManager.TextCollection.mapLevels, 14, PlayerPrefs.GetInt("Language", 0));
        if (cp == 11)
            mapText += TextTranslationManager.GetText(TextTranslationManager.TextCollection.mapLevels, 15, PlayerPrefs.GetInt("Language", 0));
        if (cp == 36)
            mapText += TextTranslationManager.GetText(TextTranslationManager.TextCollection.mapLevels, 16, PlayerPrefs.GetInt("Language", 0));

        //lEVEL 4
        if (cp == 12 || cp == 13 || cp == 14 || cp == 30 || cp == 31)
            mapText = TextTranslationManager.GetText(TextTranslationManager.TextCollection.mapLevels, 1, PlayerPrefs.GetInt("Language", 0)).ToUpper() + " 4\n";
        if (cp == 12)
            mapText += TextTranslationManager.GetText(TextTranslationManager.TextCollection.mapLevels, 17, PlayerPrefs.GetInt("Language", 0));
        if (cp == 13)
            mapText += TextTranslationManager.GetText(TextTranslationManager.TextCollection.mapLevels, 18, PlayerPrefs.GetInt("Language", 0));
        if (cp == 14)
            mapText += TextTranslationManager.GetText(TextTranslationManager.TextCollection.mapLevels, 19, PlayerPrefs.GetInt("Language", 0));
        if (cp == 30)
            mapText += TextTranslationManager.GetText(TextTranslationManager.TextCollection.mapLevels, 20, PlayerPrefs.GetInt("Language", 0));
        if (cp == 31)
            mapText += TextTranslationManager.GetText(TextTranslationManager.TextCollection.mapLevels, 21, PlayerPrefs.GetInt("Language", 0));

        //lEVEL 5
        if (cp == 15 || cp == 16 || cp == 17 || cp == 37)
            mapText = TextTranslationManager.GetText(TextTranslationManager.TextCollection.mapLevels, 1, PlayerPrefs.GetInt("Language", 0)).ToUpper() + " 5\n";
        if (cp == 15)
            mapText += TextTranslationManager.GetText(TextTranslationManager.TextCollection.mapLevels, 22, PlayerPrefs.GetInt("Language", 0));
        if (cp == 16)
            mapText += TextTranslationManager.GetText(TextTranslationManager.TextCollection.mapLevels, 23, PlayerPrefs.GetInt("Language", 0));
        if (cp == 17)
            mapText += TextTranslationManager.GetText(TextTranslationManager.TextCollection.mapLevels, 24, PlayerPrefs.GetInt("Language", 0));
        if (cp == 37)
            mapText += TextTranslationManager.GetText(TextTranslationManager.TextCollection.mapLevels, 25, PlayerPrefs.GetInt("Language", 0));

        //lEVEL 6
        if (cp == 18 || cp == 19 || cp == 20 || cp == 27)
            mapText = TextTranslationManager.GetText(TextTranslationManager.TextCollection.mapLevels, 1, PlayerPrefs.GetInt("Language", 0)).ToUpper() + " 6\n";
        if (cp == 18)
            mapText += TextTranslationManager.GetText(TextTranslationManager.TextCollection.mapLevels, 26, PlayerPrefs.GetInt("Language", 0));
        if (cp == 19)
            mapText += TextTranslationManager.GetText(TextTranslationManager.TextCollection.mapLevels, 27, PlayerPrefs.GetInt("Language", 0));
        if (cp == 20)
            mapText += TextTranslationManager.GetText(TextTranslationManager.TextCollection.mapLevels, 28, PlayerPrefs.GetInt("Language", 0));
        if (cp == 27)
            mapText += TextTranslationManager.GetText(TextTranslationManager.TextCollection.mapLevels, 29, PlayerPrefs.GetInt("Language", 0));

        //lEVEL 7
        if (cp == 21 || cp == 22 || cp == 23 || cp == 28 || cp == 29)
            mapText = TextTranslationManager.GetText(TextTranslationManager.TextCollection.mapLevels, 1, PlayerPrefs.GetInt("Language", 0)).ToUpper() + " 7\n";
        if (cp == 21)
            mapText += TextTranslationManager.GetText(TextTranslationManager.TextCollection.mapLevels, 30, PlayerPrefs.GetInt("Language", 0));
        if (cp == 22)
            mapText += TextTranslationManager.GetText(TextTranslationManager.TextCollection.mapLevels, 31, PlayerPrefs.GetInt("Language", 0));
        if (cp == 23)
            mapText += TextTranslationManager.GetText(TextTranslationManager.TextCollection.mapLevels, 32, PlayerPrefs.GetInt("Language", 0));
        if (cp == 28)
            mapText += TextTranslationManager.GetText(TextTranslationManager.TextCollection.mapLevels, 33, PlayerPrefs.GetInt("Language", 0));
        if (cp == 29)
            mapText += TextTranslationManager.GetText(TextTranslationManager.TextCollection.mapLevels, 34, PlayerPrefs.GetInt("Language", 0));

        //lEVEL 8
        if (cp == 24 || cp == 25 || cp == 26 || cp == 34 || cp == 35)
            mapText = TextTranslationManager.GetText(TextTranslationManager.TextCollection.mapLevels, 1, PlayerPrefs.GetInt("Language", 0)).ToUpper() + " 8\n";
        if (cp == 24)
            mapText += TextTranslationManager.GetText(TextTranslationManager.TextCollection.mapLevels, 35, PlayerPrefs.GetInt("Language", 0));
        if (cp == 25)
            mapText += TextTranslationManager.GetText(TextTranslationManager.TextCollection.mapLevels, 36, PlayerPrefs.GetInt("Language", 0));
        if (cp == 26)
            mapText += TextTranslationManager.GetText(TextTranslationManager.TextCollection.mapLevels, 37, PlayerPrefs.GetInt("Language", 0));
        if (cp == 34)
            mapText += TextTranslationManager.GetText(TextTranslationManager.TextCollection.mapLevels, 38, PlayerPrefs.GetInt("Language", 0));
        if (cp == 35)
            mapText += TextTranslationManager.GetText(TextTranslationManager.TextCollection.mapLevels, 39, PlayerPrefs.GetInt("Language", 0));

        mapMarkerText.text = mapText;

        if (PlayerPrefs.GetInt("Language") == 11)
        {
            mapMarkerText.font = TextTranslationManager.singleton.arabicFont;
            mapMarkerText.fontStyle = FontStyle.Bold;
        }
        else
        {
            mapMarkerText.font = originalMapFont;
            mapMarkerText.fontStyle = originalMapFontStyle;
        }
    }

    public void StartCutscene(PlayableDirector cutscene, Vector3 endPosition)
    {
        currentCutscene = cutscene;
        cutsceneEndPos = endPosition;
        cutscene.gameObject.SetActive(true);
        StartCoroutine("WaitForCutscene", cutscene);

        RendererToggle(false);
        ControlDisable(true);
    }

    public void SkipCutscene(PlayableDirector cutscene)
    {
        if (skipCSAnim.GetBool("skipOn") == false)
        {
            skipCSAnim.SetBool("skipOn", true);
            StartCoroutine("SkipWait");
        }
        else
        {
            if (input.GetButtonDown("Start"))
            {
                cutscene.time = cutscene.duration;
                StopCoroutine("WaitForCutscene");
                EndOfCutscene(cutscene);
            }
        }
    }

    public void EndOfCutscene(PlayableDirector cutscene)
    {
        currentCutscene = null;
        cutscene.gameObject.SetActive(false);
        cam.gameObject.SetActive(true);

        if (cutsceneEndPos != Vector3.one)
            tpc.transform.position = cutsceneEndPos;
        tpc.transform.forward = cam.transform.forward;

        RendererToggle(true);

        ControlDisable(false);
    }

    public IEnumerator WaitForCutscene(PlayableDirector cutscene)
    {
        while (cutscene.time != cutscene.duration)
            yield return null;

        EndOfCutscene(cutscene);
    }

    public IEnumerator SkipWait()
    {
        yield return new WaitForSeconds(2f);
        skipCSAnim.SetBool("skipOn", false);
    }

    void RendererToggle(bool enable)
    {
        RendererContent(woodle, enable);
        RendererContent(foxChara, enable);
        RendererContent(beaverChara, enable);
        RendererContent(bushChara, enable);
    }

    void RendererContent(GameObject chara, bool enable)
    {
        foreach (Renderer r in chara.GetComponentsInChildren<Renderer>())
            r.enabled = enable;
    }

    public void ControlDisable(bool disable)
    {
        for (int p = 0; p < 4; p++)
        {
            if (PlayerManager.GetPlayer(p) != null && PlayerManager.GetPlayer(p).rb != null)
            {
                PlayerManager.GetPlayer(p).rb.velocity = Vector3.zero;
                PlayerManager.GetPlayer(p).disableControl = disable;
                PlayerManager.GetPlayer(p).inCutscene = disable;
            }
        }
        cam.disableControl = disable;
    }

    public void ShowLevelTitle(string levelName)
    {
        int levelID = 0;
        if (levelName == "Level2")
            levelID = 1;
        if (levelName == "Level3")
            levelID = 2;
        if (levelName == "Level4")
            levelID = 3;
        if (levelName == "Level5")
            levelID = 4;
        if (levelName == "Level6")
            levelID = 5;
        if (levelName == "Level7")
            levelID = 6;
        if (levelName == "Level8")
            levelID = 7;

        StopCoroutine("LevelTitleCoRo");
        StartCoroutine("LevelTitleCoRo", levelID);
    }

    IEnumerator LevelTitleCoRo(int id)
    {
        while (!levelTitlesAnim.GetCurrentAnimatorStateInfo(0).IsName("Off"))
            yield return null;

        yield return new WaitForSeconds(0f);

        if (currentLevelTitle != id)
        {
            currentLevelTitle = id;

            textToTranslate.SetTextElement(levelTitlesText, null, TextTranslationManager.TextCollection.levelTitles, id, "", false, false, levelTitlesFont, levelTitlesFontStyle);

            levelTitlesAnim.Play("Animate", 0);
        }
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
}
