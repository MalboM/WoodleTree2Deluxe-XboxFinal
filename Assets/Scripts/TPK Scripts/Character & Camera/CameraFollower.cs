/// <summary>
/// CURRENT VERSION: 2.0 (Jan '16)
/// This script was originally written by Yan Dawid of Zelig Games.
/// 
/// KEY (for the annotations in this script):
/// -The one referred to as 'User' is the person who uses/edits this asset
/// -The one referred to as 'Player' is the person who plays the project build
/// -The one referred to as 'Character' is the in-game character that the player controls
/// 
/// This script is to NOT be redistributed and can only be used by the person that has purchased this asset.
/// Editing or altering this script does NOT make redistributing this script valid.
/// This asset can be used in both personal and commercial projects.
/// The user is free to edit/alter this script to their hearts content.
/// You can contact the author for support via the Zelig Games official website: http://zeliggames.weebly.com/contact.html
/// </summary>

using UnityEngine;
using System.Collections;
using Rewired;

public class CameraFollower : MonoBehaviour {
    
    Player input;
    public PauseScreen ps;
	private Camera cam;
    public Camera skyCam;
    private GameObject camStationaryRel;
    public GameObject foxChara;
    public GameObject beaverChara;
    public GameObject bushChara;
    float calculateDistance;

    public bool dampCamera;

	public float distanceAway ;													//The default distance that the camera is away from the character on the x and z plane
	public float distanceUp;									//The default distance that the camera is away from the character on the y axis
	[SerializeField] private LayerMask whatIsAnObstruction;                     //The layermask which covers all the collision layers that can be obstructions
    [SerializeField] private LayerMask whatIsGroundBelow;                     //The layermask which covers all the collision layers that can be obstructions
    [SerializeField] private LayerMask whatIsGround;							//The layermask which covers all the collision layers that count at level ground
	private float origDA;														//A float containing the original value set for 'distanceAway'
	private float origDU;														//A float containing the original value set for 'distanceUp'
	
	[HideInInspector] public bool behindMode;									//A bool set to true for when the camera is in its deault 'behind/orbiting mode'
	[HideInInspector] public bool targetMode;									//A bool set to true for when the camera is in 'targeting mode'
	[HideInInspector] public bool freeMode;										//A bool set to true for when the camera is in 'free mode'
	[HideInInspector] public bool cutsceneMode;									//A bool set to true for when the camera is being positioned externally for cutscenes
	[HideInInspector] public bool stationaryMode1;
	[HideInInspector] public bool stationaryMode2;
    bool stillFollow;
    Vector3 pointOfInt;
    Vector3 finalPointOfInt;

	[HideInInspector] public bool canFreeMode;									//A bool that restricts whether the camera can go into 'free mode'

	public bool freeInvertYAxis;								//should the y-axis of the free camera be inverted
	public bool freeInvertXAxis;								//should the x-axis of the free camera be inverted
	public float freeRotateSpeed;								//The speed (sensitivity) at which the camera rotates around the character in 'free mode'
	private TPC chara;										//A reference to the characters 'ThirdPersonController' script
	private Rigidbody charaRB;
	private float addedUp;														//A float that adds how far out the camera should move from the character when in FPV, but only when swimming
	[HideInInspector] public Transform targetTransform;							//A reference to the transform that the camera focuses on by default
	private Transform followMe;													//A reference to the transform that the camera focuses on in certain modes; is altered externally via the ThirdPersonController script
	[HideInInspector] public GameObject focusRelative;							//A reference to the transform that the camera focuses on when the character is focusing on a target
	private bool dontCheck;														//A bool that is true if the camera shouldn't adjust its position when in contact with a wall under certain circumstances
	private RaycastHit rayHit;													//A RaycastHit used when handling the cameras collisions
	private Vector3 direct;														//A Vector3 used when handling the cameras position in collisions
	
	[HideInInspector] public bool transitionBackFromCutscene;
	[HideInInspector] public Vector3 prevLookAt;
	[HideInInspector] public int transitionTimer;
	private Vector3 toTheBackRight;
	private Vector3 toTheBackLeft;
	private float charaFacingDir;
	private float charaCamAngle;
	private bool obsOnRight;
	private bool obsOnLeft;
	private RaycastHit g1;
	private RaycastHit g2;
	private float hillDif;
    private float jumpLookValue;
    private float jumpLookSpeed;
    private float prevDUSpeed;
    private bool prevDUChangeDelay;

