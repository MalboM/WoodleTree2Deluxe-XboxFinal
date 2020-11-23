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
using XInputDotNetPure;
using PigeonCoopToolkit.Effects.Trails;
using System.Collections.Generic;

[RequireComponent (typeof(Rigidbody))]							// The GameObject requires a Rigidbody component

public class ThirdPersonController : MonoBehaviour
{
    bool turnOffVibration;

    public float checkpointx;
    public float checkpointy;
    public float checkpointz;

    public int playerIndex;     // 1 = Woodle | 2 = Bush | 3 = Beaver | 4 = Fox \\
    [SerializeField] private bool p1KeyboardOnly;
    private PlayerIndex pIx;
    
	[HideInInspector] public Vector3 zeroVelocity;
    private GameObject curElevator;

    public int health;
    public int berryCount;
    [SerializeField] private int invinceFramesAmount;
    [SerializeField] private float bounceHeight;

    private bool invinceFrames;
    private bool defeated;

    [SerializeField] private GameObject leafBone;
    public GameObject dayPack;
    private GameObject leafColObj;
    private AttackSettings leafAS;
    [SerializeField] private GameObject waterDropMesh;
    [SerializeField] private GameObject waterReleaseObj;
    private GameObject dropObj;
    [SerializeField] public GameObject windball1;
    [SerializeField] public float windBall1Speed;
    [SerializeField] public float windBall1Lifetime;
    [SerializeField] public GameObject windball2;
    [SerializeField] public float windBall2Speed;
    [SerializeField] public float windBall2Lifetime;
    [SerializeField] public GameObject windball3;
    [SerializeField] public float windBall3Speed;
    [SerializeField] public float windBall3Lifetime;
    [SerializeField] public GameObject windball4;
    [SerializeField] public float windBall4Speed;
    [SerializeField] public float windBall4Lifetime;
    private GameObject[] windBalls = new GameObject[5];
    private GameObject[] windBalls2 = new GameObject[8];
    private GameObject[] windBalls3 = new GameObject[12];
    private GameObject[] windBalls4 = new GameObject[16];

    [HideInInspector] public int leafNo;

    private AudioSource sound;
    private AudioSource footSteps;
    private AudioSource interactions;
    private bool stepDelay;
    private int stepInt;
    private int prevStepInt;

    private bool onIce;

    private ContactPoint ptCS;
    private GameObject prevGround;
    private ContactPoint hwpt;
    private int iterateCS;
    private bool foundOneCS;
    private bool onASlideCS;
    private bool nxtToBoxCS;
    private bool foundMat;

    [SerializeField] private GameObject dustWalkParticles;
    [SerializeField] private GameObject dustRunParticles;
    [SerializeField] private GameObject landParticles;
    [SerializeField] private GameObject instrumentParticles;
    [SerializeField] private GameObject hitParticles;
    [SerializeField] private GameObject attackParticles;
    [SerializeField] private GameObject jumpParticles;
    [SerializeField] private GameObject doubleJumpParticles;
    [SerializeField] private GameObject killedParticles;
    [SerializeField] private GameObject splashParticles;
	[SerializeField] private GameObject boostParticles;

    private ParticleSystem.EmissionModule walkEM;
    private ParticleSystem.EmissionModule runEM;
    private ParticleSystem.EmissionModule landEM;
    private ParticleSystem.EmissionModule instrumentEM;
    private ParticleSystem.EmissionModule hitEM;
    private ParticleSystem.EmissionModule attackEM;
    private ParticleSystem.EmissionModule jumpEM;
    private ParticleSystem.EmissionModule dJumpEM;
    private ParticleSystem.EmissionModule killedEM;
    private ParticleSystem.EmissionModule splashEM;
	private ParticleSystem.EmissionModule boostEM;
    
    [SerializeField] private GameObject jumpTrail;
    [SerializeField] private GameObject leafTrail;
    [SerializeField] private GameObject runTrail;

    private SmokeTrail jumpST;
    private SmokeTrail leafST;
    private SmokeTrail runST;

    [HideInInspector] public bool inWindCol;

    private string[] grassMaterials = new string[13];
    private string[] rockMaterials = new string[7];
    private string[] snowMaterials = new string[2];
    private string[] sandMaterials = new string[3];
    private string[] woodMaterials = new string[9];
    private int groundID;                                       // 0 = Grass | 1 = Rock | 2 = Snow | 3 = Sand | 4 = Wood | 5 = Ground \\
    private bool inWaterFS;

    private PlayerInputs pI;									//the PlayerInputs.cs script attached to the handler in the scene
	private Animations currentAnim;
	private bool inFree;										//if the character is using the 'free movement' camera
	[HideInInspector] public bool jumping;						//if the character is in its jumping state
	[HideInInspector] public bool onGround;						//if the character is on the ground
	private bool startedJump;									//is set just after the character is 'jumping' and has made some adjustments
	[HideInInspector] public MeshCollider meshCol;              //reference to the characters Capsule Collider
	[HideInInspector] public float cHeight;						//the original height of the Capsule Collider
	[HideInInspector] public float cRad;						//the original radius of the capsule collider
	[HideInInspector] public Rigidbody rb;						//the character Rigidbody component
    
	public GameObject rootBoneGameObject;

    [SerializeField] private AudioClip attack1Sound;
	[SerializeField] private AudioClip attack2Sound;
	[SerializeField] private AudioClip attack3Sound;
	[SerializeField] private AudioClip attack4Sound;
	private int attackIterate;
	private int attackAnimIterate;
    [SerializeField] private AudioClip attackChargedSound;
    [SerializeField] private AudioClip attackChargingSound;
    [SerializeField] private AudioClip dieSound;
    [SerializeField] private AudioClip hit1Sound;
    [SerializeField] private AudioClip hit2Sound;
    [SerializeField] private AudioClip hit3Sound;
    private int hitIterate;
    [SerializeField] private AudioClip jump1Sound;
	[SerializeField] private AudioClip jump2Sound;
	[SerializeField] private AudioClip jump3Sound;
	private int jumpIterate;
	[SerializeField] private AudioClip doublejump1Sound;
	[SerializeField] private AudioClip doublejump2Sound;
	private int doublejumpIterate;
    [SerializeField] private AudioClip landingSound;
    [SerializeField] private AudioClip leafUpSound;
    [SerializeField] private AudioClip waterCatchSound;
    [SerializeField] private AudioClip windballSound;
    [SerializeField] private AudioClip defeatSound;
    [SerializeField] private AudioClip bongoSound;
    [SerializeField] private AudioClip leafGrow;
    [SerializeField] private AudioClip sliding;
	[SerializeField] private AudioClip boxPushing;
	[SerializeField] private AudioClip boostSound;

    [SerializeField] private AudioClip[] berrySounds = new AudioClip[4];
    private int berryIt;
    private int prevBerryIt;

    [SerializeField] private AudioClip[] grassFootSteps = new AudioClip[4];
    [SerializeField] private AudioClip[] rockFootSteps = new AudioClip[4];
    [SerializeField] private AudioClip[] snowFootSteps = new AudioClip[4];
    [SerializeField] private AudioClip[] sandFootSteps = new AudioClip[4];
    [SerializeField] private AudioClip[] woodFootSteps = new AudioClip[4];
    [SerializeField] private AudioClip[] groundFootSteps = new AudioClip[4];
    [SerializeField] private AudioClip[] waterFootSteps = new AudioClip[4];
    private AudioClip tempStep;

    public float gravityIntensity;

    [SerializeField] private int characterCollisionLayer;		//the collision layer int of the character game object
	[SerializeField] private int groundCollisionLayer;			//the collision layer int of the ground game objects
	[SerializeField] private int SlidesCollisionLayer;			//the collision layer int of the Slide game objects
	[SerializeField] private int moveableCollisionLayer;		//the collision layer int of moveable game objects
    [SerializeField] private int leafCollisionLayer;
    [SerializeField] private int waterCollisionLayer;
    [SerializeField] private int waterfallCollisionLayer;
    [SerializeField] private int enemyCollisionLayer;
    [SerializeField] private int windColumnColLayer;
    [SerializeField] private int bounceCollisionLayer;

	public float maxSpeed;                                      //the scale factor of the characters movement speed

	private float locoSpeed;									//the speed at which the character moves (to be used by the Animator)
	public float additionalSprintSpeed;							//how much faster should the character move when sprinting?
	private float sprinting;									//if the character is sprinting
	private float currentSpeed;                                 //the current speed variable that the character is moving at
	[SerializeField] private float boostSpeed;
	private bool isBoosting;
	private float iceAccel;
    public float acceleration;                                  //how quickly does the character pick up speed?
    public float deceleration;
    private float accel;										//the current acceleration variable that the character is accelerating at
    private float offGroundAccel;
    private Vector3 cliffDir;
    private bool lockVelocity;
    private float prevYVelo;
    private Vector3 prevXZVelo;
    private bool setPrevVelo;
    private float glideGravity;
    [HideInInspector] public bool inGlide;
    public float midairSensitivity;
    private bool onGFixed;
    private bool bounce;
    
	[Range(0f,1f)]	public float walkableSlideTolerence;		//set from 0(very steep) to 1(very horizontal); how steep can the ground be for the character to walk on

	[SerializeField] private int amountOfJumps;					//how many jumps can the character perform before landing again?
	[SerializeField] private float midAirMovement;				//how sensitive is the mid-air movement?

	[System.Serializable] public class JumpForce{ public string jumpName; public float forceAmount; }
	public JumpForce[] jumpForces = new JumpForce[3];

    [SerializeField] private float glideIntensity;
    [SerializeField] private float windColumnIntensity;
	private bool moveUpStairs;									//is the character currently moving up stairs? (basically if the ledge they're climbing is <= size of the capsules radius
	private int jumpNo;											//which jump is the character currently performing (initial/double/triple)
	private bool hardLanded;									//has the character made a hard landing? (as opposed to a running-land)
	private float addedSpeed;									//an added speed for certain movements
	private float jumpImpulse;									//the force of the characters jump
	private bool stoppedMidAir;
    [HideInInspector] public bool isDJumping;
    private bool hasLandedThisFrame;

    [HideInInspector] public bool inACutscene;					//is the character currently in a cutscene? (interacting with an NPC)
	[HideInInspector] public Vector3 cutsceneLookAtPos;			//the position in world space that the character looks at when interacting with an NPC in first-person view
	[HideInInspector] public string currentNPCName;				//the 'name' of the NPC gameobject that the character is currently interacting with
	[HideInInspector] public float leftX;						//a reference to the Horizontal Input Axis
	[HideInInspector] public float leftY;						//a reference to the Vertical Input Axis
	[HideInInspector] public Vector3 direction;					//a 2D vector made of leftX (for x) and leftY (for z)
	private Vector3 newDir;
	private bool hasUnClicked;									//checks if the player has let go of the triggers on a gamepad
	private bool huggingWall;									//if the character is 'hugging' against a wall
	private CameraFollower cam;                                 //a reference to the Main Camera
	private Vector3 gravityDirection;							//the direction of the gravity acting on the character
	private float gravityFactor;								//the scale factor of the gravity
	[HideInInspector] public GameObject relative;				//a transform to emulate the camera's movements without its z rotation
	[HideInInspector] public bool delayed;						//is a delay set just after jumping so that the character doesn't land right after starting a jump
	private bool prolongJumpDelay;								//is the player holding down the jump button after jumping?
	private bool adjustingCam;									//if the camera is inbetween the wall and the character when initially grabbing a ledge
	private bool againstWall;									//if the character is able to grab the ledge they are pressing against
	private float holdTimer;									//the timer to check how long the character has been pressing against a wall for without stopping
	private float xInvrt;										//for inverted horizontal axis
	private float yInvrt;										//for inverted vertical axis
	
