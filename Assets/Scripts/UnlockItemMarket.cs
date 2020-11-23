using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UnlockItemMarket : MonoBehaviour
{


    public Material materialtransparent1;
   

    public Material materialopaque1;
 

    public int numberofberriesgot;
    public int numberofberriesrequest;

	public bool useBlueBerries;

    // public GameObject thisobject;
    public BoxCollider thisobjectcollider;
    public GameObject itemgameobjecttochangematerial1;
    public GameObject itemgameobjecttochangematerial2;
    public GameObject itemgameobjecttochangematerial3;
    public GameObject itemgameobjecttochangematerial4;

//    public bool activated;

	public Animator hudAnim;
    TPC tpc;

    private void Start()
    {
        foreach (GameObject g in GameObject.FindGameObjectsWithTag("Player"))
        {
            if (g.name == "Woodle Character")
                tpc = g.gameObject.GetComponent<TPC>();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && other.gameObject.name == "Woodle Character")
        {
			CheckingBerries ();
        }
	}
    /*
	void Update(){
		if (activated && (useBlueBerries && numberofberriesgot != PlayerPrefs.GetInt ("BlueBerries")) || (!useBlueBerries && numberofberriesgot != PlayerPrefs.GetInt ("Berries")))
			CheckingBerries ();
	}
    */
	void CheckingBerries(){
		if(useBlueBerries)
			numberofberriesgot = PlayerPrefs.GetInt("BlueBerries", 0);
		else
			numberofberriesgot = PlayerPrefs.GetInt("Berries", 0);

		int number = 0;
		bool hasComp = false;
		bool hasPaidFor = false;
		if (this.transform.GetComponentInChildren<ActivateItemsMasks> () != null) {
			hasComp = true;
			number = this.transform.GetComponentInChildren<ActivateItemsMasks> ().itemID;
		}
		if (this.transform.GetComponentInChildren<ActivateItemsLeafs> () != null) {
			hasComp = true;
			number = this.transform.GetComponentInChildren<ActivateItemsLeafs> ().itemID;
		}
		if (this.transform.GetComponentInChildren<ActivateBonusAbility> () != null) {
			hasComp = true;
			number = this.transform.GetComponentInChildren<ActivateBonusAbility> ().itemID;
		}
		if (hasComp) {
			if (PlayerPrefs.GetInt ("PaidForItem" + number.ToString (), 0) == 1)
				hasPaidFor = true;
		}
		if (numberofberriesgot >= numberofberriesrequest || hasPaidFor) {
		//	activated = true;
			thisobjectcollider.enabled = true;

			if (itemgameobjecttochangematerial1.GetComponent<Renderer> () != null)
				itemgameobjecttochangematerial1.GetComponent<Renderer> ().material = materialopaque1;

			if (itemgameobjecttochangematerial2 != null)
				itemgameobjecttochangematerial2.GetComponent<Renderer> ().material = materialopaque1;

			if (itemgameobjecttochangematerial3 != null)
				itemgameobjecttochangematerial3.GetComponent<Renderer> ().material = materialopaque1;

			if (itemgameobjecttochangematerial4 != null)
				itemgameobjecttochangematerial4.GetComponent<Renderer> ().material = materialopaque1;
		} else {
			thisobjectcollider.enabled = false;
			if (itemgameobjecttochangematerial1.GetComponent<Renderer> () != null)
				itemgameobjecttochangematerial1.GetComponent<Renderer> ().material = materialtransparent1;

			if (itemgameobjecttochangematerial2 != null)
				itemgameobjecttochangematerial2.GetComponent<Renderer> ().material = materialtransparent1;

			if (itemgameobjecttochangematerial3 != null)
				itemgameobjecttochangematerial3.GetComponent<Renderer> ().material = materialtransparent1;

			if (itemgameobjecttochangematerial4 != null)
				itemgameobjecttochangematerial4.GetComponent<Renderer> ().material = materialtransparent1;
		}
	}

	public void DisplayIt(){
		if (useBlueBerries) {
			int c = PlayerPrefs.GetInt ("BlueBerries", 0);
			PlayerPrefs.SetInt ("BlueBerries", c - numberofberriesrequest);
            tpc.blueberryCount = PlayerPrefs.GetInt ("BlueBerries", 0);
			hudAnim.transform.Find ("Text").GetComponent<Text> ().text = PlayerPrefs.GetInt ("BlueBerries", 0).ToString ();
		} else {
			int c = PlayerPrefs.GetInt ("Berries", 0);
			PlayerPrefs.SetInt ("Berries", c - numberofberriesrequest);
            tpc.berryCount = PlayerPrefs.GetInt ("Berries", 0);
			hudAnim.transform.Find ("Text").GetComponent<Text> ().text = PlayerPrefs.GetInt ("Berries", 0).ToString ();
        }
        PlayerPrefs.Save();
        //
#if UNITY_XBOXONE
        DataManager.xOneEventsManager.SaveProgs();
#endif

        hudAnim.SetBool ("function", true);
        hudAnim.Play("Text Bounce", 1);
        StartCoroutine ("HUDWait");
	}

	IEnumerator HUDWait(){
		yield return new WaitForSeconds (3f);
		hudAnim.SetBool ("function", false);
	}
}