    private Vector3 desiredPos;
	private Vector3 lookAtPos;
	private float lookAtPosY;
	[SerializeField] private float aboveThreshG;
    [SerializeField] private float aboveThreshA;
    [SerializeField] private float belowThreshG;
	[SerializeField] private float belowThreshA;
	private bool followingDescent;
	[SerializeField] private float lookAheadX;
	[SerializeField] private float lookAheadY;
	[SerializeField] private float lookAheadVeloThresh;
	private float canLookAhead;
	private Vector3 lookAhead;
	private float nextAngleY;
	private float prevDU;
	[SerializeField] private float awayMax;
	[SerializeField] private float upMax;
	[SerializeField] private float awayMin1;
    [SerializeField] private float upMin1;
    [SerializeField] private float awayMin2;
	[SerializeField] private float upMin2;
	[SerializeField] private float zoomOutAngle;
	[SerializeField] private float zoomInAngle1;
	[SerializeField] private float zoomInAngle2;
	public float zoomSensitivity;
	private float zoomFloat;
    private float potentialZoomFloat;
    private float rotateFloat;
	private float gradientUp;
	private float yDifUp;
	private float gradientAngle;
	private float yDifAngle;
	private float middleAngle;
	private bool calculatedUpper;
	private bool calculatedLower1;
    private bool calculatedLower2;
    private bool moveCamBack;
	private bool correctingFromCol;
	private RaycastHit whiskerRay;
	private float hillRotation;
	[SerializeField] private float cliffDistCheck;
	[SerializeField] private float cliffDepthCheck;
	public bool inClosedSpace;
	[SerializeField] private float wideSpaceFoV;
	[SerializeField] private float lookingUpFoV;
	[SerializeField] private float closedSpaceFoV;
    [HideInInspector] public float cutVAngle;
    [HideInInspector] public float cutHAngle;
    [SerializeField] private float cutNearFoV;
    [SerializeField] private float cutFarFoV;
    [HideInInspector] public float cutFarDist;
	private bool stationary;
    private float statAngle;
    private float desiredFOV;
    private bool lerpRelative;

//    private bool hasMovedCam;
//	private bool revert;
//    private bool reverting;

	private GameObject cameraCutObj;
    private Vector2 mouseArea;
    [HideInInspector] public int currentCutID;

    private GameObject cameraPOIObj;
    private Vector3 centerSpot;
    [HideInInspector] public int currentPOIID;
    [HideInInspector] public float zoomOutAmount;
    [HideInInspector] public bool targetTransition;
    [HideInInspector] public bool cutTransition;
    private bool willTransitionAfter;
    private bool tempTransAfter;

    private Vector3 origTransPos;
    private Vector3 endTransPos;
    private float distTrans;
    private Vector3 startForward;
    private Vector3 origForward;

	Vector3 camerashift;

    private float nextRotateAngle;
    private float currentAngularSpeed;

    private float freeX;
    private float freeY;

    [HideInInspector] public bool camAutoControlled;
    [HideInInspector] public bool setToMoveDown;
    [HideInInspector] public float moveDownAmount;
    [HideInInspector] public bool setToMoveUp;
    [HideInInspector] public float moveUpAmount;
    [HideInInspector] public bool setToZoomOut;
    [HideInInspector] public float zoomDAmount;
    private float zoomMultiplier;

    private bool firstPass;														//a bool that turns true after the first pass of the Update() function to check for error
	[HideInInspector] public bool disableControl;

	void Start () {
		cam = this.GetComponent<Camera> ();

		origDA = distanceAway;																															//The original values of 'distanceAway/Up' are assigned
		origDU = distanceUp;
		prevDU = distanceUp;
		focusRelative = new GameObject ();							//The focusRelative object is created
		focusRelative.name = "FocusRelative";
		focusRelative.transform.parent = this.transform;			//The focusRelative's parented to this camera
		behindMode = true;											//The initial mode for the camera is set to 'behind mode'
		canFreeMode = true;											//The camera can enter 'free mode' on start
		rayHit = new RaycastHit ();
		g1 = new RaycastHit ();
		g2 = new RaycastHit ();

		lookAtPos = Vector3.zero;
		lookAtPosY = lookAtPos.y;
		lookAhead = Vector3.zero;

	//	revert = false;
    //    reverting = false;

		middleAngle = Mathf.Asin (origDU / (Mathf.Sqrt ((origDA * origDA) + (origDU * origDU)))) * Mathf.Rad2Deg;
		gradientUp = (awayMax - origDA) / (upMax - origDU);
		yDifUp = (gradientUp * (origDU)) - origDA;
		gradientAngle = (zoomOutAngle - middleAngle) / (upMax - origDU);
		yDifAngle = (gradientAngle * origDU) - middleAngle;
		calculatedUpper = true;
        calculatedLower1 = false;
        calculatedLower2 = false;

        camStationaryRel = new GameObject();
        camStationaryRel.name = "Cam Stationary Relative";

        int id = 0;
        GameObject[] cameraCutTriggers = GameObject.FindGameObjectsWithTag("CameraCutTrigger");
        while (id < cameraCutTriggers.Length) {
            cameraCutTriggers[id].GetComponent<EnterCutTrigger>().iD = id;
            id++;
        }
        currentCutID = -1;

        id = 0;
        GameObject[] cameraPOITrigs = GameObject.FindGameObjectsWithTag("CameraPOITrigger");
        while (id < cameraPOITrigs.Length) {
            cameraPOITrigs[id].GetComponent<EnterPOITrigger>().iD = id;
            id++;
        }
        currentPOIID = -1;
	}