	[HideInInspector] public bool masterBool;							//a bool that is false when certain other bools are all false; these are bools that are commonly checked for all being false

	[SerializeField]
	private LayerMask whatIsGround;			//the layermask that holds the collision layers that count as walkable ground
	[SerializeField]
	private LayerMask whatIsSlide;			//the layermask that holds the collision layers that count as unwalkable Slides
	[SerializeField]
	private LayerMask whatIsACollision;		//the layermask that holds the collision layers that count as any type of ground (walkable or not)
	[SerializeField]
	private LayerMask whatIsMoveable;
	[SerializeField]

	[HideInInspector] public Animator anim;						//a reference to the characters Animator (set in the child object that is the characters model)
	private Transform rayA;                                     //a ray that checks for ground below the character
    private RaycastHit rayhitA;
    private RaycastHit rayCollide1;								//the hit that is when entering a collision
	private RaycastHit rayCollide2;								//the hit that is when staying in a collision
	private RaycastHit rayCollide3;								//the hit that is when exiting a collision
	private Vector3 rayNormal;									//the normal of the ground that the character is on
	private bool firstPass;										//a bool that turns true after the first pass of the Update() function to check for errors

	[HideInInspector] public bool isPaused;						//is the game currently paused

	[HideInInspector] public bool isCrouching;					//is the character crouching?
	[HideInInspector] public bool crouchDelay;					//is the character going into or out-of a crouch?
	[HideInInspector] public bool isUnderCollision;				//is the character under a collision while crouching
	private GameObject dRelative;								//the gameObject that is instantiated to keep track of the direction that the player is moving in

    private float forwardAngle;
    private float angleToRot;
    private bool braking;
	private Vector3 prevDir;
	private Vector3 prevNewDir;
    private int rotIterate;

    private bool isPushing;
    private bool againstMoveable;
    private Rigidbody boxRB;
    private RaycastHit boxHit;

    private bool isHoldingLeaf;
    private bool cancelLeafHold;

    private bool attackPressOnly;
    private bool isHoldingAttack;
    private bool attackBuffer;
    private bool letGoOfAttack;
    private bool attackBufferX;
    private bool startedAttack;
    private bool chargingAttack;

    private bool isPlaying;
    private bool cancelInstrument;

    private bool isCelebrating;
    private bool beenHit;
    
	[HideInInspector] public bool slidingDownSlide;				//if the character is sliding down a Slide
	private bool startedSliding;								//is set just after the character is 'slidingDownSlide' and has made some adjustments
	private Vector3 slidingDir;                                 //the direction that the character is currently sliding in
    private bool slideDelay;									//is set true a short while after the character walks onto a Slide
    private GameObject slideRelative;                           //the gameObject that is instantiated to keep track of the sliding direction of a Slide
    private RaycastHit raySlide;
    private RaycastHit groundSlide;

    GameObject currentOneWay;
    Collider curOWCol;
    RaycastHit oneWayHit;

    void Start (){
        if (System.Environment.OSVersion.Platform == System.PlatformID.MacOSX || System.Environment.OSVersion.Platform == System.PlatformID.Unix)
            turnOffVibration = true;

        //Berries count
        berryCount = PlayerPrefs.GetInt("Berries", 0);

        health = 2;
        leafNo = 1;
	    pI = GameObject.FindWithTag ("Handler").GetComponent<PlayerInputs> ();						//getting the PlayerInputs.cs script from the Handler object in the scene
		currentAnim = this.GetComponent<Animations> ();
        currentAnim.anim = this.GetComponentInChildren<Animator>();
		meshCol = this.GetComponent<MeshCollider> ();													//getting the capsule collider
		cHeight = 0.57f * this.transform.localScale.y;                                                                  //the height of the capsule collider
        if (this.gameObject.tag == "Multiplayer")
            cHeight *= 0.7f;
        cRad = 0.2f * this.transform.localScale.z;																	//the radius of the capsule collider

        currentSpeed = 0f;
        iceAccel = 0f;

        AudioSource[] sources = new AudioSource[3];
        sources = this.GetComponents<AudioSource>();

        sound = sources[0];
        footSteps = sources[1];
        interactions = sources[2];

		//Creating required child objects
		GameObject followMe = new GameObject ();
		followMe.name = "FollowMe";
		followMe.transform.parent = this.transform;
		followMe.transform.localPosition = Vector3.zero;
		GameObject altFollowMe = new GameObject ();
		altFollowMe.name = "AltFollowMe";
		altFollowMe.transform.parent = this.transform;
		GameObject fpvFollowMe = new GameObject ();
		fpvFollowMe.name = "LdgFollowMe";
		fpvFollowMe.transform.parent = this.transform;
		fpvFollowMe.transform.localPosition = new Vector3 (0f, ((cHeight/2f)*(7f/8f)), 0f);
		fpvFollowMe.transform.parent = rootBoneGameObject.transform;
		GameObject rayAgo = new GameObject ();
		rayAgo.name = "RayA";
		rayAgo.transform.parent = this.transform;
		rayAgo.transform.localPosition = new Vector3 (0f, -(cHeight/2f), 0f);
		GameObject rayBgo = new GameObject ();
		rayBgo.name = "RayB";
		rayBgo.transform.parent = this.transform;
		rayBgo.transform.localPosition = new Vector3 (0f, -(cHeight/4f), -(0.515625f*cRad));
		GameObject rayCgo = new GameObject ();
		rayCgo.name = "RayC";
		rayCgo.transform.parent = this.transform;
		rayCgo.transform.localPosition = new Vector3 (0f, -(cHeight/4f), (1.03125f*cRad));
		GameObject rayDgo = new GameObject ();
		rayDgo.name = "RayD";
		rayDgo.transform.parent = this.transform;
		rayDgo.transform.localPosition = new Vector3 (0f, (cHeight*1.255f), (1.375f*cRad));
		GameObject rayEgo = new GameObject ();
		rayEgo.name = "RayE";
		rayEgo.transform.parent = this.transform;
		rayEgo.transform.localPosition = new Vector3 (0f, (cHeight*1.255f), (0.6015625f*cRad));

		rb = this.GetComponent<Rigidbody> ();																			//referring to the characters RigidBody component
		if (Camera.main != null) {
			cam = Camera.main.GetComponent<CameraFollower> ();															//finding the Main Camera for 'cam'
		}

        if (playerIndex == 1){
            relative = new GameObject();                                                                                    //creating the 'relative' transform
            relative.name = "Relative";
            if (cam != null){
                relative.transform.position = cam.transform.position;                                                       //setting 'relative's position to the cameras position
                relative.transform.rotation = new Quaternion(0f, cam.transform.rotation.y, 0f, cam.transform.rotation.w);   //setting 'relative's rotation to be the cameras, minus its z and x rotation
            }
            dRelative = new GameObject();
            dRelative.name = "dRelative";
        }

		if (this.GetComponentInChildren<Animator> () != null)
			anim = this.GetComponentInChildren<Animator> ();															//getting the Animator of the child object that is the characters model

		rb.centerOfMass = new Vector3 (0f, (-cHeight / 2f) + cRad, 0f);													//setting the rigidbodys center of mass to be the center of the 'ball' shape at the bottom of the capsule collider
		
		rayA = rayAgo.transform;
        rayhitA = new RaycastHit();
        boxHit = new RaycastHit();
        groundSlide = new RaycastHit();
        oneWayHit = new RaycastHit();

        //Setting initial values of certain variables
        delayed = true;
		jumpNo = 0;
		currentNPCName = "";
		zeroVelocity = Vector3.zero;
        
        Physics.IgnoreLayerCollision(characterCollisionLayer, leafCollisionLayer, true);
        leafColObj = new GameObject();
        leafColObj.gameObject.name = "leafColObj " + this.gameObject.name;
        leafColObj.AddComponent<BoxCollider>();
        BoxCollider boxCol = leafColObj.GetComponent<BoxCollider>();
        boxCol.isTrigger = true;
        boxCol.center = Vector3.zero;
        if(playerIndex == 1)
            boxCol.size = new Vector3(0.4f, 0.39f, 0.19f);
        if (playerIndex == 2){
            boxCol.center = new Vector3(0f, 0f, 0.15f);
            boxCol.size = new Vector3(0.5f, 0.35f, 0.4f);
        }
        if (playerIndex == 3)
            boxCol.size = new Vector3(0.4f, 0.4f, 0.3f);
        if (playerIndex == 4)
            boxCol.size = new Vector3(0.5f, 0.35f, 0.4f);
        leafColObj.AddComponent<Rigidbody>();
        Rigidbody leafRB = leafColObj.GetComponent<Rigidbody>();
        leafRB.useGravity = false;
        leafRB.isKinematic = true;
        leafColObj.gameObject.layer = leafCollisionLayer;
        leafColObj.AddComponent<LeafCollision>();
        LeafCollision lCol = leafColObj.GetComponent<LeafCollision>();
        lCol.curAnim = this.currentAnim;
        lCol.boneToFollow = leafBone.gameObject;
        if(playerIndex == 1)
            lCol.waterDrop = waterDropMesh;
//        lCol.tpc = this;
        lCol.waterLayer = waterfallCollisionLayer;
        lCol.enemyLayer = enemyCollisionLayer;
        leafColObj.AddComponent<AttackSettings>();
        leafAS = leafColObj.GetComponent<AttackSettings>();
        leafAS.attackAmount = 1;
        leafAS.activeAttack = false;
        leafAS.enemyLayer = enemyCollisionLayer;

        int wbInt = 0;
        while(wbInt < windBalls.Length){
            WBInst(wbInt, windBalls, windball1);
            wbInt++;
        }
        if (playerIndex == 1){
            wbInt = 0;
            while (wbInt < windBalls2.Length){
                WBInst(wbInt, windBalls2, windball2);
                wbInt++;
            }
            wbInt = 0;
            while (wbInt < windBalls3.Length){
                WBInst(wbInt, windBalls3, windball3);
                wbInt++;
            }
            wbInt = 0;
            while (wbInt < windBalls4.Length){
                WBInst(wbInt, windBalls4, windball4);
                wbInt++;
            }
        }

        dropObj = Instantiate(waterReleaseObj, Vector3.zero, this.transform.rotation) as GameObject;
        dropObj.gameObject.name = this.gameObject.name + " Water Release";
        dropObj.gameObject.SetActive(false);
        
        walkEM = dustWalkParticles.GetComponent<ParticleSystem>().emission;
        runEM = dustRunParticles.GetComponent<ParticleSystem>().emission;
        landEM = landParticles.GetComponent<ParticleSystem>().emission;
        instrumentEM = instrumentParticles.GetComponent<ParticleSystem>().emission;
        hitEM = hitParticles.GetComponent<ParticleSystem>().emission;
        attackEM = attackParticles.GetComponent<ParticleSystem>().emission;
        jumpEM = jumpParticles.GetComponent<ParticleSystem>().emission;
        dJumpEM = doubleJumpParticles.GetComponent<ParticleSystem>().emission;
        killedEM = killedParticles.GetComponent<ParticleSystem>().emission;
        splashEM = splashParticles.GetComponent<ParticleSystem>().emission;
		boostEM = boostParticles.GetComponent<ParticleSystem> ().emission;

        walkEM.enabled = false;
        runEM.enabled = false;
        landEM.enabled = false;
        instrumentEM.enabled = false;
        hitEM.enabled = false;
        attackEM.enabled = false;
        jumpEM.enabled = false;
        killedEM.enabled = false;
        dJumpEM.enabled = false;
        splashEM.enabled = false;
		boostEM.enabled = false;

        jumpST = jumpTrail.GetComponent<SmokeTrail>();
        leafST = leafTrail.GetComponent<SmokeTrail>();
        runST = runTrail.GetComponent<SmokeTrail>();

        jumpST.Emit = false;
        leafST.Emit = false;
        runST.Emit = false;

        grassMaterials[0] = "Grassgreendark";
        grassMaterials[1] = "Grassgreendark2";
        grassMaterials[2] = "Grassgreendarksmall";
        grassMaterials[3] = "Grassgreendarksuper";
        grassMaterials[4] = "GrassGreenDarkTiny";
        grassMaterials[5] = "GrassGreenLight";
        grassMaterials[6] = "GrassGreenLightSuper";
        grassMaterials[7] = "GrassGreenSolid";
        grassMaterials[8] = "GrassGreenSolidDark";
        grassMaterials[9] = "GrassGreenSolidDarkSuper";
        grassMaterials[10] = "GrassGreenSolidLight";
        grassMaterials[11] = "GrassGreenSolidLightSuper";
        grassMaterials[12] = "GrassNew3";

        rockMaterials[0] = "Rock3";
        rockMaterials[1] = "Rock30";
        rockMaterials[2] = "RockDark";
        rockMaterials[3] = "RockSolid";
        rockMaterials[4] = "RockSolid2";
        rockMaterials[5] = "RockSolid3";
        rockMaterials[6] = "RockSolidRed";

        snowMaterials[0] = "Snow";
        snowMaterials[1] = "White";

        sandMaterials[0] = "Sand";
        sandMaterials[1] = "Sand2";
        sandMaterials[2] = "SandSolid1";

        woodMaterials[0] = "WoodHouse";
        woodMaterials[1] = "WoodSolid";
        woodMaterials[2] = "WoodSolid2";
        woodMaterials[3] = "WoodSolid3";
        woodMaterials[4] = "WoodSolid4";
        woodMaterials[5] = "WoodSolid5";
        woodMaterials[6] = "WoodSolid6";
        woodMaterials[7] = "WoodTextured";
        woodMaterials[8] = "Bridge";

        glideGravity = 1f;

        slideDelay = true;
        slideRelative = new GameObject();
        slideRelative.name = "SlideRelative";
        slidingDir = Vector3.zero;
        slideDelay = true;
        raySlide = new RaycastHit();
    }

