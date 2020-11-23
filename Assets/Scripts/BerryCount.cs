using UnityEngine;
using System.Collections;
//using Steamworks;

public class BerryCount : MonoBehaviour {

	int berries;
	TextMesh txt;

	void Start(){
		txt = this.GetComponent<TextMesh> ();
		berries = PlayerPrefs.GetInt ("Berries", 0);
		txt.text = berries.ToString ();
	}

	public void UpdateCount(){
		if (berries != PlayerPrefs.GetInt ("Berries", 0)) {
			berries = PlayerPrefs.GetInt("Berries");
			txt.text = berries.ToString ();
		}
	}
    
}