    void Update() {                                                 //Update() is only really used for errors and miscelaneous checks
        if (!firstPass) {
            chara = PlayerManager.GetMainPlayer();
			input = chara.input;
				
            if (chara != null) 
                charaRB = chara.gameObject.GetComponent<Rigidbody>();
            
            if (chara.gameObject.transform.Find("FollowMe").gameObject != null) {
                targetTransform = chara.gameObject.transform.Find("FollowMe").transform;
                followMe = chara.gameObject.transform.Find("FollowMe").transform;
            }

            if (chara.gameObject.transform.Find("FollowMe").gameObject != null) {
                this.transform.position = new Vector3((-distanceAway * targetTransform.transform.forward.x) + targetTransform.transform.position.x,
					distanceUp + targetTransform.transform.position.y,
                   (-distanceAway * targetTransform.transform.forward.z) + targetTransform.transform.position.z);
                
            }

            firstPass = true;

            lookAtPos = targetTransform.position;
            this.transform.position = lookAtPos - (new Vector3(this.transform.forward.x, 0f, this.transform.forward.z) * distanceAway) + (Vector3.up * distanceUp);
            lookAtPosY = targetTransform.position.y + distanceUp;
            this.transform.LookAt(lookAtPos);
        } else {
            if (!behindMode && !stationaryMode1 && !stationaryMode2 && !targetMode) {       //The camera will default to 'behindMode' if not in any particular mode
                behindMode = true;
            }
            if (cutsceneMode) {
                behindMode = false;
                targetMode = false;
                freeMode = false;
                targetMode = false;
            }

			freeX = input.GetAxis("RH");
			freeY = -input.GetAxis ("RV");
            if (freeInvertXAxis)
                freeX *= -1f;
            if(freeInvertYAxis || stationaryMode1 || stationaryMode2)
                freeY *= -1f;
        }
    }
    
