using System;
using UnityEngine;
using System.Collections.Generic;

static public class PS4Achievements //: IAchievementsService
{
    #if UNITY_PS4
    static private Dictionary<string, int> _stat_index_map = new Dictionary<string, int>();
    static public PS4Trophy ps4trophy;

    static public void Initialize()
    {
        _stat_index_map["Get Killed One More Time"] = 1; //
        _stat_index_map["Get 1 Statue"] = 2; //
        _stat_index_map["Destroy a wooden toilet"] = 3; //
        _stat_index_map["Blend two items together"] = 4;
        _stat_index_map["Get 3 statues"] = 5; //
        _stat_index_map["Be the first human to travel back in time (in your dreams)"] = 6; //
        _stat_index_map["Finish the game!"] = 7; //
        _stat_index_map["Make the beers fall"] = 8; //
        _stat_index_map["Have a drink"] = 9;
        _stat_index_map["Get 6 statues"] = 10; //
        _stat_index_map["Gotchi Killer"] = 11;        


        // ...
    }

    static public void AddToStat(string statName)
    {
        try
        {
            Debug.LogError("controllo achiev : " + statName);
            if (!_stat_index_map.ContainsKey(statName))
            {
                Debug.LogError(statName + " NOT FOUND");
                return;
            }
            Debug.LogError("instance trophy : " + statName);
            PS4Manager.ps4TrophyManager.UnlockTrophy(_stat_index_map[statName]);            
            Debug.LogError(statName + " unlocked");
        }
        catch (Exception ex)
        {
            Debug.LogError("errore achiev : " + ex.Message);
        }
    }
#endif
}