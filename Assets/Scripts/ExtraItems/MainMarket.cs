using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMarket : MonoBehaviour {
    
	public ActivateItemsMasks[] allAddons;

	[HideInInspector] public int currentMaskID;
	[HideInInspector] public int currentItemID;
	[HideInInspector] public int currentHatID;

	void Start(){
		if (!PlayerPrefs.HasKey ("MaskID")) {
			PlayerPrefs.SetInt ("MaskID", -1);
			PlayerPrefs.SetInt ("ItemID", -1);
			PlayerPrefs.SetInt ("HatID", -1);
		} else {
			currentMaskID = PlayerPrefs.GetInt ("MaskID", 0);
			currentItemID = PlayerPrefs.GetInt ("ItemID", 0);
			currentHatID = PlayerPrefs.GetInt ("HatID", 0);
        }
    //    PlayerPrefs.Save();
    }

	public void ReturnCurrentAddon(bool maskOn, bool itemOn, bool hatOn){
		if (maskOn) {
			CheckAddons (currentMaskID);
			PlayerPrefs.SetInt ("MaskID", -1);
		}
		if (itemOn) {
			CheckAddons (currentItemID);
			PlayerPrefs.SetInt ("ItemID", -1);
		}
		if (hatOn) {
			CheckAddons (currentHatID);
			PlayerPrefs.SetInt ("HatID", -1);
        //    PlayerPrefs.Save();
        }
	}

	void CheckAddons(int toCheck){
		foreach (ActivateItemsMasks a in allAddons) {
			if (a.itemID == toCheck)
				a.UntouchThis ();
		}
	}

	public void SetCurrentID(int id, int type){
		if (type == 0) {
			PlayerPrefs.SetInt ("MaskID", id);
			currentMaskID = id;
		}
		if (type == 1) {
			PlayerPrefs.SetInt ("ItemID", id);
			currentItemID = id;
		}
		if (type == 2) {
			PlayerPrefs.SetInt ("HatID", id);
			currentHatID = id;
        //    PlayerPrefs.Save();
        }
	}
}
