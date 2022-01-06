using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using Users;
using Storage;
using ConsoleUtils;
//using UnityPluginLog;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Xml.Serialization;
using System.Linq;


[XmlRoot("prefs")]
public class XONESavedItems
{
    [XmlElement("item")]
    public XONESavedItem[] Items { get; set; }
}

public class XONESavedItem
{
    [XmlAttribute]
    public string key;
    [XmlAttribute]
    public byte[] value;
}

public class XOneEventsManager : MonoBehaviour
{

#if UNITY_XBOXONE

    public Users.User currentLoggedUser, oldLoggedUser;
    public ConnectedStorage mainStorage;
    public delegate void CallBackSuspend();
    static public CallBackSuspend callBackSuspend;
    TitleStorage tStorage;

    //tutti i dati della console XONE
    //public User oldLoggedUser, currentLoggedUser;
    //public ConstrainedNotifier constraintedNotifier;
    //public ConnectedStorage connectedStorage;

    public delegate void XBPLMEvent();
    public event XBPLMEvent OnConstrained;
    public event XBPLMEvent OnUnConstrained;
    public event XBPLMEvent OnSuspending;
    public event XBPLMEvent OnResuming;

    public delegate void XBUserEvent();
    public event XBUserEvent OnUserChanged;
    public event XBUserEvent OnStart;
    
    public bool isSuspended;
    bool isGoingBackToMain;
    public GameObject rewiredObj;

    public string[] levelNames = new string[10];
    //
    public UnityEngine.UI.Text syncronizingText;
    public UnityEngine.UI.Image syncImage;
    public Sprite startASprite, syncWaitSprite;

    //
    bool isUserInitialized = false;
    //
    public bool isMultiplayerOn;

    private void Awake()
    {
        DataManager.xOneEventsManager = this;      
    }
    
    void Start()
    {
        //
        DontDestroyOnLoad(this.gameObject);
        //
        isGoingBackToMain = false;
        //
        levelNames[0] = "MainPlaza7New";
        levelNames[1] = "ExternalWorld";
        levelNames[2] = "Level1.2";
        levelNames[3] = "Level2";
        levelNames[4] = "Level3";
        levelNames[5] = "Level4";
        levelNames[6] = "Level5";
        levelNames[7] = "Level6";
        levelNames[8] = "Level7";
        levelNames[9] = "Level8";

#if UNITY_EDITOR
        SceneManager.LoadScene(1);
        return;
#endif
        //
        //Debug.Log("pluginmanagr create");
        //PluginLogManager.Create();
        //Debug.Log("onlog");
        //PluginLogManager.OnLog += OnLog;
        Debug.Log("usersmanager and storage creation");
        //
        if (!XONE_UserManager.isCreated)
        {
            UsersManager.Create();
            Debug.Log("storagemanager create");
            StorageManager.Create();
            Debug.Log("all done");
            XONE_UserManager.isCreated = true;
        }
        //
        UnloadEvents();
        //
        //callback eventi user  
        UsersManager.OnUsersChanged += OnUsersChanged;
        UsersManager.OnUserSignIn += OnUserSignIn;
        UsersManager.OnSignInComplete += OnUserSignInComplete;
        UsersManager.OnUserSignOut += OnUserSignOut;
        UsersManager.OnSignOutStarted += OnUserSignOutStarted;
        //UsersManager.OnDisplayInfoChanged += OnUserDisplayInfoChange;
        UsersManager.OnAppCurrentUserChanged += OnAppCurrentUserChange;
        UsersManager.OnControllerPairingChanged += OnControllerPairingChanged;
        //
        XboxOnePLM.OnResourceAvailabilityChangedEvent += ResourceAvailabilityChangedEvent;
        XboxOnePLM.OnSuspendingEvent += Suspending;
        XboxOnePLM.OnResumingEvent += Resuming;
        XboxOnePLM.OnActivationEvent += Activation;
    }

    //
    void OnDestroy()
    {
        //
        Debug.LogError("ON DESTROY ... called");
        //
        UnloadEvents();
        //
        DataManager.xOneEventsManager = null;
        //       
    }

    //
    void UnloadEvents()
    {
        //
        UsersManager.OnUsersChanged -= OnUsersChanged;
        UsersManager.OnUserSignIn -= OnUserSignIn;
        UsersManager.OnSignInComplete -= OnUserSignInComplete;
        UsersManager.OnUserSignOut -= OnUserSignOut;
        UsersManager.OnSignOutStarted -= OnUserSignOutStarted;
        //UsersManager.OnDisplayInfoChanged += OnUserDisplayInfoChange;
        UsersManager.OnAppCurrentUserChanged -= OnAppCurrentUserChange;
        UsersManager.OnControllerPairingChanged -= OnControllerPairingChanged;
        //
        XboxOnePLM.OnResourceAvailabilityChangedEvent -= ResourceAvailabilityChangedEvent;
        XboxOnePLM.OnSuspendingEvent -= Suspending;
        XboxOnePLM.OnResumingEvent -= Resuming;
    }


    //void OnLevelWasLoaded()
    //{
    //    isGoingBackToMain = false;
    //}

    void ResourceAvailabilityChangedEvent(bool amConstrained)
    {
        //
        if (isGoingBackToMain)
            return;

        //
        Debug.LogError("------------------------------------ CHANGED RESOURCE !!!!!!!!!!!!! : " + amConstrained.ToString());
        //
        isSuspended = amConstrained;
        if (amConstrained)
        {
            DataManager.isSuspended = true;
            DataManager.isPaused = true;
            //
            Debug.LogError("------------------------------------ SUSPENDING !!!!!!!!!!!!!");

           if (OnConstrained != null) OnConstrained();
        }
        else
        {
            Debug.LogError("------------------------------------ RESUME !!!!!!!!!!!!!");

            DataManager.isPaused = false;
            if (OnUnConstrained != null) OnUnConstrained();
            //
            // CompareUsers();
        }
    }

    //
    bool amDone = false;
    bool shouldSuspend = false;
    bool readyToSuspend = false;
    //
    public IEnumerator coOnSuspend(CallBackSuspend callback)
    {

        Debug.Log("\nCoOnSuspend ...");
        //
        shouldSuspend = true;
        while (!readyToSuspend && !isSaving)
        {
            yield return null;
        }

        //Debug.Log("Storage dispose ...");
        
        //
        // Forcibly dispose of the title storage object
        // it maintains a connection to live which will
        // become invalid after we return.
       /* if (tStorage != null)
        {
            tStorage.Dispose();
            tStorage = null;
        }*/

        Debug.Log("Callback ...");
        //
        if (callback != null)
            callback();

        yield break;
    }

