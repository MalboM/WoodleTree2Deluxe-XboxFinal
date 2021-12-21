using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class WaterTearManager : MonoBehaviour
{
    public TPC mainCharacter;

    public Camera mainCamera;
    public CameraFollower cameraFollower;

    static WaterTearManager singleton;

    public GameObject mainPlaza;

    public Camera[] camerasToActivate;
    Camera currentCamera;

    public GameObject[] tearsToActivate;
    GameObject currentTear;

    public GameObject tearAppearPFX;
    private ParticleSystem.EmissionModule tearAppearEM;

    public GameObject bordersParent;

    public PlayableDirector firstTearsCutscene;
    public PlayableDirector allTearsCutscene;
    public PlayableDirector finalCutscene;

    public GameObject elevator;

    public GameObject[] cutsceneActiveObjs;

    void Awake()
    {
        if (singleton != null && singleton != this)
        {
            enabled = false;
            Destroy(this);
            return;
        }
        else if( singleton == null)
        {
            singleton = this;
        }


        tearAppearEM = tearAppearPFX.GetComponent<ParticleSystem>().emission;
        tearAppearEM.enabled = false;

        if (PlayerPrefs.GetInt("First3Tears", 0) == 1 && bordersParent != null)
            bordersParent.SetActive(false);

        mainCharacter.ps.CheckTears();
        if (mainCharacter.ps.tearCount == 24)
            elevator.SetActive(false);
    }

    public static PlayableDirector GetFinalCutscene()
    {
        if (singleton == null)
        {
            Debug.LogError("Singleton is null! Aborting.");
            return null;
        }
        return singleton._GetFinalCutscene();
    }

    PlayableDirector _GetFinalCutscene()
    {
        return finalCutscene;
    }

    public static void ShowCutscene(WaterTearCatchCameraActivate waterTear)
    {
        if (singleton == null)
        {
            Debug.LogError("Singleton is null! Aborting.");
            return;
        }

        Debug.Log("A");
        singleton._ShowCutscene(waterTear);
    }

    void _ShowCutscene(WaterTearCatchCameraActivate waterTear)
    {
        Debug.Log("B");

        if (PlayerPrefs.GetInt("FirstTear") == 0)
        {
            Debug.Log("C");

            PlayerPrefs.SetInt("FirstTear", 1);
            GameObject.FindWithTag("Pause").transform.Find("Event Text").gameObject.GetComponent<TextTriggerMain>().SetText(3);
        }

        Debug.Log("D");

        foreach (GameObject t in tearsToActivate)
        {
            if (t.name == waterTear.vasetofind) 
                currentTear = t;
        }

        Debug.Log("E");


        PlayerPrefs.SetInt(waterTear.vasetofind, 1);

        mainCharacter.ps.CheckTears();

        Debug.Log("F");

        if (mainCharacter.ps.tearCount < 10)
            mainCharacter.tearText.text = "0" + mainCharacter.ps.tearCount.ToString();
        else
            mainCharacter.tearText.text = mainCharacter.ps.tearCount.ToString();

        Debug.Log("G");

        StartCoroutine(Camerachange(waterTear));
    }

    public static void AllTearsCutscene()
    {
        if (singleton == null)
        {
            Debug.LogError("Singleton is null! Aborting.");
            return;
        }
        singleton._AllTearsCutscene();
    }

    void _AllTearsCutscene()
    {
        StartCoroutine("CutsceneRoutine", allTearsCutscene);
    }

    public static void FirstCutscene()
    {
        if (singleton == null)
        {
            Debug.LogError("Singleton is null! Aborting.");
            return;
        }
        singleton._FirstCutscene();
    }

    void _FirstCutscene()
    {
        if (bordersParent != null)
            bordersParent.SetActive(false);
        StartCoroutine("CutsceneRoutine", firstTearsCutscene);
    }

    public static void FinalCutscene()
    {
        if (singleton == null)
        {
            Debug.LogError("Singleton is null! Aborting.");
            return;
        }
        singleton._FinalCutscene();
    }

    void _FinalCutscene()
    {
        StartCoroutine("CutsceneRoutine", finalCutscene);
    }

    IEnumerator Camerachange(WaterTearCatchCameraActivate waterTear)
    {
        Debug.Log("G-1 :");

        TPC tpc = PlayerManager.GetMainPlayer();
        tpc.ps.ControlDisable(true);
        cameraFollower.disableControl = true;

        Debug.Log("G-2 :");

        foreach (Camera c in camerasToActivate)
        {
            if (c.gameObject.name == waterTear.cameratofind)
                currentCamera = c;
        }

        Debug.Log("G-3 :");

        foreach (GameObject g in cutsceneActiveObjs)
        {
            g.SetActive(true);
        }


        currentCamera.enabled = true;
        mainCamera.enabled = false;
        bool plazaActiveState = mainPlaza.activeInHierarchy;
        bool plazaFActiveState = mainPlaza.transform.parent.gameObject.activeInHierarchy;

        if (!plazaFActiveState)
            mainPlaza.transform.parent.gameObject.SetActive(true);

        Debug.Log("G-4 :");


        mainPlaza.gameObject.SetActive(true);


        tearAppearPFX.transform.position = currentTear.transform.position;
        tearAppearEM.enabled = true;
        tearAppearPFX.GetComponent<ParticleSystem>().Play();

        yield return new WaitForSeconds(1.5f);

        Debug.Log("G-5 :");

        currentTear.GetComponent<WaterTearAltairActivateSave>().waterteartoactivate.SetActive(true);

        yield return new WaitForSeconds(1.5f);
        Debug.Log("G-6 :");

        tearAppearEM.enabled = false;

        mainPlaza.gameObject.SetActive(plazaActiveState);
        mainPlaza.transform.parent.gameObject.SetActive(plazaFActiveState);
        currentCamera.enabled = false;
        mainCamera.enabled = true;

    //    foreach (GameObject g in cutsceneActiveObjs)
    //        g.SetActive(false);

        //    waterTear.DeactivateTear();

        //    waterTear.GetComponent<Collider>().enabled = false;
        if (PlayerPrefs.GetInt("First3Tears", 0) == 0 && PlayerPrefs.GetInt("Vase1Level1", 0) == 1 && PlayerPrefs.GetInt("Vase2Level1", 0) == 1 && PlayerPrefs.GetInt("Vase3Level1", 0) == 1)
        {
            Debug.Log("G-7 :");

#if UNITY_PS4
            //
            // check trophy
            PS4Manager.ps4TrophyManager.UnlockTrophy((int)PS4_TROPHIES.COMPLETE_FIRST_LEVEL);
#endif

#if UNITY_XBOXONE
            //
            // check trophy : items >= 3 and items = all 
            //
            XONEAchievements.SubmitAchievement((int)XONEACHIEVS.FIRST_TREE_SAGE_RESTORED);
#endif
            Debug.Log("G-8 :");

            PlayerPrefs.SetInt("First3Tears", 1);
            StartCoroutine("CutsceneRoutine", firstTearsCutscene);
            if(bordersParent != null)
                bordersParent.SetActive(false);
        }
        else
        {
            Debug.Log("G-9 :");

            tpc.ps.ControlDisable(false);
            cameraFollower.disableControl = false;
        }
    }

    IEnumerator CutsceneRoutine(PlayableDirector cutsceneToPlay)
    {
        TPC tpc = PlayerManager.GetMainPlayer();
        PlayerPrefs.SetInt("Intro2Watched", 1);
        StartScreen ss = tpc.ps.sS;
        if (cutsceneToPlay != finalCutscene)
        {
            for (int f = 1; f <= 60; f++)
            {
                AudioListener.volume = Mathf.Lerp(1f, 0f, (f * 1f) / 60f);
                ss.loadFS.color = Color.Lerp(Color.clear, Color.white, (f * 1f) / 60f);
                yield return null;
            }
        }
        tpc.transform.position = Vector3.up;
        tpc.anim.Play("Idle", 0);
        tpc.rb.velocity = Vector3.zero;
        tpc.disableControl = true;
        cameraFollower.disableControl = false;
        //    yield return new WaitForSeconds(3f);

        //    if(cutsceneToPlay == firstTearsCutscene)
        //        GameObject.FindWithTag("Pause").transform.Find("Event Text").gameObject.GetComponent<TextTriggerMain>().SetText(12);

        if (cutsceneToPlay != finalCutscene)
        {
            for (int f = 1; f <= 60; f++)
            {
                AudioListener.volume = Mathf.Lerp(0f, 1f, (f * 1f) / 60f);
                ss.loadFS.color = Color.Lerp(Color.white, Color.clear, (f * 1f) / 60f);

                if (tpc.ps.plazaMain != null && (!tpc.ps.plazaMain.activeInHierarchy || !tpc.ps.plazaMain.transform.parent.gameObject.activeInHierarchy))
                {
                    tpc.ps.plazaMain.SetActive(true);
                    tpc.ps.plazaMain.transform.parent.gameObject.SetActive(true);
                }

                yield return null;
            }
        }
        tpc.ps.StartCutscene(cutsceneToPlay, Vector3.up);
    //    ControlDisable(false);
    }

    public static void DeactivateAllTears()
    {
        if (singleton == null)
        {
            Debug.LogError("Singleton is null! Aborting.");
            return;
        }
        singleton._DeactivateAllTears();
    }

    void _DeactivateAllTears()
    {
        foreach(GameObject g in tearsToActivate)
        {
            g.GetComponent<WaterTearAltairActivateSave>().waterteartoactivate.SetActive(false);
        }
    }

    public static void AtivateAllTears()
    {
        if (singleton == null)
        {
            Debug.LogError("Singleton is null! Aborting.");
            return;
        }
        singleton._ActivateAllTears();
    }

    void _ActivateAllTears()
    {
        foreach (GameObject g in tearsToActivate)
        {
            g.GetComponent<WaterTearAltairActivateSave>().waterteartoactivate.SetActive(true);
        }
    }

    public static void UpdateTears()
    {
        if (singleton == null)
        {
            Debug.LogError("Singleton is null! Aborting.");
            return;
        }
        singleton._UpdateTears();
    }

    void _UpdateTears()
    {
        foreach (GameObject g in tearsToActivate)
        {
            g.GetComponent<WaterTearAltairActivateSave>().CheckActiveState();
        }

        mainCharacter.ps.CheckTears();
        if (mainCharacter.ps.tearCount == 0)
        {
            elevator.SetActive(true);
            if(bordersParent != null)
                bordersParent.SetActive(true);
        }
    }
}