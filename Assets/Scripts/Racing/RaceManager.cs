using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class RaceManager : MonoBehaviour
{
    public bool showDebugLogs;
    TPC tpc;

    public List<RaceWaypoint> raceWaypoints = new List<RaceWaypoint>();
    public Material wayPointDefaultMaterial;
    public Material wayPointNextMaterial;
    public Material wayPointClearedMaterial;

    int waypointCounter = 0;

    [HideInInspector] public bool raceStarted = false;
    bool inRaceTrigger;

    [System.Serializable] public class Racer { public NavMeshAgent agent; public Animator racerAnim; [HideInInspector] public int counter = 0; [HideInInspector] public bool traversingLink;
        public float jumpSpeed = 0.04f; [HideInInspector] public int racerID; public GameObject modelParent; public Vector3 modelOffset; [HideInInspector] public RaycastHit groundHit = new RaycastHit();
        [HideInInspector] public Vector3 origModelLocalPos;
        [HideInInspector] public float currentModelYPos; public GameObject startingPosition; [HideInInspector] public Vector3 preRacePosition; [HideInInspector] public Vector3 preRaceAngles;
        public AudioClip[] jumpSounds; [HideInInspector] public AudioSource[] sources; [HideInInspector] public int sourceSelection;
    }
    public Racer[] racers;

    List<int> winners = new List<int>();

    public LayerMask whatIsGround;

    public GameObject woodleStartingPos;
    public GameObject npcDialogueParent;
    
    [System.Serializable] public class RaceReward { public Sprite icon; public int rewardAmount; }
    public RaceReward[] raceRewards;

    Vector3 woodlePreRacePosition;

    public GameObject waypointArrow;
    Vector3 curArrowRot;
    Vector3 nextArrowRot;

    bool quitRace;

    public AudioClip raceAcceptedSound;
    public AudioClip raceStartSound;
    public AudioClip raceFinishSound;
    public AudioClip raceQuitSound;

    public AudioSource audioSourceForSounds;

    public AudioClip raceMusic;
    public AudioClip resultsMusic;

    public AudioSource audioSourceForMusic;
    float musicASVol;

    public AudioClip waypointSound;
    public AudioSource audioSourceForWaypoints;

    void Start()
    {
        foreach (RaceWaypoint rw in raceWaypoints)
            rw.gameObject.SetActive(false);

        foreach (Racer r in racers)
        {
            r.preRacePosition = r.agent.transform.position;
            r.preRaceAngles = r.agent.transform.localEulerAngles;

            if(r.agent.GetComponents<AudioSource>() != null)
                r.sources = r.agent.GetComponents<AudioSource>();

            r.origModelLocalPos = r.modelParent.transform.localPosition;
        }

        tpc = PlayerManager.GetMainPlayer();
        musicASVol = audioSourceForMusic.volume;

        audioSourceForWaypoints.clip = waypointSound;

        waypointArrow.SetActive(false);
    }

    private void Update()
    {
        if (inRaceTrigger)
        {
            if (!tpc.ps.inPause)
            {
                if(tpc.onGround && tpc.input.GetButtonDown("Leaf"))
                {
                    PlaySound(audioSourceForSounds, raceAcceptedSound);
                    ExitRaceTrigger();
                    RaceStart();
                }
            }
        }
        if (raceStarted) {
            /*
            if (winners.Count == racers.Length + 1)
            {
                raceStarted = false;
                FinishRace();
            }*/
            if (waypointArrow.activeSelf)
            {
                waypointArrow.transform.position = tpc.transform.position + (Vector3.up);
                curArrowRot = waypointArrow.transform.forward;
                waypointArrow.transform.LookAt(raceWaypoints[waypointCounter].transform.position);
                nextArrowRot = waypointArrow.transform.forward;
                waypointArrow.transform.forward = Vector3.Lerp(curArrowRot, nextArrowRot, Time.deltaTime * 5f);
            }
            if(!quitRace && !tpc.disableControl && tpc.input.GetButton("Start") && tpc.input.GetButton("Items"))
            {
                quitRace = true;
                raceStarted = false;
                FinishRace();
            }
        }
    }

    void PlaySound(AudioSource audioSource, AudioClip audioClip)
    {
        audioSource.Stop();
        audioSource.clip = audioClip;
        audioSource.PlayDelayed(0f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!raceStarted && !inRaceTrigger && other.gameObject == tpc.gameObject && !tpc.disableControl)
        {
            inRaceTrigger = true;
            if(!npcDialogueParent.activeSelf)
                npcDialogueParent.SetActive(true);
            tpc.ps.racePromptAnim.SetBool("skipOn", true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (inRaceTrigger && other.gameObject == PlayerManager.GetMainPlayer().gameObject)
            ExitRaceTrigger();
    }

    void ExitRaceTrigger()
    {
        inRaceTrigger = false;
        tpc.ps.racePromptAnim.SetBool("skipOn", false);
    }

    void RaceStart()
    {
        tpc.ps.cantPause = true;

        tpc.disableControl = true;
        tpc.rb.isKinematic = true;
        tpc.rb.velocity = Vector3.zero;
        tpc.anim.SetBool("inLocomotion", false);
        tpc.anim.Play("Idle", 0);
        woodlePreRacePosition = tpc.transform.position;

        raceStarted = true;
        winners.Clear();

        npcDialogueParent.SetActive(false);

        StartCoroutine(SetUpRace());
    }

    void FinishRace()
    {
        StartCoroutine(ShowResults());
    }
    
    public void EnteredWayPoint(RaceWaypoint waypoint)
    {
        if (raceWaypoints.IndexOf(waypoint) == waypointCounter)
        {
            if (showDebugLogs)
                Debug.Log("WAYPOINT " + waypointCounter);

            waypoint.entered = true;

            waypoint.ChangeMaterial(wayPointClearedMaterial);
            if (waypointCounter < raceWaypoints.Count - 1)
                raceWaypoints[waypointCounter + 1].ChangeMaterial(wayPointNextMaterial);

            waypointCounter++;

            if (waypointCounter != raceWaypoints.Count)
                audioSourceForWaypoints.PlayDelayed(0f);
        }
        else
        {
            if (showDebugLogs)
                Debug.Log("WRONG WAYPOINT");
        }

        if (waypointCounter == raceWaypoints.Count)
        {
            winners.Add(0);
            raceStarted = false;
            FinishRace();
        }
    }

    IEnumerator SetUpRace()
    {
        tpc.ps.ToggleWhiteFade(true);
        yield return new WaitForSeconds(1.1f);
        
        for (int identifier = 0; identifier < raceWaypoints.Count; identifier++)
        {
            raceWaypoints[identifier].waypointID = identifier;
            raceWaypoints[identifier].gameObject.SetActive(true);
            raceWaypoints[identifier].entered = false;
            if (identifier < raceWaypoints.Count - 1)
            {
                Vector3 nextWPPos = raceWaypoints[identifier + 1].gameObject.transform.position;
                raceWaypoints[identifier].gameObject.transform.LookAt(new Vector3(nextWPPos.x, raceWaypoints[identifier].gameObject.transform.position.y, nextWPPos.z));
            }
            if (identifier == 0)
                raceWaypoints[identifier].ChangeMaterial(wayPointNextMaterial);
            else
                raceWaypoints[identifier].ChangeMaterial(wayPointDefaultMaterial);
        }

        tpc.transform.position = woodleStartingPos.transform.position;

        waypointCounter = 0;
        int iterator = 1;
        foreach (Racer racer in racers)
        {
            racer.counter = 0;
            racer.racerID = iterator;
            racer.agent.transform.position = racer.startingPosition.transform.position;
            racer.agent.transform.rotation = racer.startingPosition.transform.rotation;
            racer.currentModelYPos = racer.modelParent.transform.position.y;
            iterator++;
        }

        tpc.ps.ToggleWhiteFade(false);
        tpc.ps.raceCountdownAnim.SetTrigger("beginCountdown");
        PlaySound(audioSourceForSounds, raceStartSound);

        waypointArrow.SetActive(true);

        yield return new WaitForSeconds(4f);

        PlaySound(audioSourceForMusic, raceMusic);

        tpc.disableControl = false;
        tpc.rb.isKinematic = false;

        foreach (Racer racer in racers)
            StartCoroutine(RacerMovement(racer));


        tpc.ps.raceQuitAnim.SetBool("skipOn", true);
        yield return new WaitForSeconds(4f);
        tpc.ps.raceQuitAnim.SetBool("skipOn", false);
    }

    IEnumerator RacerMovement(Racer racer)
    {
        racer.agent.SetDestination(raceWaypoints[racer.counter].transform.position);

        while(racer.agent.pathPending)
            yield return null;

        while ((racer.agent.remainingDistance > 1f) && !quitRace)
        {
            if (!racer.traversingLink)
            {
                if (racer.racerAnim.GetBool("inLocomotion") == false)
                    racer.racerAnim.SetBool("inLocomotion", true);

                racer.racerAnim.SetFloat("Speed",  Mathf.Lerp(racer.racerAnim.GetFloat("Speed"), racer.agent.velocity.sqrMagnitude, Time.deltaTime * 5f));

                if (Physics.Raycast(racer.agent.transform.position + (Vector3.up * 0.5f), -Vector3.up, out racer.groundHit, 2f, whatIsGround))
                {
                    Vector3 modelPos = racer.groundHit.point + racer.modelOffset;
                    racer.modelParent.transform.position = new Vector3(modelPos.x, Mathf.Lerp(racer.currentModelYPos, modelPos.y, Time.deltaTime * 5f), modelPos.z);
                    racer.currentModelYPos = racer.modelParent.transform.position.y;
                }

                if (racer.agent.isOnOffMeshLink)
                    StartCoroutine(TraverseLink(racer));
            }

            yield return null;
        }

        racer.counter++;

        if ((racer.counter < raceWaypoints.Count) && !quitRace)
            StartCoroutine(RacerMovement(racer));
        else
        {
            racer.racerAnim.SetTrigger("celebrate");
            racer.racerAnim.SetBool("inLocomotion", false);
            racer.agent.stoppingDistance = 1f;
            winners.Add(racer.racerID);
        }
    }

    IEnumerator TraverseLink(Racer racer)
    {
        racer.traversingLink = true;
        if (racer.agent.currentOffMeshLinkData.activated)
        {
            racer.agent.ActivateCurrentOffMeshLink(false);
            racer.racerAnim.SetTrigger("jumpNow");

            if (racer.sources.Length > 0 && racer.jumpSounds.Length > 0)
            {
                AudioSource auSo = racer.sources[racer.sourceSelection];
                auSo.Stop();
                auSo.clip = racer.jumpSounds[Random.Range(0, racer.jumpSounds.Length)];
                auSo.PlayDelayed(0f);

                racer.sourceSelection++;
                if (racer.sourceSelection >= racer.sources.Length)
                    racer.sourceSelection = 0;
            }

            Vector3 startPos = racer.agent.currentOffMeshLinkData.startPos;
            Vector3 endPos = racer.agent.currentOffMeshLinkData.endPos;

            racer.agent.transform.LookAt(new Vector3(endPos.x, racer.agent.transform.position.y, endPos.z));
            racer.modelParent.transform.localPosition = racer.origModelLocalPos;

            float jumpHeight = Mathf.Clamp(((endPos.y - startPos.y) * 1.5f), 1.5f, 5f);

            for (float iterator = 0f; (iterator <= 1f && !tpc.disableControl); iterator += racer.jumpSpeed)
            {
                Vector3 currentPos = Vector3.Lerp(startPos, endPos, iterator);
                currentPos.y += Mathf.Sin(Mathf.Clamp01(iterator) * Mathf.PI);
                racer.agent.transform.position = currentPos;

                yield return null;
            }

            racer.agent.transform.position = endPos;

            racer.agent.ActivateCurrentOffMeshLink(true);

            racer.agent.CompleteOffMeshLink();
            racer.racerAnim.SetTrigger("runningLand");
        }
        racer.traversingLink = false;
    }

    IEnumerator ShowResults()
    {
        if(!quitRace)
            PlaySound(audioSourceForSounds, raceFinishSound);
        else
            PlaySound(audioSourceForSounds, raceQuitSound);

        tpc.disableControl = true;
        tpc.rb.isKinematic = true;
        tpc.rb.velocity = Vector3.zero;
        tpc.ps.ToggleWhiteFade(true);

        for(float timer = 0f; timer < 1.1f; timer += Time.deltaTime)
        {
            audioSourceForMusic.volume = Mathf.Lerp(musicASVol, 0f, timer);
            yield return null;
        }

    //    yield return new WaitForSeconds(1.1f);

        tpc.transform.position = woodlePreRacePosition;
        tpc.onGround = true;
        tpc.anim.SetBool("inLocomotion", false);
        tpc.anim.Play("Idle", 0);

        waypointArrow.SetActive(false);

        foreach (Racer r in racers)
        {
            r.agent.Warp(r.preRacePosition);
            r.agent.transform.localEulerAngles = r.preRaceAngles;
        }
        foreach (RaceWaypoint rw in raceWaypoints)
            rw.gameObject.SetActive(false);

        yield return new WaitForSeconds(1.1f);
        tpc.ps.ToggleWhiteFade(false);

        if (!quitRace)
        {
            int endPosition = 99;
            for (int pos = 0; pos < winners.Count; pos++)
            {
                if (winners[pos] == 0)
                    endPosition = pos;
            }
            if (endPosition >= raceRewards.Length)
                endPosition = raceRewards.Length - 1;

            tpc.ps.raceResultsIcon.sprite = raceRewards[endPosition].icon;
            tpc.ps.raceRewardText.text = raceRewards[endPosition].rewardAmount.ToString();
            tpc.ps.raceResultsAnim.SetBool("showResults", true);
            audioSourceForMusic.volume = musicASVol;
            PlaySound(audioSourceForMusic, resultsMusic);
            yield return new WaitForSeconds(1f);
            StartCoroutine(BerryReward(raceRewards[endPosition].rewardAmount));

            while (!tpc.input.GetButtonDown("Jump"))
                yield return null;


            tpc.ps.raceResultsAnim.SetBool("showResults", false);
        }
        else
            quitRace = false;

        tpc.ps.cantPause = false;
        tpc.disableControl = false;
        tpc.rb.isKinematic = false;
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
}
