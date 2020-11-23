using UnityEngine;
using System.Collections;

public class WaterAltairActivate : MonoBehaviour {

    public int levelID;

    public GameObject watertear1;
    public GameObject watertear2;
    public GameObject watertear3;

    public GameObject wateractive1;
    public GameObject wateractive2;

    public GameObject watermain1;
    public GameObject watermain2;

    public GameObject treesage;
    public GameObject treesagegrey;

    PauseScreen ps;

    [HideInInspector] public bool levelcomplete;
    
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (ps == null)
        {
            ps = PlayerManager.GetMainPlayer().ps;
            ps.CheckTears();
        }

		if (!levelcomplete) {
            if ((watertear1.activeSelf && watertear2.activeSelf && watertear3.activeSelf) || PlayerPrefs.GetInt("IntroWatched", 0) == 0)
            {
                levelcomplete = true;
                if (PlayerPrefs.GetInt("IntroWatched", 0) == 1)
                {
                    if (PlayerPrefs.GetInt("Played" + levelID.ToString() + "TreeCompletion", 0) == 0)
                    {
                        PlayerPrefs.SetInt("Played" + levelID.ToString() + "TreeCompletion", 1);

                        ps.CheckTears();
                        
                        if (ps.tearCount == 24)
                        {
                            WaterTearManager.AllTearsCutscene();
                            //    GameObject.FindWithTag("Pause").transform.Find("Event Text").gameObject.GetComponent<TextTriggerMain>().SetText(5);
#if UNITY_PS4
                            //
                            // check trophy
                            PS4Manager.ps4TrophyManager.UnlockTrophy((int)PS4_TROPHIES.FIND_ALL_WATER_TEARS);
#endif

#if UNITY_XBOXONE
                            //
                            // check trophy : items >= 3 and items = all 
                            //
                            XONEAchievements.SubmitAchievement((int)XONEACHIEVS.ALL_TREE_SAGE_RESTORED);
#endif
                        }
                        else
                        {
                            if(levelID != 3 && ps.tearCount > 3)
                                GameObject.FindWithTag("Pause").transform.Find("Event Text").gameObject.GetComponent<TextTriggerMain>().SetText(4);
                        }
                    }
                }
            }
		} else {
			if (PlayerPrefs.GetInt("IntroWatched", 0) == 1 && (!watertear1.activeSelf || !watertear2.activeSelf || !watertear3.activeSelf))
				levelcomplete = false;
		}

		if (levelcomplete) { 
			if (treesage != null) {
				treesage.SetActive (true);
				treesagegrey.SetActive (false);
			}

            if (!wateractive1.activeInHierarchy)
                wateractive1.SetActive(true);

            if (!wateractive2.activeInHierarchy)
            {
                if (ps.tearCount == 24 || PlayerPrefs.GetInt("IntroWatched", 0) == 0)
                {
                    wateractive2.SetActive(true);

                    if (watermain1 != null)
                    {
                        watermain1.SetActive(true);
                        watermain2.SetActive(true);
                    }
                }

            }
		} else
        {
            if (treesage != null)
            {
                treesage.SetActive(false);
                treesagegrey.SetActive(true);
            }

            if (wateractive1.activeInHierarchy)
                wateractive1.SetActive(false);

            if (wateractive2.activeInHierarchy)
            {
                if (ps.tearCount != 24 && PlayerPrefs.GetInt("IntroWatched", 0) == 1)
                {
                    wateractive2.SetActive(false);

                    if(watermain1 != null)
                    {
                        watermain1.SetActive(false);
                        watermain2.SetActive(false);
                    }
                }
            }
		}
    }
}
