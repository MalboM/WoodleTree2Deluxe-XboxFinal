using UnityEngine;
using UnityEngine.SceneManagement;
using UnityAOT;
using System.Collections;
using System.Collections.Generic;
using DataPlatform;
using Users;
using System;
using ConsoleUtils;
using System.Text;

public enum XONEACHIEVS
{
    NONE = 0,
    THE_BASICS = 1,
    FIRST_TREE_SAGE_RESTORED,
    RED_BERRIES_LOVER,
    RED_BERRIES_PARADE,
    RED_BERRIES_CHAMPION,
    BLUE_BERRIES_LOVER,
    BLUE_BERRIES_CHAMPION,
    BEAUTIFUL_WOODLE_HOUSE,
    WONDERFUL_WOODLE_HOUSE,
    ALL_TREE_SAGE_RESTORED,
    WOODLE_SAVIOR,
    MUSIC_LOVER,
    AWAKING_FROM_A_LONG_SLEEP,
    GO_SHOPPING,
    GO_SHOPPING_FOR_EVERYTHING,
    WOODLE_FRIENDS,
    WOODLE_AVENGER,
    WOODLE_WARRIOR,
    IS_THERE_ANYONE,
    TO_THE_TOP,
    WOODLE_POWER
}

static public class XONEAchievements
{

#if UNITY_XBOXONE

    static Guid GameSessionId = Guid.NewGuid();
    static private Users.User CurrentUser;
    static public TextAsset eventManifesTXT;
    static private bool _initialized = false;
    //
    static public bool achievsRead;

    static public void Initialize(int userId)
    {
        //
        achievsRead = false;
        //
        //init achievements manager
        if (!DataManager.isAchievInitialized)
            AchievementsManager.Create();

        //
        DebugLog("SET DATAPLATFORM PLUGIN, user is " + userId);
        //
        CurrentUser = UsersManager.FindUserById(userId);

        //init dataplatform
        int result = DataPlatformPlugin.InitializePlugin(0);
        DebugLog("DataPlatformPlugin is : " + result.ToString());
        //init usermanager
        //UsersManager.Create();

        //
        if (CurrentUser != null)
        {
            //
            DebugLog("SET ACHIEVEMENTS MANAGER : Current user ID is " + CurrentUser.Id + " Current user UID is " + CurrentUser.UID);
            DebugLog("CALLING ACHIEV CREATION ...");

            //            
            DebugLog("CALLING CONSOLE UTILS CREATION ...");

            //
            DebugLog("CALLING SYNC ...");
            DebugLog("CALLING SYNC ACHIEV WITH TITLE ID : " + ConsoleUtilsManager.TitleIdInt());
            //
            AchievementsManager.GetAchievementsForTitleIdAsync
                          ( CurrentUser.Id
                          , CurrentUser.UID
                          , ConsoleUtilsManager.TitleIdInt()
                          , AchievementType.All
                          , false
                          , UnityPlugin.AchievementOrderBy.TitleId
                          , 0
                          , 10
                          , OnAchievementSnapshotReady);


            //
            DataManager.isAchievInitialized = true;
            DebugLog("ACHIEVS CREATION AND SYNC SUCCESSFUL");          
        }
        else 
            DebugLog("ACHIEVS ERROR INITIALIZING : CURRENT USER IS NULL");        
    }

    static public void OnAchievementSnapshotReady(AchievementsResult achievements, GetObjectAsyncOp<AchievementsResult> op)
    {
        achievsRead = false;
        //
        // We used the UI to take a snapshot, now we can do something with these achievements!
        if (!op.Success || achievements == null || achievements.Items == null)
        {
            DebugLog("\nAchievement snapshot failed HRESULT: 0x" + op.Result.ToString("X8"));
            //OnScreenLog.Add("Achievement snapshot failed HRESULT: 0x" + op.Result.ToString("X8"));
            if (SceneManager.GetActiveScene().buildIndex == 0)
                achievsRead = true;
            return;
        }

        StringBuilder sb = new StringBuilder();
        sb.Append("\nAchievements\n---------------------------\n\n");
        for (int i = 0; i < achievements.Items.Length; ++i)
        {
            var a = achievements.Items[i];
            bool ok = a.ProgressState == AchievementProgressState.Achieved;
            sb.Append(string.Format("[{0}] {1} - {2}\n", (ok ? "x" : "  "), a.Name.PadRight(20), (ok ? a.UnlockedDescription : a.LockedDescription)));
        }

        DebugLog("\n" + sb.ToString());
        //
        if (SceneManager.GetActiveScene().buildIndex == 0)
            achievsRead = true;
    }

    static private bool haveRequestedSignIn = false;