	void Update ()
	{
        //Berries
        PlayerPrefs.SetInt("Berries", berryCount);

        if (!firstPass){
            if(playerIndex != 1){
                relative = GameObject.Find("Relative");
                dRelative = GameObject.Find("dRelative");
            }
            RecalculateControllers();
            firstPass = true;
            return;
        }
        
        if (!isPaused) {
			if (!inACutscene && !isPlaying && !cancelInstrument && !currentAnim.inVictory && !beenHit && !defeated && !slidingDownSlide) {
				if (masterBool)
					masterBool = false;
			} else {
				if (!masterBool)
					masterBool = true;
			}

            if (slidingDownSlide) {
                onGFixed = false;
                onGround = false;
                jumpNo = 1;
                delayed = true;
                if (!currentAnim.inSliding || sound.clip != sliding){
                    anim.Play("Sliding", 0);
                    sound.clip = sliding;
                    sound.loop = true;
                    sound.PlayDelayed(0f);
                }
            }

            if (slidingDownSlide && currentAnim.inSliding){
                Physics.Raycast(this.transform.position, -Vector3.up, out groundSlide, 1f, whatIsACollision);
                if(groundSlide.collider.gameObject.layer == groundCollisionLayer) {
                    Land(false);
                    anim.Play("Idle");
                 }
            }

            if (sound.loop == true && (!slidingDownSlide || !isPushing))
                sound.loop = false;

            //We'll put the code here, why not?
            if (Physics.Raycast(this.transform.position - (Vector3.up * cHeight * 0.55f), -Vector3.up, out oneWayHit, cHeight * 3f, whatIsGround) && oneWayHit.collider.gameObject.tag == "OneWay") {
                if (currentOneWay == null || oneWayHit.collider.gameObject != currentOneWay.gameObject) {
                    currentOneWay = oneWayHit.collider.gameObject;
                    curOWCol = oneWayHit.collider;
                    curOWCol.isTrigger = false;
                }
            } else {
                if (currentOneWay != null) {
                    curOWCol.isTrigger = true;
                    currentOneWay = null;
                }
            }

//////////Direction Code
			relative.transform.position = Vector3.zero; ;                               //positions the 'relative' transform
            if (playerIndex == 1){
                leftX = pI.xAxisL;                                             //sets the leftX and leftY floats to the Input Axes
                leftY = pI.yAxisL;
            }
            if (playerIndex == 2){
                leftX = pI.xAxisL2;                                             //sets the leftX and leftY floats to the Input Axes
                leftY = pI.yAxisL2;
            }
            if (playerIndex == 3){
                leftX = pI.xAxisL3;                                             //sets the leftX and leftY floats to the Input Axes
                leftY = pI.yAxisL3;
            }
            if (playerIndex == 4){
                leftX = pI.xAxisL4;                                             //sets the leftX and leftY floats to the Input Axes
                leftY = pI.yAxisL4;
            }
            if (Vector3.Magnitude(new Vector3(leftX, 0f, leftY)) < 0.9f) {
                if (leftX < 0.01f && leftX > -0.01f)                                                  //sets dead-zones
                    leftX = 0f;
                if (leftY < 0.01f && leftY > -0.01f)
                    leftY = 0f;
            }

            direction = new Vector3(leftX, 0f, leftY);

            leftX = direction.x;
            leftY = direction.z;

            if (this.transform.parent != null)
                zeroVelocity = new Vector3(0f, rb.velocity.y, 0f);
            else
                zeroVelocity = Vector3.zero;

//////////Animation Control
			locoSpeed = Mathf.Lerp(locoSpeed, Vector3.Magnitude(rb.velocity), Time.smoothDeltaTime * 2f);

			if (locoSpeed < 0.01f)																																						//sets a deadzone for 'locoSpeed'
				locoSpeed = 0f;
			anim.SetFloat ("Speed", locoSpeed);
			anim.SetFloat ("turnAmount", Mathf.Lerp(anim.GetFloat("turnAmount"),
				                                    1.5f * -Vector3.Cross ((relative.transform.forward * direction.z) + (relative.transform.right * direction.x), this.transform.forward).y,
				                                    Time.smoothDeltaTime * 15f));		//sets the float in the animator to determine how much the character is 'leaning' to the left/right when moving in any direction
				
			if (direction != Vector3.zero && onGround && !masterBool && !jumping){
                if (onGround || !onGround){
                    if (hardLanded)
                        hardLanded = false;
                    anim.SetBool("inLocomotion", true);
                }
			}else {
				if ((huggingWall && onGround && direction != Vector3.zero && prevDir == Vector3.zero && !braking) || isBoosting){

                }else
                    anim.SetBool("inLocomotion", false);
			}
            
            forwardAngle = Vector3.Angle(this.transform.forward,
                    (leftY * relative.transform.forward) + (leftX * relative.transform.right));
            if (!masterBool && (onGround || !onGround && inGlide)) {
                if (Vector3.Magnitude(direction) >= 0.35f) {
                    if (forwardAngle < 150f) {
                        if (!braking) {
                            this.transform.forward = Vector3.Lerp(this.transform.forward,
                                (leftY * relative.transform.forward) + (leftX * relative.transform.right),
                                Time.smoothDeltaTime * 20f);
                        }
                        anim.SetFloat("turnAmount", Mathf.Lerp(anim.GetFloat("turnAmount"),
                                                                forwardAngle * (-Vector3.Cross((leftY * relative.transform.forward) + (leftX * relative.transform.right), this.transform.forward).y),
                                                                Time.smoothDeltaTime * 5f));
                    }else {
                        if (!inGlide) {
                            Vector3 crossy = Vector3.Cross(this.transform.forward,
                            (leftY * relative.transform.forward) + (leftX * relative.transform.right));
                            if (!braking) {
                                anim.SetBool("inLocomotion", true);
                                braking = true;
                                angleToRot = forwardAngle * (Mathf.Sign(crossy.y));
                                if (onGround){
                                    anim.Play("Brake", 0);
                                    rb.velocity = Vector3.zero;
                                }
                            }
                        }else {
                            this.transform.forward = (leftY * relative.transform.forward) + (leftX * relative.transform.right);
                        }
                    }
                }else {
                    if (forwardAngle > 170f) {
                        Vector3 crossy = Vector3.Cross(this.transform.forward,
                                                        (leftY * relative.transform.forward) + (leftX * relative.transform.right));
                        this.transform.RotateAround(this.transform.position, Vector3.up, (Mathf.Sign(crossy.y) * 10f));
                    }
                   if(leftX != 0f && leftY != 0f) {
                        this.transform.forward = Vector3.Lerp(this.transform.forward,
                                    (leftY * relative.transform.forward) + (leftX * relative.transform.right),
                                    Time.smoothDeltaTime * 20f);
                    }
                }
            }
            if (braking) {
                if (rotIterate == 0)
                    anim.Play("Brake", 0);
                if (rotIterate < (9))
                    this.transform.RotateAround(this.transform.position, Vector3.up, angleToRot / 8);
                else {
                    braking = false;
                    rotIterate = 0;
                }
                rotIterate += 1;
            }
            prevDir = direction;
                
            if (rb.velocity.y <= 0f || (rb.velocity.y >= 0f && inGlide && inWindCol)) {
                if (((pI.glideHold && playerIndex == 1) || (pI.glideHold2 && playerIndex == 2) || (pI.glideHold3 && playerIndex == 3) || (pI.glideHold4 && playerIndex == 4)) && !onGround) {
                    if (!inGlide && !defeated){
                        inGlide = true;
                        anim.ResetTrigger("jumpNow");
                        anim.ResetTrigger("doubleJumpNow");
                        anim.ResetTrigger("exitGlide");
                        anim.ResetTrigger("falling");
                        anim.SetTrigger("enterGlide");
                        glideGravity = 0.02f * glideIntensity;
                        sound.clip = leafUpSound;
                        sound.PlayDelayed(0f);
                    }
                }else{
                    if(inGlide)
                        ExitGlide();
                }
            }else{
                if (glideGravity != 1f)
                    glideGravity = 1f;
            }

            if (againstMoveable && onGround) {
                if (Vector3.Magnitude(direction) != 0f) {
                    if (!isPushing && currentAnim.locomotionBlend) {
                        isPushing = true;
                        anim.SetTrigger("enterPush");
                        cancelLeafHold = true;
                        sound.clip = boxPushing;
                        sound.loop = true;
                        sound.PlayDelayed(0f);
                    }
                    Physics.Linecast(this.transform.position, boxRB.position, out boxHit, whatIsMoveable);
                    this.transform.forward = new Vector3(-boxHit.normal.x, 0f, -boxHit.normal.z);
                }else {
                    if (isPushing)
                        ExitPush();
                }
            }else {
                if (isPushing)
                    ExitPush();
            }

            if ((currentAnim.inFall || currentAnim.inJump || currentAnim.inDoubleJump || currentAnim.inGlide) && onGround && delayed && !hasLandedThisFrame && !defeated
            && Physics.Linecast(rayA.position, rayA.position - (Vector3.up * (cHeight * 0.4f)), out rayhitA, whatIsGround) && !onGFixed)
                Land(true);

            if (!Physics.Linecast(rayA.position, rayA.position - (Vector3.up * (cHeight * 1.5f)), whatIsGround) && onGround){
                onGround = false;
                StartCoroutine("OffGround");
            }

            if (onGround && jumpNo != 0 && delayed)
				jumpNo = 0;
            if (!onGround && !jumping && delayed && !currentAnim.inFall) {
                anim.ResetTrigger("falling");
                anim.SetTrigger("falling");
            }

//////////Particles
            if (anim.GetBool("inLocomotion") == true && (Vector3.Magnitude(direction) <= 0.75f || (Input.GetKey(KeyCode.LeftShift)) && playerIndex == 1) && onGround){
                if (walkEM.enabled == false) {
                    walkEM.enabled = true;
                }
            }else{
                if (walkEM.enabled == true) {
                    walkEM.enabled = false;
                }
            }
            if(anim.GetBool("inLocomotion") == true && (Vector3.Magnitude(direction) > 0.75f && (!Input.GetKey(KeyCode.LeftShift)) && playerIndex == 1) && onGround) {
                if (runEM.enabled == false) {
                    runEM.enabled = true;
                    runST.Emit = true;
                }
            }else{
                if (runEM.enabled == true) {
                    runEM.enabled = false;
                    runST.Emit = false;
                }
            }
     /*       if (anim.GetBool("inLocomotion") == true && ((!pI.runHold && playerIndex == 1) || (!pI.runHold2 && playerIndex == 2) || (!pI.runHold3 && playerIndex == 3) || (!pI.runHold4 && playerIndex == 4)) && onGround){
                if (walkEM.enabled == false) {
                    walkEM.enabled = true;
                }
            }else{
                if (walkEM.enabled == true) {
                    walkEM.enabled = false;
                }
            }
            if(anim.GetBool("inLocomotion") == true && ((pI.runHold && playerIndex == 1) || (pI.runHold2 && playerIndex == 2) || (pI.runHold3 && playerIndex == 3) || (pI.runHold4 && playerIndex == 4)) && onGround) {
                if (runEM.enabled == false) {
                    runEM.enabled = true;
                    runST.Emit = true;
                }
            }else{
                if (runEM.enabled == true) {
                    runEM.enabled = false;
                    runST.Emit = false;
                }
            }*/

//////////FootSteps
            if(!stepDelay && anim.GetBool("inLocomotion") && onGround){
                stepInt = Random.Range(0, 4);
                if (stepInt == prevStepInt) {
                    stepInt++;
                    if (stepInt >= 4) { 
                        stepInt = 0;
                        if (prevStepInt == 0)
                            stepInt = 1;
                    }
                }
                prevStepInt = stepInt;

                if (!inWaterFS){
                    if (groundID == 0)
                        tempStep = grassFootSteps[stepInt];
                    if (groundID == 1)
                        tempStep = rockFootSteps[stepInt];
                    if (groundID == 2)
                        tempStep = snowFootSteps[stepInt];
                    if (groundID == 3)
                        tempStep = sandFootSteps[stepInt];
                    if (groundID == 4)
                        tempStep = woodFootSteps[stepInt];
                    if (groundID == 5)
                        tempStep = groundFootSteps[stepInt];
                }else
                    tempStep = waterFootSteps[stepInt];
                footSteps.clip = tempStep;
                footSteps.PlayDelayed(0f);
                stepDelay = true;
                StartCoroutine("ClipDelay");
            }

//////////Jump on button press
			if (((pI.jumpPress && playerIndex == 1) || (pI.jumpPress2 && playerIndex == 2) || (pI.jumpPress3 && playerIndex == 3) || (pI.jumpPress4 && playerIndex == 4)) && (!masterBool || slidingDownSlide) && !hasLandedThisFrame) {
                cancelLeafHold = true;
                if (delayed && (onGround || slidingDownSlide)) {
                    if (chargingAttack){
                        CancelAttack();
                        chargingAttack = false;
                        StopCoroutine("Charging");
                        StopCoroutine("Buffer");
                        anim.ResetTrigger("chargeAttack");
                        anim.ResetTrigger("releaseAttack");
                        anim.Play("Empty", 1);
                    }
                    if (jumpNo < amountOfJumps) {
                        if (slidingDownSlide){
                            jumpNo = amountOfJumps;
                            Jump("double");
                        }else
                            Jump("default");
                    }
                }
                if(delayed && !onGround){
                    if (jumpNo < amountOfJumps)
                        Jump("double");
                }
            }

//////////Attacking
            if (((pI.attackHold && playerIndex == 1) || (pI.attackHold2 && playerIndex == 2) || (pI.attackHold3 && playerIndex == 3) || (pI.attackHold4 && playerIndex == 4)) && !attackBuffer && !startedAttack && !defeated){
                cancelLeafHold = true;
                isHoldingLeaf = false;
                anim.ResetTrigger("leafUp");
                anim.ResetTrigger("leafDown");
                anim.ResetTrigger("quickLeafDown");
                anim.Play("Empty", 1);
                attackBuffer = true;
				StopCoroutine ("AttackNext");
                letGoOfAttack = false;
                startedAttack = false;
                anim.ResetTrigger("chargeAttack");
                anim.ResetTrigger("releaseAttack");
                StartCoroutine("Buffer");
            }

            if (attackBuffer && !attackBufferX && ((!pI.attackHold && playerIndex == 1) || (!pI.attackHold2 && playerIndex == 2) || (!pI.attackHold3 && playerIndex == 3) || (!pI.attackHold4 && playerIndex == 4))){
                letGoOfAttack = true;
                attackBufferX = true;
                StopCoroutine("Buffer");
                isHoldingAttack = false;
                attackPressOnly = true;
            }

			if (attackPressOnly && attackBufferX && !startedAttack) {
				startedAttack = true;
				leafST.Emit = true;

				if(attackAnimIterate == 0)
					anim.Play("Attack", 1, 0f);
				if(attackAnimIterate == 1)
					anim.Play("Attack2", 1, 0f);
				if(attackAnimIterate == 2)
					anim.Play("Attack3", 1, 0f);

				if (playerIndex == 1){
					attackAnimIterate++;
					if (attackAnimIterate > 2)
						attackAnimIterate = 0;
					if (attackAnimIterate > 0) {
						StopCoroutine ("AttackNext");
						StartCoroutine ("AttackNext");
					}
					attackIterate = Random.Range (0, 3);
				}else {
					attackAnimIterate = 0;
					attackIterate = Random.Range (0, 3);
				}

				if (attackIterate == 0)
					sound.clip = attack1Sound;
				if (attackIterate == 1)
					sound.clip = attack2Sound;
				if (attackIterate == 2)
					sound.clip = attack3Sound;


                sound.PlayDelayed(0.35f);
             //   attackIterate = !attackIterate;
                leafAS.activeAttack = true;
                StopCoroutine("DeactiveAttack");
                StartCoroutine("DeactiveAttack");
                if(leafNo == 4){
                    leafST.Emit = true;
                    chargingAttack = true;
                    StartCoroutine("Charging");
                }
            }

            if (isHoldingAttack && attackBufferX && !startedAttack && onGround && !inGlide){
                leafST.Emit = true;
                startedAttack = true;
                chargingAttack = true;
                sound.clip = attackChargingSound;
                sound.PlayDelayed(0f);
                anim.SetTrigger("chargeAttack");
                StartCoroutine("Charging");
            }

            if (((!pI.attackHold && playerIndex == 1) || (!pI.attackHold2 && playerIndex == 2) || (!pI.attackHold3 && playerIndex == 3) || (!pI.attackHold4 && playerIndex == 4)) && chargingAttack && currentAnim.inAttackCharge){
                StopCoroutine("Charging");
                anim.SetTrigger("releaseAttack");
                sound.clip = attackChargedSound;
                sound.PlayDelayed(0f);
                if(!turnOffVibration)
                   XInputDotNetPure.GamePad.SetVibration(pIx, 0f, 0f);
                chargingAttack = false;
                CancelAttack();
            }

//////////Leaf Actions
            if (((pI.leafHold && playerIndex == 1) || (pI.leafHold && playerIndex == 2) || (pI.leafHold && playerIndex == 3) || (pI.leafHold && playerIndex == 4)) 
                && !masterBool && onGround && delayed && !isHoldingLeaf && !currentAnim.inLeafDown && !currentAnim.inQuickLeafDown && !attackBuffer){
                isHoldingLeaf = true;
                anim.SetTrigger("leafUp");
                sound.clip = leafUpSound;
                sound.PlayDelayed(0f);
            }

            if ((((!pI.leafHold && playerIndex == 1) || (!pI.leafHold2 && playerIndex == 2) || (!pI.leafHold3 && playerIndex == 3) || (!pI.leafHold4 && playerIndex == 4)) || cancelLeafHold) && isHoldingLeaf && currentAnim.inLeafHold) {
                isHoldingLeaf = false;
                if (leafColObj.GetComponent<LeafCollision>().hasWaterDrop){
                    dropObj.gameObject.SetActive(true);
                    dropObj.GetComponent<Rigidbody>().useGravity = true;
                    dropObj.transform.position = waterDropMesh.transform.position;
                    dropObj.GetComponent<Rigidbody>().velocity = rb.velocity + (this.transform.forward * 2.5f);
                }
                leafColObj.GetComponent<LeafCollision>().DropWater();
                if (cancelLeafHold)
                    anim.SetTrigger("quickLeafDown");
                else
                    anim.SetTrigger("leafDown");
            }

            if (cancelLeafHold)
                cancelLeafHold = false;
            
//////////Playing Instrument
            if((((pI.instrumentTrig == 1f || Input.GetKey(KeyCode.E)) && playerIndex == 1) || (pI.instrumentTrig2 == 1f && playerIndex == 2) || (pI.instrumentTrig3 == 1f && playerIndex == 3) || (pI.instrumentTrig4 == 1f && playerIndex == 4)) 
                && !isPlaying && !cancelInstrument && onGround && !attackBuffer && delayed){
                isPlaying = true;
                rb.velocity = zeroVelocity;
                anim.SetTrigger("playMusic");
                StartCoroutine("PlayingInstrument");
                sound.clip = bongoSound;
                sound.PlayDelayed(0f);
                instrumentEM.enabled = true;
            }

            if ((((pI.instrumentTrig != 1f && !Input.GetKey(KeyCode.E)) && playerIndex == 1) || (pI.instrumentTrig2 != 1f && playerIndex == 2) || (pI.instrumentTrig3 != 1f && playerIndex == 3) || (pI.instrumentTrig4 != 1f && playerIndex == 4)) 
                && isPlaying && !cancelInstrument) {
                cancelInstrument = true;
                isPlaying = false;
                StopCoroutine("PlayingInstrument");
                StartCoroutine("CancelInstrument");
                sound.Stop();
            }

//////////Multiplayer Teleport
            if(rb.isKinematic == false) {
                if ((pI.startPress2 && playerIndex == 2) || (pI.startPress3 && playerIndex == 3) || (pI.startPress4 && playerIndex == 4))
                    StartCoroutine("TeleportToWoodle");
            }

//////////misc adjustments
            if (health == 0 && currentAnim.inIdle)
                Defeat();

            if ((!onGround || jumping) && this.transform.parent != null)
                this.transform.SetParent(null);

            if (inGlide){
                if (inWindCol && glideGravity != -windColumnIntensity)
                    glideGravity = -windColumnIntensity;
                if (!inWindCol && glideGravity != (0.02f * glideIntensity))
                    glideGravity = 0.02f * glideIntensity;
            }

            if (onGround && !currentAnim.inHit && rb.velocity.y <= 0f && beenHit && !invinceFrames)
                StartCoroutine("Invincibility");
        }
	}

