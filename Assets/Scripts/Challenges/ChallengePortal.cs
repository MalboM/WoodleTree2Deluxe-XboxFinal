using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class ChallengePortal : MonoBehaviour
{
    public bool turnOffFirst;
    public bool turnOffSecond;
    public int portalID;
    public string challengeScene = "Challenges0";
    public enum PortalType { entrance, exit};
    public PortalType portalType;

    Animator animator;
    TPC tpc;
    PauseScreen ps;
    AudioSource audioSource;

    bool inTrigger;
    bool warping;

    public ChallengePortal returnPortal;
    public ChallengePortal exitPortal;
    GameObject fullEXT;

    [SerializeField] private GameObject pfx;
    private ParticleSystem.EmissionModule pfxEM;

    public GameObject sleepingFlowerNPC;
    public GameObject nearbyNPC;
    public GameObject npcAddOn;
    Animator flowerAnim;

    private void Start()
    {
        tpc = PlayerManager.GetMainPlayer();
        ps = tpc.ps;
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        pfxEM = pfx.GetComponent<ParticleSystem>().emission;
        pfxEM.enabled = false;

        if (sleepingFlowerNPC != null)
            flowerAnim = sleepingFlowerNPC.GetComponent<Animator>();

        //    if(PlayerPrefs.GetInt("Portal" + portalID.ToString(), 0) == 1)
        //        animator.SetBool("Activated", true);
    }

    private void Update()
    {
        if (inTrigger && !warping && !ps.telescope.usingTelescope && !ps.inPause && !ps.cantPause && ps.loadAnim.GetBool("Loading") == false)
        {
            if (tpc.input.GetButtonDown("Leaf")){
                warping = true;
                audioSource.Stop();
                audioSource.PlayDelayed(0f);
                tpc.ps.challengeWarpAnim.SetBool("skipOn", false);

                tpc.disableControl = true;
                tpc.challengeWarping = true;
                tpc.rb.velocity = Vector3.zero;
                tpc.anim.Play("Idle", 0);
                foreach (TPC mtpc in ps.multiTPC)
                    mtpc.disableControl = true;
                ps.cam.disableControl = true;

                if (portalType == PortalType.entrance)
                {
                    StartCoroutine(WarpToChallenge());
                }
                else
                {
                    returnPortal.WarpFromChallenge();
                }
            }
        }
        if (inTrigger && ps.warping)
            ExitTrigger();
        
        if(sleepingFlowerNPC != null)
            CheckPref();
    }

    public void CheckPref()
    {
        if (!sleepingFlowerNPC.activeSelf && PlayerPrefs.GetInt("Cage" + (portalID+4).ToString(), 0) == 0)
        {
            sleepingFlowerNPC.SetActive(true);
            if(nearbyNPC != null)
                nearbyNPC.SetActive(true);
            if (npcAddOn != null)
                npcAddOn.SetActive(true);
        }

        if (sleepingFlowerNPC.activeSelf && PlayerPrefs.GetInt("Cage" + (portalID + 4).ToString(), 0) == 1)
        {
            sleepingFlowerNPC.SetActive(false);
            if (nearbyNPC != null)
                nearbyNPC.SetActive(false);
            if (npcAddOn != null)
                npcAddOn.SetActive(false);
        }
        

        if (flowerAnim.GetBool("sleeping") == false)
            flowerAnim.SetBool("sleeping", true);
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == tpc.gameObject)
        {
            inTrigger = true;
            tpc.ps.challengeWarpAnim.SetBool("skipOn", true);

            if (PlayerPrefs.GetInt("Challenge" + portalID.ToString() + "Found", 0) == 0)
                PlayerPrefs.SetInt("Challenge" + portalID.ToString() + "Found", 1);

            pfxEM.enabled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == tpc.gameObject)
            ExitTrigger();
    }

    void ExitTrigger()
    {
        inTrigger = false;
        tpc.ps.challengeWarpAnim.SetBool("skipOn", false);
        pfxEM.enabled = false;
    }

    IEnumerator WarpToChallenge()
    {
        ps.cantPause = true;

        foreach (TPC.DissolveInfo dissolve in tpc.dissolves)
        {
            if (dissolve.renderer.gameObject.activeSelf)
            {
                Material[] dissolveMatInsts = dissolve.dissolveMaterials;
                dissolve.renderer.sharedMaterials = dissolveMatInsts;
            }
        }
        tpc.ChallengePortalWarpPFX();
        Vector3 origCameraPos = tpc.cam.transform.position;
        Quaternion origCameraRot = tpc.cam.transform.rotation;
        for (float f = 0f; f <= 1.2f; f += (Time.deltaTime / 2f))
        {
            foreach (TPC.DissolveInfo dissolve in tpc.dissolves)
            {
                if (dissolve.renderer.gameObject.activeSelf)
                {
                    for (int r = 0; r < dissolve.origMaterials.Length; r++)
                    {
                        if (dissolve.renderer.sharedMaterials[r].HasProperty("_DissolveAmount"))
                            dissolve.renderer.sharedMaterials[r].SetFloat("_DissolveAmount", Mathf.Clamp01(f));
                    }
                    if (Mathf.Clamp01(f) == 1f)
                        dissolve.renderer.enabled = false;
                }
            }
            tpc.cam.transform.position = Vector3.Lerp(tpc.cam.transform.position, tpc.transform.position + (tpc.transform.forward * 3f) + Vector3.up, Mathf.Clamp01(f));
            tpc.cam.transform.LookAt(tpc.transform.position);
            tpc.shadowProjector.orthographicSize = Mathf.Lerp(1.72f, 20f, Mathf.Clamp01(f));
            yield return null;
        }

        ps.loadAnim.SetBool("Loading", true);
        ps.loadIcon.fillAmount = 0f;

        if (tpc != null && tpc.GetComponentInParent<OneWayManager>() != null)
            tpc.GetComponentInParent<OneWayManager>().currentlyChecking = true;

        Time.timeScale = 0f;
        for (int f = 1; f <= 60; f++)
        {
            AudioListener.volume = Mathf.Lerp(1f, 0f, (f * 1f) / 60f);
            ps.loadFS.color = Color.Lerp(Color.clear, Color.black, (f * 1f) / 60f);
            yield return null;
        }

        tpc.cam.transform.position = origCameraPos;
        tpc.cam.transform.rotation = origCameraRot;

        foreach (TPC.DissolveInfo dissolve in tpc.dissolves)
        {
            if (dissolve.renderer.gameObject.activeSelf)
            {
                for (int r = 0; r < dissolve.origMaterials.Length; r++)
                {
                    dissolve.renderer.enabled = true;
                    Material[] origMatInsts = dissolve.origMaterials;
                    dissolve.renderer.sharedMaterials = origMatInsts;
                }
            }
        }

        AsyncOperation async = new AsyncOperation();
        async = SceneManager.LoadSceneAsync(challengeScene, LoadSceneMode.Additive);

        while (!async.isDone)
        {
            ps.loadIcon.fillAmount = Mathf.Lerp(ps.loadIcon.fillAmount, async.progress, 0.8f);
            yield return null;
        }


        if (tpc != null && tpc.GetComponentInParent<OneWayManager>() != null)
            tpc.gameObject.GetComponentInParent<OneWayManager>().CheckOneWays();

        if (ps.curLPT != null)
            ps.curLPT.ExitTrigger();

        for (float f = 0f; f < 2.1f; f += (1 / 60f))
            yield return null;

        foreach (LowPolyTrigger lpt in ps.lowPolyTriggers)
            lpt.currentlyInside = false;
            
        while (ps.atmosphereManager.triggerCount > 0)
            ps.atmosphereManager.ExitTrigger();
        ps.atmosphereManager.curLevel = "";
            
        Time.timeScale = 1f;
        ps.cam.disableControl = false;
        yield return null;
        
        Vector3 newPos = Vector3.zero;
        foreach (GameObject g in GameObject.FindGameObjectsWithTag("WholeLevel"))
        {
            if(g.name == "Challenges Parent")
            {
                foreach (Transform t in g.GetComponentsInChildren<Transform>(true))
                {
                    if (t.parent == g.transform && t.name == portalID.ToString())
                    {
                        t.gameObject.SetActive(true);
                        exitPortal = t.GetChild(0).GetChild(0).GetComponent<ChallengePortal>();
                        newPos = t.GetChild(0).GetChild(0).position + Vector3.up;
                    }
                }
            }
        }
        foreach (GameObject g in GameObject.FindGameObjectsWithTag("WholeLevel"))
        {
            if (g.name == "External Full" && g.activeInHierarchy)
            {
                fullEXT = g;
                g.SetActive(false);
                while (g.activeInHierarchy)
                    yield return null;
            }
        }

        tpc.shadowProjector.orthographicSize = 1.72f;

        tpc.disableControl = false;
        yield return null;
        tpc.gameObject.transform.position = newPos;
        yield return null;
        tpc.rb.velocity = Vector3.zero;
        tpc.disableControl = true;
        yield return new WaitForSeconds(2f);

        for (int f = 59; f >= 0; f--)
        {
            AudioListener.volume = Mathf.Lerp(1f, 0f, (f * 1f) / 60f);
            ps.loadFS.color = Color.Lerp(Color.clear, Color.black, (f * 1f) / 60f);
            yield return null;
        }
        ps.loadAnim.SetBool("Loading", false);
        ps.PositionMultiCharacters();

        while (tpc.beingReset)
            yield return null;
        tpc.disableControl = false;
        foreach (TPC mtpc in ps.multiTPC)
            mtpc.disableControl = false;
        tpc.rb.velocity = Vector3.zero;
        ps.cantPause = false;

        tpc.challengePortal = exitPortal;

        foreach (AudioMixer am in ps.audioMixers)
            am.SetFloat("musicVol", -80f + (PlayerPrefs.GetFloat("musicVolume", 8f) * 10f));
        
        warping = false;
        inTrigger = false;
        
        exitPortal.returnPortal = this;

        tpc.challengeWarping = false;
    }
    
    public void WarpFromChallenge()
    {
        StartCoroutine(WarpFromChallengeCoRo());
    }
    
    IEnumerator WarpFromChallengeCoRo()
    {
        ps.cantPause = true;
        ps.loadAnim.SetBool("Loading", true);
        ps.loadIcon.fillAmount = 0f;

        if (tpc != null && tpc.GetComponentInParent<OneWayManager>() != null)
            tpc.GetComponentInParent<OneWayManager>().currentlyChecking = true;


        foreach (TPC.DissolveInfo dissolve in tpc.dissolves)
        {
            if (dissolve.renderer.gameObject.activeSelf)
            {
                Material[] dissolveMatInsts = dissolve.dissolveMaterials;
                dissolve.renderer.sharedMaterials = dissolveMatInsts;
            }
        }
        tpc.ChallengePortalWarpPFX();
        Vector3 origCameraPos = tpc.cam.transform.position;
        Quaternion origCameraRot = tpc.cam.transform.rotation;
        for (float f = 0f; f <= 1.2f; f += (Time.deltaTime / 2f))
        {
            foreach (TPC.DissolveInfo dissolve in tpc.dissolves)
            {
                if (dissolve.renderer.gameObject.activeSelf)
                {
                    for (int r = 0; r < dissolve.origMaterials.Length; r++)
                    {
                        if(dissolve.renderer.sharedMaterials[r].HasProperty("_DissolveAmount"))
                           dissolve.renderer.sharedMaterials[r].SetFloat("_DissolveAmount", Mathf.Clamp01(f));
                    }
                    if (Mathf.Clamp01(f) == 1f)
                        dissolve.renderer.enabled = false;
                }
            }
            tpc.cam.transform.position = Vector3.Lerp(tpc.cam.transform.position, tpc.transform.position + (tpc.transform.forward * 3f) + Vector3.up, Mathf.Clamp01(f));
            tpc.cam.transform.LookAt(tpc.transform.position);
            tpc.shadowProjector.orthographicSize = Mathf.Lerp(1.72f, 20f, Mathf.Clamp01(f));
            yield return null;
        }

        Time.timeScale = 0f;

        for (int f = 1; f <= 60; f++)
        {
            AudioListener.volume = Mathf.Lerp(1f, 0f, (f * 1f) / 60f);
            ps.loadFS.color = Color.Lerp(Color.clear, Color.black, (f * 1f) / 60f);
            yield return null;
        }

        tpc.cam.transform.position = origCameraPos;
        tpc.cam.transform.rotation = origCameraRot;

        foreach (TPC.DissolveInfo dissolve in tpc.dissolves)
        {
            if (dissolve.renderer.gameObject.activeSelf)
            {
                for (int r = 0; r < dissolve.origMaterials.Length; r++)
                {
                    dissolve.renderer.enabled = true;
                    Material[] origMatInsts = dissolve.origMaterials;
                    dissolve.renderer.sharedMaterials = origMatInsts;
                }
            }
        }

        AsyncOperation async = new AsyncOperation();
        async = SceneManager.UnloadSceneAsync(exitPortal.gameObject.scene.name);

        while (!async.isDone)
        {
            ps.loadIcon.fillAmount = Mathf.Lerp(ps.loadIcon.fillAmount, async.progress, 0.8f);
            yield return null;
        }


        if (tpc != null && tpc.GetComponentInParent<OneWayManager>() != null)
            tpc.gameObject.GetComponentInParent<OneWayManager>().CheckOneWays();

        if (ps.curLPT != null)
            ps.curLPT.EnterTrigger();

        for (float f = 0f; f < 2.1f; f += (1 / 60f))
            yield return null;

        foreach (LowPolyTrigger lpt in ps.lowPolyTriggers)
            lpt.currentlyInside = false;

        while (ps.atmosphereManager.triggerCount > 0)
            ps.atmosphereManager.ExitTrigger();
        ps.atmosphereManager.curLevel = "";

        Time.timeScale = 1f;
        ps.cam.disableControl = false;
        yield return null;

        Vector3 newPos = this.transform.position + Vector3.up;
        if (fullEXT != null) {
            fullEXT.SetActive(true);
            while (!fullEXT.activeInHierarchy)
                yield return null;
        }
        //     exitPortal.transform.parent.parent.gameObject.SetActive(false);

        tpc.shadowProjector.orthographicSize = 1.72f;
        tpc.disableControl = false;
        yield return null;
        tpc.gameObject.transform.position = newPos;
        yield return null;
        tpc.disableControl = true;
        tpc.rb.velocity = Vector3.zero;
        tpc.gameObject.transform.position = newPos;
        yield return new WaitForSeconds(4f);

        for (int f = 59; f >= 0; f--)
        {
            tpc.gameObject.transform.position = newPos;
            AudioListener.volume = Mathf.Lerp(1f, 0f, (f * 1f) / 60f);
            ps.loadFS.color = Color.Lerp(Color.clear, Color.black, (f * 1f) / 60f);
            yield return null;
        }
        ps.PositionMultiCharacters();
        ps.loadAnim.SetBool("Loading", false);

        while (tpc.beingReset)
            yield return null;
        tpc.disableControl = false;
        foreach (TPC mtpc in ps.multiTPC)
            mtpc.disableControl = false;
        tpc.rb.velocity = Vector3.zero;
        ps.cantPause = false;

        tpc.challengePortal = null;

        foreach (AudioMixer am in ps.audioMixers)
            am.SetFloat("musicVol", -80f + (PlayerPrefs.GetFloat("musicVolume", 8f) * 10f));
        
        warping = false;
        inTrigger = false;
        tpc.challengeWarping = false;
    }


    public IEnumerator ResetCharacter(TPC character)
    {
        if (character == PlayerManager.GetMainPlayer())
        {
            if (ps == null)
                ps = character.ps;

            character.disableControl = true;
            character.ExitRiverForce();
            character.ExitWaterFS();
            character.inWindCol = false;

            foreach (TPC mtpc in ps.multiTPC)
            {
                mtpc.disableControl = true;
                mtpc.ExitRiverForce();
                mtpc.ExitWaterFS();
            }
            ps.cam.disableControl = true;

            ps.cantPause = true;

            for (int f = 1; f <= 60; f++)
            {
                AudioListener.volume = Mathf.Lerp(1f, 0f, (f * 1f) / 60f);
                ps.loadFS.color = Color.Lerp(Color.clear, Color.white, (f * 1f) / 60f);
                yield return null;
            }

            character.transform.position = this.transform.position + Vector3.up;
            character.anim.enabled = true;
            character.anim.SetBool("damaged", false);
            character.anim.Play("Idle", 0);
            character.onGround = true;
            character.rb.isKinematic = true;
            ps.PositionMultiCharacters();

            ps.cam.disableControl = false;

            yield return new WaitForSeconds(2f);

            for (int f = 59; f >= 0; f--)
            {
                AudioListener.volume = Mathf.Lerp(1f, 0f, (f * 1f) / 60f);
                ps.loadFS.color = Color.Lerp(Color.clear, Color.white, (f * 1f) / 60f);
                yield return null;
            }

            character.capcol.enabled = true;
            character.rb.isKinematic = false;
            character.disableControl = false;
            foreach (TPC mtpc in ps.multiTPC)
                mtpc.disableControl = false;
            character.rb.velocity = Vector3.zero;
            ps.cantPause = false;
        }
        else
            StartCoroutine(character.RespawnCharacterWait());
    }
}
