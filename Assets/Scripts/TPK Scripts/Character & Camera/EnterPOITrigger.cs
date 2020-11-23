using UnityEngine;
using System.Collections;

public class EnterPOITrigger : MonoBehaviour {

	[SerializeField] private GameObject pointOfInterest;
    [SerializeField] private float zoomAwayScale;

    [HideInInspector] public int iD;

    private GameObject camera;
    private CameraFollower camF;

   void Start () {
        camera = GameObject.FindWithTag("MainCamera");
        camF = camera.GetComponent<CameraFollower>();
	}

    void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Player" && other.gameObject.name == "Woodle Character") {
            camF.currentPOIID = iD;
            camF.zoomOutAmount = zoomAwayScale;
            camF.EnterPOI(pointOfInterest);
        }
    }
}