    void LateUpdate() { 
		if (!disableControl) {
			if (cutTransition || targetTransition || transitionBackFromCutscene) {
				freeY = 0f;
				freeX = 0f;
			}

			if (chara.onGround) {
				if (followingDescent)
					followingDescent = false;
				if (targetTransform.position.y > lookAtPosY + aboveThreshG || targetTransform.position.y < lookAtPosY - belowThreshG || targetTransform.parent != null || chara.inWindCol && chara.gliding) {
					lookAtPosY = targetTransform.position.y;
				}
			}
			if (!chara.onGround && !chara.jumping || chara.onGround && freeY != 0f) {
				if (targetTransform.position.y < lookAtPosY - belowThreshA || followingDescent || chara.onGround && freeY != 0f) {
					lookAtPosY = targetTransform.position.y;
					if (!followingDescent)
						followingDescent = true;
				}
			}
			if (!chara.onGround) {
				if (targetTransform.position.y > lookAtPosY + aboveThreshA) {
					lookAtPosY += Mathf.Abs (targetTransform.position.y - lookAtPosY) - aboveThreshA;
				}
			}
            centerSpot = targetTransform.position;
            if (foxChara.activeInHierarchy)
            {
                centerSpot = foxChara.transform.position;
                if (beaverChara.activeInHierarchy)
                    centerSpot = Vector3.Lerp(centerSpot, beaverChara.transform.position, 0.5f);
                if (bushChara.activeInHierarchy)
                    centerSpot = Vector3.Lerp(centerSpot, bushChara.transform.position, 0.5f);

                centerSpot = Vector3.Lerp(centerSpot, targetTransform.position, 0.75f);
            }

            lookAtPos = new Vector3 (Mathf.Lerp (lookAtPos.x, centerSpot.x, Time.smoothDeltaTime * 10f), 
				Mathf.Lerp (lookAtPos.y, lookAtPosY, Time.smoothDeltaTime * 5f),
				Mathf.Lerp (lookAtPos.z, centerSpot.z, Time.smoothDeltaTime * 10f));

//////////When the camera is in BEHIND/ORBITING MODE
			if (behindMode) {
				////Setting Position
				if (float.IsNaN (distanceAway) || float.IsNaN (distanceUp)) {                                         //The distance away/up can result as value that's not a number, so this counters that error
					dontCheck = true;                                                                               //If it is the case that they're not numbers, the camera will not check for collisions
					if (float.IsNaN (distanceAway))
						distanceAway = origDA;
					if (float.IsNaN (distanceUp))
						distanceUp = origDU;
				} else
					dontCheck = false;

				if (cutTransition)
					dontCheck = true;

				charaFacingDir = Vector3.Dot (new Vector3 (chara.transform.forward.x, 0f, chara.transform.forward.z), new Vector3 (this.transform.forward.x, 0f, this.transform.forward.z));

			/*	if (reverting) {
					this.transform.eulerAngles = Vector3.Lerp (this.transform.eulerAngles,
						new Vector3 (this.transform.eulerAngles.x, nextAngleY, this.transform.eulerAngles.z),
						Time.fixedDeltaTime * 10f);
					if (Mathf.Abs (this.transform.eulerAngles.y - nextAngleY) < 0.1f) {
						if (nextAngleY >= 360f)
							nextAngleY = 0f;
						nextAngleY = Mathf.Round (nextAngleY);
						this.transform.eulerAngles = new Vector3 (this.transform.eulerAngles.x, nextAngleY, this.transform.eulerAngles.z);
					}
					prevDU = Mathf.Lerp (prevDU, origDU, Time.smoothDeltaTime * 0.5f);
				} else {*/
				rotateFloat = 0f;
				rotateFloat = 0f;
				toTheBackRight = Quaternion.Euler (0f, -25f, 0f) * new Vector3 (this.transform.forward.x, 0f, this.transform.forward.z);
				toTheBackLeft = Quaternion.Euler (0f, 50f, 0f) * toTheBackRight;

				if (Physics.Linecast (chara.transform.position + (chara.transform.forward * chara.boxZ * 7f), chara.transform.position - (toTheBackRight * chara.boxZ * 10f),
						out whiskerRay, whatIsAnObstruction)) {
					if (whiskerRay.collider.tag != "NonObstructing" && chara.onGround && input.GetAxis ("LH") > 0.5f)
						obsOnRight = true;
					else
						obsOnRight = false;
				} else
					obsOnRight = false;
                    
				if (Physics.Linecast (chara.transform.position + (chara.transform.forward * chara.boxZ * 7f), chara.transform.position - (toTheBackLeft * chara.boxZ * 10f),
						out whiskerRay, whatIsAnObstruction)) {
					if (whiskerRay.collider.tag != "NonObstructing" && chara.onGround && input.GetAxis ("LH") < -0.5f)
						obsOnLeft = true;
					else
						obsOnLeft = false;
				} else
					obsOnLeft = false;

				if (obsOnLeft && obsOnRight) {
					obsOnLeft = false;
					obsOnRight = false;
				}
			//	if (obsOnLeft || obsOnRight)
			//		hasMovedCam = true;
                    
				if (obsOnRight)
					rotateFloat = .5f;
				if (obsOnLeft)
					rotateFloat = -.5f;
				if (freeX != 0f)
					rotateFloat = freeX;
				if (prevDUSpeed != 1f && !prevDUChangeDelay)
					prevDUSpeed = 1f;
				if (freeY != 0f) {
					zoomFloat = freeY;
					prevDUChangeDelay = false;
					StopCoroutine ("PrevDUChange");
				} else {
					if (!chara.onGround && !chara.frozen) {
						if (charaRB.velocity.y > 2f) {
							if (jumpLookValue == -0.8f)
								jumpLookSpeed = 5f;
							else
								jumpLookSpeed = 2f;
							jumpLookValue = -0.5f;
						} else {
							if (jumpLookValue == -0.5f)
								jumpLookSpeed = 5f;
							else
								jumpLookSpeed = 2f;
							jumpLookValue = -0.8f;
						}
						potentialZoomFloat = Mathf.Lerp (potentialZoomFloat, jumpLookValue * Mathf.Sign (charaRB.velocity.y), Time.deltaTime * jumpLookSpeed);
						zoomFloat = potentialZoomFloat;
						if (!dampCamera)
							zoomFloat = 0f;
				//		if (!hasMovedCam)
				//			hasMovedCam = true;
					} else {
						if (potentialZoomFloat != 0f) {
							potentialZoomFloat = 0f;
							prevDUSpeed = 0.2f;
							prevDUChangeDelay = true;
							StopCoroutine ("PrevDUChange");
							StartCoroutine ("PrevDUChange");
                            prevDU = origDU;
						}
						zoomFloat = 0f;
					}
				}
			//	}
				RotateCamera (rotateFloat);
				ZoomCamera (zoomFloat);
                
                if(foxChara.activeInHierarchy)
                    desiredPos += Vector3.up * (Mathf.Lerp(0f, 3f, (calculateDistance + 5f) / 30f));

                if (!targetMode) {
					if (firstPass && Vector3.Magnitude (new Vector3 (charaRB.velocity.x, 0f, charaRB.velocity.z)) >= lookAheadVeloThresh)
						canLookAhead = 1f;
					else
						canLookAhead = 0f;
					if (Vector3.Magnitude (charaRB.velocity) >= lookAheadVeloThresh)
						lookAhead = Vector3.Lerp (lookAhead, (new Vector3 (this.transform.right.x, 0f, this.transform.right.z) * input.GetAxis ("LH") * lookAheadX * canLookAhead) +
						(new Vector3 (this.transform.forward.x, 0f, this.transform.forward.z) * input.GetAxis ("LV") * lookAheadY * canLookAhead),
							Time.smoothDeltaTime * 5f);
					else {
						if (lookAhead != Vector3.zero) {
							lookAhead = Vector3.Lerp (lookAhead, Vector3.zero, Time.smoothDeltaTime * 5f);
							if (Mathf.Abs (Vector3.Distance (lookAhead, Vector3.zero)) <= 0.05f)
								lookAhead = Vector3.zero;
						}
					}
					if (targetTransition) {
						PositionTransition ();
						this.transform.forward = startForward;
						this.transform.position = desiredPos;
					} else {
						desiredPos = lookAtPos - (new Vector3 (this.transform.forward.x, 0f, this.transform.forward.z) * distanceAway) +
						(Vector3.up * distanceUp) + lookAhead;
                    
						if (setToZoomOut) {
							zoomMultiplier = Mathf.Lerp (zoomMultiplier, zoomDAmount, Time.smoothDeltaTime * 5f);
							if (Mathf.Abs (zoomMultiplier - zoomDAmount) <= 0.01f)
								zoomMultiplier = zoomDAmount;
						} else {
							zoomMultiplier = Mathf.Lerp (zoomMultiplier, 0f, Time.smoothDeltaTime * 5f);
							if (Mathf.Abs (zoomMultiplier) <= 0.01f)
								zoomMultiplier = 0f;
						}
						desiredPos -= this.transform.forward * zoomMultiplier;
					}
				} else {
					if (targetTransition) {
						PositionTransition ();
						this.transform.forward = startForward;
						this.transform.position = desiredPos;
					} else 
						desiredPos = centerSpot - (new Vector3 (this.transform.forward.x, 0f, this.transform.forward.z) * distanceAway) + (Vector3.up * distanceUp);
					
				}

				////Check Collisions
				if (!dontCheck)
					CameraChecks (desiredPos);

				////Set Position
				if (moveCamBack) {
					this.transform.position = Vector3.Lerp (this.transform.position, desiredPos, Time.smoothDeltaTime * 10f);
					if (Vector3.Distance (this.transform.position, desiredPos) < 0.5f)
						moveCamBack = false;
				} else {
					if (cutTransition || transitionBackFromCutscene) {
						PositionTransition ();
						if (!tempTransAfter)
							this.transform.forward = Vector3.Lerp (startForward, origForward, distTrans);
						else {
							camStationaryRel.transform.position = this.transform.position;
							camStationaryRel.transform.LookAt (lookAtPos);
							this.transform.forward = Vector3.Lerp (startForward, camStationaryRel.transform.forward, distTrans);
						}
					}
					this.transform.position = desiredPos;
				}
            
				if (inClosedSpace) {
					if (cam.fieldOfView != closedSpaceFoV) {
						cam.fieldOfView = Mathf.Lerp (cam.fieldOfView, closedSpaceFoV, Time.smoothDeltaTime * 5f);
						if (Mathf.Abs (cam.fieldOfView - closedSpaceFoV) < 0.1f)
							cam.fieldOfView = closedSpaceFoV;
					}
				} else {
					if (cam.fieldOfView < wideSpaceFoV) {
						cam.fieldOfView = Mathf.Lerp (cam.fieldOfView, wideSpaceFoV, Time.smoothDeltaTime * 5f);
						if (Mathf.Abs (cam.fieldOfView - wideSpaceFoV) < 0.1f)
							cam.fieldOfView = wideSpaceFoV;
					} else
						cam.fieldOfView = Mathf.Lerp (wideSpaceFoV, lookingUpFoV, (origDA - distanceAway) / awayMax);
				}

                /*
				if (!hasMovedCam && freeY != 0f && freeX != 0f)
					hasMovedCam = true;
				if ((hasMovedCam) && freeY == 0f && freeX == 0f && !revert && !camAutoControlled && chara.leftX == 0f && chara.leftY == 0f) {
					revert = true;
					StartCoroutine ("RevertToDefault");
				}
				if ((hasMovedCam) && revert && !camAutoControlled) {
					if (freeY != 0f || freeX != 0f || rotateFloat != 0f || zoomFloat != 0f) {
						revert = false;
						reverting = false;
						StopCoroutine ("RevertToDefault");
					}
				}
                if(camAutoControlled && revert)
                {
					revert = true;
                    StopCoroutine("RevertToDefault");
                }*/

                if (input.GetButtonDown("Camera") || input.GetButtonDown("CameraFront"))
                    MoveCameraToBack();
            }

			////The stationary mode after a camera cut where the camera stays still and can be slightly controlled by the player
			if (stationaryMode1) {
				if (cutTransition) {
					PositionTransition ();
                    if (stillFollow)
                        cameraCutObj.transform.LookAt(chara.transform.position);
                	this.transform.forward = Vector3.Lerp (startForward, cameraCutObj.transform.forward, distTrans);
                    this.transform.position = desiredPos;
				}
                
				mouseArea = new Vector2 (freeX, freeY);
                
                if (stillFollow)
                {
                    camStationaryRel.transform.LookAt(chara.transform.position);
                    pointOfInt = (camStationaryRel.transform.up * mouseArea.y * cutHAngle) + (camStationaryRel.transform.right * mouseArea.x * cutVAngle);
                    
                    finalPointOfInt = cameraCutObj.transform.position + camStationaryRel.transform.forward + pointOfInt;
                    
                    //    finalPointOfInt = Vector3.Lerp(cameraCutObj.transform.position + camStationaryRel.transform.forward, cameraCutObj.transform.position + camStationaryRel.transform.forward + pointOfInt, Time.deltaTime * 50f);
                }
                else
                    finalPointOfInt = cameraCutObj.transform.position + cameraCutObj.transform.forward +
                                                 (cameraCutObj.transform.up * mouseArea.y * cutHAngle) + (cameraCutObj.transform.right * mouseArea.x * cutVAngle); ;
                
                camStationaryRel.transform.LookAt (finalPointOfInt);
                if (Vector3.Magnitude(mouseArea) != 0f)
                    this.transform.forward = Vector3.Lerp(this.transform.forward, camStationaryRel.transform.forward, Time.smoothDeltaTime * 10f);
                else
                    this.transform.forward = Vector3.Lerp(this.transform.forward, camStationaryRel.transform.forward, Time.smoothDeltaTime * 10f);
				statAngle = Vector3.Angle (new Vector3 (0f, cameraCutObj.transform.forward.y, cameraCutObj.transform.forward.z), 
					new Vector3 (0f, this.transform.forward.y, this.transform.forward.z));
				if (this.transform.forward.y < cameraCutObj.transform.forward.y)
					statAngle *= -1f;
				desiredFOV = Mathf.Lerp (cutNearFoV, cutFarFoV, Input.mousePosition.y / Screen.height);
				cam.fieldOfView = Mathf.Lerp (cam.fieldOfView, desiredFOV, Time.smoothDeltaTime);
			}

			////The stationary mode after a camera cut where the camera stays still and just looks at the character
			if (stationaryMode2) {
				if (cutTransition) {
					PositionTransition ();
					this.transform.forward = Vector3.Lerp (startForward, cameraCutObj.transform.forward, distTrans);
					this.transform.position = desiredPos;
				}
				camStationaryRel.transform.LookAt (targetTransform.transform.position);
				this.transform.forward = Vector3.Lerp (this.transform.forward, camStationaryRel.transform.forward, Time.smoothDeltaTime);
				desiredFOV = Mathf.Lerp (cutNearFoV, cutFarFoV, Vector3.Distance (new Vector3 (lookAtPos.x, 0f, lookAtPos.z), new Vector3 (this.transform.position.x, 0f, this.transform.position.z)) / cutFarDist);
				cam.fieldOfView = Mathf.Lerp (cam.fieldOfView, desiredFOV, Time.smoothDeltaTime);
			}
		}

        if (skyCam.fieldOfView != cam.fieldOfView)
            skyCam.fieldOfView = cam.fieldOfView;

     //   chara.RelativeSet(lerpRelative);
    }

