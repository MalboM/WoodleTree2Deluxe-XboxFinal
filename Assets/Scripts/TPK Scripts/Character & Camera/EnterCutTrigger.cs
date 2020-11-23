using UnityEngine;
using System.Collections;

public class EnterCutTrigger : MonoBehaviour {

    [SerializeField] private GameObject cameraCutPosition;
    [SerializeField] private bool canMoveCamera;
    [SerializeField] private bool cameraFollowsCharacter = true;
    [SerializeField] private bool transitionToCut;
    [SerializeField] private float horizontalMovement;
    [SerializeField] private float verticalMovement;
    [SerializeField] private float maxCharacterDistFOV;

    [HideInInspector] public int iD;
    
    private CameraFollower camF;
    
    void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Player" && other.gameObject.name == "Woodle Character") {
            camF = PlayerManager.GetMainPlayer().cam.GetComponent<CameraFollower>();

            if (camF != null && camF.currentCutID != iD) {
                camF.currentCutID = iD;
                camF.cutHAngle = horizontalMovement;
                camF.cutVAngle = verticalMovement;
                if(!canMoveCamera)
                    camF.cutFarDist = maxCharacterDistFOV;
                camF.EnterCameraCut(cameraCutPosition, !canMoveCamera, transitionToCut, cameraFollowsCharacter);
            }
        }
    }
}
