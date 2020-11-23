using UnityEngine;
using System.Collections;
//using Steamworks;

public class DarkBossDefeated : MonoBehaviour {


    public GameObject darkboss;
    public GameObject darkbossdefeated;
    public GameObject finalsequence;

    public GameObject soundtrackstopped;

    public GameObject redball1;
    public GameObject redball2;
    public GameObject redball3;
    RedBallToHIt red1;
    RedBallToHIt red2;
    RedBallToHIt red3;
    bool finished;

    public bool debuggingdefeat;
    public int bosshits;
    public bool hitexit1;
    public bool hitexit2;
    public bool hitexit3;

    public GameObject waterfall1;
    public GameObject waterfallcollider1;
    public GameObject waterfallparticle1;

    public GameObject waterfall2;
    public GameObject waterfallcollider2;
    public GameObject waterfallparticle2;

    public GameObject waterfall3;
    public GameObject waterfallcollider3;
    public GameObject waterfallparticle3;
    TPC tpc;

    // Update is called once per frame
    void Update() {
        if (red1 == null || red2 == null || red3 == null)
        {
            red1 = redball1.GetComponent<RedBallToHIt>();
            red2 = redball2.GetComponent<RedBallToHIt>();
            red3 = redball3.GetComponent<RedBallToHIt>();
        }
        else
        {
            if (!finished && red1.hit && red2.hit && red3.hit)
            {

                StartCoroutine(KilledFinalBoss());

            }

            if (debuggingdefeat)
            {

                StartCoroutine(KilledFinalBoss());

            }
        }

        //calculate number of hits

        if (red1.hit && hitexit1 == false)
        {

            bosshits++;
            hitexit1 = true;
            RemoveWaterfall();
        }



        if (red2.hit && hitexit2 == false)
        {

            bosshits++;
            hitexit2 = true;
            RemoveWaterfall();
        }


        if (red3.hit && hitexit3 == false)
        {

            bosshits++;
            hitexit3 = true;
            RemoveWaterfall();
        }
    }

    bool finishgameAchieved;
    void RemoveWaterfall()
    {
        if (tpc == null)
            tpc = PlayerManager.GetMainPlayer();

        if (tpc.currentWaterfall != null)
        {
            if(tpc.currentWaterfall.gameObject == waterfallcollider1)
            {
                waterfall1.SetActive(false);
                waterfallcollider1.SetActive(false);
                waterfallparticle1.SetActive(false);
            }
            if (tpc.currentWaterfall.gameObject == waterfallcollider2)
            {
                waterfall2.SetActive(false);
                waterfallcollider2.SetActive(false);
                waterfallparticle2.SetActive(false);
            }
            if (tpc.currentWaterfall.gameObject == waterfallcollider3)
            {
                waterfall3.SetActive(false);
                waterfallcollider3.SetActive(false);
                waterfallparticle3.SetActive(false);
            }
        }
    }

    IEnumerator KilledFinalBoss()
    {
        PlayerPrefs.SetInt("FinalBossDefeated", 1);

#if UNITY_PS4
        //
        // check trophy
        if (!finishgameAchieved)
        {
            finishgameAchieved = true;
            PS4Manager.ps4TrophyManager.UnlockTrophy((int)PS4_TROPHIES.FINISH_THE_GAME);
        }       
#endif

#if UNITY_XBOXONE
        //
        // check trophy : items >= 3 and items = all 
        //
        if (!finishgameAchieved)
        {
            finishgameAchieved = true;
            XONEAchievements.SubmitAchievement((int)XONEACHIEVS.WOODLE_SAVIOR);
        }        
#endif

        if (tpc == null)
            tpc = PlayerManager.GetMainPlayer();

        tpc.disableControl = true;
        tpc.rb.velocity = Vector3.zero;
        tpc.rb.isKinematic = true;
        yield return null;
        tpc.anim.SetFloat("Speed", 0f);
        if(tpc.onGround)
            tpc.anim.Play("Idle", 0);

        yield return new WaitForSeconds(3f);


        finished = true;
        PlayerManager.GetMainPlayer().cam.SetActive(false);
        darkboss.gameObject.SetActive(false);
        GameObject.FindWithTag("Pause").GetComponent<PauseScreen>().cantPause = true;
        finalsequence.gameObject.SetActive(true);
        darkbossdefeated.gameObject.SetActive(true);
        //StartCoroutine(FinalSequenceStart());

        soundtrackstopped.gameObject.SetActive(false);
        this.enabled = false;


        yield return new WaitForSeconds(5.0f);

       
        WaterTearManager.FinalCutscene();


        yield return new WaitForSeconds(0.1f);
        finalsequence.gameObject.SetActive(false);
        GameObject.FindWithTag("Pause").GetComponent<PauseScreen>().cantPause = false;



    }

    /*
    IEnumerator FinalSequenceStart()
    {
        yield return new WaitForSeconds(3.0f);
        WaterTearManager.FinalCutscene();
    //    Finalsequenceoutside.gameObject.SetActive(true);
    }
    */
}
