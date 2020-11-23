using UnityEngine;

using System.Collections;
//
#if UNITY_PS4
using UnityEngine.PS4;

using System;

//
public enum PS4_TROPHIES
{
    PLATINUM,
    COMPLETE_TUTORIAL,
    COMPLETE_FIRST_LEVEL,
    COLLECT_100_RED_BERRIES,
    COLLECT_1000_RED_BERRIES,
    COLLECT_3000_RED_BERRIES,
    COLLECT_100_BLUE_BERRIES,
    COLLECT_ALL_BLUE_BERRIES,
    COLLECT_HALF_OF_THE_COLLECTIBLE,
    COLLECT_ALL_OF_THE_COLLECTIBLE,
    FIND_ALL_WATER_TEARS,
    FINISH_THE_GAME,
    FREE_ALL_MUSICIANS,
    AWAKE_ALL_SACRED_FLOWERS,
    BUY_3_ITEMS_AT_THE_SHOP,
    BUY_ALL_ITEMS_AT_THE_SHOP,
    PLAY_WITH_A_FRIEND,
    DEFEAT_100_NATURAL_ENEMIES,
    ANNIHILIATE_100_DARK_ENEMIES,
    MEET_THE_LOST_VILLAGER,
    TOP_ICY_MOUNTAIN,
    GET_ALL_POWER_UPS
}

public class PS4Trophy : MonoBehaviour
{
    int actualTrophy = 1;

    //The active current user logged

    public PS4Input.LoggedInUser loggedInUser;

    // NPToolkit2 and Trophy initialization

    //
    bool initialized;

    private void Awake()
    {
        //
        if (!initialized)
        {
            DontDestroyOnLoad(this.gameObject);
            //
            PS4Manager.ps4TrophyManager = this;
            //
            PS4Achievements.Initialize();
            //
            Init();
            //
            initialized = true;
        }
    }

    public Sony.NP.InitResult initResult;
    void Init()
    {
        Sony.NP.Main.OnAsyncEvent += Main_OnAsyncEvent;
        
        Sony.NP.InitToolkit init = new Sony.NP.InitToolkit();

        init.contentRestrictions.DefaultAgeRestriction = 2;

        Sony.NP.AgeRestriction[] ageRestrictions = new Sony.NP.AgeRestriction[1];

        ageRestrictions[0] = new Sony.NP.AgeRestriction(10, new Sony.NP.Core.CountryCode("en"));
        //ageRestrictions[1] = new Sony.NP.AgeRestriction(15, new Sony.NP.Core.CountryCode("au"));

        init.contentRestrictions.AgeRestrictions = ageRestrictions;

        // Only do this if age restriction isn't required for the product. See documentation for details.
        // init.contentRestrictions.ApplyContentRestriction = false;

        init.threadSettings.affinity = Sony.NP.Affinity.AllCores; // Sony.NP.Affinity.Core2 | Sony.NP.Affinity.Core4;

        // Mempools
        init.memoryPools.JsonPoolSize = 6 * 1024 * 1024;
        init.memoryPools.SslPoolSize *= 4;

        init.memoryPools.MatchingSslPoolSize *= 4;
        init.memoryPools.MatchingPoolSize *= 4;

        init.SetPushNotificationsFlags(Sony.NP.PushNotificationsFlags.None);
        init.contentRestrictions.DefaultAgeRestriction = 0;

        //You can add several age restrictions by region

     
        //For this example we use the first user

        loggedInUser = PS4Input.RefreshUsersDetails(0);

        try
        {
            initResult = Sony.NP.Main.Initialize(init);

            if (initResult.Initialized == true)
            {
                Debug.LogError("NpToolkit Initialized ");
                Debug.LogError("Plugin SDK Version : " + initResult.SceSDKVersion.ToString());
                Debug.LogError("Plugin DLL Version : " + initResult.DllVersion.ToString());
                //
                RegisterTrophyPack();
            }
            else
            {
                Debug.LogError("NpToolkit not initialized ");
            }
        }
        catch (Sony.NP.NpToolkitException e)
        {
            Debug.LogError("Exception During Initialization : " + e.ExtendedMessage);
        }
#if UNITY_EDITOR
        catch (DllNotFoundException e)
        {
            Debug.LogError("Missing DLL Expection : " + e.Message);
            Debug.LogError("The sample APP will not run in the editor.");
        }
#endif
    }

    public void RegisterTrophyPack()
    {
        try
        {
            Sony.NP.Trophies.RegisterTrophyPackRequest request = new Sony.NP.Trophies.RegisterTrophyPackRequest();
            //
            request.UserId = loggedInUser.userId;

            Sony.NP.Core.EmptyResponse response = new Sony.NP.Core.EmptyResponse();

            // Make the async call which returns the Request Id 

            int requestId = Sony.NP.Trophies.RegisterTrophyPack(request, response);

            Debug.LogError("RegisterTrophyPack Async : Request Id = " + requestId + " userId = " + loggedInUser.userId);
        }

        catch (Sony.NP.NpToolkitException e)
        {

            Debug.LogError("Exception : " + e.ExtendedMessage);

        }

    }

    public void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.Joystick1Button0))
        {
            UnlockTrophy(actualTrophy);
        }*/
    }

    public void UnlockTrophy(int nextTrophyId)
    {
        //
        try
        {

            Sony.NP.Trophies.UnlockTrophyRequest request = new Sony.NP.Trophies.UnlockTrophyRequest();

            request.TrophyId = nextTrophyId;

            request.UserId = loggedInUser.userId;

            Sony.NP.Core.EmptyResponse response = new Sony.NP.Core.EmptyResponse();

            // Make the async call which returns the Request Id 

            int requestId = Sony.NP.Trophies.UnlockTrophy(request, response);

            Debug.LogError("GetUnlockedTrophies Async : Request Id = " + requestId);

        }

        catch (Sony.NP.NpToolkitException e)
        {

            Debug.LogError("Exception : " + e.ExtendedMessage);

        }

    }

    // NOTE : This is called on the "Sony NP" thread and is independent of the Behaviour update.
    // This thread is created by the SonyNP.dll when NpToolkit2 is initialised.

    private void Main_OnAsyncEvent(Sony.NP.NpCallbackEvent callbackEvent)
    {

        //Print some useful info on the current event: 

        Debug.LogError("Event: Service = (" + callbackEvent.Service + ") : API Called = (" + callbackEvent.ApiCalled + ") : Request Id = (" + callbackEvent.NpRequestId + ") : Calling User Id = (" + callbackEvent.UserId + ")");

        HandleAsyncEvent(callbackEvent);

    }

    //Here we manage the response of the previous request

    private void HandleAsyncEvent(Sony.NP.NpCallbackEvent callbackEvent)
    {

        try
        {

            if (callbackEvent.Response != null)
            {

                //We got an error response 

                if (callbackEvent.Response.ReturnCodeValue < 0)
                {

                    Debug.LogError("Response : " + callbackEvent.Response.ConvertReturnCodeToString(callbackEvent.ApiCalled));

                }

                else
                {

                    //The callback of the event is a trophyUnlock event

                    if (callbackEvent.ApiCalled == Sony.NP.FunctionTypes.TrophyUnlock)
                    {

                        Debug.LogError("Trophy Unlock : " + callbackEvent.Response.ConvertReturnCodeToString(callbackEvent.ApiCalled));

                    }

                }

            }

        }

        catch (Sony.NP.NpToolkitException e)
        {

            Debug.LogError("Main_OnAsyncEvent Exception = " + e.ExtendedMessage);

        }

    }

}
#endif