using UnityEngine;
using System.Collections;
using System.IO;
using System.Globalization;

public class PS4StorageManager : MonoBehaviour
{

#if UNITY_PS4

    const int kSaveDataMaxSize = 64 * 1024;
    bool initialized;

    //
    private void Awake()
    {
        //DataManager.ps4StorageManager = this;
        //DataManager.playerData = new SavedData();
    }

    //
    void Init()
    {
        //
        // Construct some game data to save.
        //
        PS4Manager.ps4StorageManager = this;     
        //
        initialized = true;
    }

    private void Start()
    {
        //
        DontDestroyOnLoad(transform.gameObject);

        //
        if (!initialized)
             Init();
     
        //  
        Sony.PS4.SavedGame.Main.OnLog += OnLog;
        Sony.PS4.SavedGame.Main.OnLogWarning += OnLogWarning;
        Sony.PS4.SavedGame.Main.OnLogError += OnLogError;
        Sony.PS4.SavedGame.SaveLoad.OnGameSaved += OnSavedGameSaved;
        Sony.PS4.SavedGame.SaveLoad.OnGameLoaded += OnSavedGameLoaded;
        Sony.PS4.SavedGame.SaveLoad.OnGameDeleted += OnSavedGameDeleted;
        Sony.PS4.SavedGame.SaveLoad.OnCanceled += OnSavedGameCanceled;
        Sony.PS4.SavedGame.SaveLoad.OnSaveError += OnSaveError;
        Sony.PS4.SavedGame.SaveLoad.OnLoadError += OnLoadError;
        Sony.PS4.SavedGame.SaveLoad.OnLoadNoData += OnLoadNoData;
        //
        Sony.PS4.SavedGame.Main.Initialise();
      
        //
        Sony.PS4.SavedGame.Main.Update();

        Debug.LogError("STORAGE INITIALIZED");

        //
        LoadData();
    }
  
    // Update is called once per frame
    void Update()
    {
        //Sony.PS4.SavedGame.Main.Update();
    }

    void SetupGameParams(ref Sony.PS4.SavedGame.SaveLoad.SavedGameSlotParams saveParams)
    {
        saveParams.userId = 0;	// by passing a userId of 0 we use the default user that started the title
        saveParams.titleId = null; // by passing null we use the game's title id from the publishing settings
       
        //saveParams.sizeKiB = kSaveDataMaxSize / 1024;		// is this max size of the file or the folder ?
    }


    public bool SaveData()
    {     
        //invoco il salvataggio
        //byte[] bytes = data.WriteToBuffer();

        //
        Sony.PS4.SavedGame.SaveLoad.SavedGameSlotParams slotParams = new Sony.PS4.SavedGame.SaveLoad.SavedGameSlotParams();

        SetupGameParams(ref slotParams);
        //
        slotParams.dirName = "WTSaveDir"; // if we are autosaving we need to provide a directory 'slot' to save it in
        slotParams.title = "WTSaveData";
        slotParams.newTitle = "";       // displayed in the box that can be selected to create a new savegame
        slotParams.subTitle = "WoodleTree save file";
        slotParams.detail = "The save file for WoodleTree ";
        slotParams.iconPath = Application.streamingAssetsPath + "/SaveIcon.png";
        UnityEngine.PS4.PS4PlayerPrefs.SetTitleStrings(slotParams.title, slotParams.subTitle, slotParams.detail);
        //
       

        Debug.LogError("Saving Game... " + slotParams.title);
        Debug.LogError(" Directory: " + slotParams.dirName);

        // optionally we can configure the save so that if the disk is full we are able to display the NOSPACE_CONTINUABLE message
        slotParams.noSpaceSysMsg = Sony.PS4.SavedGame.SaveLoad.DialogSysmsgType.NOSPACE_CONTINUABLE;

        //
        /*PlayerPrefs.SetInt("punteggio", DataManager.playerData.punteggio);
        PlayerPrefs.SetInt("punteggiolevel1", DataManager.playerData.punteggiolevel1);
        PlayerPrefs.SetInt("punteggiolevel2", DataManager.playerData.punteggiolevel2);
        PlayerPrefs.SetInt("punteggiolevel3", DataManager.playerData.punteggiolevel3);
        PlayerPrefs.SetInt("punteggiolevel4", DataManager.playerData.punteggiolevel4);
        PlayerPrefs.SetInt("punteggiolevel5", DataManager.playerData.punteggiolevel5);
        PlayerPrefs.SetInt("punteggiolevel6", DataManager.playerData.punteggiolevel6);
        PlayerPrefs.SetInt("punteggiolevel7", DataManager.playerData.punteggiolevel7);
        PlayerPrefs.SetInt("saltolivello6", DataManager.playerData.saltolivello6);
        PlayerPrefs.SetInt("attivalevel3_3", DataManager.playerData.attivalevel3_3);
        PlayerPrefs.SetInt("attivalevel4_3", DataManager.playerData.attivalevel4_3);
        PlayerPrefs.SetInt("attivalevel5_3", DataManager.playerData.attivalevel5_3);
        PlayerPrefs.SetInt("achievementprimolivello", DataManager.playerData.achievementprimolivello);
        PlayerPrefs.SetInt("giocofinito", DataManager.playerData.giocofinito);
        PlayerPrefs.SetInt("achievementbeaver", DataManager.playerData.achievementbeaver);*/

        //
        PlayerPrefs.Save();

        //
        Debug.LogError("SAVED");

        return true;
        //return Sony.PS4.SavedGame.SaveLoad.SaveGame(bytes, slotParams, true);

        ////--------
    }

