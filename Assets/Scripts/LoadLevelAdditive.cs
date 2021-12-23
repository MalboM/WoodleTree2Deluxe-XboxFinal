using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityStandardAssets.ImageEffects;
using CinemaDirector;

public class LoadLevelAdditive : MonoBehaviour
{
	public string level;

	public Animator animator;
	public GameObject noncolliderzone;
	public Image loadIcon;
    
	TPC tpc;
    TPC tpcBeaver;
	CameraFollower cam;

    [HideInInspector] public GameObject fullSceneObject;

    public ObjDeactivateManager odm;

    public Level5EntranceTrigger entTrig;
    bool inLevelLoad;

    [HideInInspector] public bool insideThisTrigger;
    public BoxCollider lptCollider;

    void Start(){
		
	}
    
    void OnTriggerEnter(Collider other){
        if (!insideThisTrigger && other.gameObject == PlayerManager.GetMainPlayer().gameObject && !PlayerManager.GetMainPlayer().challengeWarping && !PlayerManager.GetMainPlayer().disableControl && (entTrig == null || entTrig.isInsideThis))
        {
       //     if (level == "Level5")
         //       Debug.Log("ENTER LEVEL LOAD");

            if(entTrig)
                entTrig.keepOn = true;
            
            tpc = other.gameObject.GetComponent<TPC>();
        //    tpc.ps.ShowLevelTitle(level);

            if (!SceneManager.GetSceneByName(level).isLoaded)
            {
                tpcBeaver = PlayerManager.GetPlayer(1);
                cam = tpc.cam.GetComponent<CameraFollower>();
                StartCoroutine(Loadinglevel());
            }
            else
            {
                if (fullSceneObject == null)
                    FindFullLevel();
                if (fullSceneObject != null)
                    odm.ActivateObject(fullSceneObject);

                StartCoroutine("WaitToDeact");
                //   if (noncolliderzone != null)
                //       odm.DeactivateObject(noncolliderzone, null);
            }

            if (lptCollider != null)
            {
                lptCollider.gameObject.SetActive(false);
                lptCollider.enabled = true;
                lptCollider.gameObject.SetActive(true);
            }
        }
	}

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == PlayerManager.GetMainPlayer().gameObject && !PlayerManager.GetMainPlayer().challengeWarping && (entTrig == null || entTrig.isInsideThis))
        {
       //     if (level == "Level5")
       //         Debug.Log("EXIT LEVEL LOAD");
            if (entTrig)
            {
                entTrig.keepOn = false;
            //    Debug.Log(entTrig.isInsideThis +"  "+ entTrig.colliderToActivate.enabled);
                if (!entTrig.isInsideThis)
                    entTrig.colliderToActivate.enabled = false;
            }
            StopCoroutine("WaitToDeact");
            if (noncolliderzone != null)
                odm.ActivateObject(noncolliderzone);
            if (fullSceneObject == null)
                FindFullLevel();
            if (fullSceneObject != null)
                odm.DeactivateObject(fullSceneObject, null);

            if (lptCollider != null)
                lptCollider.enabled = false;
        }
    }

    bool IsInAnotherTrigger()
    {
        foreach (LoadLevelAdditive lla in tpc.ps.sS.loadLevelAdditives)
        {
            if (lla != this)
            {
                if (lla.insideThisTrigger)
                    return true;
            }
        }
        return false;
    }

    IEnumerator Loadinglevel(){
        insideThisTrigger = true;

        while (IsInAnotherTrigger())
            yield return null;

        inLevelLoad = true;

        if (tpc != null && tpc.GetComponentInParent<OneWayManager>() != null)
            tpc.GetComponentInParent<OneWayManager>().currentlyChecking = true;

        tpc.ps.cantPause = true;
		bool fsOn = false;
		if (animator.GetBool ("Loading") != true) {
			animator.SetBool ("Loading", true);

			loadIcon.fillAmount = 0f;
		//	Time.timeScale = 0f;
		} else
			fsOn = true;

		for(int t = 0; t < 60; t++)
			yield return null;

        int checkedScene = -1;
        if (level == "Level1.2")
            checkedScene = CheckForLevel(4);
        if (level == "Level2")
            checkedScene = CheckForLevel(5);
        if (level == "Level3")
            checkedScene = CheckForLevel(6);
        if (level == "Level4")
            checkedScene = CheckForLevel(7);
        if (level == "Level5")
            checkedScene = CheckForLevel(8);
        if (level == "Level6")
            checkedScene = CheckForLevel(9);
        if (level == "Level7")
            checkedScene = CheckForLevel(10);
        if (level == "Level8")
            checkedScene = CheckForLevel(1);

        if (checkedScene > -1)
        {
        //    Debug.Log("LLA: "+ checkedScene + " . " + this.gameObject.name + " . " + level);

            Application.backgroundLoadingPriority = ThreadPriority.Low;

            AsyncOperation async = new AsyncOperation();
            async = SceneManager.UnloadSceneAsync(checkedScene);

            while (!async.isDone)
            {
                loadIcon.fillAmount = Mathf.Lerp(loadIcon.fillAmount, async.progress, 0.85f) / 2f;
                yield return null;
            }

            yield return null;
            yield return null;
            yield return null;
            yield return null;

            AsyncOperation async2 = new AsyncOperation();
            async2 = SceneManager.LoadSceneAsync(level, LoadSceneMode.Additive);

            async2.allowSceneActivation = false;
            while (async2.progress < 0.9f)
            {
                loadIcon.fillAmount = (Mathf.Lerp(loadIcon.fillAmount, async2.progress, 0.85f) / 2f) + 0.5f;
                yield return null;
            }
            async2.allowSceneActivation = true;
        }
        else
        {
            AsyncOperation async = new AsyncOperation();
            async = SceneManager.LoadSceneAsync(level, LoadSceneMode.Additive);

            while (!async.isDone)
            {
                loadIcon.fillAmount = Mathf.Lerp(loadIcon.fillAmount, async.progress, 0.85f);
                yield return null;
            }
        }

        if (noncolliderzone != null)
            StartCoroutine("WaitToDeact");

        Vector3 cPo = tpc.gameObject.transform.position;
        Time.timeScale = 1.0f;

        if (level == "ExternalWorld")
        {
            for (int t = 0; t < 120; t++)
            {
                tpc.gameObject.transform.position = cPo;
                yield return null;
            }
        }


        animator.SetBool("Loading", false);


        cam.disableControl = false;
        tpc.disableControl = false;
        tpcBeaver.disableControl = false;
        tpc.ps.cantPause = false;

        yield return null;
        FindFullLevel();

        yield return new WaitForSeconds(1f);
        if (tpc != null && tpc.GetComponentInParent<OneWayManager>() != null)
            tpc.GetComponentInParent<OneWayManager>().CheckOneWays();

        yield return null;

        bool thisSceneLoaded = false;
        for (int s = 0; s < SceneManager.sceneCount; s++)
        {
            if (SceneManager.GetSceneAt(s).name == level)
            {
                if (!thisSceneLoaded)
                    thisSceneLoaded = true;
                else
                {
                    SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(s));
                    break;
                }
            }
        }
        inLevelLoad = false;
        insideThisTrigger = false;
    }

    public void FindFullLevel()
    {
        if (fullSceneObject == null)
        {
            foreach (GameObject g in GameObject.FindGameObjectsWithTag("WholeLevel"))
            {
                if (g.name == level)
                    fullSceneObject = g.gameObject;
            }
        }
    }

    private int CheckForLevel(int check)
    {  //3 - 10
        int c = 1;
        while (c <= 8)
        {
            int d = check + c;
            if (d > 10)
                d -= 8;
            if (SceneManager.GetSceneByBuildIndex(d).isLoaded)
                return d;
            c++;
        }
        return -1;
    }

    IEnumerator WaitToDeact()
    {
        yield return new WaitForSeconds(1f);
        while (inLevelLoad)
            yield return null;
        yield return new WaitForSeconds(1f);
        if (noncolliderzone != null)
            odm.DeactivateObject(noncolliderzone, null);

    }
}