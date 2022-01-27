using UnityEngine;
using System.Collections;

public class Collectible : MonoBehaviour
{

    public int value;
    public string collectibleType;

    private TPC chara;

    private int berryIt;
    [HideInInspector] public bool movingBerry;
    private GameObject bagpack;
    private Vector3 origPos;

    public SphereCollider sphereCol;
    public bool deactParent;

    Material shadowMat;
    Color startCol;

    [HideInInspector] public bool canCollect = false;

    //
    public int totalRBCount;

    private void Awake()
    {
        StartCoroutine("WaitToCollect");
    }

    void OnTriggerStay(Collider other)
    {
        if ((other.gameObject.tag == "Player"))
        {
            //    Debug.Log(this.transform.parent.name + " " + canCollect +" "+ this.movingBerry);
            if (canCollect && !this.movingBerry)
            {
                if (collectibleType == "Berry")
                {
                    chara = other.gameObject.GetComponent<TPC>();
                    chara.berryPFX.PlayEffect(0, this.transform.position, null, Vector3.zero, false, false);
                    this.bagpack = chara.dayPack;
                    this.origPos = this.transform.position;
                    if (sphereCol != null)
                        sphereCol.enabled = false;
                    if (PlayerPrefs.GetInt("AllFlowers", 0) == 1)
                        value *= 2;
                    AddToTotalBerries(value);
                    chara.berryCount += value;
                    BerrySpawnManager.PlayBerryNoise(false);
                    chara.UpdateBerryHUDRed();
                    this.movingBerry = true;
                    this.berryIt = 0;
                    StartCoroutine(MoveBerry());
                }
                if (collectibleType == "BigBerry")
                {
                    chara = other.gameObject.GetComponent<TPC>();
                    chara.berryPFX.PlayEffect(0, this.transform.position, null, Vector3.zero, false, false);
                    this.bagpack = chara.dayPack;
                    this.origPos = this.transform.position;
                    if (sphereCol != null)
                        sphereCol.enabled = false;
                    value = 5;
                    if (PlayerPrefs.GetInt("AllFlowers", 0) == 1)
                        value *= 2;
                    AddToTotalBerries(value);
                    StartCoroutine(MultiCollect(value));
                    BerrySpawnManager.PlayBerryNoise(true);
                    this.movingBerry = true;
                    this.berryIt = 0;
                    StartCoroutine(MoveBerry());
                }
                if (collectibleType == "BlueBerry")
                {
                    if (PlayerPrefs.GetInt("FirstBlueBerry", 0) == 0)
                    {
                        PlayerPrefs.SetInt("FirstBlueBerry", 1);
                        GameObject.FindWithTag("Pause").transform.Find("Event Text").gameObject.GetComponent<TextTriggerMain>().SetText(1);
                    }

                    chara = other.gameObject.GetComponent<TPC>();
                    chara.berryPFX.PlayEffect(1, this.transform.position, null, Vector3.zero, false, false);
                    this.bagpack = chara.dayPack;
                    this.origPos = this.transform.position;
                    if (sphereCol != null)
                        sphereCol.enabled = false;
                    chara.blueberryCount += value;
                    BerrySpawnManager.PlayBlueBerryNoise(other.gameObject.GetComponent<TPC>().pID);
                    chara.UpdateBerryHUDBlue();
                    this.movingBerry = true;
                    this.berryIt = 0;
                    StartCoroutine(MoveBerry());
                }
            }
        }
    }

    void AddToTotalBerries(int valueToAdd)
    {
        int totalRedBerries = PlayerPrefs.GetInt("TotalRedBerries", chara.berryCount);
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
        //
        // check trophy
        //if (totalRedBerries >= 100)
        //{
        //    XONEAchievements.SubmitAchievement((int)XONEACHIEVS.RED_BERRIES_LOVER);
        //}
        ////
        //if (totalRedBerries >= 1000)
        //{
        //    XONEAchievements.SubmitAchievement((int)XONEACHIEVS.RED_BERRIES_PARADE);
        //}
        ////
        //if (totalRedBerries >= 3000)
        //{
        //    XONEAchievements.SubmitAchievement((int)XONEACHIEVS.RED_BERRIES_CHAMPION);
        //}
#endif
    }

    IEnumerator MoveBerry()
    {
        bool hasParentShadow = false;
        if (this.transform.parent != null && this.transform.parent.parent != null && this.transform.parent.parent.Find("BlobShadowProjector") != null)
            hasParentShadow = true;
        if (hasParentShadow)
        {
            shadowMat = this.transform.parent.parent.Find("BlobShadowProjector").gameObject.GetComponentInChildren<MeshRenderer>().material;
            this.transform.parent.parent.Find("BlobShadowProjector").gameObject.GetComponentInChildren<MeshRenderer>().material = shadowMat;
            startCol = shadowMat.GetColor("_Color");
        }

        while (this.berryIt < 40)
        {
            this.transform.position = Vector3.Slerp(this.origPos, chara.gameObject.transform.position + (this.bagpack.transform.localPosition.z * (chara.gameObject.transform.forward * 1.6f)), (berryIt / 40f));
            this.transform.localScale = Vector3.one * ((40f - this.berryIt) / 40f);
            if (hasParentShadow)
                shadowMat.SetColor("_Color", Color.Lerp(startCol, Color.clear, (berryIt / 40f)));
            this.berryIt++;
            yield return null;
        }

        if (chara.gameObject.name == "Woodle Character")
            chara.berryPFX.PlayEffect(2, this.transform.position, null, Vector3.zero, false, false);

        if (hasParentShadow)
            this.transform.parent.parent.Find("BlobShadowProjector").gameObject.SetActive(false);

        if (this.transform.parent.gameObject.GetComponent<ActivateAtDistanceNew2>() != null)
            Destroy(this.transform.parent.gameObject.GetComponent<ActivateAtDistanceNew2>());

        if (this.transform.parent != null && this.transform.parent.parent != null
            && !this.transform.parent.parent.name.Contains("Berry Spawn") && this.transform.parent.parent.gameObject.GetComponent<IActivablePrefab>() != null)
            Destroy(this.transform.parent.parent.gameObject.GetComponent<IActivablePrefab>());

        yield return new WaitForSeconds(1f);

        this.movingBerry = false;

        if (deactParent)
            this.transform.parent.gameObject.SetActive(false);
        else
            this.gameObject.SetActive(false);

    }

    IEnumerator WaitToCollect()
    {
        yield return new WaitForSeconds(1f);
        canCollect = true;
    }

    IEnumerator MultiCollect(int amount)
    {
        chara.berryCount++;
        chara.UpdateBerryHUDRed();
        amount--;
        yield return new WaitForSeconds(0.2f);
        if (amount != 0)
            StartCoroutine(MultiCollect(amount));
    }
}