	public void RelativeSet(bool lerpRelative){
//////////Selecting the relative transform
		if (cam != null) {
            if (lerpRelative) { 
			    relative.transform.rotation = Quaternion.Lerp(relative.transform.rotation,
					                                            new Quaternion(0f, cam.transform.rotation.y, 0f, cam.transform.rotation.w),
					                                            Time.smoothDeltaTime);
		    }else
			    relative.transform.rotation = new Quaternion (0f, cam.transform.rotation.y, 0f, cam.transform.rotation.w);
		}
    }
    
	void FixedUpdate (){
//////////Movement on the ground
		if (!masterBool) {
			if (direction != Vector3.zero || isBoosting) {
				if (Mathf.Abs (Vector3.Magnitude (direction)) < 0.6f) {			//If there is little input while the character is on an incline, this will prevent odd movements
					addedSpeed = 0f;
				} else {
					addedSpeed = 1f;
				}

				newDir = Vector3.Normalize (direction);
				newDir *= Vector3.Magnitude (direction);
                if(Vector3.Magnitude(direction) > 1.1f)
                    newDir = Vector3.Normalize(direction);
                if (lockVelocity && Vector3.Magnitude(direction) != 0f) {
                    if (Vector3.Angle(newDir, cliffDir) <= 100f) {
                        lockVelocity = false;
                        StopCoroutine("Locking");
                    }
                }
                if (lockVelocity)
                    newDir = Vector3.zero;

				if(Vector3.Magnitude(direction) >= 0.9f && !isBoosting)
					prevNewDir = newDir;

                if (onGround) {
                    if (Vector3.Magnitude(direction) > accel || Vector3.Magnitude(direction) != 0f)  
                        accel = Mathf.Lerp(accel, Vector3.Magnitude(new Vector3(newDir.x, 0f, newDir.z)), Time.fixedDeltaTime * acceleration);
                    else
                        accel = Vector3.Magnitude(new Vector3(newDir.x, 0f, newDir.z));
                }else
                    accel = Mathf.Lerp(accel, Vector3.Magnitude(new Vector3(newDir.x, 0f, newDir.z)), Time.fixedDeltaTime * acceleration);
                //SPRINTSPEED
                         if (((pI.runHold && playerIndex == 1) || (pI.runHold2 && playerIndex == 2) || (pI.runHold3 && playerIndex == 3) || (pI.runHold4 && playerIndex == 4)) && !currentAnim.inAttackCharge)
                             currentSpeed = maxSpeed + additionalSprintSpeed;
                         else

                if (Input.GetKey(KeyCode.LeftShift) && playerIndex == 1)
                    currentSpeed = 2.5f;
                else
                    currentSpeed = maxSpeed;

				if (isBoosting) {
					currentSpeed = boostSpeed;
					accel = 1f;
					if (Vector3.Magnitude(direction) < 0.9f) {
						if(Vector3.Magnitude(prevNewDir) >= Vector3.Magnitude(newDir))
							newDir = prevNewDir;
					}
				}

               // if(Input.GetKey(KeyCode.C))
                    //currentSpeed = 10f;

                if (onIce)
                    iceAccel = 0.01f;
                else
                    iceAccel = 1f;

                //**Remove LERP functionality if you want more precise (less realistic) movement*
                if (!braking) {
                    if (Vector3.Magnitude(direction) >= 0.9f){
                        prevXZVelo = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
                        if (setPrevVelo)
                            setPrevVelo = false;
                    }
                    if (onGround){
                        rb.velocity = Vector3.Lerp(rb.velocity, zeroVelocity +
                                            new Vector3(0f, rb.velocity.y * addedSpeed, 0f) +
                                            (this.transform.forward * (Vector3.Magnitude(newDir)) * currentSpeed), accel * iceAccel);
                    }else {
                        rb.velocity = Vector3.Lerp(rb.velocity,
                                           new Vector3(0f, rb.velocity.y * addedSpeed, 0f) +
                                           (relative.transform.forward * (newDir.z) * currentSpeed) +
                                           (relative.transform.right * (newDir.x) * currentSpeed), accel * midairSensitivity);
                    }
                } else {
                    rb.AddForce((relative.transform.forward * newDir.y) + (this.transform.right * newDir.x));
                }
            } else {
                if (onIce){
                    rb.velocity = Vector3.Lerp(rb.velocity, zeroVelocity, Time.fixedDeltaTime);
                }else {
                    if (!onGround)
                        offGroundAccel = 0.001f;
                    else
                        offGroundAccel = 1f;
                    if (!setPrevVelo)
                    {
                        rb.velocity = new Vector3(prevXZVelo.x, rb.velocity.y, prevXZVelo.z);
                        setPrevVelo = true;
                    }
                    accel = Mathf.Lerp(accel, Vector3.Magnitude(new Vector3(leftX, 0f, leftY)), Time.fixedDeltaTime * deceleration * offGroundAccel);
                    if (onGround)
                        rb.velocity = Vector3.Lerp(rb.velocity, new Vector3(0f, rb.velocity.y, 0f), 1f - accel);
                    else
                        rb.velocity = new Vector3(rb.velocity.x * 0.99f, rb.velocity.y, rb.velocity.z * 0.99f);
                }
            }
        }

//////////Jumping movement
        if (jumping) {
			if (!startedJump) {                                                                                 //When the jump is just started this will cancel out any rolling or anything that will oddly affect the speed
                this.transform.position += (Vector3.up * 0.05f);
                if (slidingDownSlide){
                    jumpNo = 2;
                    rb.AddForce(rb.mass * new Vector3(slideRelative.transform.forward.x, 0f, slideRelative.transform.forward.z) * 10f, ForceMode.Impulse);
                }
                slidingDownSlide = false;
                sound.loop = false;
                rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
				rb.AddForce ((Vector3.up * jumpImpulse * rb.mass), ForceMode.Impulse);
				startedJump = true;
			}
			if (rb.velocity.y < 0f && !currentAnim.inFall && delayed) {                //At the peak of the characters jump, the character will begin 'falling'
                if (isDJumping)
                    isDJumping = false;
                anim.ResetTrigger ("jumpNow");
				anim.ResetTrigger ("doubleJumpNow");
				anim.SetTrigger ("falling");
				jumping = false;
			}
		}
		if (prolongJumpDelay) {
			if(((pI.jumpHold && playerIndex == 1) || (pI.jumpHold2 && playerIndex == 2) || (pI.jumpHold3 && playerIndex == 3) || (pI.jumpHold4 && playerIndex == 4)))
				rb.AddForce(Vector3.up * rb.mass * (jumpImpulse/2f));
			else
				prolongJumpDelay = false;
		}
        if (prevYVelo != 0f && rb.velocity.y > prevYVelo && delayed && !onGround && rb.velocity.y > 0f)
            rb.velocity = new Vector3(rb.velocity.x, prevYVelo, rb.velocity.z);
        prevYVelo = rb.velocity.y;

//////////Sliding down a Slide
		if (slidingDownSlide) {
			Physics.Raycast(this.transform.position, -Vector3.up, out raySlide, cHeight, whatIsSlide);
			slideRelative.transform.forward = raySlide.normal;
			slideRelative.transform.RotateAround(slideRelative.transform.position, slideRelative.transform.right, 90f);
			slidingDir = slideRelative.transform.forward;
            this.transform.forward = Vector3.Lerp(this.transform.forward, new Vector3(slideRelative.transform.forward.x, 0f, slideRelative.transform.forward.z), Time.fixedDeltaTime * 5f);

			if(Vector3.Magnitude(direction) > 0.1f){
				int rotationSide = 0;
				float deltAngle = Mathf.DeltaAngle (cam.transform.localEulerAngles.y, slideRelative.transform.localEulerAngles.y);
				rotationSide = 1;
				if (deltAngle > 135f || deltAngle <= -135f)
					rotationSide = 3;
				if (deltAngle > 45f && deltAngle <= 135f)
					rotationSide = 4;
				if (deltAngle < -45f && deltAngle > -135f)
					rotationSide = 2;
                
                rb.velocity = Vector3.Lerp(rb.velocity, slideRelative.transform.forward * 3f, Time.fixedDeltaTime * 2f);

                if (leftY < -0.4f && rotationSide == 1 || leftY > 0.4f && rotationSide == 3 || 
					leftX > 0.4f && rotationSide == 4 || leftX < -0.4f && rotationSide == 2) {
                    if(rb.velocity.y < -0.5f)
    					rb.AddForce(-slidingDir * rb.mass * maxSpeed, ForceMode.Force);
                }
				if(leftY < -0.4f && rotationSide == 3 || leftY > 0.4f && rotationSide == 1 || 
					leftX > 0.4f && rotationSide == 2 || leftX < -0.4f && rotationSide == 4)
					rb.AddForce(slidingDir * rb.mass * maxSpeed * 2f, ForceMode.Force);
				if(leftY < -0.4f && rotationSide == 2 || leftY > 0.4f && rotationSide == 4 || 
					leftX > 0.4f && rotationSide == 1 || leftX < -0.4f && rotationSide == 3)
					rb.AddForce(this.transform.right * 1.5f * rb.mass, ForceMode.Force);
				if(leftY < -0.4f && rotationSide == 4 || leftY > 0.4f && rotationSide == 2 || 
					leftX > 0.4f && rotationSide == 3 || leftX < -0.4f && rotationSide == 1)
					rb.AddForce(-this.transform.right * 1.5f * rb.mass, ForceMode.Force);
			}
		}

//////////Gravity Control
        if (!onGround || !delayed)
			gravityDirection = new Vector3 (0f, Physics.gravity.y, 0f);													//Gravity always points downwards when not on the ground

		if (Physics.GetIgnoreLayerCollision (characterCollisionLayer, SlidesCollisionLayer) || crouchDelay || this.transform.parent != null)
			gravityDirection = Vector3.zero;																			//If that layer collision is being ignored during an animation then we don't want anything moving the collider
        
        if (rb.velocity.y > -20f) {
			if ((gravityDirection.x != 0f || gravityDirection.y != 0f) && direction == Vector3.zero && onGround && delayed && !jumping && !onIce && !slidingDownSlide && !isBoosting){
                rb.velocity = zeroVelocity;
            }else{
                if(slidingDownSlide)
                    rb.AddForce(gravityDirection * rb.mass * gravityIntensity * 0.05f, ForceMode.Force);
                else
                    rb.AddForce(gravityDirection * rb.mass * gravityIntensity * glideGravity, ForceMode.Force);                     //GRAVITY IS APPLIED HERE
            }
		}

        if (inGlide && !inWindCol)
            rb.velocity = Vector3.Lerp(rb.velocity, new Vector3(rb.velocity.x, -0.6f, rb.velocity.z), Time.fixedDeltaTime * 5f);

//////////Bounce
        if(bounce){
            onGround = false;
            this.transform.position += (Vector3.up * 0.1f);
            rb.velocity = zeroVelocity;
            rb.AddForce(Vector3.up * bounceHeight * rb.mass, ForceMode.Impulse);
            jumpNo = 0;
            anim.SetTrigger("jumpNow");
            Jump("");
            bounce = false;
        }

        //////////Misc
        if (onGround && !isHoldingLeaf && !attackBuffer && !jumping && !isDJumping && delayed) {
            if ((Input.GetButtonDown("Select") && playerIndex == 1) || (Input.GetButtonDown("Select2") && playerIndex == 2) || (Input.GetButtonDown("Select3") && playerIndex == 3) || (Input.GetButtonDown("Select4") && playerIndex == 4))
                Victory();
        }

        if (isPlaying || cancelInstrument || currentAnim.inVictory || currentAnim.inDefeated)
            rb.velocity = zeroVelocity;
    }

/// <summary>
/// /These following OnCollision functions deal with what sort of ground that the character is on and how it changes the gravity direction or behaviour of the characters movements.
	/// OCEnter is when the character lands on a collider and how it deals with it
	/// OCStay is when the character is moving on the ground or on a Slide and how it deals with different situations (and if the character is against a climbable object)
	/// OCExit is when the character leaves the ground and checks if it was just a small bump on the ground
/// </summary>
	private void OnCollisionEnter (Collision collision){
        ContactPoint cp = new ContactPoint();
        int cpIt = 0;
        while (cpIt < collision.contacts.Length){
            cp = collision.contacts[0];
            if (delayed && !onGround && !jumping && rb.velocity.y <= 0.01f){
                if(cp.otherCollider.gameObject.layer == bounceCollisionLayer) {
                    bounce = true;
                }else{ 
                    Physics.Linecast(cp.point + cp.normal, cp.point - cp.normal, out rayCollide1, whatIsGround);
                    if (rayCollide1.normal.y >= walkableSlideTolerence && (whatIsGround & 1 << cp.otherCollider.gameObject.layer) != 0 && !hasLandedThisFrame){
                        if (cp.otherCollider.gameObject.tag == "Elevator" && cp.otherCollider.gameObject != this.transform.parent){
                            this.transform.SetParent(cp.otherCollider.gameObject.transform);
                            curElevator = cp.otherCollider.gameObject;
                        }
                        if (cp.otherCollider.gameObject.tag != "Elevator" && this.transform.parent != null)
                            this.transform.SetParent(null);
                        Land(false);
                    }
                }
            }
            cpIt++;
        }
	}
    