    static public void SetGameProgression(float value)
    {
        //DebugLog("SETTO GAME PROG");
        //if (UsersManager.GetAppCurrentUser() != null)
        //{
        //     DebugLog("Current user is : " + UsersManager.GetAppCurrentUser().GameDisplayName);
        //    try
        //    {
        //        XDP_COTSG_Events.SetGameProgress(UsersManager.GetAppCurrentUser().UID, ref GameSessionId, value, 0, 0, 0);
        //        DebugLog("2- settato Game prog");
        //    }
        //    catch (Exception ex)
        //    {
        //        DebugLog("ERRORE ERRORE ERRORE STAT: " + ex.Message);
        //    }
        //}
    }

    //static public void AddToStat(string name)
    //{
    //    DebugLog("STAT:" + name);
    //    if (UsersManager.GetAppCurrentUser() != null)
    //    {
    //        DebugLog("1- setto STAT:" + name);
    //        DebugLog("Current user is : " + UsersManager.GetAppCurrentUser().GameDisplayName);
    //        try
    //        {
    //          XDP_COTSG_Events.SetAchiev(name, UsersManager.GetAppCurrentUser().UID, ref GameSessionId, 0,0,0,0);
    //          DebugLog("2- settato STAT:" + name);
    //        }
    //        catch(Exception ex)
    //        {
    //            DebugLog("ERRORE ERRORE ERRORE STAT: " + ex.Message);
    //        }
    //    }
    //}

    //static public void AddToStat(string name, int value)
    //{
    //    DebugLog("STAT:" + name);
    //    if (UsersManager.GetAppCurrentUser() != null)
    //    {
    //        DebugLog("1- setto STAT:" + name);
    //        DebugLog("Current user is : " + UsersManager.GetAppCurrentUser().GameDisplayName);
    //        try
    //        {
    //            XDP_COTSG_Events.SetAchiev(name, UsersManager.GetAppCurrentUser().UID, ref GameSessionId, value, 0, 0, 0);
    //            DebugLog("2- settato STAT:" + name);
    //        }
    //        catch (Exception ex)
    //        { DebugLog("ERRORE ERRORE ERRORE STAT:" + ex.Message); }
    //    }
    //}

    //static public void AddToStat(string name, float value)
    //{
    //    DebugLog("STAT:" + name);
    //    if (UsersManager.GetAppCurrentUser() != null)
    //    {
    //        DebugLog("1- setto STAT:" + name);
    //        DebugLog("Current user is : " + UsersManager.GetAppCurrentUser().GameDisplayName);
    //        try
    //        {
    //            XDP_COTSG_Events.SetAchiev(name, UsersManager.GetAppCurrentUser().UID, ref GameSessionId, value, 0, 0, 0);
    //            DebugLog("2- settato STAT:" + name);
    //        }
    //        catch (Exception ex)
    //        { DebugLog("ERRORE ERRORE ERRORE STAT:" + ex.Message); }
    //    }
    //}

    //
    static public void AddFrag()
    { 
       //SetHeroStat_S_FRAG_COUNTER
       //AddToStat("S_FRAG_COUNTER");
    }
    //

    private static Queue<int> _achievementsIdsToSubmit = new Queue<int>();
    
    static public void SubmitAchievement(int id)
    {
        if (CurrentUser != null)
        {
            _achievementsIdsToSubmit.Enqueue(id);

            ProcessAchievementsSubmitted();
        }
    }


    static bool _isSubmittingAnAchievement;
    static void ProcessAchievementsSubmitted()
    {
        if (_achievementsIdsToSubmit.Count > 0 && !_isSubmittingAnAchievement)
        {
            _isSubmittingAnAchievement = true;
            AchievementsManager.UpdateAchievementAsync(CurrentUser.Id, CurrentUser.UID, _achievementsIdsToSubmit.Dequeue().ToString(), 100, UpdateAchievementCallback);
        }
    }


    static void UpdateAchievementCallback(UnityPlugin.AsyncStatus status, UnityAOT.ActionAsyncOp op)
    {
        DebugLog("UpdateAchievementAsync call complete: " + op.IsComplete);
        DebugLog("UpdateAchievementAsync success: " + op.Success);
        DebugLog("UpdateAchievementAsync HRESULT:  0x" + op.Result.ToString("X8"));
        DebugLog("UpdateAchievementAsync returned status: " + status.GetHashCode());
        
        _isSubmittingAnAchievement = false;
        
        if(_achievementsIdsToSubmit.Count > 0)
        {
            ProcessAchievementsSubmitted();
        }
    }

    static public void hAchievementUnlocked(AchievementUnlockedEventArgs payload)
    {
        // When an achievement comes in we don't get much information about it. You have to compare the achievement against
        // a snapshot achievements list to actually get it's name.
        DebugLog("Achievement notice: " + payload.AchievementId + " by user: " + payload.XboxUserId);
    }
    //
    static private void DebugLog(string text)
    {
        Debug.LogError(text);
    }
    //
    static public void OpenAchievementsBar()
    {
        AchievementsManager.LaunchAchievementsUIAsync((uint)CurrentUser.Id, ConsoleUtilsManager.TitleIdInt(), null);
    }


#endif
}