    public static float Smooth(float source, float target, float rate, float dt){
        return Mathf.Lerp(source, target, 1 - Mathf.Pow(rate, dt));
    }

    void RotateCamera(float angle) {
		if (dampCamera)
            nextRotateAngle = Mathf.Lerp(nextRotateAngle, angle, Time.smoothDeltaTime * (25 / 5f));
        else
			nextRotateAngle = angle;
        this.transform.RotateAround(lookAtPos, Vector3.up, freeRotateSpeed * nextRotateAngle);
    }
    
	void ZoomCamera(float amount){
        if (setToMoveDown)
            amount = -Mathf.Abs(moveDownAmount);
        if (setToMoveUp)
            amount = moveUpAmount;
    //    if (setToMoveDown || setToMoveUp)
    //        hasMovedCam = true;
        
        if (dampCamera)
        {
            if (prevDU < upMin1 && prevDU >= upMin2)
            {
                prevDU = Mathf.Lerp(prevDU, prevDU + (amount * zoomSensitivity), Time.fixedDeltaTime * 0.5f);
            }
            else
            {
                prevDU = Mathf.Lerp(prevDU, prevDU + (amount * zoomSensitivity), Time.fixedDeltaTime * 10f);
            }
            prevDU = Mathf.Clamp(prevDU, upMin2, upMax);
            distanceUp = Mathf.Lerp(distanceUp, prevDU, Time.smoothDeltaTime * 25f * prevDUSpeed);
        }
        else
        {
            if (amount != 0f)
            {
                if (prevDU < upMin1 && prevDU >= upMin2)
                    prevDU = prevDU + (amount * zoomSensitivity * 0.1f);
                else
                    prevDU = prevDU + (amount * zoomSensitivity);
                prevDU = Mathf.Clamp(prevDU, upMin2, upMax);
                distanceUp = Mathf.Lerp(distanceUp, prevDU, prevDUSpeed);
            }
        }
        if (distanceUp >= origDU)
        {
            if (calculatedLower1)
                calculatedLower1 = false;
            if (!calculatedUpper)
            {
                gradientUp = (awayMax - origDA) / (upMax - origDU);
                yDifUp = (gradientUp * (origDU)) - origDA;
                gradientAngle = (zoomOutAngle - middleAngle) / (upMax - origDU);
                yDifAngle = (gradientAngle * origDU) - middleAngle;
                calculatedUpper = true;
            }
        }
        if (distanceUp < origDU && distanceUp >= upMin1)
        {
            if (calculatedUpper)
                calculatedUpper = false;
            if (calculatedLower2)
                calculatedLower2 = false;
            if (!calculatedLower1)
            {
                gradientUp = (origDA - awayMin1) / (origDU - upMin1);
                yDifUp = (gradientUp * (upMin1)) - awayMin1;
                gradientAngle = (middleAngle - zoomInAngle1) / (origDU - upMin1);
                yDifAngle = (gradientAngle * upMin1) - zoomInAngle1;
                calculatedLower1 = true;
            }
        }
        if (distanceUp < upMin1 && distanceUp >= upMin2)
        {
            if (calculatedLower1)
                calculatedLower1 = false;
            if (!calculatedLower2)
            {
                gradientUp = (awayMin1 - awayMin2) / (upMin1 - upMin2);
                yDifUp = (gradientUp * (upMin2)) - awayMin2;
                gradientAngle = (zoomInAngle1 - zoomInAngle2) / (upMin1 - upMin2);
                yDifAngle = (gradientAngle * upMin2) - zoomInAngle2;
                calculatedLower2 = true;
            }
        }

        if (!foxChara.activeInHierarchy)
            distanceAway = (gradientUp * distanceUp) - yDifUp;
        else
        {
            if (bushChara.activeInHierarchy)
            {
                calculateDistance = Vector3.Distance(chara.transform.position, foxChara.transform.position);
                if (Vector3.Distance(chara.transform.position, beaverChara.transform.position) > calculateDistance)
                    calculateDistance = Vector3.Distance(chara.transform.position, beaverChara.transform.position);
                if (Vector3.Distance(chara.transform.position, bushChara.transform.position) > calculateDistance)
                    calculateDistance = Vector3.Distance(chara.transform.position, bushChara.transform.position);
            }
            else
            {
                if (beaverChara.activeInHierarchy)
                {
                    calculateDistance = Vector3.Distance(chara.transform.position, foxChara.transform.position);
                    if (Vector3.Distance(chara.transform.position, beaverChara.transform.position) > calculateDistance)
                        calculateDistance = Vector3.Distance(chara.transform.position, beaverChara.transform.position);
                }
                else
                    calculateDistance = Vector3.Distance(chara.transform.position, foxChara.transform.position);
            }
            distanceAway = ((gradientUp * distanceUp) - yDifUp) + Mathf.Lerp(0f, 10f, (calculateDistance + 5f) / 30f);
        }

        this.transform.eulerAngles = new Vector3(((gradientAngle * distanceUp) - yDifAngle) + hillRotation,
                                                    this.transform.eulerAngles.y, this.transform.eulerAngles.z);
	}