    public void LoadData()
    {
        Debug.LogError("Loading Game ...");
        Sony.PS4.SavedGame.SaveLoad.SavedGameSlotParams slotParams = new Sony.PS4.SavedGame.SaveLoad.SavedGameSlotParams();
        //
        if (slotParams == null)
            Debug.LogError("SLOTPARAMS call failed !!!!");
        //
        SetupGameParams(ref slotParams);
        //
        slotParams.dirName = "WTSaveDir"; // if we are autosaving we need to provide a directory 'slot' to save it in
        slotParams.title = "WTSaveData";
        slotParams.newTitle = "";       // displayed in the box that can be selected to create a new savegame
        slotParams.subTitle = "WoodleTree save file";
        slotParams.detail = "The save file for WoodleTree ";
        slotParams.iconPath = Application.streamingAssetsPath + "/SaveIcon.png";
        Debug.LogError("Set playerprefs ...");
        UnityEngine.PS4.PS4PlayerPrefs.SetTitleStrings(slotParams.title, slotParams.subTitle, slotParams.detail);
       
        //
        /*DataManager.playerData.punteggio = PlayerPrefs.GetInt("punteggio");
        DataManager.playerData.punteggiolevel1 = PlayerPrefs.GetInt("punteggiolevel1");
        DataManager.playerData.punteggiolevel2 = PlayerPrefs.GetInt("punteggiolevel2");
        DataManager.playerData.punteggiolevel3 = PlayerPrefs.GetInt("punteggiolevel3");
        DataManager.playerData.punteggiolevel4 = PlayerPrefs.GetInt("punteggiolevel4");
        DataManager.playerData.punteggiolevel5 = PlayerPrefs.GetInt("punteggiolevel5");
        DataManager.playerData.punteggiolevel6 = PlayerPrefs.GetInt("punteggiolevel6");
        DataManager.playerData.punteggiolevel7 = PlayerPrefs.GetInt("punteggiolevel7");
        DataManager.playerData.saltolivello6 = PlayerPrefs.GetInt("saltolivello6");
        DataManager.playerData.attivalevel3_3 = PlayerPrefs.GetInt("attivalevel3_3");
        DataManager.playerData.attivalevel4_3 = PlayerPrefs.GetInt("attivalevel4_3");
        DataManager.playerData.attivalevel5_3 = PlayerPrefs.GetInt("attivalevel5_3");
        DataManager.playerData.achievementprimolivello = PlayerPrefs.GetInt("achievementprimolivello");
        DataManager.playerData.giocofinito = PlayerPrefs.GetInt("giocofinito");
        DataManager.playerData.achievementbeaver = PlayerPrefs.GetInt("achievementbeaver");*/
    }

    void OnLog(Sony.PS4.SavedGame.Messages.PluginMessage msg)
    {
        Debug.LogError(msg.Text);
    }

    void OnLogWarning(Sony.PS4.SavedGame.Messages.PluginMessage msg)
    {
        Debug.LogError("WARNING: ");
    }

    void OnLogError(Sony.PS4.SavedGame.Messages.PluginMessage msg)
    {
        Debug.LogError("ERROR: " + msg.Text);
    }

    void OnSavedGameSaved(Sony.PS4.SavedGame.Messages.PluginMessage msg)
    {
        Debug.LogError("Game Saved!");
    }

    void OnSavedGameDeleted(Sony.PS4.SavedGame.Messages.PluginMessage msg)
    {
        Debug.LogError("Game deleted!");
    }


    void OnSavedGameLoaded(Sony.PS4.SavedGame.Messages.PluginMessage msg)
    {

    }

    void OnSavedGameCanceled(Sony.PS4.SavedGame.Messages.PluginMessage msg)
    {
        Debug.LogError("Canceled");
    }

    void OnSaveError(Sony.PS4.SavedGame.Messages.PluginMessage msg)
    {
        Debug.LogError("Failed to save");
    }

    void OnLoadError(Sony.PS4.SavedGame.Messages.PluginMessage msg)
    {
        Debug.LogError("Failed to load");
    }

    void OnLoadNoData(Sony.PS4.SavedGame.Messages.PluginMessage msg)
    {
        Debug.LogError("Nothing to load");
    }

#endif
}