    bool SuspendCheck()
    {
        if (shouldSuspend)
        {
            readyToSuspend = true;
            return true;
        }
        return false;
    }

    public void coOnResume(int userId)
    {
        amDone = true;
        shouldSuspend = false;
        readyToSuspend = false;
        isSuspended = false;
        //
        Debug.Log("[TitleStorage] reinit after resume\n");
        tStorage = TitleStorage.Create(userId);
    }

    //
    void Suspending()
    {
        //
        isSuspended = true;
        XboxOnePLM.AmReadyToSuspendNow();

        /*
        //
        if (tStorage != null)
        {
            Debug.Log("Storage is still active suspending in a bit.");
            // We get the storage system to shut down on a suspend request.
            // NOTE: We have to dispose of the storage object and recreate it on resume as it has
            //       hooks that are talking to live and those go invalid across a suspend / resume.
            StartCoroutine(coOnSuspend(() =>
            {
                Debug.LogError("SUSPENDING - AmReadyToSuspendNow ");
                XboxOnePLM.AmReadyToSuspendNow();
            }));
        }
        else
        {
            Debug.Log("Immediate suspend! Shutting down now.");
            Debug.LogError("SUSPENDING - AmReadyToSuspendNow ");
            XboxOnePLM.AmReadyToSuspendNow();
        }        */
    }

    //
    void Resuming(double span)
    {
        if (UsersManager.Users[0] != null)
            coOnResume(UsersManager.Users[0].Id);
    }

    //
    void Activation(ActivatedEventArgs args)
    {

    }

    public void InvokeSignInScreen()
    {
        Debug.Log("INVOKE SIGN SCREEN : " + DataManager.isOnSignScreen);
        if (!DataManager.isOnSignScreen)
        {
            DataManager.isOnSignScreen = true;
            UsersManager.RequestSignIn(AccountPickerOptions.AllowGuests);
        }
    }

    //
    void OnControllerPairingChanged(string controllerType, System.UInt64 controllerId, User previousUser, User user)
    {
        //
        Debug.LogError("USER controller CHANGED, multiplayer " + isMultiplayerOn);
        //
        if (isMultiplayerOn)
        {
            Debug.LogError("Multiplayer detected");
            return;
        }
        //
        Debug.LogError("Old USER is " + previousUser.Id);
        Debug.LogError("Current USER is " + user.Id);
        //
        if (previousUser.Id != user.Id)
        {
            //
            Debug.Log("USER CHANGED ");
            //
            isGoingBackToMain = true;
            //
            BackToMainMenu();
        }
    }

    //
    void OnUsersChanged(int id, bool wasAdded)
    {
        //
        if (isMultiplayerOn)
        {
            Debug.LogError("Multiplayer detected");
            return;
        }

        if (oldLoggedUser == null) Debug.Log("OLD LOGGED USER is NULL");
        // 
        currentLoggedUser = UsersManager.FindUserById(id);
        if (currentLoggedUser != null && oldLoggedUser != null)
        {
            if (currentLoggedUser.Id != oldLoggedUser.Id)
            {
                //
                isGoingBackToMain = true;
                BackToMainMenu();
                return;
            }
        }
    }

    void OnUserSignIn(int id)
    {
        
    }