	private void OnCollisionStay (Collision collision){
        ptCS = new ContactPoint ();
		iterateCS = 0;
		foundOneCS = false;
		onASlideCS = false;
		nxtToBoxCS = false;
		while (iterateCS < collision.contacts.Length && !foundOneCS){
            ptCS = collision.contacts [iterateCS];
			if ((ptCS.otherCollider.gameObject.layer == enemyCollisionLayer && ptCS.otherCollider.gameObject.GetComponent<EnemyHP> ().instantKill) && !defeated) {
				Defeat ();
				return;
			}
            if (ptCS.otherCollider.gameObject.layer == enemyCollisionLayer && !invinceFrames && !beenHit)
                GetHurt(ptCS.otherCollider.gameObject);
            Physics.Linecast (ptCS.point + ptCS.normal, ptCS.point - ptCS.normal, out rayCollide2, whatIsGround);
			if ((rayCollide2.normal.y > walkableSlideTolerence) && ((whatIsGround & 1 << ptCS.otherCollider.gameObject.layer) != 0) && !foundOneCS
                && (Vector3.Angle(ptCS.normal, Vector3.up) < (90f*walkableSlideTolerence))) {                     //If the character is moving on to Ground with a different gradient...
                foundOneCS = true;
                if (!Physics.Raycast(rayA.transform.position, -Vector3.up, cHeight * 0.5f, whatIsGround) && Vector3.Angle(ptCS.normal, Vector3.up) > 20f
                    && (currentAnim.locomotionBlend || currentAnim.inIdle)) {
                    foundOneCS = false;
                }
                if (foundOneCS && (ptCS.normal.x != 0f || ptCS.normal.z != 0f)) {
                    RaycastHit cliffCheck = new RaycastHit();
                    if (Physics.Raycast(new Vector3(this.transform.position.x, ptCS.point.y - 0.05f, this.transform.position.z), new Vector3(-ptCS.normal.x, 0f, -ptCS.normal.z), out cliffCheck, cRad, whatIsGround)) {
                        if (cliffCheck.normal.y == 0f && (Vector3.Angle(this.transform.forward, cliffCheck.normal) > 90f && Vector3.Magnitude(direction) != 0f || Vector3.Magnitude(direction) == 0f)) {
                            if (!Physics.Linecast(rayA.position, rayA.position - (Vector3.up * (cHeight * 0.425f)), whatIsGround) || Mathf.Abs(cliffCheck.point.y - this.transform.position.y) < 0.6f){
                                huggingWall = true;
                                foundOneCS = false;
                                StopCoroutine("Locking");
                                lockVelocity = true;
                                StartCoroutine("Locking");
                                cliffDir = cliffCheck.normal;
                            }
                        }
                    }
                }
                if (foundOneCS) {
                    if (ptCS.otherCollider.gameObject.tag == "Ice")
                        onIce = true;
                    else
                        onIce = false;
                }
			}
			if (ptCS.normal.y > 0.2f && (whatIsSlide & 1 << ptCS.otherCollider.gameObject.layer) != 0)                                                //If the character is about to move onto a Slide and the Slide isn't a vertical wall (basically if the gradient is less than 0.2f...
                onASlideCS = true;
			if (onASlideCS && foundOneCS)                                                                                                           //If the character is on both, they will only act like they're on Ground
                onASlideCS = false;
            if (!nxtToBoxCS && ptCS.otherCollider.gameObject.layer == moveableCollisionLayer && ptCS.normal.y == 0f && forwardAngle < 90f && Vector3.Angle(this.transform.forward, ptCS.normal) >= 170f){
                nxtToBoxCS = true;
                boxRB = ptCS.otherCollider.gameObject.GetComponent<Rigidbody>();
                if (boxRB.velocity == Vector3.zero && Vector3.Angle(this.transform.forward, ptCS.normal) == 180f) {
                    nxtToBoxCS = false;
                }
            }
            iterateCS += 1;
		}
        if (nxtToBoxCS && !againstMoveable)
            againstMoveable = true;
        if (!nxtToBoxCS && againstMoveable)
            againstMoveable = false;
		int newIterate = 0;
		hwpt = new ContactPoint ();
		while (!huggingWall && newIterate < collision.contacts.Length) {
			if (newIterate != iterateCS) {																											//Checks if character is hugging a wall and isn't the ground the character is on already
				hwpt = collision.contacts [newIterate];
                if ((whatIsSlide & 1 << hwpt.otherCollider.gameObject.layer) != 0 && hwpt.normal.y <= walkableSlideTolerence ||
					(whatIsGround & 1 << hwpt.otherCollider.gameObject.layer) != 0 && hwpt.normal.y <= walkableSlideTolerence) {
					huggingWall = true;
				} else
					huggingWall = false;
			} else
				huggingWall = false;
			if (!huggingWall)
				newIterate += 1;
		}
		if (foundOneCS) {                                                                                                             //If they're on Ground, they will be made to 'land' just in case
			if (prevGround == null || ptCS.otherCollider.gameObject != prevGround && ptCS.otherCollider.gameObject.GetComponent<MeshRenderer>() != null){
                string groundMaterial = ptCS.otherCollider.gameObject.GetComponent<MeshRenderer>().material.name;
                foundMat = false;
                CycleMaterials(grassMaterials, groundMaterial);
                if(!foundMat)
                    CycleMaterials(rockMaterials, groundMaterial);
                if (!foundMat)
                    CycleMaterials(snowMaterials, groundMaterial);
                if (!foundMat)
                    CycleMaterials(sandMaterials, groundMaterial);
                if (!foundMat)
                    CycleMaterials(woodMaterials, groundMaterial);
                if (!foundMat)
                    groundID = 5;
                prevGround = ptCS.otherCollider.gameObject;
            }
            if (ptCS.otherCollider.gameObject.tag != "Elevator" && this.transform.parent != null)
                this.transform.SetParent(null);
            if (slidingDownSlide && !onASlideCS)
                slidingDownSlide = false;
            if (!onGround && delayed) {
				onGround = true;
				if (currentAnim.inFall) {
					Land (false);
                }
			}
			InclineGravity ((ptCS.point - (this.transform.position + rb.centerOfMass)).normalized);							//The direction from the point of contact to the colliders center of mass is passed through to be used as the gravitys direction
		} else {
            if (huggingWall && onGround && !Physics.Linecast(rayA.position, rayA.position - (Vector3.up * (cHeight * 0.5f)))){
                onGround = false;
                onGFixed = false;
            }
            if (onASlideCS)
                slidingDownSlide = true;
            if (!onASlideCS){
                if (slidingDownSlide){
                    slidingDownSlide = false;
                    sound.loop = false;
                    sound.Stop();
                }
				if (!huggingWall || !Physics.Raycast(this.transform.position, -Vector3.up,0.6f*cHeight, whatIsGround))
					gravityDirection = new Vector3 (0f, Physics.gravity.y, 0f);
				if(rayCollide2.normal.y < walkableSlideTolerence && rayCollide2.normal.y > 0f){
					if(rb.velocity.y > 1f)
						rb.velocity = Vector3.Lerp(rb.velocity, zeroVelocity, Time.fixedDeltaTime *2f);
                }
			}
		}
	}

    
	private void OnCollisionExit (Collision collision){
        if (Physics.Linecast (this.transform.position, this.transform.position - (Vector3.up * cHeight), out rayCollide3, whatIsGround) && delayed) {			//This checks if the character has gone off ground but hasn't jumped or anything like that...
            if (rayCollide3.normal.y > walkableSlideTolerence) {
                if (rb.velocity.y > 0.3f && !jumping) {									//If it was just a bump in the ground that pushed the character off for a split second, this will push...
                    rb.velocity = new Vector3 (rb.velocity.x, -rb.velocity.y, rb.velocity.z);										//...the character back down to the ground to counter it
					gravityDirection = new Vector3 (0f, Physics.gravity.y, 0f);
				}
			}
		} else {
            if (this.transform.parent != null)
                this.transform.SetParent(null);
            onGround = false;
            onIce = false;
            onGFixed = false;
            if (huggingWall)
				huggingWall = false;
            if (jumpNo == 0 && !jumping && delayed && !slidingDownSlide)
                jumpNo = 1;
            StartCoroutine("OffGround");
            slidingDownSlide = false;
            sound.loop = false;
        }
	}

