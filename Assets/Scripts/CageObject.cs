using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CageObject : MonoBehaviour
{
    public bool debugResetPref;

    public int cageID;

    public int helpTextID = 5;
    public int freeTextID = 6;

    public GameObject cage;
    public GameObject npc;
    public GameObject npcAddOn;

    public Text npcText;

    public Animator npcAnimator;
    Renderer npcRenderer;
    public string victoryAnimationName;

    public AudioSource audioSource;

    TPC tpc;
    GameObject woodle;

    public int berryReward = 50;

    public bool trpSet;

    private void Awake()
    {
        //    if(debugResetPref)
        //        PlayerPrefs.SetInt("Cage" + cageID.ToString(), 0);
    }

    private void OnEnable()
    {
        //    string newText = TextTranslationManager.GetText(TextTranslationManager.TextCollection.npc, 5, PlayerPrefs.GetInt("Language"));
        //    npcText.gameObject.transform.parent.parent.parent.parent.gameObject.GetComponent<NPCInteraction>().UpdateText(newText);

        npcText.gameObject.transform.parent.parent.parent.parent.gameObject.GetComponent<NPCInteraction>().textID = helpTextID;

        if (cageID != -1)
        {
            if (PlayerPrefs.GetInt("Cage" + cageID.ToString(), 0) == 1)
            {
                audioSource.enabled = false;
                if (cage.activeInHierarchy)
                    ToggleObjects(false);
            }
            if (PlayerPrefs.GetInt("Cage" + cageID.ToString(), 0) == 0 && !cage.activeInHierarchy)
                ToggleObjects(true);
        }
    }

    public void CageBroken()
    {
        if (cageID != -1)
        {
            PlayerPrefs.SetInt("Cage" + cageID.ToString(), 1);

            CagedNPCManager.RunThroughNPCs();

            if (cageID >= 4)
                CountFreedFlowers();
            //    CagedNPCManager.SpecialCaseCheck(cageID);
        }

        audioSource.enabled = true;

        npcText.gameObject.transform.parent.parent.parent.parent.gameObject.GetComponent<NPCInteraction>().textID = freeTextID;

        if (npcAnimator != null && victoryAnimationName != "")
            npcAnimator.Play(victoryAnimationName, 0);

        tpc = PlayerManager.GetMainPlayer();
        woodle = tpc.gameObject;

        if (PlayerPrefs.GetInt("AllFlowers", 0) == 1)
            berryReward *= 2;

        int totalRedBerries = PlayerPrefs.GetInt("TotalRedBerries", tpc.berryCount);
        totalRedBerries += berryReward;
        PlayerPrefs.SetInt("TotalRedBerries", totalRedBerries);

#if UNITY_PS4
            //
            // check trophy
            if (totalRBCount >= 100)
            {
                PS4Manager.ps4TrophyManager.UnlockTrophy((int)PS4_TROPHIES.COLLECT_100_RED_BERRIES);
            }
            //
            if (totalRBCount >= 1000)
            {
                PS4Manager.ps4TrophyManager.UnlockTrophy((int)PS4_TROPHIES.COLLECT_1000_RED_BERRIES);
            }
            //
            if (totalRBCount >= 3000)
            {
                PS4Manager.ps4TrophyManager.UnlockTrophy((int)PS4_TROPHIES.COLLECT_3000_RED_BERRIES);
            }
#endif
        StartCoroutine(BerryReward(berryReward));

        if (npcAnimator != null)
            npcRenderer = npcAnimator.GetComponentInChildren<Renderer>();
        StartCoroutine(FinishInteraction());
    }

    IEnumerator FinishInteraction()
    {
        if (npcAnimator != null)
        {
            Vector3 endPos = npc.transform.position - (Vector3.up * 0.35f);
            for (float m = 0f; m < 1f; m += Time.deltaTime)
            {
                npc.transform.position = Vector3.Lerp(npc.transform.position, endPos, Time.deltaTime * 5f);
                yield return null;
            }

            while (npcRenderer.isVisible || Vector3.SqrMagnitude(this.transform.position - woodle.transform.position) <= 200f)
                yield return null;
        }

        ToggleObjects(false);
    }

    void ToggleObjects(bool activate)
    {
        cage.SetActive(activate);
        if (npc != null)
            npc.SetActive(activate);
        npcAddOn.SetActive(activate);
    }

    IEnumerator BerryReward(int amount)
    {
        if (amount >= 5)
            tpc.berryCount += 5;
        else
            tpc.berryCount += amount;
        tpc.UpdateBerryHUDRed();
        BerrySpawnManager.PlayBerryNoise(false);
        amount -= 5;
        yield return new WaitForSeconds(0.2f);
        if (amount > 0)
            StartCoroutine(BerryReward(amount));
    }

    void CountFreedFlowers()
    {
        int counted = 0;
        for (int ff = 0; ff < 12; ff++)
        {
            if (PlayerPrefs.GetInt("Cage" + (ff + 4).ToString(), 0) == 1)
                counted++;
        }
        if (counted == 12)
        {
            PlayerManager.GetMainPlayer().ps.textMain.SetText(14);
            PlayerPrefs.SetInt("AllFlowers", 1);

#if UNITY_PS4
            //
            // check trophy flowers
            //
            if (!trpSet)
            {
                PS4Manager.ps4TrophyManager.UnlockTrophy((int)PS4_TROPHIES.AWAKE_ALL_SACRED_FLOWERS);
                trpSet = true;
            }
#endif

#if UNITY_XBOXONE
            //
            // check trophy musicians if all cages are 1 countedPrefs = 4
            if (!trpSet)
            {
                trpSet = true;
                XONEAchievements.SubmitAchievement((int)XONEACHIEVS.AWAKING_FROM_A_LONG_SLEEP);
            }
#endif
        }
    }
}
