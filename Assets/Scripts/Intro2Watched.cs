using UnityEngine;
using System.Collections;
using CinemaDirector;
using UnityEngine.Playables;

public class Intro2Watched : MonoBehaviour {

    public PauseScreen pauseScreen;
	public PlayableDirector cutsceneToPlay;

	public GameObject objectposition;

	public GameObject[] objsToDeactivate;
	public GameObject musicToActivate;
    
	TPC tpc;

	void Start(){
        tpc = PlayerManager.GetMainPlayer();
	}

	void OnTriggerEnter (Collider col){
		if (col.gameObject == tpc.gameObject && PlayerPrefs.GetInt ("Intro2Watched", 0) == 0) {
			PlayerPrefs.SetInt ("Intro2Watched", 1);
        //    PlayerPrefs.Save();

            pauseScreen.StartCutscene(cutsceneToPlay, objectposition.transform.position);
		}
	}
}