    void CycleMaterials(string[] matList, string matName){
        int groundIT = 0;
        while (groundIT < matList.Length && !foundMat) {
            if (matList[groundIT] == matName || matList[groundIT] + " (Instance)" == matName)
                foundMat = true;
            groundIT++;
        }
        if (foundMat){
            if (matList == grassMaterials)
                groundID = 0;
            if (matList == rockMaterials)
                groundID = 1;
            if (matList == snowMaterials)
                groundID = 2;
            if (matList == sandMaterials)
                groundID = 3;
            if (matList == woodMaterials)
                groundID = 4;
        }
    }


	void OnTriggerEnter(Collider other){
		if (other.gameObject.tag == "Boost") {
			isBoosting = true;
			StopCoroutine ("BoostLength");
			StartCoroutine ("BoostLength");
			sound.clip = boostSound;
			sound.loop = false;
			sound.PlayDelayed (0f);
			if (boostParticles != null) {
				boostEM.enabled = true;
				boostParticles.GetComponent<ParticleSystem> ().Play ();
				StartCoroutine (DeactivateParticle (boostParticles, boostEM));
			}
		}
		if (other.gameObject.layer == waterCollisionLayer && other.gameObject.tag == "Shallow"){
			if (!inWaterFS)
				inWaterFS = true;
		}
		if (other.gameObject.layer == windColumnColLayer){
			if (!inWindCol)
				inWindCol = true;
		}
	}