    public void MoveCameraToBack()
    {
        camerashift = Vector3.Cross(new Vector3(this.transform.forward.x, 0f, this.transform.forward.z), chara.transform.forward);
        RotateCamera(Vector3.Angle(new Vector3(this.transform.forward.x, 0f, this.transform.forward.z), chara.transform.forward) * Mathf.Sign(camerashift.y) * 0.25f);
    }

    private bool CheckForRenderer(GameObject go)
    {
        if (go.GetComponent<Renderer>() == null)
            return true;
        else
        {
            if (go.GetComponent<Renderer>().enabled)
                return true;
            else
                return false;
        }
    }

	/// <summary>
	/// This method is called each lateUpdate whenever the camera is in behind/target/free mode, it runs many checks to have it act as a dynamic camera
	/// </summary>
	void CameraChecks(Vector3 nextPos){
    ////////Check for obstructions between the character and the camera
      //  if (!foxChara.activeInHierarchy)
        {
            if (Physics.Linecast(followMe.transform.position, nextPos, out rayHit, whatIsAnObstruction) && !rayHit.collider.isTrigger && CheckForRenderer(rayHit.collider.gameObject) &&

                rayHit.collider.tag != "NonObstructing" && rayHit.collider.tag != "OneWay"){        //If there is a collision between the character and the camera...
                desiredPos = rayHit.point + (rayHit.normal * 0.15f);                                //...the camera moves to in front of the closest collision to the character
                direct = chara.transform.position;
                correctingFromCol = true;
            }
            else
            {
                if (!moveCamBack && !cutTransition)
                {
                    if (correctingFromCol)
                        moveCamBack = true;
                }
                else
                {
                    if (Physics.Raycast(nextPos, -this.transform.forward, 1f, whatIsAnObstruction)){           //...and if there's a collision directly behind the camera...
                        return;                                                                                //...the camera will not try to get back to its default distance away from the character
                    }
                    else
                    {                                                                                          //If there is no collision behind the camera, it will start moving back to its default distsance away from the character
                        if (!Physics.Linecast(chara.transform.position, chara.transform.position + ((rayHit.point + Vector3.up - this.transform.forward) - direct), whatIsAnObstruction)                                                                        //...if there is, it will cause the camera to continuously move backwards and forwards
                            && !Physics.Linecast(chara.transform.position,
                                              chara.transform.position + ((rayHit.point - Vector3.up - this.transform.forward) - direct), whatIsAnObstruction))
                        {
                            correctingFromCol = false;
                        }
                    }
                }
            }
        }

	////////Does 'whisker' checks for obstructions that may come to obstruct the players view; rotates the camera around the obstruction to avoid it
		if (behindMode && !foxChara.activeInHierarchy) {
        ////////Camera pans down when going down a hill and up when going up
            if (Mathf.Abs(charaFacingDir) > 0.8f && chara.onGround &&
				Physics.Raycast(chara.transform.position + (Vector3.up*chara.boxY) + (chara.transform.forward*chara.boxZ*3f), -Vector3.up, out g1, chara.boxY*2f, whatIsGround) &&
				Physics.Raycast(chara.transform.position + (Vector3.up*chara.boxY) - (chara.transform.forward*chara.boxZ*2f), -Vector3.up, out g2, chara.boxY*2f, whatIsGround)){
				hillDif = Mathf.Lerp(hillDif, g1.point.y - g2.point.y, Time.smoothDeltaTime);
				if(hillDif != 0f)
					hillRotation = hillDif * -10f;
				else
					hillRotation = 0f;
			}

	    ////////Camera looks down when the character is near a cliff (approaching a drop)	#IMPROVE Length + depth
			if(charaFacingDir > 0.6f && chara.onGround &&
			   !Physics.Linecast(chara.transform.position, chara.transform.position + (new Vector3(this.transform.forward.x, 0f, this.transform.forward.z)*cliffDistCheck),
                              whatIsGroundBelow) &&
			   !Physics.Raycast(chara.transform.position + (new Vector3(this.transform.forward.x, 0f, this.transform.forward.z)*cliffDistCheck),
			                Vector3.down, cliffDepthCheck, whatIsGroundBelow)){
				ZoomCamera(1f);
			}

	    ////////Camera moves up when small obstacles are in the way
			if (Physics.Raycast (lookAtPos, -new Vector3 (this.transform.forward.x, 0f, this.transform.forward.z), chara.boxZ * 5f, whatIsAnObstruction)
				&& !Physics.Raycast(lookAtPos, -new Vector3(this.transform.forward.x, 0f, this.transform.forward.z) + (Vector3.up*.5f), chara.boxZ * 5f, whatIsAnObstruction))
            {
                ZoomCamera (1f);
			}
		}
	}

