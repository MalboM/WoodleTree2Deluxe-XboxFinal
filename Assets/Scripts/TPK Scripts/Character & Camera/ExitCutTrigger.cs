using UnityEngine;
using System.Collections;

public class ExitCutTrigger : MonoBehaviour {

    [SerializeField] private float cameraYAngle;
	private GameObject camera;
    private CameraFollower camF;

   void Start () {

	}

    void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Player" && other.gameObject.name == "Woodle Character")
        {
            camera = PlayerManager.GetMainPlayer().cam;
            camF = camera.GetComponent<CameraFollower>();
            if (camF != null)
            {
                if (camF.stationaryMode1 || camF.stationaryMode2)
                {
                    camF.ExitCameraCut();
                    //    camera.transform.eulerAngles = new Vector3(camera.transform.eulerAngles.x, cameraYAngle, camera.transform.eulerAngles.z);
                }
            }
        }
    }
}
