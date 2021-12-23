using UnityEngine;
using System.Collections;
using Rewired;

public class CameraAction : MonoBehaviour {
    
	[SerializeField] private bool moveDown;
    [SerializeField] private float moveDownAmount = 1f;
    public float moveDownMaxHeight;
    [SerializeField] private bool moveUp;
    [SerializeField] private float moveUpAmount = 1f;
    public float moveUpMaxHeight;
    [SerializeField] private bool zoomOut;
    [SerializeField] private float zoomDist;

    float movement;
    float difference;

    Player input;
    bool playerControlled;
    bool playerLetGo;

    private CameraFollower camF;

	void Start () {
        input = ReInput.players.GetPlayer(0);
        playerControlled = false;
        playerLetGo = false;

        if (moveDown && moveUp){
            moveUp = false;
            Debug.Log(this.gameObject + " is set to both 'moveDown' and 'moveUp'. Please just set it to one or neither. It's now set to 'moveDown' only.");
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player" && other.gameObject.name == "Woodle Character" &&  (camF != null || PlayerManager.GetMainPlayer().cam != null))
        {
            camF = PlayerManager.GetMainPlayer().cam.GetComponent<CameraFollower>();
            if (!playerControlled && camF != null) { 
                camF.camAutoControlled = true;
                if (moveDown)
                {
                    if (camF.distanceUp >= moveDownMaxHeight)
                    {
                        if (!camF.setToMoveDown)
                        {
                            camF.setToMoveDown = true;
                            movement = moveDownAmount;
                            difference = Mathf.Abs(camF.distanceUp - moveDownMaxHeight);
                        }
                        movement = Mathf.Lerp(0.1f, moveDownAmount, Mathf.Abs(camF.distanceUp - moveDownMaxHeight)/difference);
                        camF.moveDownAmount = movement;
                    }
                    else
                        camF.setToMoveDown = false;
                }
                if (moveUp)
                {
                    if (camF.distanceUp <= moveUpMaxHeight && !PlayerManager.GetMainPlayer().gliding)
                    {
                        if (!camF.setToMoveUp)
                        {
                            camF.setToMoveUp = true;
                            movement = moveUpAmount;
                            difference = Mathf.Abs(camF.distanceUp - moveUpMaxHeight);
                        }
                        movement = Mathf.Lerp(0.05f, moveUpAmount, Mathf.Abs(camF.distanceUp - moveUpMaxHeight) / difference);
                        camF.moveUpAmount = movement;
                    }
                    else
                        camF.setToMoveUp = false;
                }
                if (zoomOut)
                {
                 //   if (zoomDistCounter <= zoomMax)
                    {
                        camF.setToZoomOut = true;
                        camF.zoomDAmount = zoomDist;
                    }
                    //   else
                    //       camF.setToZoomOut = false;
                }

                if (Mathf.Abs(input.GetAxis("RV")) <= 0.2f)
                    playerLetGo = true;

                if (playerLetGo && Mathf.Abs(input.GetAxis("RV")) >= 0.3f)
                {
                    playerControlled = true;
                    camF.setToMoveDown = false;
                    camF.setToMoveUp = false;
                    camF.camAutoControlled = false;
                }
            }
        }
    }

    void OnTriggerExit(Collider other){
        if(other.tag == "Player" && other.gameObject.name == "Woodle Character")
        {
            camF.camAutoControlled = false;
            camF.setToMoveDown = false;
            camF.setToMoveUp = false;
            camF.setToZoomOut = false;
            playerControlled = false;
            playerLetGo = false;
        }
    }
}