    public void EnterCameraCut(GameObject positionObj, bool noRotate, bool transitioning, bool stillFollowsCharacter){
		lerpRelative = true;
		StartCoroutine (LerRelativeTimer ());
        if (transitioning){
            cutTransition = transitioning;
            origTransPos = this.transform.position;
            endTransPos = positionObj.transform.position;
            startForward = this.transform.forward;
            distTrans = 0f;
            willTransitionAfter = transitioning;
        }
        camStationaryRel.transform.position = positionObj.transform.position;
        cameraCutObj = positionObj.gameObject;
        stillFollow = stillFollowsCharacter;
        finalPointOfInt = cameraCutObj.transform.forward;
        if (!noRotate)
			stationaryMode1 = true;
		else
			stationaryMode2 = true;
		behindMode = false;
	}

    public void ExitCameraCut()
    {
        if (currentCutID != -1)
        {
            currentCutID = -1;
            tempTransAfter = false;
            moveCamBack = false;
            if (willTransitionAfter)
            {
                cutTransition = true;
                origTransPos = camStationaryRel.transform.position;
                endTransPos = (targetTransform.position) - (new Vector3(startForward.x, 0f, startForward.z) * distanceAway) +
                            (Vector3.up * distanceUp);
                distTrans = 0f;
                origForward = startForward;
                startForward = this.transform.forward;
                willTransitionAfter = false;
                tempTransAfter = true;
            }
            lerpRelative = true;
            StartCoroutine(LerRelativeTimer());
            stationaryMode1 = false;
            stationaryMode2 = false;
            behindMode = true;
        }
    }