    void OnTriggerExit(Collider other) {
        if (inWaterFS)
            inWaterFS = false;
        if (inWindCol)
            inWindCol = false;
    }


    /// <summary>
    /// /This method uses the normal that is passed through it as the direction of gravity to act on the player
    /// </summary>
    private void InclineGravity (Vector3 thisNormal)
	{
		gravityFactor = 1f + ((1f - thisNormal.y) / 1.5f);											//The steeper the ground is, the larger the gravity factor will be
		if (thisNormal == Vector3.zero)															//If there's no normal, or it's equal to zero, then gravity points downwards
			thisNormal = Vector3.up;
		gravityDirection = -thisNormal * Physics.gravity.y * gravityFactor;						//!!the gravity direction is set as the normal of the ground times the factor times the y value of the gravity vector in the project settings
	}

	private void Jump (string type){
        int j = 0;
        while (j < jumpForces.Length) {
            if (jumpForces[j].jumpName == type) {
                jumpImpulse = jumpForces[j].forceAmount;
            }
            j++;
        }
		startedJump = false;
		jumpNo++;
		jumping = true;
        jumpST.Emit = true;
		prolongJumpDelay = true;
		delayed = false;
		anim.ResetTrigger ("land");
        anim.ResetTrigger("runningLand");
        anim.ResetTrigger ("falling");
		if (type == "default") {
			anim.SetTrigger ("jumpNow");
			jumpIterate = Random.Range (0,3);
            if (jumpIterate == 0)
				sound.clip = jump1Sound;
			if (jumpIterate == 1)
				sound.clip = jump2Sound;
			if (jumpIterate == 2)
				sound.clip = jump3Sound;
            sound.PlayDelayed(0f);
          //  jumpIterate = !jumpIterate;
            jumpEM.enabled = true;
            jumpParticles.GetComponent<ParticleSystem>().Play();
            StartCoroutine(DeactivateParticle(jumpParticles, jumpEM));
        }
        if (type == "double") {
            ExitGlide();
            if(leftY != 0f || leftX != 0f)
                this.transform.forward = (leftY * relative.transform.forward) + (leftX * relative.transform.right);
            rb.velocity = (this.transform.forward * Vector3.Magnitude(new Vector3(rb.velocity.x, 0f, rb.velocity.z)));
			anim.SetTrigger ("doubleJumpNow");
			doublejumpIterate = Random.Range (0,3);
			if (doublejumpIterate == 0)
				sound.clip = doublejump1Sound;
			if (doublejumpIterate == 1)
				sound.clip = doublejump2Sound;
            sound.PlayDelayed(0f);
            dJumpEM.enabled = true;
            doubleJumpParticles.GetComponent<ParticleSystem>().Play();
            StartCoroutine(DeactivateParticle(doubleJumpParticles, dJumpEM));
            isDJumping = true;
        }
        if (jumpNo > 0)
			StartCoroutine (Delay (1));
        cancelLeafHold = true;
	}

    private void Land(bool allow) {
        ExitGlide();
        if (beenHit && !invinceFrames){
            StartCoroutine("Invincibility");
            if (health == 0)
                Defeat();
            else
                StartCoroutine("RefillLife");
        }
        if (!onGround || onGround && allow){
            onGround = true;
            onGFixed = true;
            jumpST.Emit = false;
            if (!hasLandedThisFrame){
                hasLandedThisFrame = true;
                StartCoroutine("WaitForEnd");
            }
            if (prolongJumpDelay)
                prolongJumpDelay = false;
            if (Vector3.Magnitude(direction) > 0.4f)
                anim.SetTrigger("runningLand");
            else
                anim.SetTrigger("land");
            landEM.enabled = true;
            landParticles.GetComponent<ParticleSystem>().Play();
            if (!allow){
                sound.clip = landingSound;
                sound.PlayDelayed(0f);
            }
            StartCoroutine(DeactivateParticle(landParticles, landEM));
            anim.ResetTrigger("falling");
            StartCoroutine(QuickRumble(0.4f, 0.25f));
            StartCoroutine("FixedGround");
        }
	}

    private void ExitGlide(){
        inGlide = false;
        anim.ResetTrigger("enterGlide");
        anim.SetTrigger("exitGlide");
        if (glideGravity != 1f)
            glideGravity = 1f;
    }

    private void ExitPush() {
        isPushing = false;
        sound.loop = false;
        sound.Stop();
        anim.ResetTrigger("enterPush");
        anim.SetTrigger("exitPush");
    }

    private void CancelAttack(){
        if(!turnOffVibration)
           XInputDotNetPure.GamePad.SetVibration(pIx, 0f, 0f);
        startedAttack = false;
        attackPressOnly = false;
        isHoldingAttack = false;
        chargingAttack = false;
        attackBuffer = false;
        attackBufferX = false;
        leafST.Emit = false;
    }

    private void Warp() {
        this.transform.position = Vector3.up*1.8f;
    }

    private void Victory() {
        anim.SetTrigger("celebrate");
    }

    public void CollectedWater() {
        sound.clip = waterCatchSound;
        sound.PlayDelayed(0f);
    }

	public void PlayBerryNoise() {
		PlayerPrefs.SetInt("Berries", berryCount);
        berryIt = Random.Range(0, 4);
        if (berryIt == prevBerryIt) {
            berryIt += 1;
            if (berryIt == 4)
                berryIt = 0;
        }
        prevBerryIt = berryIt;
        interactions.clip = berrySounds[berryIt];
        interactions.PlayDelayed(0f);
    }

    public void GetHurt(GameObject enemyObj){
        //Physics.IgnoreLayerCollision(characterCollisionLayer, enemyCollisionLayer, true);
        ExitGlide();
        if (enemyObj.GetComponent<EnemyHP>().instantKill) {
            health = 0;
            Defeat();
        }else { 
            beenHit = true;
            LoseHealth(enemyObj.GetComponent<EnemyHP>().damageAmount);
            if (health == 0) {
                StopCoroutine("RefillLife");
                defeated = true;
				isBoosting = false;
                if (playerIndex == 1){
                    anim.SetLayerWeight(2, 1f);
                    anim.SetBool("damaged", true);
                
                }
            }else {
                if (playerIndex == 1){
                    anim.SetLayerWeight(2, 0.7f);
                    anim.SetBool("damaged", true);
                    StartCoroutine("LeafShrink");
                }
            }
            anim.SetTrigger("hurt");
            hitIterate++;
            if (hitIterate > 2){
                hitIterate = 0;
                sound.clip = hit1Sound;
            }
            if (hitIterate == 1)
                sound.clip = hit2Sound;
            if (hitIterate == 2)
                sound.clip = hit3Sound;
            sound.PlayDelayed(0f);
            lockVelocity = true;
            StartCoroutine("Locking");
            StartCoroutine(QuickRumble(0.7f, 0.3f));
            onGround = true;
            HurtVelo(enemyObj);
            StartCoroutine(PersistSpeed(enemyObj));
        }
    }

    public void AddHealth(int amount){
        health += amount;
        if (health > 2)
            health = 2;
    }

    public void LoseHealth(int amount){
        health -= amount;
        if (health < 0)
            health = 0;
    }

    public void MadeActive(){
   //     cam.AddMultiCharacter(this.gameObject);
    }

    public void MadeInnactive(){
     //   cam.RemoveMultiCharacter(this.gameObject);
    }

    void Defeat(){
        rb.velocity = zeroVelocity;
        rb.drag = 1000f;
        defeated = true;
		isBoosting = false;
        StopAllCoroutines();
        CancelAttack();
        leafAS.activeAttack = false;
        invinceFrames = true;
        anim.SetTrigger("fail");
        StartCoroutine("ResetWoodle");
        sound.clip = defeatSound;
        sound.PlayDelayed(0f);
    }

    IEnumerator Checkpoint(){
        yield return new WaitForSeconds(3);
        this.transform.position = new Vector3(checkpointx, checkpointy, checkpointz);
       // StartCoroutine("ResetWoodle");
    }

    void HurtVelo(GameObject enemyObj){
        rb.velocity = zeroVelocity;
        rb.position += Vector3.up * cHeight * 0.3f;
        this.transform.LookAt(new Vector3(enemyObj.transform.position.x, this.transform.position.y, enemyObj.transform.position.z));
        gravityDirection = Vector3.zero;
        rb.velocity = -(this.transform.forward * 2f) + (Vector3.up * 3f);
    }

