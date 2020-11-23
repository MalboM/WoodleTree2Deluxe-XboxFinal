using UnityEngine;
using System.Collections;

public class ActivateOnPlayerEnter : MonoBehaviour {

    public GameObject object1;
   // public GameObject object2;
   // public GameObject object3;
   // public GameObject object4;

	public bool temporary;
	public float temporaryDuration;
	public bool usePref;
	public string prefName;

	public BoxCollider thisEnter;

	void Start(){
		
	}

	void Update(){
		if (thisEnter != null) {
			if (usePref && PlayerPrefs.GetInt (prefName, 0) == 1 && thisEnter.enabled)
				thisEnter.enabled = false;
			if (usePref && PlayerPrefs.GetInt (prefName, 0) == 0 && !thisEnter.enabled)
				thisEnter.enabled = true;
		}
	}

    void OnTriggerEnter(Collider other)
    {
		if (other.tag == "Player" && other.gameObject.name == "Woodle Character" && ((usePref && PlayerPrefs.GetInt (prefName) == 0) || !usePref)) {
			object1.SetActive (true);
            if (usePref)
            {
                PlayerPrefs.SetInt(prefName, 1);
            //    PlayerPrefs.Save();
            }
		//	if (temporary)
		//		StartCoroutine (AutoExit ());

			//    object2.SetActive(true);
			//    object3.SetActive(true);
			//   object4.SetActive(true);
		}
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player" && other.gameObject.name == "Woodle Character")
        {
			ExitThis ();
        }


    }

	void ExitThis(){

		object1.SetActive(false);
		//   object2.SetActive(false);
		//   object3.SetActive(false);
		//   object4.SetActive(false);
	}

	IEnumerator AutoExit(){
		yield return new WaitForSeconds (temporaryDuration);
		this.gameObject.GetComponent<BoxCollider> ().enabled =false;
		ExitThis ();
		CameraFollower camF = GameObject.FindWithTag ("MainCamera").gameObject.GetComponent<CameraFollower> ();
		if (camF.stationaryMode1 || camF.stationaryMode2) {
			camF.ExitCameraCut();
            if(GetComponent<Camera>() != null)
    			GetComponent<Camera>().transform.eulerAngles = new Vector3(GetComponent<Camera>().transform.eulerAngles.x, 0f, GetComponent<Camera>().transform.eulerAngles.z);
		}
	}

}