    void OnUserSignInComplete(int rtype, int id)
    {
        //
        if (isMultiplayerOn)
        {
            Debug.LogError("Multiplayer detected");
            return;
        }

        //
        // if first screen, create storage... else jump to first screen
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            //
            currentLoggedUser = UsersManager.FindUserById(id);
            if (currentLoggedUser == null)
            {
                //
                isGoingBackToMain = true;
                BackToMainMenu();
            }
            //
            DataManager.lastXONEUserID = currentLoggedUser.Id;
            firstLogin = false;
            isUserInitialized = true;
            mainStorage = null;
            creatingStorage = false;
            isGoingBackToMain = false;
            //
            Update();
        }
        else 
        //
        if (UsersManager.FindUserById(id) != null)
        {
            //
            currentLoggedUser = UsersManager.FindUserById(id);
            DataManager.lastXONEUserID = currentLoggedUser.Id;
            Debug.Log("USER_SIGNIN " + currentLoggedUser.GameDisplayName);
            DataManager.isOnSignScreen = false;
            //
            CompareUsers();
            //
            //reimpostiamo la creazione storage
            mainStorage = null;
            creatingStorage = false;
            //
            if (firstLogin)
            {
                firstLogin = false;
                isUserInitialized = true;
            }
        }
        else
        {
            //
            isGoingBackToMain = true;
            BackToMainMenu();
        }
    }

    void OnUserSignOut(int id)
    {
        //
        Debug.LogError("LOGOUT");
        Debug.LogError("ID to be logged out : " + id);
        //
        Debug.LogError("logged user is " + DataManager.lastXONEUserID);
        //
        if (id == DataManager.lastXONEUserID)
        {
            // decomm
            isGoingBackToMain = true;
            BackToMainMenu();
        }
    }

    void OnUserSignOutStarted(int id, IntPtr deferred)
    {
       
    }

    //
    bool progsRead = false;
    //
    public void hStorageCreated(ConnectedStorage storage, CreateConnectedStorageOp op)
    {
        //StorageKeyValue.storageInstance = storage;
        //check success
        if (!op.Success)
        {
            Debug.LogError("*****  ****** STORAGE CREATION FAILED [0x" + op.Result.ToString("X") + "]");
            creatingStorage = false;
            startPressed = false;
            //
            isGoingBackToMain = true;
            //
            BackToMainMenu();
            return;
        }
        mainStorage = storage;
        //
        // once the storage is created we update the old user info too
        //
        oldLoggedUser = currentLoggedUser;
        DataManager.lastXONEUserID = currentLoggedUser.Id;
        //
        Debug.LogError("STORAGE CREATED OKKKKKKKKKKKKKKKKKKKKKKKKKKKK");

        //leggiamo i settaggi
        //LoadOptions();

        //
        StorageKeyValue.storageInstance = mainStorage;
        tStorage = TitleStorage.Create(currentLoggedUser.Id);

        // Set Default values
        //
        GetSystemLanguage();

        // mainStorage.SetActiveContainer("WoodleTree2");

        // leggiamo le progressioni
        LoadData();
        //
        XONEAchievements.Initialize(DataManager.lastXONEUserID);
    }


    void OnSubmitDone(ContainerContext storage, SubmitDataMapUpdatesAsyncOp op)
    {
        // If we are being asked to suspend do not try to issue more operations
        // our storage object will be invalid when we return from suspension
        if (SuspendCheck()) return;

        bool ok = op.Success && op.Status == ConnectedStorageStatus.SUCCESS;
        Debug.LogError("**** OnSubmitDone: IsComplete = " + op.IsComplete + ", Success = " + op.Success + ", HRESULT = 0x" + op.Result.ToString("X8"));

        // Now try to read that data we just wrote back
        if (op.Success)
        {
            //
            isSaving = false;
            //
            //op.Complete((uint)op.Status, op.Result);
            //op.FreeHandle();

            // Data saved!!
            Debug.LogError(" ------------ ALL DATA SUCCESSFULLY SAVED --------------");
            //
            //storage.QueryBlobInfoAsync("", QueryBlobInfoReturns);
        }
    }
    //
    void SaveStorage(DataMap mDataToSave)
    {
        try
        {
            Debug.LogError(" ------------ CALLING SUBMIT ASYNC --------------");
            mainStorage.SubmitUpdatesAsync(mDataToSave, null, OnSubmitDone);
        }
        catch (Exception ex)
        { }
    }

    //
    Dictionary<string, byte[]> storageSavedBytes = new Dictionary<string, byte[]>();
    public void SetStorageString(DataMap mDataToSave, string prefName, int val)
    {

        //
        if (!storageSavedBytes.ContainsKey(prefName))
             storageSavedBytes.Add(prefName, System.BitConverter.GetBytes(val));
        else
            Debug.LogError("The KEY " + prefName + " ALREADY EXISTS: skipping data");
        return;

        //
        mDataToSave.AddOrReplaceBuffer(prefName, System.BitConverter.GetBytes(val));
        //mDataToSave.AddOrReplaceBuffer("var_name", System.Text.Encoding.UTF8.GetBytes(sceneName));
        //
        
        /*
        StorageKeyValue.SetString(prefName, val.ToString()).OnSaveDone += delegate (StorageKeyValue savedKeyValue) 
        {
            Debug.LogError("SAVED KEY '" + savedKeyValue.key + "'" + " VALUE = " + savedKeyValue.value);
        };*/
    }
    public void SetStorageString(DataMap mDataToSave, string prefName, float val)
    {
        Debug.LogError("SAVING " + prefName); // + " VALUE = " + val.ToString());

        //
        if (!storageSavedBytes.ContainsKey(prefName))
            storageSavedBytes.Add(prefName, System.BitConverter.GetBytes(val));
        else
            Debug.LogError("The KEY " + prefName + " ALREADY EXISTS: skipping data");
        return;

        mDataToSave.AddOrReplaceBuffer(prefName, System.BitConverter.GetBytes(val));
        //mDataToSave.AddOrReplaceBuffer("var_name", System.Text.Encoding.UTF8.GetBytes(sceneName));

        /*StorageKeyValue.SetString(prefName,val.ToString()).OnSaveDone += delegate (StorageKeyValue savedKeyValue)
        {
            Debug.LogError("SAVED KEY '" + savedKeyValue.key + "'" + " VALUE = " + savedKeyValue.value);
        };*/
    }
    public void SetStorageString(DataMap mDataToSave, string prefName, string val)
    {
        Debug.LogError("SAVING " + prefName); // + " VALUE = " + val.ToString());

        //
        if (!storageSavedBytes.ContainsKey(prefName))
            storageSavedBytes.Add(prefName, System.Text.Encoding.UTF8.GetBytes(val));
        else
            Debug.LogError("The KEY " + prefName + " ALREADY EXISTS: skipping data");

        return;


        //mDataToSave.AddOrReplaceBuffer("var_name", System.BitConverter.GetBytes(val));
        mDataToSave.AddOrReplaceBuffer(prefName, System.Text.Encoding.UTF8.GetBytes(val));

        /*
        StorageKeyValue.SetString(prefName, val.ToString()).OnSaveDone += delegate (StorageKeyValue savedKeyValue)
        {
            Debug.LogError("SAVED KEY '" + savedKeyValue.key + "'" + " VALUE = " + savedKeyValue.value);
        };*/
    }
    
    //
    const string BUFFER = "WoodleTree2";
    public void LoadData()
    {
        //
        mainStorage.GetAsync(new string[] { BUFFER }, GetAsyncReturns);
    }


    void GetAsyncReturns(ContainerContext storage, GetDataMapViewAsyncOp op, DataMapView view)
    {
        // If we are being asked to suspend do not try to issue more operations
        // our storage object will be invalid when we return from suspension
        if (SuspendCheck()) return;
        //

        //
        if (op.Success)
        {
            // Data loaded!!
            byte[] buffer = view.GetBuffer("WoodleTree2");
            //
            Debug.LogError("!!!!!!!!!!!!!! BLOB FOUND !!!!!!!");
            Debug.LogError("         BUFFERINFO: LENGTH: [" + buffer.Length + "] ");
            //
            LoadProgs(buffer, view);
        }
        else
        {
            if (op.Result.ToString() == "2156068872")
            {
                Debug.LogError(" !!!!!!!!!!!!!!  ----   DATA NOT FOUND!");
                DeletePrefs();
            }
            else
            Debug.LogError("!!!!!!!!! ERROR SYNCING DATA : " + op.Result.ToString());
            
            //
            if (SceneManager.GetActiveScene().buildIndex == 0)
                progsRead = true; // it has to load splash screen anyway while the user is new
        }
    }

    void DeletePrefs()
    {
        Debug.LogError("Deleting prefs ...");

        int vibInt = PlayerPrefs.GetInt("Vibration", 1);
        int aaInt = PlayerPrefs.GetInt("AA", 1);
        float musicFT = PlayerPrefs.GetFloat("musicVolume", 8f);
        float effectsFT = PlayerPrefs.GetFloat("effectsVolume", 8f);


        PlayerPrefs.SetInt("Berries", 0);
        PlayerPrefs.SetInt("BlueBerries", 0);


        foreach (string s in levelNames)
        {
            string newString = "";
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

            //DELETE ALL BLUE BERRIES COLLECTED
            for (int bb = 0; bb < total; bb++)
                newString = newString.Insert(0, "0");
            PlayerPrefs.SetString(s + "BlueBerry", newString);
        }

        //
        PlayerPrefs.SetInt("AllBlueBerries", 0);
        PlayerPrefs.SetInt("BlueBerryTotal", 0);
        PlayerPrefs.SetInt("RedBerryTotal", 0);
        PlayerPrefs.SetInt("WoodleFriends", 0);
        PlayerPrefs.SetInt("IsThere", 0);
        PlayerPrefs.SetInt("Top", 0);

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
        
        // ITEMS TAKEN
        PlayerPrefs.SetInt("PaidForItem", 0);

        // ITEMS TAKEN
        PlayerPrefs.SetInt("PaidItemsCount", 0);
        PlayerPrefs.SetInt("PowerUpsBought", 0);
        
        // ENEMIES
        PlayerPrefs.SetInt("DarkEnemiesKilledCount", 0);
        PlayerPrefs.SetInt("DarkEnemiesAchiev", 0);
        PlayerPrefs.SetInt("NormalEnemiesKilledCount", 0);
        PlayerPrefs.SetInt("NormalEnemiesAchiev", 0);


        //CHALLENGES
        for (int ch = 0; ch <= 11; ch++)
        {
            PlayerPrefs.SetInt("Challenge" + ch.ToString() + "Found", 0);
        }
        PlayerPrefs.SetInt("AllFlowers", 0);

        //CUTSCENES
        PlayerPrefs.SetInt("IntroWatched", 0);
        PlayerPrefs.SetInt("LastCheckpoint", -1);
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

        PlayerPrefs.SetInt("Vibration", vibInt);
        PlayerPrefs.SetFloat("musicVolume", musicFT);
        PlayerPrefs.SetFloat("effectsVolume", effectsFT);
        PlayerPrefs.SetInt("AA", aaInt);

        PlayerPrefs.SetInt("Checkpoint0", 1);
        PlayerPrefs.SetInt("Checkpoint0Scene", 2);

        //PlayerPrefs.Save();
    }

    //
    public void GetSystemLanguage()
    {
        /*
            English,
            Russian,
            Spanish,
            Italian,
            Chinese,
            French,
            Portuguese,
            Dutch,
            German,
            Japanese,
            Turkish,
            Arabic,
            Polish,
            Danish,
            Korean
      */

        // make sure languages are in the manifest file
        SystemLanguage lang = Application.systemLanguage;
        switch (lang)
        {
            //
            case SystemLanguage.English:
            PlayerPrefs.SetInt("Language", 0);
            break;
            case SystemLanguage.Romanian:
            PlayerPrefs.SetInt("Language", 1);
            break;            //
            case SystemLanguage.Spanish:
            PlayerPrefs.SetInt("Language", 2);
            break;            //
            case SystemLanguage.Italian:
            PlayerPrefs.SetInt("Language", 3);
            break;            //
            case SystemLanguage.Chinese:
            PlayerPrefs.SetInt("Language", 4);
            break;            //
            case SystemLanguage.French:
            PlayerPrefs.SetInt("Language", 5);
            break;            //
            case SystemLanguage.Portuguese:
            PlayerPrefs.SetInt("Language", 6);
            break;            //
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
        }
    }

    void QueryBlobInfoReturns(ContainerContext storage, BlobInfoQueryAsyncOp op)
    {
        // If we are being asked to suspend do not try to issue more operations
        // our storage object will be invalid when we return from suspension
        if (SuspendCheck()) return;

        //
        if (op.Success)
        {
            for (uint i = 0; i < op.Query.Length; ++i)
            {
                BlobInfo ifo = op.Query[i];
                Debug.LogError("***** Found Blob " + ifo.Name + " [" + ifo.TotalSize + "]\n");
            }
            op.Query.Dispose();
        }else
        {
            Debug.LogError("!!!!!!!!!!! QUERY BLOB FAILED : " + op.Result.ToString());
        }
    }

    
    //
    bool isSaving = false;
    public void SaveProgs()
    {
        //return;
        //
        // Debug.LogError("SAVING PROGS");
        //
        if (!isSaving) // SavingCoRo();
             StartCoroutine(SavingCoRo());
    }


    public void SaveBlueBerry(string BerryID, string BerryValue)
    {
        StartCoroutine(SaveBlueBerryCo(BerryID, BerryValue));
    }

    IEnumerator SaveBlueBerryCo(string BerryID, string BerryValue)
    {
        isSaving = true;

        storageSavedBytes.Clear();

        StorageKeyValue.storageInstance = mainStorage;

        DataMap dataMap = DataMap.Create();

        SetStorageString(dataMap, "Berries", PlayerPrefs.GetInt("Berries"));
        SetStorageString(dataMap, "BlueBerries", PlayerPrefs.GetInt("BlueBerries"));
        
        SetStorageString(dataMap, "AllBlueBerries", PlayerPrefs.GetInt("AllBlueBerries"));
        SetStorageString(dataMap, "BlueBerryTotal", PlayerPrefs.GetInt("BlueBerryTotal"));

        SetStorageString(dataMap, BerryID, BerryValue);

        yield return null;

        byte[] byteArr = new byte[0];

        XmlSerializer serializer = new XmlSerializer(typeof(XONESavedItems), new XmlRootAttribute("prefs"));
        
        try
        {
            using (var ms = new System.IO.MemoryStream())
            {
                XONESavedItems xoneSavedItems = new XONESavedItems();
                xoneSavedItems.Items = storageSavedBytes.Select(kv => new XONESavedItem() { key = kv.Key, value = kv.Value }).ToArray();
                foreach (XONESavedItem it in xoneSavedItems.Items)
                    Debug.Log("\n***** Serializing " + it.key.ToString() + " with value " + it.value.ToString());
                //
                serializer.Serialize(ms, xoneSavedItems);
                byteArr = ms.ToArray();
                Debug.Log("***** Data serialized");
            }
        }
        catch (Exception ex)
        {
            Debug.Log("**** Serialization error : " + ex.InnerException.ToString());
        }

        dataMap.AddOrReplaceBuffer("WoodleTree2", byteArr);
        
        SaveStorage(dataMap);
    }

    IEnumerator SavingCoRo()
    {
        //
        isSaving = true;
        storageSavedBytes.Clear();

        //
        StorageKeyValue.storageInstance = mainStorage;
        DataMap map = DataMap.Create();

        // CHECKPOINT INFO
        SetStorageString(map, "LastCheckpoint", PlayerPrefs.GetInt("LastCheckpoint"));
        SetStorageString(map, "FirstCheckpoint", PlayerPrefs.GetInt("FirstCheckpoint"));

        // COUNTERS
        SetStorageString(map, "Berries", PlayerPrefs.GetInt("Berries"));
        SetStorageString(map, "BlueBerries", PlayerPrefs.GetInt("BlueBerries"));
        //
        SetStorageString(map, "AllBlueBerries", PlayerPrefs.GetInt("AllBlueBerries"));
        SetStorageString(map, "BlueBerryTotal", PlayerPrefs.GetInt("BlueBerryTotal"));
        SetStorageString(map, "RedBerryTotal", PlayerPrefs.GetInt("RedBerryTotal"));

        SetStorageString(map, "WoodleFriends", PlayerPrefs.GetInt("WoodleFriends"));
        SetStorageString(map, "IsThere", PlayerPrefs.GetInt("IsThere"));
        SetStorageString(map, "Top", PlayerPrefs.GetInt("Top"));

        // test save

        yield return null;

        foreach (string s in levelNames)
        {
            SetStorageString(map, s + "BlueBerry", PlayerPrefs.GetString(s + "BlueBerry"));
        }

        //TEARS
        for (int lvl = 1; lvl <= 8; lvl++)
        {
            for (int tearNo = 1; tearNo <= 3; tearNo++)
                SetStorageString(map, "Vase" + tearNo.ToString() + "Level" + lvl.ToString(), PlayerPrefs.GetInt("Vase" + tearNo.ToString() + "Level" + lvl.ToString()));
            //
            SetStorageString(map, "Played" + lvl.ToString() + "TreeCompletion", PlayerPrefs.GetInt("Played" + lvl.ToString() + "TreeCompletion"));
            //yield return null;
        }

        SetStorageString(map, "AllTears", PlayerPrefs.GetInt("AllTears"));

        //CHECKPOINTS
        for (int cp = 1; cp <= 37; cp++)
        {
            SetStorageString(map, "Checkpoint_" + cp.ToString(), PlayerPrefs.GetInt("Checkpoint" + cp.ToString()));
            //    PlayerPrefs.SetInt("Checkpoint" + cp.ToString() + "Scene", 0);
            //if((cp % 10) == 0)
            // yield return null;
        }
        // CHECKPOINTS
        SetStorageString(map, "Checkpoint", PlayerPrefs.GetInt("Checkpoint"));
        SetStorageString(map, "Checkpoint0", PlayerPrefs.GetInt("Checkpoint0"));
        SetStorageString(map, "Checkpoint0Scene", PlayerPrefs.GetInt("Checkpoint0Scene"));

        //CHALLENGES
        for (int ch = 0; ch <= 11; ch++)
        {
            SetStorageString(map, "Challenge" + ch.ToString() + "Found", PlayerPrefs.GetInt("Challenge" + ch.ToString() + "Found"));
        }

        yield return null;


        //SHOPS
        for (int xyz = 0; xyz < 25; xyz++)
        {
            SetStorageString(map, "UsingItem" + xyz.ToString(), PlayerPrefs.GetInt("UsingItem" + xyz.ToString()));
            SetStorageString(map, "PaidForItem" + xyz.ToString(), PlayerPrefs.GetInt("PaidForItem" + xyz.ToString()));
            //if((xyz % 10) == 0)
            //yield return null;
        }

        //BUTTONS
        for (int ab = 0; ab <= 124; ab++)
        {
            SetStorageString(map, "Button" + ab.ToString(), PlayerPrefs.GetInt("Button" + ab.ToString()));
            //if((ab % 10) == 0)
            //yield return null;

        }

        // COLLECTABLES
        SetStorageString(map, "marmellade1", PlayerPrefs.GetInt("marmellade1"));
        SetStorageString(map, "marmellade2", PlayerPrefs.GetInt("marmellade2"));
        SetStorageString(map, "marmellade3", PlayerPrefs.GetInt("marmellade3"));
        SetStorageString(map, "plantjar", PlayerPrefs.GetInt("plantjar"));
        SetStorageString(map, "plantjar2", PlayerPrefs.GetInt("plantjar2"));
        SetStorageString(map, "book1", PlayerPrefs.GetInt("book1"));
        SetStorageString(map, "book2", PlayerPrefs.GetInt("book2"));
        SetStorageString(map, "paint", PlayerPrefs.GetInt("paint"));
        SetStorageString(map, "paint2", PlayerPrefs.GetInt("paint2"));
        SetStorageString(map, "gameboy", PlayerPrefs.GetInt("gameboy"));
        SetStorageString(map, "bell", PlayerPrefs.GetInt("bell"));
        SetStorageString(map, "heater", PlayerPrefs.GetInt("heater"));
        SetStorageString(map, "globe", PlayerPrefs.GetInt("globe"));
        SetStorageString(map, "cupbear", PlayerPrefs.GetInt("cupbear"));
        SetStorageString(map, "compass", PlayerPrefs.GetInt("compass"));
        SetStorageString(map, "carpet", PlayerPrefs.GetInt("carpet"));
        SetStorageString(map, "candle", PlayerPrefs.GetInt("candle"));
        SetStorageString(map, "statue1", PlayerPrefs.GetInt("statue1"));
        SetStorageString(map, "statue2", PlayerPrefs.GetInt("statue2"));
        SetStorageString(map, "statue3", PlayerPrefs.GetInt("statue3"));
        SetStorageString(map, "mask1", PlayerPrefs.GetInt("mask1"));
        SetStorageString(map, "mask2", PlayerPrefs.GetInt("mask2"));
        SetStorageString(map, "mask3", PlayerPrefs.GetInt("mask3"));
        SetStorageString(map, "map", PlayerPrefs.GetInt("map"));
        SetStorageString(map, "jukebox", PlayerPrefs.GetInt("jukebox"));
        SetStorageString(map, "inbox", PlayerPrefs.GetInt("inbox"));
        SetStorageString(map, "CollectablesTotal", PlayerPrefs.GetInt("CollectablesTotal"));

        yield return null;

        // ITEMS TAKEN
        SetStorageString(map, "DarkEnemiesKilledCount", PlayerPrefs.GetInt("DarkEnemiesKilledCount"));
        SetStorageString(map, "DarkEnemiesAchiev", PlayerPrefs.GetInt("DarkEnemiesAchiev"));
        SetStorageString(map, "NormalEnemiesKilledCount", PlayerPrefs.GetInt("NormalEnemiesKilledCount"));
        SetStorageString(map, "NormalEnemiesAchiev", PlayerPrefs.GetInt("NormalEnemiesAchiev"));
        SetStorageString(map, "AllFlowers", PlayerPrefs.GetInt("AllFlowers"));
        SetStorageString(map, "IntroWatched", PlayerPrefs.GetInt("IntroWatched"));
        SetStorageString(map, "Intro2Watched", PlayerPrefs.GetInt("Intro2Watched"));
        SetStorageString(map, "SeenLogo", PlayerPrefs.GetInt("SeenLogo"));
        SetStorageString(map, "First3Tears", PlayerPrefs.GetInt("First3Tears"));
        SetStorageString(map, "FinalBossDefeated", PlayerPrefs.GetInt("FinalBossDefeated"));
        SetStorageString(map, "FirstRedBerry", PlayerPrefs.GetInt("FirstRedBerry"));
        SetStorageString(map, "FirstBlueBerry", PlayerPrefs.GetInt("FirstBlueBerry"));
        SetStorageString(map, "FirstCheckpoint", PlayerPrefs.GetInt("FirstCheckpoint"));
        SetStorageString(map, "FirstTear", PlayerPrefs.GetInt("FirstTear"));
        SetStorageString(map, "GotHurt", PlayerPrefs.GetInt("GotHurt"));
        SetStorageString(map, "WrongLeafBlock", PlayerPrefs.GetInt("WrongLeafBlock"));
        SetStorageString(map, "WrongLeafBlockBlack", PlayerPrefs.GetInt("WrongLeafBlockBlack"));

        yield return null;
        
        // END save

        // OPTIONS
        SetStorageString(map, "Vibration", PlayerPrefs.GetInt("Vibration"));
        SetStorageString(map, "musicVolume", PlayerPrefs.GetFloat("musicVolume", 8f));
        SetStorageString(map, "effectsVolume", PlayerPrefs.GetFloat("effectsVolume", 8f));
        SetStorageString(map, "AA", PlayerPrefs.GetInt("Vibration"));
        SetStorageString(map, "Language", PlayerPrefs.GetInt("Language"));

        yield return null;

        byte[] byteArr = new byte[0];
        XmlSerializer serializer = new XmlSerializer(typeof(XONESavedItems), new XmlRootAttribute("prefs"));
        //
        try
        {
            using (var ms = new System.IO.MemoryStream())
            {
                XONESavedItems xoneSavedItems = new XONESavedItems();
                xoneSavedItems.Items = storageSavedBytes.Select(kv => new XONESavedItem() { key = kv.Key, value = kv.Value }).ToArray();
                foreach (XONESavedItem it in xoneSavedItems.Items)
                         Debug.Log("\n***** Serializing " + it.key.ToString() + " with value " + it.value.ToString());
                //
                serializer.Serialize(ms, xoneSavedItems);
                byteArr = ms.ToArray();
                Debug.Log("***** Data serialized");
            }
        }
        catch (Exception ex)
        {
            Debug.Log("**** Serialization error : " + ex.InnerException.ToString());
        }
        map.AddOrReplaceBuffer("WoodleTree2", byteArr);
        //
        SaveStorage(map);
        //
        StopCoroutine(SavingCoRo());
    }

    //
    public void LoadFloatPref(DataMapView view, string name)
    {
        //
        if (!storageSavedBytes.ContainsKey(name)) return;
        //
        try
        {
            byte[] bytes = storageSavedBytes[name]; // view.GetBuffer(name);

            PlayerPrefs.SetFloat(name, BitConverter.ToSingle(bytes, 0));
        }
        catch (Exception ex)
        {
            Debug.Log("Error on " + name);
        }
        //string firstSetting = System.Text.Encoding.UTF8.GetString(bytes, 0, bytes.Length);

        /*StorageKeyValue.GetString(name).OnLoadDone += delegate (StorageKeyValue loadedKeyValue)
        {
            if (loadedKeyValue.value + "" != "")
            {
                Debug.LogError("LOADED KEY '" + loadedKeyValue.key + "' - VALUE = " + loadedKeyValue.value);
                float res;
                float.TryParse(loadedKeyValue.value, out res);
                PlayerPrefs.SetFloat(name, res);
            }
        };*/
    }

    //
    public void LoadIntPref(DataMapView view, string name)
    {

        //
        if (!storageSavedBytes.ContainsKey(name)) return;
        //
        try
        {
            byte[] bytes = storageSavedBytes[name]; // view.GetBuffer(name);

            int val = BitConverter.ToInt32(bytes, 0);
            Debug.Log("key " + name + " val is " + val.ToString());
            PlayerPrefs.SetInt(name, val);
        }
        catch (Exception ex)
        {
            Debug.Log("Error on " + name);
        }
        //string firstSetting = System.Text.Encoding.UTF8.GetString(fisrtSettingBytes, 0, fisrtSettingBytes.Length);

        //
        /*StorageKeyValue.GetString(name).OnLoadDone += delegate (StorageKeyValue loadedKeyValue)
        {
            if (loadedKeyValue.value + "" != "")
            {
                Debug.LogError("LOADED KEY '" + loadedKeyValue.key + "' - VALUE = " + loadedKeyValue.value);
                int res;
                int.TryParse(loadedKeyValue.value, out res);
                PlayerPrefs.SetInt(name, res);
            }
        };*/
    }

    //
    //YAN MODIFIC 1
    public void LoadStringPref(DataMapView view, string name)
    {

        //
        if (!storageSavedBytes.ContainsKey(name)) return;
        //
        try
        {
            byte[] bytes = storageSavedBytes[name]; // view.GetBuffer(name);

            PlayerPrefs.SetString(name, BitConverter.ToString(bytes));
        }
        catch (Exception ex)
        {
            Debug.Log("Error on " + name);
        }
    }

    //
    public void LoadProgs(byte[] byteArr, DataMapView view)
    {
        //
        Debug.LogError("READING SAVED VALUES ...");
        XmlSerializer xs = new XmlSerializer(typeof(XONESavedItems), new XmlRootAttribute("prefs"));
        //
        storageSavedBytes = null;
        //
        try
        {
            using (var ms = new System.IO.MemoryStream(byteArr))
            {
                var obj = xs.Deserialize(ms) as XONESavedItems;
                storageSavedBytes = (obj.Items).ToDictionary(i => i.key, i => i.value);
                /*foreach (KeyValuePair<string, byte[]> it in storageSavedBytes)
                         Debug.Log("\n***** Deserializing " + it.Key.ToString());*/
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("\n\n ************* ERROR READING SAVED VALUES  : " + ex.InnerException);
        }

        Debug.LogError("DESERIALIZED ...");

        // CHECKPOINT INFO
        LoadIntPref(view, "LastCheckpoint");
        LoadIntPref(view, "FirstCheckpoint");

        // COUNTERS
        LoadIntPref(view, "Berries");
        LoadIntPref(view, "BlueBerries");

        //
        LoadIntPref(view, "AllBlueBerries");
        LoadIntPref(view, "BlueBerryTotal");
        LoadIntPref(view, "RedBerryTotal");

        LoadIntPref(view, "WoodleFriends");
        LoadIntPref(view, "IsThere");
        LoadIntPref(view, "Top");

        // test save


        foreach (string s in levelNames)
        {
            LoadStringPref(view, s + "BlueBerry");
        }

        //
        //YAN MODIFIC 2
        /*foreach (string s in levelNames) {
            //        LoadStringPref(view, s + "BlueBerry");
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

            //LOAD ALL BLUE BERRIES COLLECTED
            for (int bb = 0; bb < total; bb++)
                LoadIntPref(view, s + "BlueBerry" + bb.ToString());
        }*/

        //TEARS
        for (int lvl = 1; lvl <= 8; lvl++)
        {
            for (int tearNo = 1; tearNo <= 3; tearNo++)
                LoadIntPref(view, "Vase" + tearNo.ToString() + "Level" + lvl.ToString());
            //
            LoadIntPref(view, "Played" + lvl.ToString() + "TreeCompletion");
        }

        LoadIntPref(view, "AllTears");

        //CHECKPOINTS
        for (int cp = 1; cp <= 37; cp++)
        {
            LoadIntPref(view, "Checkpoint_" + cp.ToString());
            //    PlayerPrefs.SetInt("Checkpoint" + cp.ToString() + "Scene", 0);
        }
        LoadIntPref(view, "Checkpoint");
        LoadIntPref(view, "Checkpoint0");
        LoadIntPref(view, "Checkpoint0Scene");

        //CHALLENGES
        for (int ch = 0; ch <= 11; ch++)
            LoadIntPref(view, "Challenge" + ch.ToString() + "Found");

        //SHOPS
        for (int xyz = 0; xyz < 25; xyz++)
        {
            LoadIntPref(view, "UsingItem" + xyz.ToString());
            LoadIntPref(view, "PaidForItem" + xyz.ToString());
        }

        //BUTTONS
        for (int ab = 0; ab <= 124; ab++)
            LoadIntPref(view, "Button" + ab.ToString());

        // COLLECTABLES
        LoadIntPref(view, "marmellade1");
        LoadIntPref(view, "marmellade2");
        LoadIntPref(view, "marmellade3");
        LoadIntPref(view, "plantjar");
        LoadIntPref(view, "plantjar2");
        LoadIntPref(view, "book1");
        LoadIntPref(view, "book2");
        LoadIntPref(view, "paint");
        LoadIntPref(view, "paint2");
        LoadIntPref(view, "gameboy");
        LoadIntPref(view, "bell");
        LoadIntPref(view, "heater");
        LoadIntPref(view, "globe");
        LoadIntPref(view, "cupbear");
        LoadIntPref(view, "compass");
        LoadIntPref(view, "carpet");
        LoadIntPref(view, "candle");
        LoadIntPref(view, "statue1");
        LoadIntPref(view, "statue2");
        LoadIntPref(view, "statue3");
        LoadIntPref(view, "mask1");
        LoadIntPref(view, "mask2");
        LoadIntPref(view, "mask3");
        LoadIntPref(view, "map");
        LoadIntPref(view, "jukebox");
        LoadIntPref(view, "inbox");
        LoadIntPref(view, "CollectablesTotal");

        // ITEMS TAKEN
        LoadIntPref(view, "DarkEnemiesKilledCount");
        LoadIntPref(view, "DarkEnemiesAchiev");
        LoadIntPref(view, "NormalEnemiesKilledCount");
        LoadIntPref(view, "NormalEnemiesAchiev");
        LoadIntPref(view, "AllFlowers");
        LoadIntPref(view, "IntroWatched");
        LoadIntPref(view, "Intro2Watched");
        LoadIntPref(view, "SeenLogo");
        LoadIntPref(view, "First3Tears");
        LoadIntPref(view, "FinalBossDefeated");
        LoadIntPref(view, "FirstRedBerry");
        LoadIntPref(view, "FirstBlueBerry");
        LoadIntPref(view, "FirstCheckpoint");
        LoadIntPref(view, "FirstTear");
        LoadIntPref(view, "GotHurt");
        LoadIntPref(view, "WrongLeafBlock");
        LoadIntPref(view, "WrongLeafBlockBlack");
        LoadIntPref(view, "AllTears");

        // test save end

        // OPTIONS
        LoadFloatPref(view, "Vibration");
        LoadFloatPref(view, "musicVolume");
        LoadFloatPref(view, "effectsVolume");
        LoadFloatPref(view, "AA");
        LoadIntPref(view, "Language");

        Debug.LogError("DATA LOADED OK ...");

        //
        progsRead = false;
        if (SceneManager.GetActiveScene().buildIndex == 0)
            progsRead = true;
    }

    /*
    void OnLog(LogChannels channels, string message)
    {
        if (channels == LogChannels.kLogErrors || channels == LogChannels.kLogExceptions)
            Debug.LogError(message);
    }*/

    void OnAppCurrentUserChange()
    {
        //
        Debug.Log("---- ON APP USER CHANGE");
        //
        Users.User u = UsersManager.GetAppCurrentUser();

        Debug.Log("CHANGE USER : last user was " + oldLoggedUser.Id + " current is " + u.Id);
        if (u.Id == oldLoggedUser.Id)
        {
            Debug.LogError("SAME USER");
            return;
        }

        string info = "no App Current User";
        if (null != u)
        {
            info = u.Id + ": " + u.OnlineID;
        }
        Debug.LogError("APP CURRENT USERCHANGED " + u.GameDisplayName);
        currentLoggedUser = u;
      
        if (OnUserChanged != null) OnUserChanged(); //ERA QUESTO?*/

        Debug.LogError("ACTIVE SCENE NAME " + SceneManager.GetActiveScene().name);

        if (SceneManager.GetActiveScene().buildIndex != 0)
        {
            // decomm
            isGoingBackToMain = true;
            Debug.LogError("GOING BACK TO START");
        }
        else
        {
            //
            mainStorage = null;
            creatingStorage = false;
            Debug.LogError("DESTROYING STORAGE");
        }
    }

    //
    void CompareUsers()
    {
        // not on multiplayer
        //
        if (isMultiplayerOn)
        {
            Debug.LogError("Multiplayer detected");
            return;
        }
       
        //
        if (currentLoggedUser != null && oldLoggedUser != null)
        {
            // Debug.Log("OLD user was : " + oldLoggedUser.Id + " NEW user is " + currentLoggedUser.Id);
            //
            if (currentLoggedUser.Id != oldLoggedUser.Id)
            {
                //
                isGoingBackToMain = true;
                BackToMainMenu();
                return;
            }
            /*else
                Debug.Log("---------------------------------------------- NO CHANGES");*/
        }
        else
        {
            //
            // if (oldLoggedUser == null) Debug.Log("OLD user is NULL");
            if (currentLoggedUser == null)
            {
                Debug.Log("CURRENT user is NULL");
                //
                isGoingBackToMain = true;
                BackToMainMenu();
            }
        }
    }

    bool creatingStorage;
    float waitTime = 3, maxWaitTime = 3f;
    bool firstLogin;
    float timeSync;
    string strSync;
    bool startPressed;
    //
    void Update()
    {

#if UNITY_EDITOR
        return;
#endif

        //
        if (isSuspended)
            return;
        //
        if (SuspendCheck()) return;

        //
        if (SceneManager.GetActiveScene().buildIndex == 0 && XONEAchievements.achievsRead && progsRead)
        {
            // 
            XONEAchievements.achievsRead = false;
            progsRead = false;
            //
            Debug.LogError("Load Splash Screen ...");
            SceneManager.LoadScene(1);
            return;
        }

//#if !UNITY_EDITOR
        //
        if (!isUserInitialized && SceneManager.GetActiveScene().buildIndex == 0)
        {
            //
            if (startPressed)
            {
                // wait until OS response for syncing
                //
                if (syncImage != null)
                {
                    /*timeSync += Time.deltaTime;
                    if (timeSync > 0.25f)
                    {
                        timeSync = 0;
                        strSync += ".";
                        if (strSync.Length > 3)
                            strSync = "";
                    }*/
                    syncImage.sprite = syncWaitSprite;
                    //
                    //syncronizingText.text = "Please wait while syncing user's data " + strSync;
                    Canvas.ForceUpdateCanvases();
                }
            }
            
            //
            if (XboxOneInput.GetKeyDown(XboxOneKeyCode.Gamepad1ButtonA) && !creatingStorage)
            {
                //
                UsersManager.RefreshUsersList();
                //
                startPressed = true;
                //
                if (syncImage != null)
                {
                    //
                    //syncronizingText.text = "Please wait while syncing user's data ...";
                    syncImage.sprite = syncWaitSprite;
                    Canvas.ForceUpdateCanvases();
                }
                //
                // listen to gamepad to press A
                uint gamepadIndex = XboxOneInput.GetGamepadIndexFromGamepadButton(XboxOneKeyCode.Gamepad1ButtonA);
                int userId = XboxOneInput.GetUserIdForGamepad(gamepadIndex);
                User user = UsersManager.FindUserById(userId);
                if (user != null)
                {
                    // This is the user that's paired with the gamepad whose button was just pressed
                    currentLoggedUser = user;
                    // Assign the given Xbox One gamepad to the gamepad that Unity's InputManager will listen to. This will give menu control to the gamepad.
                    XboxOneInput.RemapGamepadToIndex(gamepadIndex, 1);
                    isUserInitialized = true;
                    DataManager.lastXONEUserID = currentLoggedUser.Id;
                    //
                    Debug.LogError("USER LOGGED : " + DataManager.lastXONEUserID + " name is " + currentLoggedUser.GameDisplayName);
                }
                else
                {
                    firstLogin = true;
                    InvokeSignInScreen();
                    return;
                }
                //
                return;
            }
            //
            return;
        }

        //
        if (isGoingBackToMain)
            return;
        //
        if (!UsersManager.IsSomeoneSignedIn)
        {
            waitTime += Time.deltaTime;
            if (waitTime < 0.5f)
                return;
            waitTime = 0;
            DataManager.userIsLogged = false;
            Debug.LogError("*** NOT SIGNED ***");
            DataManager.isOnSignScreen = false;
            InvokeSignInScreen();
            return;
        }
        else
        {
            //
            UsersManager.RefreshUsersList();
            //
            if (currentLoggedUser == null)
                currentLoggedUser = UsersManager.FindUserById(DataManager.lastXONEUserID);
            if (currentLoggedUser == null)
            {
                Debug.LogError("[!][!][!][!][!] ****  WARNING : GOT APP CURRENT USER, not the logged one");
                currentLoggedUser = UsersManager.GetAppCurrentUser(); // last option... the default
                //
                BackToMainMenu();
                //
                return;
            }
            if (currentLoggedUser != null)
            {
                //cfr utenti
                CompareUsers();
                //
                DataManager.lastXONEUserID = currentLoggedUser.Id;
                //
                DataManager.isOnSignScreen = false;
                //Debug.Log("*** SIGNED *** " + currentLoggedUser.GameDisplayName);
                DataManager.userIsLogged = true;
                //
                if (mainStorage == null && !creatingStorage)
                {
                    creatingStorage = true;
                    Debug.LogError("CREATE STORAGE FOR USER ID : " + currentLoggedUser.Id + "...");
                    //apriamo lo storage
                    if (StorageManager.AmFullyInitialized())
                        ConnectedStorage.CreateAsync(currentLoggedUser.Id, "WoodleTree2", hStorageCreated, 0);
                }
            }
            else
            {
                DataManager.userIsLogged = false;
                Debug.Log("*** USER NULL *** ");
                Debug.Log("LAST ID : " + DataManager.lastXONEUserID);

                waitTime += Time.deltaTime;
                if (waitTime >= maxWaitTime)
                {
                    DataManager.isOnSignScreen = false;
                    InvokeSignInScreen();
                    waitTime = 0;
                }
                return;
            }
        }
    }
    
    //
    public void BackToMainMenu()
    {
        //
        Debug.LogError("Back to main ...");
        //
        //if (!isGoingBackToMain) return;
        //
        DataManager.lastXONEUserID = -1;
        currentLoggedUser = null;
        DataManager.userIsLogged = false;
        DataManager.isOnSignScreen = false;
        //
        Debug.LogError("******* Deleting scene content");
        //
        // moves the current object on destroyable objects
        SceneManager.MoveGameObjectToScene(this.gameObject, SceneManager.GetActiveScene());
        //
        //destroy all except this one
        Destroy(GameObject.Find("DontDestroyOnLoad"));
        //
        // back to main scene
        Debug.LogError("******* Load first scene back");
        SceneManager.LoadScene(0);
    }


    static public void StartSignInScreen()
    {

    }

#endif
}
