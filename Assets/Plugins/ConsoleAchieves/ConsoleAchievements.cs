using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_PS4

//
public enum ConsoleAchievs
{
    //
    COMPLETE_TUTORIAL,
    COLLECT_100_RED_BERRIES,
    COLLECT_1000_RED_BERRIES,
    COLLECT_100_BLUE_BERRIES,
    COLLECT_ALL_BLUE_BERRIES,
    COLLECT_HALF_COLLECTIBLES,
    COLLECT_ALL_COLLECTIBLES,
    COMPLETE_LEVEL_1,
    FIND_ALL_WATER_TEARS,
    FINISH_THE_GAME,
    FREE_ALL_MUSICIANS,
    AWAKE_ALL_SACRED_FLOWERS,
    BUY_3_ITEMS_AT_SHOPS,
    BUY_ALL_ITEMS_AT_SHOPS,
    PLAY_WITH_FRIEND_LOCAL,
    GROW_ALL_DRY_PLANTS,
    TALK_TO_ALL_PHILOSOTREES,
    ANNIHILIATE_100_DARK_ENEMIES,
    MEET_THE_LOST_VILLAGER,
    TO_THE_TOP,
    GET_ALL_POWER_UP
}

public class ConsoleAchievements : MonoBehaviour
{
    //
    [HideInInspector] public static ConsoleAchievements singleton;

    //
    bool[] isSet;

    //
    void Awake()
    {
        //
        if (singleton != null)
        {
            enabled = false;
            Destroy(this);
            return;
        }

        //
        singleton = this;
        isSet = new bool[21];
    }

    //
    public void SetAchiev(ConsoleAchievs achiev)
    {
        //
        if (isSet[(int)achiev])
            return;


#if UNITY_PS4

        //
        PS4Manager.ps4TrophyManager.UnlockTrophy((int)achiev);

#elif UNITY_XBOXONE


#endif

        //
        isSet[(int)achiev] = true;
    }
}
#endif