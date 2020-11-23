using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CagedNPCManager : MonoBehaviour
{
    static CagedNPCManager singleton;

    public List<DeactivateExtraItem> npcsToEnable;

    [System.Serializable] public class ExtraToEnable { public int cageID; public DeactivateExtraItem objectToEnable; }
    public ExtraToEnable[] extrasToEnable;

    public NPCInteraction musicianText;

    void Awake()
    {
        if (singleton != null)
        {
            enabled = false;
            Destroy(this);
            return;
        }
        singleton = this;
        RunThroughNPCs();
    }

    public static void RunThroughNPCs()
    {
        if (singleton == null)
        {
            Debug.LogError("Singleton is null! Aborting.");
            return;
        }
        singleton._RunThroughNPCs();
    }

    void _RunThroughNPCs()
    {
        for (int i = 0; i < npcsToEnable.Count; i++)
        {
            npcsToEnable[i].playerprefstocheck = "Cage" + i.ToString();

            foreach(ExtraToEnable ete in extrasToEnable)
            {
                if(ete.cageID == i)
                    ete.objectToEnable.playerprefstocheck = "Cage" + i.ToString();
            }
            SpecialCaseCheck(i);
        }
    }

    public static int GetAmount()
    {
        if (singleton == null)
        {
            Debug.LogError("Singleton is null! Aborting.");
            return 0;
        }
        return singleton._GetAmount();
    }

    int _GetAmount()
    {
        return singleton.npcsToEnable.Count;
    }

    public static void SpecialCaseCheck(int cageID)
    {
        if (singleton == null)
        {
            Debug.LogError("Singleton is null! Aborting.");
            return;
        }
        singleton._SpecialCaseCheck(cageID);
    }

    void _SpecialCaseCheck(int cageID)
    {
        if(cageID <= 3)
        {
            StartCoroutine(ReEnable(extrasToEnable[0].objectToEnable.gameObject.transform.parent.gameObject));

            SetTextForMusician();
        }
    }

    public static void SetTextForMusician()
    {
        if (singleton == null)
        {
            Debug.LogError("Singleton is null! Aborting.");
            return;
        }
        singleton._SetTextForMusician();
    }

    bool freeMusAchiev;
    //
    void _SetTextForMusician()
    {
        int countedPrefs = 0;
        for (int i = 0; i < 4; i++)
            countedPrefs += PlayerPrefs.GetInt("Cage" + i.ToString(), 0);
        
        musicianText.textID = countedPrefs;

#if UNITY_PS4
        //
        // check trophy musicians if all cages are 1 countedPrefs = 4
        if (countedPrefs == 4 && !freeMusAchiev)
        {
            freeMusAchiev = true;
            PS4Manager.ps4TrophyManager.UnlockTrophy((int)PS4_TROPHIES.FREE_ALL_MUSICIANS);
        }
            
#endif

#if UNITY_XBOXONE
        //
        // check trophy musicians if all cages are 1 countedPrefs = 4
        if (countedPrefs == 4 && !freeMusAchiev)
        {
            freeMusAchiev = true;
            XONEAchievements.SubmitAchievement((int)XONEACHIEVS.MUSIC_LOVER);
        }
#endif
    }

    IEnumerator ReEnable(GameObject g)
    {
        g.SetActive(false);
        yield return new WaitForSeconds(1f) ;
        g.SetActive(true);
    }
}