    public void EnterPOI(GameObject pointOfInt) {
        targetTransition = true;
        targetMode = true;
        cameraPOIObj = pointOfInt;
        origTransPos = this.transform.position;
        endTransPos = centerSpot - (new Vector3(this.transform.forward.x, 0f, this.transform.forward.z) * distanceAway) + (Vector3.up * distanceUp)
                        - (this.transform.forward * Vector3.Distance(lookAtPos,
                        centerSpot) * (zoomOutAmount));
        startForward = this.transform.forward;
        distTrans = 0f;
    }

    public void ExitPOI() {
        targetTransition = true;
        startForward = this.transform.forward;
        lerpRelative = true;
        StartCoroutine(LerRelativeTimer());
        currentPOIID = -1;
        targetMode = false;
        behindMode = true;
        distTrans = 0f;

        origTransPos = this.transform.position;
        endTransPos = lookAtPos - (new Vector3(this.transform.forward.x, 0f, this.transform.forward.z) * distanceAway) +
                    (Vector3.up * distanceUp) + lookAhead;
    }

    public void PositionTransition() {
        if (tempTransAfter){
            if (cutTransition && (stationaryMode1 || stationaryMode2))
                endTransPos = camStationaryRel.transform.position;
            else
                endTransPos = lookAtPos - (new Vector3(this.transform.forward.x, 0f, this.transform.forward.z) * distanceAway) +
                        (Vector3.up * distanceUp) + lookAhead;
        }
        desiredPos = Vector3.Lerp(origTransPos, endTransPos, distTrans);
        distTrans += Time.smoothDeltaTime;
        if (distTrans >= 1f) {
            targetTransition = false;
            cutTransition = false;
            transitionBackFromCutscene = false;
        }
    }

	IEnumerator LerRelativeTimer(){
		yield return new WaitForSeconds (2f);
		lerpRelative = false;
	}

    /*IEnumerator RevertToDefault(){
		yield return new WaitForSeconds (3f);
        float yAngle = this.transform.eulerAngles.y;
        if (yAngle > 22.5f && yAngle <= 67.5f)
            nextAngleY = 45f;
        if (yAngle > 67.5f && yAngle <= 112.5f)
            nextAngleY = 90f;
        if (yAngle > 122.5f && yAngle <= 157.5f)
            nextAngleY = 135f;
        if (yAngle > 157.5f && yAngle <= 202.5f)
            nextAngleY = 180f;
        if (yAngle > 202.5f && yAngle <= 247.5f)
            nextAngleY = 225f;
        if (yAngle > 247.5f && yAngle <= 292.5f)
            nextAngleY = 270f;
        if (yAngle > 292.5f && yAngle <= 337.5f)
            nextAngleY = 315f;
        if (yAngle <= 22.5f)
            nextAngleY = 0f;
        if (yAngle > 337.5f) {
            nextAngleY = 360f;
        }
        reverting = true;
        StartCoroutine("EndReverting");
	}

    IEnumerator EndReverting()
    {
        yield return new WaitForSeconds(1f);
        reverting = false;
        revert = false;
        hasMovedCam = false;
    }*/

    IEnumerator PrevDUChange() {
        yield return new WaitForSeconds(1f);
        prevDUChangeDelay = false;
    }
}
