using UnityEngine;
using System.Collections;

public class Checkpoint : MonoBehaviour {

	public int checkpointID;

    //bool activatedcheckpoint;
    public GameObject checkpointposition;

    //
    public bool activateTutorialTrophy, activateIcyPeakTrophy;
    bool tutorialTrophyActivated, icyPeakTrophyActivated;

    float positionx;
    float positiony;
    float positionz;

	GameObject character;
    
    PauseScreen ps;
    Animator animator;

    //public TPC scriptcamera;


    void Start(){
		if (checkpointID == 0 && !activateIcyPeakTrophy)
			ActivateCheckpoint (false);

        if (character == null)
            character = PlayerManager.GetMainPlayer().gameObject;

        animator = GetComponent<Animator>();

        //    activatedcheckpoint = false;
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == character)
        {
         //   if (!activatedcheckpoint)
                EnterCheckpoint();
        }
    }

    /*
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == character)
        {
            if (activatedcheckpoint)
                activatedcheckpoint = false;
        }
    }*/

    void EnterCheckpoint()
    {
        //
        if (activateIcyPeakTrophy)
        {
            if (!icyPeakTrophyActivated)
            {
                //
                icyPeakTrophyActivated = true;
                //
#if UNITY_PS4
                //
                // check icy peak trophy
                //
                PS4Manager.ps4TrophyManager.UnlockTrophy((int)PS4_TROPHIES.TOP_ICY_MOUNTAIN);
#endif

#if UNITY_XBOXONE
                //
                icyPeakTrophyActivated = true;
                PlayerPrefs.SetInt("Top", 1);
                XONEAchievements.SubmitAchievement((int)XONEACHIEVS.TO_THE_TOP);
#endif
            }
            //
            return;
        }

        //
        if (activateTutorialTrophy && !tutorialTrophyActivated)
        {
            //
            tutorialTrophyActivated = true;
#if UNITY_PS4
            //
            // check tutorial trophy
            PS4Manager.ps4TrophyManager.UnlockTrophy((int)PS4_TROPHIES.COMPLETE_TUTORIAL);
#endif

#if UNITY_XBOXONE
            //
            // check trophy musicians if all cages are 1 countedPrefs = 4
            tutorialTrophyActivated = true;
            XONEAchievements.SubmitAchievement((int)XONEACHIEVS.THE_BASICS);
#endif
        }

        //    activatedcheckpoint = true;
        if (PlayerPrefs.GetInt("FirstCheckpoint", 0) == 0)
        {
            PlayerPrefs.SetInt("FirstCheckpoint", 1);
            GameObject.FindWithTag("Pause").transform.Find("Event Text").gameObject.GetComponent<TextTriggerMain>().SetText(2);

        }

        if (ps == null)
            ps = GameObject.FindWithTag("Pause").GetComponent<PauseScreen>();
        if (ps != null)
        {
            ps.checkpointAnim.ResetTrigger("checkpoint");
            ps.checkpointAnim.SetTrigger("checkpoint");
        }
        animator.SetBool("Activated", true);

        PlayerPrefs.SetInt("LastCheckpoint", checkpointID);
        ActivateCheckpoint(true);
    }

	public void ActivateCheckpoint(bool saveThis)
    {
        positionx = checkpointposition.transform.position.x;
		positiony = this.transform.position.y + 1f;
		positionz = checkpointposition.transform.position.z;

		PlayerPrefs.SetInt ("Checkpoint" + checkpointID.ToString (), 1);
		PlayerPrefs.SetInt ("Checkpoint" + checkpointID.ToString () + "Scene", this.gameObject.scene.buildIndex);
		PlayerPrefs.SetFloat ("Checkpoint" + checkpointID.ToString () + "X", positionx);
		PlayerPrefs.SetFloat ("Checkpoint" + checkpointID.ToString () + "Y", positiony);
		PlayerPrefs.SetFloat ("Checkpoint" + checkpointID.ToString () + "Z", positionz);

        //
        PlayerPrefs.Save();
        //
#if UNITY_XBOXONE
        if (saveThis)
            DataManager.xOneEventsManager.SaveProgs();
#endif

    }
}
