using UnityEngine;
using System.Collections;
using Users;
using TextSystems;
using UnityPlugin;
using UnityAOT;
using ConsoleUtils;
using System;
using System.Text;

static public class XBRichPresence
{  
    #region strings
    public const string RPS_MENU_MAIN = "RPS_MENU_MAIN";
    public const string RPS_MENU_OPTIONS = "RPS_MENU_OPTIONS";
    public const string RPS_MENU_LOAD = "RPS_MENU_LOAD";

    public const string RPS_GAME_LEVEL_PREFIX = "RPS_GAME_LEVEL_";
    #endregion

    #region Methods
    static private bool _initialized = false;
    static public void SetString(string str)
    {
        //if (XBUser.Self.currentUser == null) return;
        //int userID = XBUser.Self.currentUser.Id;
        try
        {
            if (!_initialized)
            {
                UsersManager.SetTraceLevelForUsersLiveContext(UsersManager.GetAppCurrentUser().Id, XboxServicesDiagnosticsTraceLevel.Verbose);
                _initialized = true;
            }

            Debug.LogError("Set RP string: " + str);
            PresenceData data = PresenceService.CreatePresenceData(ConsoleUtilsManager.PrimaryServiceConfigId(), str, null);
            PresenceService.SetPresenceAsync(UsersManager.GetAppCurrentUser().Id, true, data, OnPresenceDataSet);
            Debug.LogError("-- -- RP OK");
        }
        catch (Exception ex)
        { }
    }


    static public void SetSceneString(string str)
    {
        if (string.IsNullOrEmpty(str)) return;
        SetString(str);
    }


    static private void OnPresenceDataSet(AsyncStatus status, ActionAsyncOp op)
    {
        if (!op.Success)
        {
            //Debug.LogError(op.Status+"  ");
            //Debug.LogError("Failed to set presence string return code: [0x" + op.Result.ToString("X") + "]");
            return;
        }
        // NOTE: Presence string setting will succeed even if the event associated with the stat has never fired
        //       this is because the presence string is rendered dynamically on every request and it may be the case
        //       you are setting the presence string while the event is still in the wind. Xbox Live allows for this.
        //Debug.LogError("Attempt to set presence string succeeded. Remember rendering this presence string can still fail if require statistics are not defined.");

        PresenceService.GetPresenceAsync(UsersManager.GetAppCurrentUser().Id, UsersManager.GetAppCurrentUser().UID, OnGetPresence);
    }

    static private void OnGetPresence(PresenceRecord result, GetObjectAsyncOp<PresenceRecord> op)
    {
        if (!op.Success)
        {
            //Debug.LogError("Failed to get presence string return code: [0x" + op.Result.ToString("X") + "] " + (op.Result == 0x80190190 ? "HTTP_STATUS_BAD_REQUEST: Bad request (400). The request could not be processed by the server due to invalid syntax." : ""));
            return;
        }
        // You will get multiple records back. The first record is usually the record associated with doing something in the HUD
        // and will have a presence string of ???.
        //Debug.LogError("Get Presence Returned Ok with " + result.PresenceTitleRecords.Length + " records.");
        StringBuilder sb = new StringBuilder();
        foreach (PresenceDeviceRecord rec in result.PresenceDeviceRecords)
        {
            sb.AppendFormat(" Presence:   {0}\n", rec.ToString());
            sb.AppendFormat(" TitleName:  {0}\n", rec.PresenceTitleRecords.ToString());
            sb.AppendFormat(" DeviceType: {0}\n", rec.DeviceType.ToString());
            sb.AppendFormat("\n");
        }
        //Debug.LogError(sb.ToString());
    }
    #endregion
}
