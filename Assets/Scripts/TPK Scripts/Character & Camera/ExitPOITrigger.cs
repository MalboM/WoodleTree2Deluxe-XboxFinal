using UnityEngine;
using System.Collections;

public class ExitPOITrigger : MonoBehaviour {

	[SerializeField] private float cameraYAngle;
	private GameObject camera;
    private CameraFollower camF;

   void Start () {
        camera = GameObject.FindWithTag("MainCamera");
        camF = camera.GetComponent<CameraFollower>();
	}

    void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Player" && other.gameObject.name == "Woodle Character") {
            if (camF.targetMode) {
                camF.ExitPOI();
            }
        }
    }
}