    void WBInst(int index, GameObject[] wbArray, GameObject wBType){
        wbArray[index] = Instantiate(wBType, Vector3.zero, this.transform.rotation) as GameObject;
        wbArray[index].gameObject.name = this.gameObject.name + " Wind Ball Projectile " + index.ToString();
        wbArray[index].AddComponent<AttackSettings>();
        wbArray[index].GetComponent<AttackSettings>().attackAmount = 2;
        wbArray[index].GetComponent<AttackSettings>().activeAttack = true;
        wbArray[index].GetComponent<AttackSettings>().enemyLayer = enemyCollisionLayer;
        wbArray[index].AddComponent<AudioSource>();
        AudioSource wBAS = wbArray[index].GetComponent<AudioSource>();
        wBAS.spread = 190;
        wBAS.rolloffMode = AudioRolloffMode.Linear;
        wBAS.clip = windballSound;
        wbArray[index].gameObject.SetActive(false);
    }

    void RecalculateControllers(){
        if (!p1KeyboardOnly) {
            if (playerIndex == 1)
                pIx = PlayerIndex.One;
            if (playerIndex == 2)
                pIx = PlayerIndex.Two;
            if (playerIndex == 3)
                pIx = PlayerIndex.Three;
            if (playerIndex == 4)
                pIx = PlayerIndex.Four;
        }else {
            if (playerIndex == 2)
                pIx = PlayerIndex.One;
            if (playerIndex == 3)
                pIx = PlayerIndex.Two;
            if (playerIndex == 4)
                pIx = PlayerIndex.Three;
        }
    }

    public void CheckCamDist(){
        if (this.rb.isKinematic == false && Vector3.Distance(this.transform.position, GameObject.Find("Woodle Character").gameObject.transform.position) > 12f)
            StartCoroutine("TeleportToWoodle");
    }

    IEnumerator TeleportToWoodle(){
        this.rb.isKinematic = true;
        int t = 0;
        while(t < 30){
            this.transform.position = Vector3.Lerp(this.transform.position, GameObject.Find("Woodle Character").gameObject.transform.position + (Vector3.up * 1.5f), t/29f);
            yield return new WaitForEndOfFrame();
            t++;
        }
        this.rb.isKinematic = false;
    }

    IEnumerator PersistSpeed(GameObject enemyObj){
        yield return new WaitForSeconds(Time.deltaTime * 4f);
        onGround = true;
        HurtVelo(enemyObj);
    }

    IEnumerator ResetWoodle(){
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(Checkpoint());
        rb.drag = 0f;
        yield return new WaitForSeconds(5f);
        health = 2;
        defeated = false;
        StartCoroutine("Invincibility");
        lockVelocity = false;
        anim.Play("Idle", 0);
        if (playerIndex == 1){
            anim.SetBool("damaged", false);
        }
    }

    IEnumerator Invincibility(){
        beenHit = false;
        Physics.IgnoreLayerCollision(characterCollisionLayer, enemyCollisionLayer, false);
        invinceFrames = true;
        yield return new WaitForSeconds(Time.deltaTime * invinceFramesAmount);
        invinceFrames = false;
    }

    IEnumerator RefillLife(){
        yield return new WaitForSeconds(5f);
        AddHealth(1);
        sound.clip = leafGrow;
        sound.PlayDelayed(0f);
    }

    IEnumerator LeafShrink() {
        yield return new WaitForSeconds(5f);
        anim.SetBool("damaged", false);
    }

    IEnumerator Locking() {
        yield return new WaitForSeconds(Time.deltaTime * 30f);
        lockVelocity = false;
    }
	
    IEnumerator WaitForEnd() {
        yield return new WaitForEndOfFrame();
        hasLandedThisFrame = false;
    }

    IEnumerator OffGround(){
        if (!jumping && !slidingDownSlide) {
            int j = 0;
            while (j < 10){
                yield return new WaitForEndOfFrame();
                j++;
            }
            if (!onGround)
                jumpNo = 1;
        }
    }

    IEnumerator Charging() {
        if (leafNo != 4){
           
            yield return new WaitForSeconds(0.3f);
            if(!turnOffVibration)
                XInputDotNetPure.GamePad.SetVibration(pIx, 0.35f, 0f);
            if (leafNo == 1 || leafNo == 2)
                yield return new WaitForSeconds(0.3f);
        }
        if (chargingAttack) {
            anim.SetTrigger("releaseAttack");
            chargingAttack = false;
            yield return new WaitForSeconds(0.5f);
            if (!turnOffVibration)
                XInputDotNetPure.GamePad.SetVibration(pIx, 0f, 0f);
            sound.clip = attackChargedSound;
            sound.PlayDelayed(0f);
            yield return new WaitForSeconds(0.2f);
            int wb = 0;
            bool found = false;
            if ((leafNo == 1 && playerIndex == 1) || playerIndex == 2 || playerIndex == 3 || playerIndex == 4) {
                while (wb < windBalls.Length && !found){
                    if (windBalls[wb].gameObject.activeSelf == false)
                        found = true;
                    else
                        wb++;
                }
                windBalls[wb].gameObject.SetActive(true);
                windBalls[wb].transform.position = leafBone.transform.position;
                windBalls[wb].GetComponent<Rigidbody>().velocity = this.transform.forward * windBall1Speed;
                StartCoroutine(WindBall(wb, windBalls, windBall1Lifetime));
                windBalls[wb].GetComponent<AudioSource>().Play();
            }
            if (leafNo == 2 && playerIndex == 1) {
                while (wb < windBalls2.Length && !found){
                    if (windBalls2[wb].gameObject.activeSelf == false)
                        found = true;
                    else
                        wb++;
                }
                windBalls2[wb].gameObject.SetActive(true);
                windBalls2[wb].transform.position = leafBone.transform.position;
                windBalls2[wb].GetComponent<Rigidbody>().velocity = this.transform.forward * windBall2Speed;
                StartCoroutine(WindBall(wb, windBalls2, windBall2Lifetime));
                windBalls2[wb].GetComponent<AudioSource>().Play();
            }
            if (leafNo == 3 && playerIndex == 1) {
                while (wb < windBalls3.Length && !found){
                    if (windBalls3[wb].gameObject.activeSelf == false)
                        found = true;
                    else
                        wb++;
                }
                windBalls3[wb].gameObject.SetActive(true);
                windBalls3[wb].transform.position = leafBone.transform.position;
                windBalls3[wb].GetComponent<Rigidbody>().velocity = this.transform.forward * windBall3Speed;
                StartCoroutine(WindBall(wb, windBalls3, windBall3Lifetime));
                windBalls3[wb].GetComponent<AudioSource>().Play();
            }
            if (leafNo == 4 && playerIndex == 1) {
                while (wb < windBalls4.Length && !found){
                    if (windBalls4[wb].gameObject.activeSelf == false)
                        found = true;
                    else
                        wb++;
                }
                windBalls4[wb].gameObject.SetActive(true);
                windBalls4[wb].transform.position = leafBone.transform.position;
                windBalls4[wb].GetComponent<Rigidbody>().velocity = this.transform.forward * windBall4Speed;
                StartCoroutine(WindBall(wb, windBalls4, windBall4Lifetime));
                windBalls4[wb].GetComponent<AudioSource>().Play();
            }
            CancelAttack();
        }
    }

    IEnumerator QuickRumble(float intensity, float length){
        if (!turnOffVibration)
            XInputDotNetPure.GamePad.SetVibration(pIx, intensity, 0f);
        yield return new WaitForSeconds(length);
        if (!turnOffVibration)
            XInputDotNetPure.GamePad.SetVibration(pIx, 0f, 0f);
    }

    IEnumerator WindBall(int index, GameObject[] wbArray, float lifetime){
        if (!turnOffVibration)
            XInputDotNetPure.GamePad.SetVibration(pIx, 0.35f, 0f);
        yield return new WaitForSeconds(0.3f);
        if (!turnOffVibration)
            XInputDotNetPure.GamePad.SetVibration(pIx, 0f, 0f);
        yield return new WaitForSeconds(lifetime);
        wbArray[index].gameObject.SetActive(false);
    }

    IEnumerator Buffer() {
        int a = 0;
		while(a < 16 && !letGoOfAttack){
            yield return new WaitForEndOfFrame();
            a++;
        }
        attackBufferX = true;
        if (((pI.attackHold && playerIndex == 1) || (pI.attackHold2 && playerIndex == 2) || (pI.attackHold3 && playerIndex == 3) || (pI.attackHold4 && playerIndex == 4)) && !letGoOfAttack) {
            if (((pI.attackHold && playerIndex == 1) || (pI.attackHold2 && playerIndex == 2) || (pI.attackHold3 && playerIndex == 3) || (pI.attackHold4 && playerIndex == 4)) && !letGoOfAttack)
                isHoldingAttack = true;
            else
                attackPressOnly = true;
        }else
            attackPressOnly = true;
        if(leafNo == 4) {
            isHoldingAttack = false;
            attackPressOnly = true;
        }
    }

    IEnumerator DeactiveAttack() {
        yield return new WaitForSeconds(0.4f);
        if (!turnOffVibration)
            XInputDotNetPure.GamePad.SetVibration(pIx, 0.35f, 0f);
        yield return new WaitForSeconds(0.15f);
        leafAS.activeAttack = false;
        CancelAttack();
        yield return new WaitForSeconds(0.25f);
        if (!turnOffVibration)
            XInputDotNetPure.GamePad.SetVibration(pIx, 0f, 0f);
    }

    IEnumerator PlayingInstrument(){
        yield return new WaitForSeconds(3.6f);
        isPlaying = false;
        cancelInstrument = true;
        StartCoroutine("CancelInstrument");
        Warp();
    }

    IEnumerator CancelInstrument(){
        anim.SetTrigger("cancelMusic");
        instrumentEM.enabled = false;
        yield return new WaitForSeconds(0.5f);
        cancelInstrument = false;
    }

    IEnumerator DeactivateParticle(GameObject pObj, ParticleSystem.EmissionModule tempEM){
        yield return new WaitForSeconds(pObj.GetComponent<ParticleSystem>().duration + 0.1f);
        tempEM.enabled = false;
    }

    IEnumerator ClipDelay() {
        if (Vector3.Magnitude(direction) <= 0.75f)
            yield return new WaitForSeconds(0.45f);
        else
            yield return new WaitForSeconds(0.3f);
   /*     if (((pI.runHold && playerIndex == 1) || (pI.runHold2 && playerIndex == 2) || (pI.runHold3 && playerIndex == 3) || (pI.runHold4 && playerIndex == 4)))
            yield return new WaitForSeconds(0.3f);
        else
            yield return new WaitForSeconds(0.45f); */
        stepDelay = false;
    }

    /// <summary>
    /// /This IEnumerator delays certain bools from turning true/false depending on the situation
    /// </summary>
    IEnumerator Delay (int delayNo){
		if (delayNo == 1) {
            yield return new WaitForSeconds (0.2f);
			delayed = true;
			yield return new WaitForSeconds (1f);
			prolongJumpDelay = false;
		}
	}

	IEnumerator BoostLength(){
		yield return new WaitForSeconds (4f);
		isBoosting = false;
	}

    IEnumerator FixedGround(){
        int f = 0;
        while (f < 10){
            yield return new WaitForEndOfFrame();
            f++;
        }
        onGFixed = false;
    }

	IEnumerator AttackNext(){
		yield return new WaitForSeconds (1f);
		attackAnimIterate = 0;
	}
}
