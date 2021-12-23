using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BerryManagerTrigger : MonoBehaviour
{

    GameObject[] berries;
    GameObject woodle;
    TPC tpc;
    Material shadowMat;
    Color startCol;

    TextTriggerMain textMain;

    public enum BerryType { red, blue, redCircle };

    void Start()
    {
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("Player"))
        {
            if (go.name == "Woodle Character")
                woodle = go;
        }
        tpc = woodle.GetComponent<TPC>();

        berries = new GameObject[this.transform.childCount];
        int b = 0;
        foreach (Transform t in transform)
        {
            berries[b] = t.gameObject;
            b++;
        }

        textMain = GameObject.FindWithTag("Pause").transform.Find("Event Text").gameObject.GetComponent<TextTriggerMain>();
    }

    /*
    private void OnEnable()
    {
        StartCoroutine("CheckDistance");
    }
    */

    public void CollectBerry(GameObject berry, BerryType berryType, int amount, bool playFX, int initAmount)
    {
        if (amount <= 0)
        {
            amount = 1;
            initAmount = 1;
        }
        if (berryType == BerryType.red || berryType == BerryType.redCircle)
        {
            if (playFX)
            {
                if (PlayerPrefs.GetInt("FirstRedBerry", 0) == 0)
                {
                    PlayerPrefs.SetInt("FirstRedBerry", 1);
                    textMain.SetText(0);
                }
                tpc.berryPFX.PlayEffect(0, berry.transform.position, null, Vector3.zero, false, false);

                if (amount == 1)
                    BerrySpawnManager.PlayBerryNoise(false);
                else
                    BerrySpawnManager.PlayBerryNoise(true);
            }
            int valueToAdd = 1;
            if (PlayerPrefs.GetInt("AllFlowers", 0) == 1)
                valueToAdd = 2;
            tpc.berryCount += valueToAdd;
            tpc.UpdateBerryHUDRed();

            int totalRedBerries = PlayerPrefs.GetInt("TotalRedBerries", tpc.berryCount);
            totalRedBerries += valueToAdd;
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
#if UNITY_XBOXONE

        // check trophy
        if (totalRBCount >= 100)
        {
            // check friend trophy
            XONEAchievements.SubmitAchievement((int)XONEACHIEVS.RED_BERRIES_LOVER);
        }
        //
        if (totalRBCount >= 1000)
        {
            XONEAchievements.SubmitAchievement((int)XONEACHIEVS.RED_BERRIES_PARADE);
        }
        //
        if (totalRBCount >= 3000)
        {
            XONEAchievements.SubmitAchievement((int)XONEACHIEVS.RED_BERRIES_CHAMPION);
        }
       
#endif
        }

        if (berryType == BerryType.blue)
        {

            if (playFX)
            {
                if (PlayerPrefs.GetInt("FirstBlueBerry") == 0)
                {
                    PlayerPrefs.SetInt("FirstBlueBerry", 1);
                    textMain.SetText(1);
                }
                tpc.berryPFX.PlayEffect(1, berry.transform.position, null, Vector3.zero, false, false);
            }

            tpc.blueberryCount += amount;
            tpc.UpdateBerryHUDBlue();

            //     tpc.ps.CheckBlues();
            PlayerPrefs.SetInt("BlueBerryTotal", PlayerPrefs.GetInt("BlueBerryTotal") + 1);

            if (PlayerPrefs.GetInt("BlueBerryTotal") == 465)
            {
                PlayerPrefs.SetInt("HalfBlueBerries", 1);
                textMain.SetText(15);
            }

            if (PlayerPrefs.GetInt("BlueBerryTotal") == 930)
            {
                PlayerPrefs.SetInt("AllBlueBerries", 1);

                textMain.SetText(13);
                BerrySpawnManager.PlayBerryNoise(true);

                tpc.berryCount += 1000;
                tpc.UpdateBerryHUDRed();
            }
            else
                BerrySpawnManager.PlayBlueBerryNoise(0);
#if UNITY_PS4
            // check trophy
            if (PlayerPrefs.GetInt("BlueBerryTotal") >= 100)
            {
                PS4Manager.ps4TrophyManager.UnlockTrophy((int)PS4_TROPHIES.COLLECT_100_BLUE_BERRIES);
            }
            //
            if (PlayerPrefs.GetInt("BlueBerryTotal") >= 930)
            {
                PS4Manager.ps4TrophyManager.UnlockTrophy((int)PS4_TROPHIES.COLLECT_ALL_BLUE_BERRIES);
            }
#endif

#if UNITY_XBOXONE
            // check trophy
            if (PlayerPrefs.GetInt("BlueBerryTotal") >= 100)
            {
                // check friend trophy
                XONEAchievements.SubmitAchievement((int)XONEACHIEVS.BLUE_BERRIES_LOVER);
            }
            //
            if (PlayerPrefs.GetInt("BlueBerryTotal") >= 930)
            {
                // check friend trophy
                XONEAchievements.SubmitAchievement((int)XONEACHIEVS.BLUE_BERRIES_CHAMPION);
            }
#endif

        }
        if (playFX)
        {
            berry.GetComponentInChildren<SphereCollider>().enabled = false;
            StartCoroutine(MoveBerry(berry, berryType, initAmount));
        }
    }

    IEnumerator MoveBerry(GameObject berry, BerryType berryType, int initAmount)
    {
        int berryIt = 0;
        Vector3 origPos = berry.transform.position;
        GameObject bagpack = tpc.dayPack;

        if (berry.transform.parent.Find("BlobShadowProjector") != null)
        {
            shadowMat = berry.transform.parent.Find("BlobShadowProjector").gameObject.GetComponentInChildren<MeshRenderer>().material;
            berry.transform.parent.Find("BlobShadowProjector").gameObject.GetComponentInChildren<MeshRenderer>().material = shadowMat;
            startCol = shadowMat.GetColor("_Color");
        }

        while (berryIt < 40)
        {
            berry.transform.position = Vector3.Slerp(origPos, woodle.transform.position + (bagpack.transform.localPosition.z * (woodle.transform.forward * 1.6f)), (berryIt / 40f));
            berry.transform.localScale = Vector3.one * ((40f - berryIt) / 40f);
            if (berry.transform.parent.Find("BlobShadowProjector") != null)
                shadowMat.SetColor("_Color", Color.Lerp(startCol, Color.clear, (berryIt / 40f)));
            berryIt++;
            yield return null;
        }

        if (tpc.gameObject.name == "Woodle Character")
            tpc.berryPFX.PlayEffect(2, this.transform.position, null, Vector3.zero, false, false);

        //   if (this.transform.parent.gameObject.GetComponent<ActivateAtDistanceNew2>() != null)
        //       Destroy(this.transform.parent.gameObject.GetComponent<ActivateAtDistanceNew2>());

        if (berry.transform.parent.Find("BlobShadowProjector") != null)
            berry.transform.parent.Find("BlobShadowProjector").gameObject.SetActive(false);

        yield return new WaitForSeconds(0.2f * (initAmount - 1));

        //    berry.transform.parent.gameObject.GetComponent<IActivablePrefab>().CancelRepeat();
        berry.gameObject.SetActive(false);

        if (berryType == BerryType.redCircle)
        {
            int b = 0;
            for (int a = 0; a < berry.transform.parent.parent.childCount; a++)
            {
                if (berry.transform.parent.parent.GetChild(a).GetChild(1).gameObject.activeInHierarchy)
                    b++;
            }
            if (b == 0)
                Destroy(berry.transform.parent.parent.parent.gameObject);
        }
    }

    IEnumerator CheckDistance()
    {
        yield return new WaitForSeconds(1f);
        foreach (GameObject g in berries)
        {
            if (Vector3.Distance(g.transform.position, woodle.transform.position) <= 60f)
            {
                if (!g.activeInHierarchy)
                    g.SetActive(true);
            }
            else
            {
                if (g.activeInHierarchy)
                    g.SetActive(false);
            }
        }
        StartCoroutine("CheckDistance");
    }
}
