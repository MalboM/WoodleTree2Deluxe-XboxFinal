using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Rewired;
using PigeonCoopToolkit.Effects.Trails;

public class TPC : MonoBehaviour
{

    [HideInInspector] public Player input;
    public string playerID;
    [HideInInspector] public int pID;

    public float distanceForMultiChara = 80f;
    GameObject woodle;

    public Transform initialParent;

    [HideInInspector] public float checkpointx;
    [HideInInspector] public float checkpointy;
    [HideInInspector] public float checkpointz;

    [HideInInspector] public bool disableControl;
    public GameObject dayPack;
    [HideInInspector] public GameObject cam;
    CameraFollower camF;
    [HideInInspector] public GameObject relative;
    [HideInInspector] public Rigidbody rb;
    Animations currentAnim;
    [HideInInspector] public Animator anim;
    [HideInInspector] public CapsuleCollider capcol;
    [HideInInspector] public bool inShop;

    [HideInInspector] public BoxCollider box;
    [HideInInspector] public float boxX;
    [HideInInspector] public float boxY;
    [HideInInspector] public float boxZ;
    [HideInInspector] public Vector3 boxCentre;
    public Projector shadowProjector;

    public Collider shieldCol;

    public BerryPFX berryPFX;

    public PauseScreen ps;
    bool startedCharacterRespawn;
    Telescope telescope;

    public GameObject checkSea;
    RaycastHit seaHit;
    bool hitSea;

    [HideInInspector] public bool inButtonCutscene;
    [HideInInspector] public bool beingReset;

    [HideInInspector] public ChallengePortal challengePortal;

    //

    [HideInInspector] public float leftX;
    [HideInInspector] public float leftY;
    [HideInInspector] public Vector3 direction;
    [HideInInspector] public bool waterPhysics;
    [HideInInspector] public bool slowMovement;
    [HideInInspector] public bool superslowMovement;
    [HideInInspector] public bool noDirectional;
    [HideInInspector] public bool onSpline;

    //

    public float gravityForce;
    public float groundSpeed;
    public float sprintSpeed;
    public float boostSpeed;
    public float slideSpeed;
    public bool onGround;
    bool hardLanded;
    bool landed;
    bool onIncline;
    float locoSpeed;
    Vector3 inclineNormal;
    float tempY;
    bool onSlide;
    bool endingSlide;
    float turnSpeed;
    float movementSpeed;
    GameObject[] groundCheckers = new GameObject[4];
    GameObject lookAt;
    int lookInt;
    Vector3 lookLast;
    Vector3[] slopeCheckers = new Vector3[4];
    RaycastHit[] groundHits = new RaycastHit[4];
    RaycastHit[] slideHits = new RaycastHit[4];
    bool[] slideCheckers = new bool[4];
    bool onIce;
    [HideInInspector] public bool isBoosting;
    float boostRandom;
    bool bounce;
    bool bounceSuper;
    bool bounceNinfea;
    bool bounceDrums;
    Vector3 bounceDir;
    int gv;
    bool gvFound;
    bool gvIncline;
    bool gvSlide;
    bool gvIce;
    Vector3 slideHigh;

    bool startedGroundDelay;
    bool canGlideFromGround;

    [HideInInspector] public bool runByDefault;

    [HideInInspector] public bool jumped;
    bool slowedJump;
    bool doubleJumped;
    [HideInInspector] public bool delayed;
    bool jumpChance;
    bool fallNow;
    public float jumpImpulse;
    public float doubleJumpImpulse;
    public float glideSpeed;
    public float windColumnSpeed;
    public float waterColumnSpeed;
    public float bambooSpeed;
    [HideInInspector] public bool gliding;
    float fallHeight;

    [Range(0f, 1f)] public float groundTolerance;

    [HideInInspector] public bool inWindCol;
    //   [HideInInspector] public bool inWaterCol;
    [HideInInspector] public bool jumping;
    bool inBamboo;
    float curBambooSpeed;
    BoxCollider currentWindCol;

    Collider currentOneWay;
    RaycastHit oneWayHit;

    public bool hasTripleJump;
    bool hasTripJumped;

    bool glideDelay;

    bool firstWallJumped;

    public bool hasLeafSlide;
    [HideInInspector] public bool inLeafSlide;
    public float leafSlideSpeed;

    public bool hasSuperLeaf;
    public bool hasUltraLeaf;

    bool inIdleRoutine;

    //

    [SerializeField] private GameObject leafBone;
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
    private GameObject[] windBalls = new GameObject[16];
    private GameObject[] windBalls2 = new GameObject[16];
    private GameObject[] windBalls3 = new GameObject[16];
    private GameObject[] windBalls4 = new GameObject[16];
    GameObject wbPar1;
    GameObject wbPar2;
    GameObject wbPar3;
    GameObject wbPar4;
    private GameObject leafColObj;
    private LeafCollision leafCol;
    [HideInInspector] public AttackSettings leafAS;
    [HideInInspector] public int leafNo;
    [HideInInspector] public bool isHoldingLeaf;
    [HideInInspector] public bool cancelLeafHold;
    bool attackBuffer;
    bool startedAttack;
    [HideInInspector] public bool defeated;
    bool letGoOfAttack;
    bool attackBufferX;
    bool isHoldingAttack;
    bool attackPressOnly;
    bool chargingAttack;
    bool fullyCharged;
    bool canAttackNext;
    MeshRenderer curGroundMR;
    bool checkingAttackAnim;
    bool inHitFreeze;

    //

    [HideInInspector] public int berryCount;
    [HideInInspector] public int blueberryCount;
    private int berryIt;
    private int prevBerryIt;
    [HideInInspector] public bool wearingMask;
    [HideInInspector] public bool holdingItem;
    [HideInInspector] public bool wearingHat;

    //

    bool invinceFrames;
    bool beenHit;
    [HideInInspector] public int health;
    bool lockVelocity;
    bool killedByDark;

    //

    bool pushing;
    RaycastHit pushRay;

    //

    bool isPlaying;
    bool cancelInstrument;

    //

    public LayerMask whatIsGround;
    public LayerMask whatIsSlide;
    public LayerMask whatIsMoveable;
    public LayerMask whatIsFootPrinted;
    public LayerMask whatIsSea;
    public LayerMask whatIsShield;
    public int leafCollisionLayer;
    public int waterfallCollisionLayer;
    public int enemyCollisionLayer;
    public int windColumnColLayer;
    public int characterCollisionLayer;
    public int waterCollisionLayer;
    public int bouncyLayer;
    public int bouncySuperLayer;

    float waterFSHeight;
    //

    private AudioSource sound;
    private AudioSource atkSound;
    private AudioSource footSteps;
    private AudioSource interactions;
    private AudioSource enHit1Sound;
    private AudioSource enHit2Sound;
    private AudioSource enHit3Sound;
    private AudioSource boostingSound;
    private AudioSource bbFinderSource;
    private bool stepDelay;
    private int stepInt;
    private int prevStepInt;
    bool inWaterFS;
    [SerializeField] private AudioClip[] grassFootSteps = new AudioClip[4];
    [SerializeField] private AudioClip[] rockFootSteps = new AudioClip[4];
    [SerializeField] private AudioClip[] snowFootSteps = new AudioClip[4];
    [SerializeField] private AudioClip[] sandFootSteps = new AudioClip[4];
    [SerializeField] private AudioClip[] woodFootSteps = new AudioClip[4];
    [SerializeField] private AudioClip[] groundFootSteps = new AudioClip[4];
    [SerializeField] private AudioClip[] waterFootSteps = new AudioClip[4];
    public AudioClip bounceSound;
    public AudioClip bounceSuperSound;
    public AudioClip bounceNinfeaSound;
    private AudioClip tempStep;
    int groundID;
    private string[] grassMaterials = new string[34];
    private string[] rockMaterials = new string[13];
    private string[] snowMaterials = new string[4];
    private string[] iceMaterials = new string[2];
    private string[] sandMaterials = new string[7];
    private string[] woodMaterials = new string[10];
    private string[] nonWallJumpMaterials = new string[5];
    bool foundMat;
    GameObject prevGround;

    //

    [SerializeField] private GameObject dustWalkParticles;
    [SerializeField] private GameObject dustRunParticles;
    [SerializeField] private GameObject splashMoveParticles;
    [SerializeField] private GameObject rippleParticles;
    [SerializeField] private GameObject rippleCParticles;
    [SerializeField] private GameObject landParticles;
    [SerializeField] private GameObject instrumentParticles;
    [SerializeField] private GameObject hitGParticles;
    [SerializeField] private GameObject hitYParticles;
    [SerializeField] private GameObject hitRParticles;
    [SerializeField] private GameObject hitWParticles;
    [SerializeField] private GameObject knockedParticles;
    [SerializeField] private GameObject attackParticles;
    [SerializeField] private GameObject chargedParticles;
    [SerializeField] private GameObject leafRefilGParticles;
    [SerializeField] private GameObject leafRefilYParticles;
    [SerializeField] private GameObject leafRefilRParticles;
    [SerializeField] private GameObject leafRefilWParticles;
    [SerializeField] private GameObject wallGParticles;
    [SerializeField] private GameObject wallYParticles;
    [SerializeField] private GameObject wallRParticles;
    [SerializeField] private GameObject wallWParticles;
    [SerializeField] private GameObject blockStarParticles;
    [SerializeField] private GameObject jumpParticles;
    [SerializeField] private GameObject doubleJumpParticles;
    [SerializeField] private GameObject killedParticles;
    [SerializeField] private GameObject splashParticles;
    [SerializeField] private GameObject boostParticles;
    [SerializeField] private GameObject boostFlowerParticles;
    [SerializeField] private GameObject boostingParticles;
    [SerializeField] private GameObject leafSlidingParticles;
    [SerializeField] private GameObject enemyParticles;
    [SerializeField] private GameObject buttonParticles;

    [SerializeField] private GameObject bbFinderParticles;
    [SerializeField] private GameObject portal1Particles;
    [SerializeField] private GameObject portal2Particles;

    private ParticleSystem.EmissionModule walkEM;
    private ParticleSystem.EmissionModule runEM;
    private ParticleSystem.EmissionModule splashMoveEM;
    private ParticleSystem.EmissionModule rippleEM;
    private ParticleSystem.EmissionModule rippleCEM;
    private ParticleSystem.EmissionModule landEM;
    private ParticleSystem.EmissionModule instrumentEM;
    private ParticleSystem.EmissionModule hitGEM;
    private ParticleSystem.EmissionModule hitYEM;
    private ParticleSystem.EmissionModule hitREM;
    private ParticleSystem.EmissionModule hitWEM;
    private ParticleSystem.EmissionModule knockedEM;
    private ParticleSystem.EmissionModule attackEM;
    private ParticleSystem.EmissionModule chargedEM;
    private ParticleSystem.EmissionModule refilGEM;
    private ParticleSystem.EmissionModule refilYEM;
    private ParticleSystem.EmissionModule refilREM;
    private ParticleSystem.EmissionModule refilWEM;
    private ParticleSystem.EmissionModule wallGEM;
    private ParticleSystem.EmissionModule wallYEM;
    private ParticleSystem.EmissionModule wallREM;
    private ParticleSystem.EmissionModule wallWEM;
    private ParticleSystem.EmissionModule blockStarEM;
    private ParticleSystem.EmissionModule jumpEM;
    private ParticleSystem.EmissionModule dJumpEM;
    private ParticleSystem.EmissionModule killedEM;
    private ParticleSystem.EmissionModule splashEM;
    private ParticleSystem.EmissionModule boostEM;
    private ParticleSystem.EmissionModule boostFlowerEM;
    private ParticleSystem.EmissionModule boostingEM;
    private ParticleSystem.EmissionModule leafSlidingEM;
    private ParticleSystem.EmissionModule enemyEM;

    private ParticleSystem.EmissionModule bbFinderEM;
    private ParticleSystem.EmissionModule portal1EM;
    private ParticleSystem.EmissionModule portal2EM;

    //

    [SerializeField] private AudioClip attack1Sound;
    [SerializeField] private AudioClip attack2Sound;
    [SerializeField] private AudioClip attack3Sound;
    [SerializeField] private AudioClip attack4Sound;
    private int attackIterate;
    private int attackAnimIterate;
    bool isThirdAttack;
    [SerializeField] private AudioClip attackChargedSound;
    [SerializeField] private AudioClip attackFullyChargedSound;
    [SerializeField] private AudioClip attackChargingSound;
    [SerializeField] private AudioClip hit1Sound;
    [SerializeField] private AudioClip hit2Sound;
    [SerializeField] private AudioClip hit3Sound;
    [SerializeField] private AudioClip enemyHitSound;
    [SerializeField] private AudioClip enemyHitSound2;
    [SerializeField] private AudioClip enemyHitSound3;
    [SerializeField] private AudioClip enemyDefendSound;
    [SerializeField] private AudioClip enemyDefendSound2;
    [SerializeField] private AudioClip enemyDefendSound3;
    private int hitIterate;
    [SerializeField] private AudioClip jump1Sound;
    [SerializeField] private AudioClip jump2Sound;
    [SerializeField] private AudioClip jump3Sound;
    private int jumpIterate;
    [SerializeField] private AudioClip doublejump1Sound;
    [SerializeField] private AudioClip doublejump2Sound;
    [SerializeField] private AudioClip triplejump1Sound;
    [SerializeField] private AudioClip triplejump2Sound;
    private int doublejumpIterate;
    [SerializeField] private AudioClip wallKickSound;
    [SerializeField] private AudioClip landingSound;
    [SerializeField] private AudioClip landingSound2;
    [SerializeField] private AudioClip landingSound3;
    int landingSoundIterate;
    [SerializeField] private AudioClip leafUpSound;
    [SerializeField] private AudioClip waterCatchSound;
    [SerializeField] private AudioClip windballSound;
    [SerializeField] private AudioClip defeatSound;
    [SerializeField] private AudioClip defeatCollideSound;
    [SerializeField] private AudioClip bongoSound;
    [SerializeField] private AudioClip leafShrink;
    [SerializeField] private AudioClip leafGrow;
    [SerializeField] private AudioClip sliding;
    [SerializeField] private AudioClip boxPushing;
    [SerializeField] private AudioClip boostSound;
    [SerializeField] private AudioClip ledgeClimbSound;
    [SerializeField] private AudioClip enterLeafSlide;
    [SerializeField] private AudioClip leafSliding;
    [SerializeField] private AudioClip leafWallHit;
    [SerializeField] private AudioClip victorySound;
    [SerializeField] private AudioClip[] splashSounds;
    [SerializeField] private AudioClip respawnSound;
    [SerializeField] private AudioClip freezeSound;
    [SerializeField] private AudioClip unfreezeSound;

    //

    [SerializeField] private GameObject jumpTrail;
    [SerializeField] private GameObject leafTrail;
    [SerializeField] private GameObject runTrail;
    [SerializeField] private GameObject leafSlideTrail;

    private SmokeTrail jumpST;
    private SmokeTrail leafST;
    private SmokeTrail runST;
    private SmokeTrail leafSlideST;

    //

    [HideInInspector] public Vector3 zeroVelocity;

    //

    RaycastHit rrr;
    GameObject curFloor;
    GameObject curBox;
    RaycastHit boxChecker;

    //

    public GameObject checkLedge;
    RaycastHit ledgeChecker;
    bool ledgeHopping;

    //

    public SkinnedMeshRenderer[] bodyRenderers;
    public SkinnedMeshRenderer[] toHideRender;
    bool curOpaque;
    public SkinnedMeshRenderer leafMesh;
    public Material greyLeaf;
    public Material whiteLeaf;
    Material curLeafMat;

    //

    public GameObject leftFoot;
    public GameObject rightFoot;
    RaycastHit leftRay;
    RaycastHit rightRay;
    bool leftAboveZero;
    bool rightAboveZero;
    int leftInt;
    int rightInt;
    public GameObject[] leftFPs = new GameObject[5];
    public GameObject[] rightFPs = new GameObject[5];

    //

    bool againstWall;
    RaycastHit wallJumpRay;
    bool startedWallCheck;

    //

    public Animator berryHUD;
    public Animator blueberryHUD;
    public Animator compassHUD;
    public Animator tearHUD;
    public Animator itemHUD;
    public Text berryText;
    public Text blueberryText;
    public Text tearText;
    public Text itemText;

    [HideInInspector] public bool inCutscene;

    public Renderer eyesRenderer;
    public Texture eyesOpen;
    public Texture eyesClosed;
    /*
    bool inStoneMode;
    bool stoneModeFirstFrame;
    public AttackSettings stoneAttack;
    */

    [HideInInspector] public bool jumpButtonDown;
    bool glideButtonDown;
    bool runButtonDown;
    bool jumpButtonHold;
    bool glideButtonHold;
    bool runButtonHold;

    RiverForce currentRiverForce;
    bool inRiverForce;
    Vector3 riverForce;
    float rfMultiplier;

    //

    [HideInInspector] public bool flowerPower;
    [HideInInspector] public bool umbrellaPower;
    float umbrellaDescent;

    [HideInInspector] public bool bandanaPower;
    [HideInInspector] public bool acornPower;
    [HideInInspector] public bool elfPower;
    [HideInInspector] public bool bbPower;

    [HideInInspector] public bool chubbyPower;
    [HideInInspector] public bool elderPower;
    [HideInInspector] public bool goofyPower;

    //

    bool startedGlideTimer;
    bool glidedTooLong;
    float gTimer;

    //

    [HideInInspector] public int slowCount;
    [HideInInspector] public int quickSandCount;

    //

    int f = 1;
    bool reverse = false;
    float iterator = 0f;
    int mCount = 0;
    Color endCol = new Color(1f, 0f, 0f, 0f);
    public Renderer[] renderers;
    List<Color> origRimCol = new List<Color>();
    List<float> origRimInt = new List<float>();

    //

    [HideInInspector] public bool frozen;
    bool freezeInvince;

    [HideInInspector] public Collider currentWaterfall;

    [System.Serializable] public class DissolveInfo { public Renderer renderer; public Material[] origMaterials; public Material[] dissolveMaterials; }
    public DissolveInfo[] dissolves;
    public Material greenLeafDissolve;
    public Material yellowLeafDissolve;
    public Material redLeafDissolve;
    public Material blueLeafDissolve;
    [HideInInspector] public bool challengeWarping;

    List<BlueBerryIDCreator> blueBerryIDCreators;
    public AudioClip bbFinderSound;
    public AudioClip bbFinderONSound;
    public AudioClip bbFinderOFFSound;
    public Animator bbFinderAnim;
    bool usingBBFinder;
    float bbFinderIntensity;
    public MeshRenderer bbFinderMeshRenderer;
    Material bbFinderMat;

    [System.Serializable] public class SMRColors { public Color tint; public Color rim; public float sharpness; public float intensity; }
    List<SMRColors> smrColors = new List<SMRColors>();

    public GameObject noIntObj;
    public GameObject yesIntObj;

    #region ANDREA RICCARDI ADDONS

    public void CollectBerry()
    {
        berryCount++;
    }

    void Awake()
    {
        //    PlayerManager.AddPlayer(this, int.Parse(playerID));
    }

    #endregion

    void Start()
    {
        input = ReInput.players.GetPlayer(playerID);
        pID = int.Parse(playerID);

        if (pID != 0)
        {
            distanceForMultiChara *= distanceForMultiChara;
            woodle = PlayerManager.GetMainPlayer().gameObject;
        }

        inCutscene = true;

        rb = this.gameObject.GetComponent<Rigidbody>();
        capcol = this.gameObject.GetComponent<CapsuleCollider>();

        if (this.gameObject.GetComponent<Telescope>())
            telescope = this.gameObject.GetComponent<Telescope>();

        boxX = 0.3f;
        boxY = 0.9f;
        boxZ = 0.3f;
        boxCentre = new Vector3(0f, -0.15f, 0.135f);

        anim = this.GetComponentInChildren<Animator>();

        currentAnim = this.GetComponent<Animations>();
        currentAnim.anim = this.GetComponentInChildren<Animator>();

        berryCount = PlayerPrefs.GetInt("Berries", 0);
        berryText.text = berryCount.ToString();
        blueberryCount = PlayerPrefs.GetInt("BlueBerries", 0);
        blueberryText.text = blueberryCount.ToString();
        if (tearText != null)
        {
            ps.CheckTears();
            if (ps.tearCount < 10)
                tearText.text = "0" + ps.tearCount.ToString();
            else
                tearText.text = ps.tearCount.ToString();
        }
        if (itemText != null)
            itemText.text = PlayerPrefs.GetInt("CollectablesTotal", 0).ToString();

        GameObject followMe = new GameObject();
        followMe.name = "FollowMe";
        followMe.transform.parent = this.transform;
        followMe.transform.localPosition = Vector3.zero;

        lookAt = new GameObject(this.gameObject.name + " Look At");

        groundCheckers[0] = new GameObject("Check1");
        groundCheckers[0].transform.parent = this.transform;
        groundCheckers[0].transform.localPosition = boxCentre - (Vector3.up * boxY * 0.49f);

        groundCheckers[1] = new GameObject("Check2");
        groundCheckers[1].transform.parent = this.transform;
        groundCheckers[1].transform.localPosition = boxCentre - (Vector3.up * boxY * 0.49f) + (this.transform.forward * boxZ / 2f);

        groundCheckers[2] = new GameObject("Check3");
        groundCheckers[2].transform.parent = this.transform;
        groundCheckers[2].transform.localPosition = boxCentre - (Vector3.up * boxY * 0.49f);

        groundCheckers[3] = new GameObject("Check4");
        groundCheckers[3].transform.parent = this.transform;
        groundCheckers[3].transform.localPosition = boxCentre - (Vector3.up * boxY * 0.49f);

        groundHits[0] = new RaycastHit();
        groundHits[1] = new RaycastHit();
        groundHits[2] = new RaycastHit();
        groundHits[3] = new RaycastHit();

        slideHits[0] = new RaycastHit();
        slideHits[1] = new RaycastHit();
        slideHits[2] = new RaycastHit();
        slideHits[3] = new RaycastHit();

        ledgeChecker = new RaycastHit();

        pushRay = new RaycastHit();

        leftRay = new RaycastHit();
        rightRay = new RaycastHit();

        AudioSource[] sources = this.GetComponents<AudioSource>();

        grassMaterials[0] = "TexturePratoScuro";
        grassMaterials[1] = "GrassGreenSolid2";
        grassMaterials[2] = "GrassGreenSolidLight2";
        grassMaterials[3] = "GrassGreenLightNew";
        grassMaterials[4] = "GrassNew3";
        grassMaterials[5] = "GrassLight";
        grassMaterials[6] = "GrassOrangeSolid";
        grassMaterials[7] = "GrassPinkSolid";
        grassMaterials[8] = "GrassRed";
        grassMaterials[9] = "GrassRedLight";
        grassMaterials[10] = "GrassRedSolid";
        grassMaterials[11] = "GrassSolid";
        grassMaterials[12] = "GrassSolid1";
        grassMaterials[13] = "GrassSolid2";
        grassMaterials[14] = "GrassSolidDark";
        grassMaterials[15] = "GrassSolidDark2";
        grassMaterials[16] = "GrassSolidDarkSuper";
        grassMaterials[17] = "GrassSolidDarkSuper2";
        grassMaterials[18] = "GrassOrangeSolid";
        grassMaterials[19] = "GrassRedLight2";
        grassMaterials[20] = "GrassSolidLightSuper";
        grassMaterials[21] = "GrassSolidOrange";
        grassMaterials[22] = "GrassSolidPink";
        grassMaterials[23] = "GrassSolidRed";
        grassMaterials[24] = "GrassYellow";
        grassMaterials[25] = "GrassYellowSolid";
        grassMaterials[26] = "GrassYellowSolid1";
        grassMaterials[27] = "GrassYellowSolid2";
        grassMaterials[28] = "GrassGreenSolidDark2";
        grassMaterials[29] = "GrassGreenSolidLight";
        grassMaterials[30] = "GrassGreenSolidLight3";
        grassMaterials[31] = "GrassGreenSolidLight4";
        grassMaterials[32] = "GrassGreenSolid";
        grassMaterials[33] = "GrassGreenSolidDark";

        rockMaterials[0] = "Rock";
        rockMaterials[1] = "Rock2";
        rockMaterials[2] = "Rock3";
        rockMaterials[3] = "RockDark";
        rockMaterials[4] = "RockSolid";
        rockMaterials[5] = "RockSolid2";
        rockMaterials[6] = "RockSolid3";
        rockMaterials[7] = "RockSolidRed";
        rockMaterials[8] = "Stone2";
        rockMaterials[9] = "StoneWhite";
        rockMaterials[10] = "Ice";
        rockMaterials[11] = "Ice2";
        rockMaterials[12] = "Ice3";


        snowMaterials[0] = "Snow";
        snowMaterials[1] = "White";
        snowMaterials[2] = "Igloo";
        snowMaterials[3] = "Snow2";

        sandMaterials[0] = "Sand";
        sandMaterials[1] = "Sand2";
        sandMaterials[2] = "SandSolid2";
        sandMaterials[3] = "SandWaterfall";
        sandMaterials[4] = "WoodSolidYellow";
        sandMaterials[5] = "Sand3";
        sandMaterials[6] = "SandSolid1";

        woodMaterials[0] = "WoodSolid";
        woodMaterials[1] = "WoodSolid2";
        woodMaterials[2] = "WoodSolid3";
        woodMaterials[3] = "WoodSolid4";
        woodMaterials[4] = "WoodSolid6";
        woodMaterials[5] = "WoodSolid10";
        woodMaterials[6] = "WoodSolid11";
        woodMaterials[7] = "WoodSolid12";
        woodMaterials[8] = "WoodSolidSpiral";
        woodMaterials[9] = "Wood";

        nonWallJumpMaterials[0] = "Snow";
        nonWallJumpMaterials[1] = "Snow2";
        nonWallJumpMaterials[2] = "Ice";
        nonWallJumpMaterials[3] = "Ice2";
        nonWallJumpMaterials[4] = "Mug";

        iceMaterials[0] = "Ice";
        iceMaterials[1] = "Ice2";
        iceMaterials[1] = "Ice3";

        sound = sources[0];
        atkSound = sources[1];
        footSteps = sources[2];
        interactions = sources[3];
        enHit1Sound = sources[4];
        enHit2Sound = sources[5];
        enHit3Sound = sources[6];
        boostingSound = sources[7];
        if (pID == 0)
            bbFinderSource = sources[8];

        walkEM = dustWalkParticles.GetComponent<ParticleSystem>().emission;
        runEM = dustRunParticles.GetComponent<ParticleSystem>().emission;
        rippleEM = rippleParticles.GetComponent<ParticleSystem>().emission;
        rippleCEM = rippleCParticles.GetComponent<ParticleSystem>().emission;
        splashMoveEM = splashMoveParticles.GetComponent<ParticleSystem>().emission;
        landEM = landParticles.GetComponent<ParticleSystem>().emission;
        instrumentEM = instrumentParticles.GetComponent<ParticleSystem>().emission;
        hitGEM = hitGParticles.GetComponent<ParticleSystem>().emission;
        hitYEM = hitYParticles.GetComponent<ParticleSystem>().emission;
        hitREM = hitRParticles.GetComponent<ParticleSystem>().emission;
        hitWEM = hitWParticles.GetComponent<ParticleSystem>().emission;
        knockedEM = knockedParticles.GetComponent<ParticleSystem>().emission;
        attackEM = attackParticles.GetComponent<ParticleSystem>().emission;
        chargedEM = chargedParticles.GetComponent<ParticleSystem>().emission;
        refilGEM = leafRefilGParticles.GetComponent<ParticleSystem>().emission;
        refilYEM = leafRefilYParticles.GetComponent<ParticleSystem>().emission;
        refilREM = leafRefilRParticles.GetComponent<ParticleSystem>().emission;
        refilWEM = leafRefilWParticles.GetComponent<ParticleSystem>().emission;
        wallGEM = wallGParticles.GetComponent<ParticleSystem>().emission;
        wallYEM = wallYParticles.GetComponent<ParticleSystem>().emission;
        wallREM = wallRParticles.GetComponent<ParticleSystem>().emission;
        wallWEM = wallWParticles.GetComponent<ParticleSystem>().emission;
        blockStarEM = blockStarParticles.GetComponent<ParticleSystem>().emission;
        jumpEM = jumpParticles.GetComponent<ParticleSystem>().emission;
        dJumpEM = doubleJumpParticles.GetComponent<ParticleSystem>().emission;
        killedEM = killedParticles.GetComponent<ParticleSystem>().emission;
        splashEM = splashParticles.GetComponent<ParticleSystem>().emission;
        boostEM = boostParticles.GetComponent<ParticleSystem>().emission;
        boostFlowerEM = boostParticles.GetComponent<ParticleSystem>().emission;
        boostingEM = boostingParticles.GetComponent<ParticleSystem>().emission;
        leafSlidingEM = leafSlidingParticles.GetComponent<ParticleSystem>().emission;
        enemyEM = enemyParticles.GetComponent<ParticleSystem>().emission;
        enemyParticles.transform.SetParent(this.transform.parent);

        walkEM.enabled = false;
        runEM.enabled = false;
        rippleEM.enabled = false;
        rippleCEM.enabled = false;
        splashMoveEM.enabled = false;
        landEM.enabled = false;
        instrumentEM.enabled = false;
        hitGEM.enabled = false;
        hitYEM.enabled = false;
        hitREM.enabled = false;
        hitWEM.enabled = false;
        knockedEM.enabled = false;
        chargedEM.enabled = false;
        refilGEM.enabled = false;
        refilYEM.enabled = false;
        refilREM.enabled = false;
        refilWEM.enabled = false;
        wallGEM.enabled = false;
        wallYEM.enabled = false;
        wallREM.enabled = false;
        wallWEM.enabled = false;
        blockStarEM.enabled = false;
        attackEM.enabled = false;
        jumpEM.enabled = false;
        killedEM.enabled = false;
        dJumpEM.enabled = false;
        splashEM.enabled = false;
        boostEM.enabled = false;
        boostFlowerEM.enabled = false;
        boostingEM.enabled = false;
        leafSlidingEM.enabled = false;

        if (pID == 0)
        {
            bbFinderEM = bbFinderParticles.GetComponent<ParticleSystem>().emission;
            bbFinderEM.enabled = false;

            portal1EM = portal1Particles.GetComponent<ParticleSystem>().emission;
            portal2EM = portal2Particles.GetComponent<ParticleSystem>().emission;
            portal1EM.enabled = false;
            portal2EM.enabled = false;

            bbFinderMat = bbFinderMeshRenderer.material;
            bbFinderMeshRenderer.material = bbFinderMat;
        }

        rippleParticles.gameObject.transform.SetParent(this.transform.parent);
        rippleCParticles.gameObject.transform.SetParent(this.transform.parent);

        boostingParticles.gameObject.transform.SetParent(this.transform.parent);
        leafSlidingParticles.gameObject.transform.SetParent(this.transform.parent);

        hitGParticles.gameObject.SetActive(false);
        hitYParticles.gameObject.SetActive(false);
        hitRParticles.gameObject.SetActive(false);
        hitWParticles.gameObject.SetActive(false);
        leafRefilGParticles.gameObject.SetActive(false);
        leafRefilYParticles.gameObject.SetActive(false);
        leafRefilRParticles.gameObject.SetActive(false);
        leafRefilWParticles.gameObject.SetActive(false);
        wallGParticles.gameObject.SetActive(false);
        wallYParticles.gameObject.SetActive(false);
        wallRParticles.gameObject.SetActive(false);
        wallWParticles.gameObject.SetActive(false);
        blockStarParticles.gameObject.SetActive(false);

        jumpST = jumpTrail.GetComponent<SmokeTrail>();
        leafST = leafTrail.GetComponent<SmokeTrail>();
        runST = runTrail.GetComponent<SmokeTrail>();
        if (leafSlideTrail)
            leafSlideST = leafSlideTrail.GetComponent<SmokeTrail>();
        jumpST.Emit = false;
        leafST.Emit = false;
        runST.Emit = false;
        if (leafSlideST)
            leafSlideST.Emit = false;

        leafNo = 1;
        LeafCreate();

        health = 2;

        onGround = false;
        delayed = true;
        canAttackNext = true;

        rrr = new RaycastHit();
        boxChecker = new RaycastHit();
        wallJumpRay = new RaycastHit();
        seaHit = new RaycastHit();

        //   stoneAttack.tpc = this;

        if (playerID == "0")
            StartCoroutine("Blink");

        if (shieldCol != null)
            Physics.IgnoreCollision(capcol, shieldCol, true);

        SetDefaultLeafMat();

        if (renderers.Length == 0)
            renderers = this.gameObject.GetComponentsInChildren<Renderer>();
        foreach (Renderer r in renderers)
        {
            if (!r.name.Contains("eaf"))
            {
                foreach (Material m in r.materials)
                {
                    if (m.HasProperty("_RimColor") && m.HasProperty("_RimIntensityF"))
                    {
                        origRimCol.Add(m.GetColor("_RimColor"));
                        origRimInt.Add(m.GetFloat("_RimIntensityF"));
                    }
                }
            }
        }

        blueBerryIDCreators = new List<BlueBerryIDCreator>();
    }

    void OnEnable()
    {
        StartCoroutine("SetLookAt");
        if (playerID == "0")
        {
            StopCoroutine("Blink");
            StartCoroutine("Blink");
        }
    }

    void Update()
    {
        if (pID != 0 && inCutscene && !PlayerManager.GetMainPlayer().inCutscene)
            inCutscene = false;

        if (!inCutscene && !onSpline)
        {
            //null checks
            if (cam == null)
            {
                cam = GameObject.FindWithTag("MainCamera");
                relative = new GameObject(this.gameObject.name + " Relative");
                StartCoroutine("SetLookAt");
            }

#if UNITY_EDITOR
            if (ps.enableDebugTab)
            {
                if (Input.GetKeyDown(KeyCode.U))
                {
                    PlayerPrefs.SetInt("Berries", berryCount + 20000);
                    berryCount = PlayerPrefs.GetInt("Berries", 0);
                }
                if (Input.GetKeyDown(KeyCode.I))
                {
                    PlayerPrefs.SetInt("BlueBerries", blueberryCount + 500);
                    blueberryCount = PlayerPrefs.GetInt("BlueBerries", 0);
                }
                if (Input.GetKeyDown(KeyCode.O))
                {
                    PlayerPrefs.SetInt("Berries", 0);
                    berryCount = PlayerPrefs.GetInt("Berries", 0);
                    PlayerPrefs.SetInt("BlueBerries", 0);
                    blueberryCount = PlayerPrefs.GetInt("BlueBerries", 0);
                }
                if (Input.GetKeyDown(KeyCode.K))
                {
                    for (int xyz = 0; xyz < 21; xyz++)
                    {
                        PlayerPrefs.SetInt("UsingItem" + xyz.ToString(), 0);
                        PlayerPrefs.SetInt("PaidForItem" + xyz.ToString(), 0);
                    }
                }
                if (Input.GetKeyDown(KeyCode.J))
                {
                    for (int lvl = 1; lvl <= 8; lvl++)
                    {
                        for (int tearNo = 1; tearNo <= 3; tearNo++)
                            PlayerPrefs.SetInt("Vase" + tearNo.ToString() + "Level" + lvl.ToString(), 1);
                    }
                }
            }
#endif

            //    if (inStoneMode && !disableControl)
            //        disableControl = true;

            if (!inLeafSlide && capcol.radius != 0.2f)
                capcol.radius = 0.2f;

            //main
            if (!disableControl && cam != null)
            {

                jumpButtonDown = (input.GetButtonDown("Jump") && !inShop);
                //    if (jumpButtonDown)
                //      Debug.Log("JUMP BUTTON PRESSED : " + disableControl + " . " + lockVelocity +" . "+ inCutscene);
                glideButtonDown = input.GetButtonDown("Glide");
                //    if (glideButtonDown)
                //        Debug.Log("GLIDE BUTTON PRESSED : " + Time.timeSinceLevelLoad + " . " + disableControl + " . " + lockVelocity + " . " + inCutscene);
                runButtonDown = input.GetButtonDown("Run");
                jumpButtonHold = (input.GetButton("Jump") && !inShop);
                glideButtonHold = input.GetButton("Glide");
                runButtonHold = input.GetButton("Run");

                if (!ps.enableDebugTab)
                {
                    if (gliding && (runButtonDown || runButtonHold))
                    {
                        runButtonDown = false;
                        runButtonHold = false;
                    }
                }

                if ((Physics.Raycast(this.transform.position + (this.transform.forward * boxCentre.z) + (this.transform.up * boxCentre.y),
                    -Vector3.up, out oneWayHit, boxY * 20f, whatIsGround) && oneWayHit.collider.gameObject.tag == "OneWay") ||
                    (Physics.Raycast(this.transform.position + (this.transform.up * boxCentre.y),
                    -Vector3.up, out oneWayHit, boxY * 20f, whatIsGround) && oneWayHit.collider.gameObject.tag == "OneWay") ||
                    (Physics.Raycast(this.transform.position - (this.transform.forward * boxCentre.z) + (this.transform.up * boxCentre.y),
                    -Vector3.up, out oneWayHit, boxY * 20f, whatIsGround) && oneWayHit.collider.gameObject.tag == "OneWay"))
                {
                    if (currentOneWay == null || oneWayHit.collider.gameObject != currentOneWay)
                    {
                        if (currentOneWay != null)
                            Physics.IgnoreCollision(this.gameObject.GetComponent<Collider>(), currentOneWay, true);
                        currentOneWay = oneWayHit.collider;
                        Physics.IgnoreCollision(this.gameObject.GetComponent<Collider>(), currentOneWay, false);
                    }
                }
                else
                {
                    if (currentOneWay != null)
                    {
                        Physics.IgnoreCollision(this.gameObject.GetComponent<Collider>(), currentOneWay, true);
                    }
                }

                relative.transform.localEulerAngles = new Vector3(0f, cam.transform.localEulerAngles.y, 0f);

                leftX = input.GetAxis("LH");
                leftY = input.GetAxis("LV");

                if (noDirectional || currentAnim.inAttack || currentAnim.inAttack2 || currentAnim.inAttack3)
                {
                    leftX = 0f;
                    leftY = 0f;
                }

                direction = new Vector3(leftX, 0f, leftY);
                direction.Normalize();

                gv = 0;
                gvFound = false;
                gvIncline = false;
                gvSlide = false;
                gvIce = false;

                slopeCheckers[0] = this.transform.position;
                slopeCheckers[1] = this.transform.position - (Vector3.forward * boxZ / 2f);
                slopeCheckers[2] = this.transform.position + (Vector3.right * boxX / 2f);
                slopeCheckers[3] = this.transform.position - (Vector3.right * boxX / 2f);
                slideHigh = Vector3.zero;

                if (!onGround && Physics.Raycast(this.transform.position, -Vector3.up, out boxChecker, boxY, whatIsMoveable) && !boxChecker.collider.gameObject.GetComponent<Rigidbody>().isKinematic)
                    boxChecker.collider.gameObject.GetComponent<Rigidbody>().isKinematic = true;

                while (gv < 4 && !bounce)
                {
                    if (Physics.Raycast(groundCheckers[gv].transform.position, -Vector3.up, out groundHits[gv], 0.1f, whatIsGround) && !groundHits[gv].collider.isTrigger)
                    {
                        if (groundHits[gv].normal.y >= groundTolerance)
                        {
                            if (!gvFound)
                            {
                                if (prevGround == null || groundHits[gv].collider.gameObject != prevGround &&
                                    (groundHits[gv].collider.gameObject.GetComponent<MeshRenderer>() != null || groundHits[gv].collider.gameObject.GetComponentInChildren<MeshRenderer>() != null))
                                {
                                    if (groundHits[gv].collider.gameObject.GetComponent<MeshRenderer>() != null)
                                        curGroundMR = groundHits[gv].collider.gameObject.GetComponent<MeshRenderer>();
                                    else
                                        curGroundMR = groundHits[gv].collider.gameObject.GetComponentInChildren<MeshRenderer>();

                                    foundMat = false;
                                    if (curGroundMR != null && curGroundMR.material != null)
                                    {
                                        string groundMaterial = curGroundMR.material.name;
                                        CycleMaterials(grassMaterials, groundMaterial);
                                        if (!foundMat)
                                            CycleMaterials(rockMaterials, groundMaterial);
                                        if (!foundMat)
                                            CycleMaterials(snowMaterials, groundMaterial);
                                        if (!foundMat)
                                            CycleMaterials(sandMaterials, groundMaterial);
                                        if (!foundMat)
                                            CycleMaterials(woodMaterials, groundMaterial);
                                    }
                                    if (!foundMat)
                                        groundID = 5;

                                    prevGround = groundHits[gv].collider.gameObject;
                                }
                                if (gv == 0 || gv == 1)
                                    curFloor = groundHits[gv].collider.gameObject;
                            }

                            gvFound = true;
                            if (delayed && (groundHits[gv].collider.gameObject.tag == "Elevator" || (groundHits[gv].collider.gameObject.tag == "OneWay" && groundHits[gv].collider.gameObject.name == "OneWay Moving"))
                                && groundHits[gv].collider.gameObject != this.transform.parent)
                            {
                                this.transform.SetParent(groundHits[gv].collider.gameObject.transform);
                            }
                            if (groundHits[gv].collider.gameObject.tag != "Elevator" && groundHits[gv].collider.gameObject.tag != "OneWay" && groundHits[gv].collider.gameObject.name != "OneWay Moving" && this.transform.parent != null)
                                this.transform.SetParent(initialParent);
                        }
                        if (groundHits[gv].normal.y < 1f)
                        {
                            gvIncline = true;
                            inclineNormal = groundHits[gv].normal;
                        }

                        if (curGroundMR != null && curGroundMR.material != null)
                        {
                            foreach (string iceMat in iceMaterials)
                            {
                                if (iceMat == curGroundMR.material.name || iceMat + " (Instance)" == curGroundMR.material.name)
                                    gvIce = true;
                            }
                        }
                    }
                    slideCheckers[gv] = false;
                    if (Physics.Raycast(slopeCheckers[gv], -Vector3.up, out slideHits[gv], 1f, whatIsSlide))
                    {
                        gvSlide = true;
                        slideCheckers[gv] = true;
                    }
                    gv++;
                }
                if (!gvIncline && inclineNormal != Vector3.up)
                    inclineNormal = Vector3.up;
                if (gvFound && delayed)
                {
                    onGround = true;
                    ledgeHopping = false;
                    StopCoroutine("JumpWait");
                    canGlideFromGround = false;
                    startedGroundDelay = false;
                    StopCoroutine("GroundDelay");
                    jumpChance = true;
                    fallNow = false;
                    if (startedGlideTimer)
                    {
                        startedGlideTimer = false;
                        glidedTooLong = false;
                        StopCoroutine("GlideTimer");
                    }
                    if (!landed)
                    {
                        landed = true;
                        if ((fallHeight - this.transform.position.y) >= 1.5f)
                            HDRumbleMain.PlayVibrationPreset(pID, "B02_Bu2", 0.4f, 1, 0.3f);

                        jumpST.Emit = false;

                        if (!currentAnim.inHit && !anim.GetNextAnimatorStateInfo(0).IsName("Hit"))
                        {
                            landingSoundIterate = Random.Range(0, 3);
                            if (landingSoundIterate == 0)
                                sound.clip = landingSound;
                            if (landingSoundIterate == 1)
                                sound.clip = landingSound2;
                            if (landingSoundIterate == 2)
                                sound.clip = landingSound3;
                            sound.PlayDelayed(0f);
                        }
                        landEM.enabled = true;
                        landParticles.GetComponent<ParticleSystem>().Play();
                        StartCoroutine(DeactivateParticle(landParticles, landEM));
                        anim.ResetTrigger("falling");
                        anim.ResetTrigger("land");
                        anim.ResetTrigger("runningLand");
                        if (!currentAnim.locomotionBlend && !currentAnim.inLand && !currentAnim.inRunLand)
                        {
                            if (!isBoosting && Vector3.Magnitude(new Vector3(leftX, 0f, leftY)) <= 0.1f)
                            {
                                hardLanded = true;
                                if (!inLeafSlide && !inRiverForce)
                                    anim.SetTrigger("land");
                                leftX = 0f;
                                leftY = 0f;
                                direction = Vector3.zero;
                                rb.velocity = Vector3.zero;
                            }
                            else
                            {
                                if (!inLeafSlide && !inRiverForce)
                                {
                                    anim.Play("RunningLand");
                                    anim.SetFloat("rollingVelo", Vector3.Magnitude(new Vector3(rb.velocity.x, 0f, rb.velocity.z)));
                                }
                            }
                        }
                        if (beenHit && !invinceFrames)
                        {
                            StartCoroutine("Invincibility");
                            if (health == 0)
                                Defeat();
                            if (health == 1)
                                StartCoroutine("RefillLife");
                        }
                    }
                    if (jumped)
                    {
                        jumped = false;
                        doubleJumped = false;
                        hasTripJumped = false;
                    }
                }
                else
                {
                    if (onGround)
                        fallHeight = this.transform.position.y;

                    onGround = false;

                    if (!startedGlideTimer)
                        StartCoroutine("GlideTimer");

                    anim.ResetTrigger("land");
                    anim.ResetTrigger("runningLand");
                    if (!startedGroundDelay && isHoldingLeaf && !leafCol.hasWaterDrop && !jumped)
                        StartCoroutine("GroundDelay");
                    curFloor = null;
                    this.transform.SetParent(initialParent);
                    landed = false;
                    hardLanded = false;
                    StartCoroutine("JumpWait", 0.5f);
                }
                if (gvIncline)
                    onIncline = true;
                else
                    onIncline = false;

                if (gvSlide && !onGround && delayed)
                {
                    onSlide = true;
                    if (endingSlide)
                    {
                        StopCoroutine("EndSliding");
                        endingSlide = false;
                    }
                    if (gliding)
                    {
                        //    Debug.Log("EXIT GLIDE BECAUSE ONGROUND");
                        gliding = false;
                        anim.SetTrigger("exitGlide");
                    }

                    StopCoroutine("JumpWait");
                    if (jumped || !jumpChance)
                    {
                        jumpChance = true;
                        jumped = false;
                        doubleJumped = false;
                        hasTripJumped = false;
                    }
                    if (sound.clip != sliding)
                    {
                        anim.SetBool("inLocomotion", false);
                        anim.Play("Sliding", 0);
                        sound.clip = sliding;
                        sound.loop = true;
                        sound.PlayDelayed(0f);
                    }
                    else
                    {
                        if (!currentAnim.inSliding)
                            anim.Play("Sliding", 0);
                    }
                    if (input.GetButtonDown("Jump"))
                        sound.loop = false;
                }
                else
                {
                    if (!endingSlide)
                        StartCoroutine("EndSliding");
                }
                if (gvIce)
                    onIce = true;
                else
                    onIce = false;
                if (inRiverForce)
                {
                    if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Dragged") && !anim.GetNextAnimatorStateInfo(0).IsName("Dragged"))
                        anim.SetTrigger("drag");

                    if (currentRiverForce == null || !currentRiverForce.gameObject.activeInHierarchy)
                        ExitRiverForce();

                }
                else
                {
                    if (anim.GetCurrentAnimatorStateInfo(0).IsName("Dragged"))
                    {
                        anim.SetTrigger("unDrag");
                    }
                }

                lookAt.transform.position = this.transform.position;
                if (!onSlide)
                {
                    if (direction != Vector3.zero || leafSliding)
                    {
                        lookAt.transform.LookAt(this.transform.position + (relative.transform.forward * direction.z * 10f) + (relative.transform.right * direction.x * 10f));
                    }
                }
                else
                {
                    gv = 0;
                    while (gv < 4)
                    {
                        if (slideCheckers[gv])
                        {
                            if (slideHigh == Vector3.zero)
                                slideHigh = slideHits[gv].point;
                            if (slideHits[gv].point.y >= slideHigh.y)
                                slideHigh = slideHits[gv].point;
                        }
                        gv++;
                    }

                    if (new Vector3(rb.velocity.x, 0f, rb.velocity.z) != Vector3.zero)
                        lookAt.transform.forward = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
                }

                if (onGround && gliding)
                {
                    //    Debug.Log("EXIT GLIDE BECAUSE ONGROUND 2");
                    gliding = false;
                    anim.SetTrigger("exitGlide");
                }

                if (!onGround && this.transform.position.y > fallHeight)
                    fallHeight = this.transform.position.y;

                if (!stepDelay && anim.GetBool("inLocomotion") && onGround && !isBoosting)
                {
                    stepInt = Random.Range(0, 4);
                    if (stepInt == prevStepInt)
                    {
                        stepInt++;
                        if (stepInt >= 4)
                        {
                            stepInt = 0;
                            if (prevStepInt == 0)
                                stepInt = 1;
                        }
                    }
                    prevStepInt = stepInt;

                    if (!inWaterFS)
                    {
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
                    }
                    else
                        tempStep = waterFootSteps[stepInt];
                    footSteps.clip = tempStep;
                    footSteps.PlayDelayed(0f);
                    stepDelay = true;
                    StartCoroutine("ClipDelay");
                }

                if (!onGround && !jumped && !doubleJumped && !beenHit && rb.velocity.y > 0f)
                {
                    rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
                }

                if (!onSpline)
                {
                    //

                    if (!glidedTooLong && (glideButtonDown || (canGlideFromGround)))
                    {
                        //    Debug.Log("IN GLIDE: " + Time.timeSinceLevelLoad + " . " + glideButtonDown + " . " + canGlideFromGround);
                        if (canGlideFromGround)
                        {
                            anim.ResetTrigger("falling");
                            anim.ResetTrigger("exitGlide");
                            anim.SetTrigger("enterGlide");
                            canGlideFromGround = false;
                        }
                        if (!onGround && !gliding && delayed && !againstWall && !startedWallCheck && (!glideDelay || inWindCol || inBamboo) && !ledgeHopping && !inLeafSlide && !onSlide)
                        {
                            //    Debug.Log("START GLIDING " + Time.timeSinceLevelLoad);
                            gliding = true;
                            anim.ResetTrigger("exitGlide");
                            anim.SetTrigger("enterGlide");
                            sound.clip = leafUpSound;
                            sound.PlayDelayed(0f);
                        }
                        else
                        {
                            if (gliding)
                            {
                                //    Debug.Log("EXIT GLIDING (A): " + Time.timeSinceLevelLoad + " . " + gliding + " . " + delayed + " . " + againstWall + " . " + startedWallCheck + " . " + glideDelay + " . " + inWindCol + " . " + inBamboo + " . " + ledgeHopping + " . " + inLeafSlide + " . " + onSlide);
                                gliding = false;
                                anim.SetTrigger("exitGlide");
                            }

                            if (inBamboo)
                            {
                                inBamboo = false;
                                StopCoroutine("InTheBamboo");
                            }
                        }
                    }

                    //

                    CalculateBasicAnimation(leftX, leftY);

                    if (sound.loop && !onSlide && !pushing)
                        sound.loop = false;

                    if (!pushing && sound.clip == boxPushing)
                        sound.Stop();

                    if (isBoosting && onGround)
                    {
                        if (boostingEM.enabled == false)
                            boostingEM.enabled = true;
                        boostingParticles.transform.position = this.transform.position + (Vector3.up * -0.556f) + (this.transform.forward * -0.637f);
                        boostingParticles.transform.up = Vector3.Lerp(boostingParticles.transform.up, -this.transform.forward, Time.deltaTime * 5f);
                    }
                    else
                    {
                        if (boostingEM.enabled == true)
                            boostingEM.enabled = false;
                    }

                    if (inLeafSlide && onGround)
                    {
                        if (leafSlidingEM.enabled == false)
                            leafSlidingEM.enabled = true;
                        leafSlidingParticles.transform.position = this.transform.position + (Vector3.up * -0.556f) + (this.transform.forward * -0.637f);
                        leafSlidingParticles.transform.up = Vector3.Lerp(leafSlidingParticles.transform.up, -this.transform.forward, Time.deltaTime * 5f);
                    }
                    else
                    {
                        if (leafSlidingEM.enabled == true)
                            leafSlidingEM.enabled = false;
                    }

                    //xAttack

                    if (!currentAnim.inAttackAnim && !startedAttack && !attackBuffer && !attackBufferX && leafST.Emit)
                        leafST.Emit = false;

                    if ((!input.GetButton("Attack") || pID >= 1) && attackBuffer && startedAttack && canAttackNext && !fullyCharged)
                    {
                        StopCoroutine("Buffer");
                        StopCoroutine("Charging");
                        startedAttack = false;
                        attackPressOnly = false;
                        isHoldingAttack = false;
                        if (chargingAttack)
                            anim.SetTrigger("releaseAttack");
                        chargingAttack = false;
                        attackBuffer = false;
                        attackBufferX = false;
                    }

                    if (input.GetButtonDown("Attack") && !inHitFreeze && !inShop && !noDirectional && !isBoosting && !inLeafSlide && !attackBuffer && !startedAttack && !defeated)
                    {
                        rb.velocity = rb.velocity / 3f;
                        StopCoroutine("RandomiseMainIdle");
                        StopCoroutine("VelocityDelay");
                        StartCoroutine("VelocityDelay");
                        cancelLeafHold = true;
                        isHoldingLeaf = false;
                        IfHasDrop();
                        anim.ResetTrigger("leafUp");
                        anim.ResetTrigger("leafDown");
                        anim.ResetTrigger("quickLeafDown");
                        if (!currentAnim.inAttack && !currentAnim.inAttack2 && !currentAnim.inAttack3)
                            anim.Play("Empty", 1);
                        attackBuffer = true;
                        StopCoroutine("AttackNext");
                        letGoOfAttack = false;
                        startedAttack = false;
                        anim.ResetTrigger("chargeAttack");
                        anim.ResetTrigger("releaseAttack");
                        StartCoroutine("Buffer");
                        canAttackNext = false;
                        StartCoroutine("WaitAttack");
                    }

                    if (attackBuffer && !attackBufferX && !input.GetButton("Attack"))
                    {
                        letGoOfAttack = true;
                        attackBufferX = true;
                        StopCoroutine("Buffer");
                        isHoldingAttack = false;
                        attackPressOnly = true;
                    }

                    if (attackPressOnly && attackBufferX && !startedAttack)
                    {
                        startedAttack = true;
                        leafST.Emit = true;

                        lookAt.transform.LookAt(this.transform.position + (relative.transform.forward * input.GetAxis("LV") * 10f) + (relative.transform.right * input.GetAxis("LH") * 10f));
                        this.transform.rotation = lookAt.transform.rotation;

                        if (!currentAnim.inAttack && !currentAnim.inAttack2 && !currentAnim.inAttack3)
                        {
                            anim.ResetTrigger("atk1");
                            anim.ResetTrigger("atk2");
                            anim.ResetTrigger("atk3");
                            if (attackAnimIterate == 0)
                                anim.Play("Attack", 1, 0f);
                            if (attackAnimIterate == 1)
                                anim.Play("Attack2", 1, 0f);
                            if (attackAnimIterate == 2)
                                anim.Play("Attack3", 1, 0f);
                        }
                        else
                        {
                            if (attackAnimIterate == 0)
                            {
                                anim.SetTrigger("atk1");
                            }
                            if (attackAnimIterate == 1)
                            {
                                anim.SetTrigger("atk2");
                            }
                            if (attackAnimIterate == 2)
                            {
                                anim.SetTrigger("atk3");
                            }
                        }

                        isThirdAttack = false;

                        if (attackAnimIterate == 0)
                            HDRumbleMain.PlayVibrationPreset(pID, "B04_Bam1", 1f, 1, 0.3f);
                        if (attackAnimIterate == 1)
                            HDRumbleMain.PlayVibrationPreset(pID, "B05_Bam2", 1f, 1, 0.3f);
                        if (attackAnimIterate == 2)
                        {
                            HDRumbleMain.PlayVibrationPreset(pID, "B06_Bump1", 1f, 1, 0.3f);
                            isThirdAttack = true;
                        }

                        if (hasUltraLeaf)
                            LaunchWindball();
                        else
                        {
                            if (attackAnimIterate == 2 && hasSuperLeaf)
                                LaunchWindball();
                        }


                        attackAnimIterate++;
                        if (attackAnimIterate > 2)
                            attackAnimIterate = 0;
                        if (attackAnimIterate > 0)
                        {
                            StopCoroutine("AttackNext");
                            StartCoroutine("AttackNext");
                        }
                        attackIterate = Random.Range(0, 3);



                        if (attackIterate == 0)
                            atkSound.clip = attack1Sound;
                        if (attackIterate == 1)
                            atkSound.clip = attack2Sound;
                        if (attackIterate == 2)
                            atkSound.clip = attack3Sound;
                        atkSound.PlayDelayed(0f);

                        if (onIce || (!ps.enableDebugTab && gliding))
                            rb.AddForce(this.transform.forward * rb.mass * 2f, ForceMode.Impulse);
                        else
                            rb.AddForce(this.transform.forward * rb.mass * 20f, ForceMode.Impulse);
                        if (onGround)
                            StartCoroutine("AttackSpeedStop");

                        leafAS.thisCollider.enabled = false;
                        leafAS.activeAttack = true;
                        StopCoroutine("DeactiveAttack");
                        StartCoroutine("DeactiveAttack", 0.35f);
                    }

                    if (isHoldingAttack && attackBufferX && !startedAttack && onGround)
                    {
                        leafST.Emit = true;
                        startedAttack = true;
                        chargingAttack = true;
                        lockVelocity = false;
                        atkSound.clip = attackChargingSound;
                        atkSound.PlayDelayed(0f);
                        anim.SetTrigger("chargeAttack");
                        StartCoroutine("Charging");
                    }

                    if (!input.GetButton("Attack") && !fullyCharged && chargingAttack && currentAnim.inAttackCharge)
                    {
                        StopCoroutine("Charging");
                        anim.SetTrigger("releaseAttack");
                        atkSound.clip = attackChargedSound;
                        atkSound.PlayDelayed(0f);
                        StopVibrate();
                        chargingAttack = false;
                        CancelAttack();
                    }

                    if (isHoldingAttack)
                    {
                        lookAt.transform.LookAt(this.transform.position + (relative.transform.forward * input.GetAxis("LV") * 10f) + (relative.transform.right * input.GetAxis("LH") * 10f));
                        this.transform.rotation = lookAt.transform.rotation;
                    }

                    //

                    if (pID == 0)
                    {
                        if (input.GetButtonDown("Leaf") && onGround && delayed && ps.challengeWarpAnim.GetBool("skipOn") == false && ps.racePromptAnim.GetBool("skipOn") == false
                            && !isHoldingLeaf && !currentAnim.inLeafDown && !currentAnim.inQuickLeafDown && !attackBuffer && !inLeafSlide)
                        {
                            isHoldingLeaf = true;
                            anim.ResetTrigger("leafDown");
                            anim.ResetTrigger("quickLeafDown");
                            anim.SetTrigger("leafUp");
                            sound.clip = leafUpSound;
                            sound.PlayDelayed(0f);
                        }

                        if ((input.GetButtonDown("Leaf") || cancelLeafHold || jumped || inLeafSlide) && isHoldingLeaf && currentAnim.inLeafHold)
                        {
                            isHoldingLeaf = false;
                            IfHasDrop();
                            if (cancelLeafHold)
                                anim.SetTrigger("quickLeafDown");
                            else
                                anim.SetTrigger("leafDown");
                        }

                        if (cancelLeafHold)
                            cancelLeafHold = false;
                    }
                    //

                    if (hasLeafSlide)
                    {
                        if (onGround && !anim.GetCurrentAnimatorStateInfo(0).IsName("LeafSlideJump") && !currentAnim.inAttackAnim && !inLeafSlide && !invinceFrames && health > 1)
                        {
                            if (input.GetButton("LeafSlide"))
                            {
                                inLeafSlide = true;
                                leafSlideST.Emit = true;
                                capcol.radius = 0.5f;
                                anim.ResetTrigger("exitSlide");
                                anim.SetTrigger("enterSlide");
                                if ((atkSound.clip != enterLeafSlide && atkSound.clip != leafSliding) || !atkSound.isPlaying)
                                    StartCoroutine("SlideSound");
                            }
                        }
                        if (inLeafSlide && !currentAnim.inLeafSlide && anim.GetAnimatorTransitionInfo(0).nameHash != 741357581 && !anim.GetCurrentAnimatorStateInfo(0).IsName("LeafSlideJump"))
                        {
                            anim.ResetTrigger("exitSlide");
                            anim.SetTrigger("enterSlide");
                            if ((atkSound.clip != enterLeafSlide && atkSound.clip != leafSliding) || !atkSound.isPlaying)
                                StartCoroutine("SlideSound");
                        }
                        if (inLeafSlide && input.GetButtonUp("LeafSlide"))
                            CancelLeafSlide();
                    }
                    else
                    {
                        if (inLeafSlide)
                            CancelLeafSlide();
                    }

                    //

                    if ((input.GetButtonDown("Victory") || input.GetButtonDown("Victory2") || input.GetButtonDown("Victory3")) && !anim.GetCurrentAnimatorStateInfo(0).IsName("Victory") && !anim.GetNextAnimatorStateInfo(0).IsName("Victory") && onGround)
                    {
                        if (input.GetButtonDown("Victory"))
                            anim.SetFloat("celebrateFloat", 0f);
                        if (input.GetButtonDown("Victory2"))
                            anim.SetFloat("celebrateFloat", 1f);
                        if (input.GetButtonDown("Victory3"))
                            anim.SetFloat("celebrateFloat", 2f);
                        sound.clip = victorySound;
                        sound.PlayDelayed(0f);
                        anim.SetTrigger("celebrate");
                    }

                    //

                    if (pID == 0)
                    {
                        if (input.GetButtonDown("HUD") && berryHUD.GetBool("function") == false)
                        {
                            berryText.text = berryCount.ToString();
                            blueberryText.text = blueberryCount.ToString();

                            ps.CheckTears();
                            if (ps.tearCount < 10)
                                tearText.text = "0" + ps.tearCount.ToString();
                            else
                                tearText.text = ps.tearCount.ToString();
                            itemText.text = PlayerPrefs.GetInt("CollectablesTotal", 0).ToString();

                            berryHUD.SetBool("function", true);
                            blueberryHUD.SetBool("function", true);
                            if (compassHUD != null)
                                compassHUD.SetBool("function", true);
                            if (tearHUD != null)
                                tearHUD.SetBool("function", true);
                            if (itemHUD != null)
                                itemHUD.SetBool("function", true);
                            StartCoroutine(HUDWait());
                        }
                    }

                    //

                    if (!onGround && !againstWall && jumped && rb.velocity.y <= 0f && !ledgeHopping)
                    {
                        if (Physics.Raycast(checkLedge.transform.position, -Vector3.up, out ledgeChecker, 0.4f, whatIsGround) && !ledgeChecker.collider.isTrigger && ledgeChecker.collider.tag != "NoLedgeHop")
                        {
                            this.transform.LookAt(new Vector3(ledgeChecker.point.x, this.transform.position.y, ledgeChecker.point.z));

                            float ledgeRotation = this.transform.localEulerAngles.y;
                            if (ledgeRotation <= 45f || ledgeRotation > 315f)
                                ledgeRotation = 0f;
                            else
                            {
                                if (ledgeRotation <= 135f && ledgeRotation > 45f)
                                    ledgeRotation = 90f;
                                else
                                {
                                    if (ledgeRotation <= 225f && ledgeRotation > 135f)
                                        ledgeRotation = 180f;
                                    else
                                    {
                                        if (ledgeRotation <= 315f && ledgeRotation > 225f)
                                            ledgeRotation = 270f;
                                    }
                                }
                            }
                            this.transform.localEulerAngles = new Vector3(0f, ledgeRotation, 0f);

                            anim.SetTrigger("doubleJumpNow");
                            sound.clip = ledgeClimbSound;
                            sound.PlayDelayed(0f);
                            ledgeHopping = true;

                            if (gliding)
                            {
                                //   Debug.Log("EXIT GLIDE BECAUSE LEDGEHOPPING");
                                gliding = false;
                                anim.SetTrigger("exitGlide");
                            }

                            StartCoroutine("LedgeHop");
                        }
                    }
                }
                if (pID != 0)
                {
                    if (!startedCharacterRespawn && (this.transform.position - woodle.transform.position).sqrMagnitude >= distanceForMultiChara)
                    {
                        StartCoroutine("RespawnCharacterWait");
                    }
                    if (startedCharacterRespawn && (this.transform.position - woodle.transform.position).sqrMagnitude < distanceForMultiChara)
                    {
                        StopCoroutine("RespawnCharacterWait");
                        startedCharacterRespawn = false;
                    }
                    if (!startedCharacterRespawn && input.GetButtonDown("Start"))
                    {
                        StopCoroutine("RespawnCharacterWait");
                        startedCharacterRespawn = true;
                        RespawnCharacter();
                    }
                }

                if (pID == 0 && PlayerPrefs.GetInt("HalfBlueBerries", 0) == 1)
                {
                    if (input.GetButtonDown("BlueBerryFind") && PlayerPrefs.GetInt("AllBlueBerries", 0) == 0)
                    {
                        usingBBFinder = !usingBBFinder;
                        bbFinderSource.Stop();
                        if (usingBBFinder)
                        {
                            bbFinderSource.clip = bbFinderONSound;
                            StartCoroutine("BlueBerryFinder", true);
                            bbFinderAnim.SetBool("activate", true);
                        }
                        else
                        {
                            bbFinderSource.clip = bbFinderOFFSound;
                            StopCoroutine("BlueBerryFinder");
                            bbFinderAnim.SetBool("activate", false);
                        }
                        bbFinderSource.pitch = 1f;
                        bbFinderSource.PlayDelayed(0f);
                    }
                    if (usingBBFinder && PlayerPrefs.GetInt("AllBlueBerries", 0) == 1)
                    {
                        StopCoroutine("BlueBerryFinder");
                        bbFinderSource.Stop();
                        bbFinderSource.clip = bbFinderOFFSound;
                        bbFinderSource.pitch = 1f;
                        bbFinderSource.PlayDelayed(0f);
                        bbFinderAnim.SetBool("activate", false);
                    }
                }

                /*
                if (pID == 0 && !inStoneMode && input.GetButtonDown("Stone") && !disableControl)
                {
                    inStoneMode = true;
                    stoneAttack.activeAttack = true;
                    stoneModeFirstFrame = true;
                    disableControl = true;
                    anim.Play("Idle");
                }
                */
            }
            else
            {
                /*
                if (inStoneMode && rb.velocity.y >= -0.1f && !stoneModeFirstFrame)
                    rb.isKinematic = true;

                if (inStoneMode && !ps.inPause && !input.GetButton("Stone") && rb.isKinematic)
                {
                    inStoneMode = false;
                    stoneAttack.activeAttack = false;
                    disableControl = false;
                    rb.isKinematic = false;
                }
                if (inStoneMode && !rb.isKinematic)
                    rb.velocity = -Vector3.up * 25f;
                if (stoneModeFirstFrame)
                    stoneModeFirstFrame = false;
                */
            }

            //    if (!jumpButtonDown && input.GetButtonDown("Jump"))
            //        Debug.Log("INPUT NOT READ: " + disableControl +" "+ cam);

            if (currentAnim.inIdle && !inIdleRoutine && !defeated)
            {
                inIdleRoutine = true;
                StartCoroutine("IdleRoutine");
            }
            if (!currentAnim.inIdle && inIdleRoutine)
            {
                inIdleRoutine = false;
                StopCoroutine("RandomiseMainIdle");
                StopCoroutine("IdleRoutine");
            }

            if (gliding && !currentAnim.inGlide && anim.GetNextAnimatorStateInfo(0).IsName("Glide"))
            {
                anim.ResetTrigger("enterGlide");
                anim.SetTrigger("enterGlide");
            }

            if (currentAnim.onWall && onGround)
                anim.Play("Idle", 0);
        }
        else
        {
            if (Physics.Raycast(groundCheckers[0].transform.position, -Vector3.up, out groundHits[0], 0.1f, whatIsGround) && !groundHits[0].collider.isTrigger)
            {
                if (delayed && (groundHits[0].collider.gameObject.tag == "Elevator" || (groundHits[0].collider.gameObject.tag == "OneWay" && groundHits[0].collider.gameObject.name == "OneWay Moving"))
                    && groundHits[0].collider.gameObject != this.transform.parent)
                {
                    this.transform.SetParent(groundHits[0].collider.gameObject.transform);
                }
                if (groundHits[0].collider.gameObject.tag != "Elevator" && groundHits[0].collider.gameObject.tag != "OneWay" && groundHits[0].collider.gameObject.name != "OneWay Moving" && this.transform.parent != null)
                    this.transform.SetParent(initialParent);
            }
        }


        //null checks
        if (cam == null)
            return;

        //main
        if (!disableControl)
        {
            if (!lockVelocity && !onSpline)
            {
                if (inRiverForce)
                {
                    rfMultiplier = 1f;
                    if (inLeafSlide)
                        rfMultiplier = 0.1f;
                    zeroVelocity = riverForce * 0.1f * rfMultiplier;
                }

                if (!onSlide)
                {
                    float inputMagnitude = Mathf.Clamp01(Vector3.Magnitude(new Vector3(leftX, 0f, leftY)));

                    if (onGround)
                    {
                        if (!runByDefault)
                        {
                            if (!runButtonHold)
                                movementSpeed = groundSpeed * inputMagnitude;
                            else
                                movementSpeed = sprintSpeed * inputMagnitude;
                        }
                        else
                        {
                            if (runButtonHold)
                                movementSpeed = groundSpeed * inputMagnitude;
                            else
                                movementSpeed = sprintSpeed * inputMagnitude;
                        }
                    }
                    else
                    {
                        if (!runByDefault)
                        {
                            if (!runButtonHold || gliding)
                                movementSpeed = Mathf.Lerp(movementSpeed, groundSpeed * inputMagnitude, Time.fixedDeltaTime * 5f);
                            else
                                movementSpeed = Mathf.Lerp(movementSpeed, sprintSpeed * inputMagnitude, Time.fixedDeltaTime * 5f);
                        }
                        else
                        {
                            if (runButtonHold || gliding)
                                movementSpeed = Mathf.Lerp(movementSpeed, groundSpeed * inputMagnitude, Time.fixedDeltaTime * 5f);
                            else
                                movementSpeed = Mathf.Lerp(movementSpeed, sprintSpeed * inputMagnitude, Time.fixedDeltaTime * 5f);
                        }
                    }

                    if (isBoosting)
                        movementSpeed = boostSpeed;
                    if (inLeafSlide)
                        movementSpeed = leafSlideSpeed;

                    if (inLeafSlide && isBoosting)
                        movementSpeed = leafSlideSpeed * 1.2f;

                    if (chargingAttack || currentAnim.inAttackRelease)
                        movementSpeed *= 0.5f;

                    if (slowMovement)
                        movementSpeed *= 0.5f;

                    if (superslowMovement)
                        movementSpeed *= 0.2f;

                    if (flowerPower && movementSpeed > 0.01f)
                        movementSpeed += (0.2f * inputMagnitude);


                    float lerpFactor = 1f;
                    if (waterPhysics)
                    {
                        movementSpeed *= 0.9f;
                        lerpFactor = 0.5f;
                    }

                    if (inLeafSlide)
                    {
                        rb.velocity = Vector3.Lerp(rb.velocity, zeroVelocity + new Vector3(lookAt.transform.forward.x * movementSpeed, rb.velocity.y, lookAt.transform.forward.z * movementSpeed), Time.fixedDeltaTime * lerpFactor);
                    }
                    else
                    {
                        if (!onIce)
                        {
                            if (onGround || (!onGround && Vector3.Angle(this.transform.forward, lookAt.transform.forward) > 175f))
                                rb.velocity = zeroVelocity + new Vector3(lookAt.transform.forward.x * movementSpeed, rb.velocity.y, lookAt.transform.forward.z * movementSpeed);
                            else
                                rb.velocity = Vector3.Lerp(rb.velocity, zeroVelocity + new Vector3(this.transform.forward.x * movementSpeed, rb.velocity.y, this.transform.forward.z * movementSpeed), Time.fixedDeltaTime * 5f * lerpFactor);
                        }
                        else
                            rb.velocity = Vector3.Lerp(rb.velocity, zeroVelocity + new Vector3(this.transform.forward.x * movementSpeed, rb.velocity.y, this.transform.forward.z * movementSpeed), Time.fixedDeltaTime * lerpFactor);
                    }

                    if (onGround)
                    {
                        if (movementSpeed == 0f)
                        {
                            if (!onIce)
                                rb.velocity = zeroVelocity;
                            else
                                rb.velocity = Vector3.Lerp(rb.velocity, zeroVelocity, Time.fixedDeltaTime * lerpFactor);
                        }
                        //    if (pID == 0)
                        //        Debug.Log(rb.velocity + " " + zeroVelocity + " " + movementSpeed +" " + riverForce +" "+ onIncline +" "+ inclineNormal +" " + gvIncline);
                        //    else
                        //        rb.AddForce(-inclineNormal * rb.mass * gravityForce * 2f, ForceMode.Force);
                    }
                }
                else
                {
                    rb.AddForce(((relative.transform.forward * leftY * 4f) + (relative.transform.right * leftX * 4f) + (Vector3.up * -8f)) * rb.mass * 2f, ForceMode.Force);
                }

                if (!onGround)
                {
                    if (delayed && Physics.Raycast(this.transform.position, this.transform.forward, out wallJumpRay, 0.5f, whatIsGround) && !wallJumpRay.collider.isTrigger && wallJumpRay.normal.y < 0.1f
                                && wallJumpRay.collider.tag != "NoWallJump" && wallJumpRay.collider.GetComponentInChildren<Renderer>() && CanWallJumpOnMaterial(wallJumpRay.collider.GetComponentInChildren<Renderer>().material.name)
                                && Vector3.Magnitude(new Vector3(leftX, 0f, leftY)) >= 0.6f)
                    {
                        if (!startedWallCheck)
                            StartCoroutine("WallCheck");
                        else
                        {
                            if (againstWall)
                                anim.ResetTrigger("falling");
                        }
                    }
                    else
                    {
                        if (startedWallCheck)
                        {
                            startedWallCheck = false;
                            StopCoroutine("WallCheck");
                        }
                        if (againstWall)
                        {
                            againstWall = false;
                            anim.SetTrigger("falling");
                        }
                    }

                    if (!againstWall)
                    {
                        if (gliding)
                        {
                            if (currentAnim.inHit)
                            {
                                anim.ResetTrigger("exitGlide");
                                anim.SetTrigger("enterGlide");
                            }
                            if (health < 2)
                                rb.velocity = new Vector3(rb.velocity.x, -glideSpeed * 2f, rb.velocity.z);
                            else
                                rb.velocity = new Vector3(rb.velocity.x, -glideSpeed, rb.velocity.z);
                        }
                        if (!gliding)
                        {
                            if (jumped && !slowedJump && !jumpButtonHold)
                            {
                                rb.velocity = Vector3.Lerp(rb.velocity, new Vector3(rb.velocity.x, 0f, rb.velocity.z), 0.5f);
                                slowedJump = true;
                            }
                            if (umbrellaPower)
                                umbrellaDescent = 0.8f;
                            else
                                umbrellaDescent = 1f;

                            rb.AddForce(-Vector3.up * rb.mass * gravityForce * umbrellaDescent, ForceMode.Force);
                        }
                    }
                    else
                        rb.velocity = new Vector3(0f, -glideSpeed, 0f);
                }
                else
                {
                    if (againstWall)
                    {
                        againstWall = false;
                        anim.SetTrigger("falling");
                    }
                    if (startedWallCheck)
                    {
                        startedWallCheck = false;
                        StopCoroutine("WallCheck");
                    }
                }

                if (inWindCol || inBamboo)
                {
                    if (gliding && inBamboo)
                        rb.velocity = new Vector3(rb.velocity.x, curBambooSpeed, rb.velocity.z);
                    else
                    {
                        if (gliding && inWindCol)
                        {
                            if (this.transform.position.y >= currentWindCol.transform.position.y + ((currentWindCol.size.y * 0.49f) * currentWindCol.transform.lossyScale.y))
                                rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
                            else
                                rb.velocity = new Vector3(rb.velocity.x, windColumnSpeed, rb.velocity.z);
                        }
                    }
                    //    if (inWaterCol)
                    //        rb.velocity = new Vector3(rb.velocity.x, waterColumnSpeed, rb.velocity.z);
                }

                if (jumpButtonDown && !bounce && delayed)
                {
                    Jump();
                }
                else
                {
                    //    if (jumpButtonDown)
                    //       Debug.Log("MISSED JUMP INPUT? " + delayed +" "+ bounce);
                }

                if (bounce)
                {
                    bounce = false;
                    jumped = true;
                    this.transform.SetParent(initialParent);
                    if (!onIncline)
                        this.transform.position += (Vector3.up * 0.05f);
                    else
                        this.transform.position += (Vector3.up * 0.15f);
                    rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
                    rb.AddForce(bounceDir * jumpImpulse * 1.7f, ForceMode.Impulse);
                    cancelLeafHold = true;
                    jumpChance = false;
                    fallNow = true;
                    doubleJumped = false;
                    hasTripJumped = false;
                    if (!inLeafSlide)
                        jumpST.Emit = true;
                    slowedJump = true;
                    if (!bounceSuper)
                    {
                        if (bounceNinfea)
                            sound.clip = bounceNinfeaSound;
                        else
                        {
                            if (!bounceDrums)
                                sound.clip = bounceSound;
                            else
                                bounceDrums = false;
                        }
                    }
                    else
                        sound.clip = bounceSuperSound;

                    if (pID == 0 && sound.clip == bounceNinfeaSound)
                        anim.SetTrigger("jumpSpinNow");
                    else
                        anim.SetTrigger("jumpNow");

                    sound.loop = false;
                    sound.PlayDelayed(0f);
                    jumpEM.enabled = true;
                    jumpParticles.GetComponent<ParticleSystem>().Play();
                    StartCoroutine(DeactivateParticle(jumpParticles, jumpEM));
                    delayed = false;
                    StartCoroutine("Delay");
                }

                if (inRiverForce)
                {
                    rb.AddForce(riverForce * rfMultiplier, ForceMode.Acceleration);
                }

            }
            else
            {
                if (waterPhysics)
                    rb.AddForce(-Vector3.up * rb.mass * gravityForce * 0.5f, ForceMode.Force);
                else
                    rb.AddForce(-Vector3.up * rb.mass * gravityForce, ForceMode.Force);
            }
        }
        if ((Physics.Raycast(checkSea.transform.position, -Vector3.up, out seaHit, 2.4f, whatIsSea) && seaHit.collider.name.Contains("sea")) ||
            (Physics.Raycast(checkSea.transform.position + (Vector3.up * 0.2f), -Vector3.up, out seaHit, 0.4f, whatIsSea) && seaHit.collider.name.Contains("Underwater")))
        {
            if (!hitSea)
            {
                hitSea = true;
                if (seaHit.collider.name.Contains("sea"))
                    PlaySplashEffect(seaHit.point + (Vector3.up * 2.4f));
                else
                    PlaySplashEffect(seaHit.point);
            }
        }
        else
        {
            if (hitSea)
                hitSea = false;
        }
    }

    public void Jump()
    {
        //   Debug.Log("JUMP SUCCESSFULL: " + jumpChance + " " + jumped + " " + onGround + " " + againstWall);
        if (jumpChance && !jumped)
        {
            jumped = true;
            //    Debug.Log("JUMP SUCCESSFUL");
            if (superslowMovement)
                superslowMovement = false;
            canGlideFromGround = false;
            onSlide = false;
            firstWallJumped = false;
            this.transform.SetParent(initialParent);
            if (!onIncline)
                this.transform.position += (Vector3.up * 0.05f);
            else
                this.transform.position += (Vector3.up * 0.15f);
            if (rb.velocity.y < 0f || onIncline)
            {
                rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            }
            rb.AddForce(Vector3.up * jumpImpulse, ForceMode.Impulse);
            cancelLeafHold = true;
            jumpChance = false;
            fallNow = true;
            if (!inLeafSlide)
            {
                anim.SetTrigger("jumpNow");
                jumpST.Emit = true;
            }
            else
            {
                anim.SetFloat("skateboardJump", (float)Random.Range(0, 4));
                anim.SetTrigger("jumpSlide");
            }
            slowedJump = false;
            jumpIterate = Random.Range(0, 3);
            sound.loop = false;
            if (jumpIterate == 0)
                sound.clip = jump1Sound;
            if (jumpIterate == 1)
                sound.clip = jump2Sound;
            if (jumpIterate == 2)
                sound.clip = jump3Sound;
            sound.PlayDelayed(0f);
            jumpEM.enabled = true;
            jumpParticles.GetComponent<ParticleSystem>().Play();
            StartCoroutine(DeactivateParticle(jumpParticles, jumpEM));
            delayed = false;
            StartCoroutine("Delay");
        }
        else
        {
            if (!onGround)
            {
                if (againstWall)
                {
                    againstWall = false;
                    startedWallCheck = false;
                    StopCoroutine("WallCheck");

                    lockVelocity = true;
                    if (gliding)
                    {
                        //    Debug.Log("EXIT GLIDE BECAUSE AGAINST WALL");
                        gliding = false;
                        anim.SetTrigger("exitGlide");
                    }

                    this.transform.forward = new Vector3(wallJumpRay.normal.x, 0f, wallJumpRay.normal.z);
                    rb.velocity = Vector3.up * 10f;
                    rb.AddForce((new Vector3(wallJumpRay.normal.x, 0f, wallJumpRay.normal.z) * doubleJumpImpulse), ForceMode.Impulse);
                    doublejumpIterate = Random.Range(0, 2);
                    sound.clip = wallKickSound;
                    HDRumbleMain.PlayVibrationPreset(pID, "B04_Bam1", 1f, 1, 0.2f);
                    sound.loop = false;
                    sound.PlayDelayed(0f);
                    anim.SetTrigger("doubleJumpNow");

                    if (firstWallJumped)
                    {
                        glideDelay = true;
                        StopCoroutine("GlideDelay");
                        StartCoroutine("GlideDelay");
                    }
                    firstWallJumped = true;

                    StartCoroutine("Locking2", 10f);
                    delayed = false;
                    StartCoroutine("Delay");
                }
                else
                {
                    if (!inLeafSlide && ((hasTripleJump && !hasTripJumped) || !doubleJumped))
                    {
                        if (gliding)
                        {
                            gliding = false;
                            anim.SetTrigger("exitGlide");
                            //    Debug.Log("EXIT GLIDE BECAUSE DOUBLEJUMPING");
                        }
                        float inputMagnitude = Mathf.Clamp01(Vector3.Magnitude(new Vector3(leftX, 0f, leftY)));
                        if (!runByDefault)
                        {
                            if (!runButtonHold)
                                movementSpeed = groundSpeed * inputMagnitude;
                            else
                                movementSpeed = sprintSpeed * inputMagnitude;
                        }
                        else
                        {
                            if (runButtonHold)
                                movementSpeed = groundSpeed * inputMagnitude;
                            else
                                movementSpeed = sprintSpeed * inputMagnitude;
                        }
                        this.transform.forward = lookAt.transform.forward;
                        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

                        float jumpMultiply = 1f;
                        if (anim.GetBool("damaged") == true)
                            jumpMultiply = 0.8f;
                        rb.AddForce(Vector3.up * doubleJumpImpulse * jumpMultiply, ForceMode.Impulse);
                        if (doubleJumped)
                        {
                            HDRumbleMain.PlayVibrationPreset(pID, "K04_FadingPatter1", 0.5f, 1, 0.2f);
                            hasTripJumped = true;
                            EmitWallHitParticles();
                            if (pID == 0)
                                StartCoroutine("LeafInDoubleJump");
                        }
                        if (!doubleJumped)
                        {
                            HDRumbleMain.PlayVibrationPreset(pID, "B07_Bump2", 0.5f, 1, 0.2f);

                            doubleJumped = true;
                            //    Debug.Log("DOUBLE JUMP SUCCESSFUL");
                            EmitWallHitParticles();
                            if (pID == 0 && !hasTripleJump)
                                StartCoroutine("LeafInDoubleJump");
                        }
                        doublejumpIterate = Random.Range(0, 2);
                        if (hasTripJumped)
                        {
                            if (doublejumpIterate == 0)
                                sound.clip = triplejump1Sound;
                            if (doublejumpIterate == 1)
                                sound.clip = triplejump2Sound;
                        }
                        else
                        {
                            if (doublejumpIterate == 0)
                                sound.clip = doublejump1Sound;
                            if (doublejumpIterate == 1)
                                sound.clip = doublejump2Sound;
                        }
                        sound.loop = false;
                        sound.PlayDelayed(0f);
                        dJumpEM.enabled = true;
                        doubleJumpParticles.GetComponent<ParticleSystem>().Play();
                        StartCoroutine(DeactivateParticle(doubleJumpParticles, dJumpEM));
                        anim.SetTrigger("doubleJumpNow");
                        delayed = false;
                        StartCoroutine("Delay");
                    }
                    //    else
                    //        Debug.Log("MISSED DOUBLE JUMP: " + inLeafSlide + " . " + hasTripleJump + " . " + hasTripJumped + " . " + doubleJumped);
                }
            }
        }
    }

    IEnumerator BlueBerryFinder(bool justOpened)
    {
        if (justOpened)
            yield return new WaitForSeconds(2f);

        blueBerryIDCreators.Clear();
        foreach (BlueBerryIDCreator bbidc in FindObjectsOfType<BlueBerryIDCreator>())
        {
            bool useThisOne = false;
            foreach (string s in ps.sS.levelNames)
            {
                if (s == bbidc.gameObject.scene.name)
                {
                    if (PlayerPrefs.GetString(s + "BlueBerry").Contains("0"))
                        useThisOne = true;
                }
            }
            if (useThisOne)
                blueBerryIDCreators.Add(bbidc);
        }
        bool isABerry = false;
        float smallestDistance = 9999999f;
        if (blueBerryIDCreators.Count != 0)
        {
            Vector3 closestPosition = Vector3.zero;
            foreach (BlueBerryIDCreator bbidc in blueBerryIDCreators)
            {
                int berryIndex = 0;
                string prefToCheck = PlayerPrefs.GetString(bbidc.gameObject.scene.name + "BlueBerry");
                foreach (Transform t in bbidc.berries)
                {
                    if (prefToCheck[berryIndex].ToString() == "0")
                    {
                        if ((t.position - this.transform.position).sqrMagnitude < smallestDistance)
                        {
                            smallestDistance = (t.position - this.transform.position).sqrMagnitude;
                            closestPosition = t.position;
                        }
                    }
                    berryIndex++;
                }
            }
            isABerry = true;
        }
        else
        {
            /*    bool useThisOne = false;
                int sceneToCheck = 0;
                int sceneCounter = 0;
                foreach (string s in ps.sS.levelNames)
                {
                    if (!useThisOne)
                    {
                        if (s != "MainPlaza7New" && s != "ExternalWorld")
                        {
                            int total = 80;
                            if (s == "Level2")
                                total = 90;
                            if (s == "Level6")
                                total = 120;

                            if (PlayerPrefs.GetString(s + "BlueBerry").Contains("0"))
                            {
                                useThisOne = true;
                                sceneToCheck = sceneCounter;
                                smallestDistance = (ps.sS.loadLevelAdditives[sceneToCheck].transform.position - this.transform.position).sqrMagnitude;
                            }

                            sceneCounter++;
                        }
                    }
                }*/
        }
        if (!isABerry)
            bbFinderIntensity = Mathf.Lerp(1f, 0f, smallestDistance / 700000f);
        else
            bbFinderIntensity = Mathf.Lerp(1f, 0f, smallestDistance / 10000f);

        if (bbFinderIntensity > 0f)
        {
            bbFinderEM.enabled = true;
            bbFinderParticles.gameObject.SetActive(true);
            bbFinderParticles.GetComponent<ParticleSystem>().Play();
            StartCoroutine(DeactivateParticle(bbFinderParticles, bbFinderEM));

            bbFinderSource.Stop();
            if (bbFinderSource.clip != bbFinderSound)
                bbFinderSource.clip = bbFinderSound;
            bbFinderSource.pitch = Mathf.Lerp(0.6f, 1.4f, bbFinderIntensity);
            bbFinderSource.PlayDelayed(0f);

            bbFinderAnim.SetTrigger("signal");

            StartCoroutine("BBFinderMaterialFlash");
        }

        yield return new WaitForSeconds(Mathf.Lerp(3f, 1f, bbFinderIntensity));

        if (usingBBFinder)
            StartCoroutine("BlueBerryFinder", false);
    }

    IEnumerator BBFinderMaterialFlash()
    {
        for (float t = 0f; t <= 1f; t += Time.deltaTime)
        {
            bbFinderMat.SetFloat("_RimSharpnessF", Mathf.Lerp(0f, 1.16f, t));
            yield return null;
        }
        bbFinderMat.SetFloat("_RimSharpnessF", 1.16f);
    }

    void FixedUpdate()
    {
    }

    void LateUpdate()
    {
        if (cam != null)
        {
            if (Vector3.Distance(this.transform.position, cam.transform.position) < 0.65f)
            {
                if (curOpaque)
                {
                    curOpaque = false;
                    foreach (SkinnedMeshRenderer smr in toHideRender)
                        smr.enabled = false;
                }
            }
            else
            {
                if (!curOpaque)
                {
                    curOpaque = true;
                    foreach (SkinnedMeshRenderer smr in toHideRender)
                        smr.enabled = true;
                }
            }
            if (onGround && anim.gameObject.activeSelf && anim.gameObject.activeInHierarchy && anim.GetBool("inLocomotion") && (this.gameObject.transform.parent == null || this.gameObject.transform.parent.name == "Character and Camera Home"))
            {
                if (leftAboveZero)
                {
                    if (leftFoot.transform.localEulerAngles.y <= 0f || leftFoot.transform.localEulerAngles.y > 180f)
                    {
                        if (Physics.Raycast(leftFoot.transform.position, -Vector3.up, out leftRay, 1f, whatIsFootPrinted) && !leftRay.collider.isTrigger)
                        {
                            if (leftRay.collider.GetComponentInChildren<MeshRenderer>() == null || !leftRay.collider.GetComponentInChildren<MeshRenderer>().material.name.Contains("Ice"))
                            {
                                leftAboveZero = false;
                                leftFPs[leftInt].transform.SetParent(initialParent);
                                leftFPs[leftInt].transform.position = leftRay.point + (Vector3.up * 0.01f);
                                leftFPs[leftInt].transform.forward = this.transform.forward;
                                leftInt++;
                                if (leftInt >= leftFPs.Length)
                                    leftInt = 0;
                            }
                        }
                    }
                }
                else
                {
                    if (leftFoot.transform.localEulerAngles.y > 0f && leftFoot.transform.localEulerAngles.y < 180f)
                        leftAboveZero = true;
                }
                if (rightAboveZero)
                {
                    if (rightFoot.transform.localEulerAngles.y <= 0f || rightFoot.transform.localEulerAngles.y > 180f)
                    {
                        if (Physics.Raycast(rightFoot.transform.position, -Vector3.up, out rightRay, 1f, whatIsFootPrinted) && !rightRay.collider.isTrigger)
                        {
                            if (rightRay.collider.GetComponentInChildren<MeshRenderer>() == null || !rightRay.collider.GetComponentInChildren<MeshRenderer>().material.name.Contains("Ice"))
                            {
                                rightAboveZero = false;
                                rightFPs[rightInt].transform.SetParent(initialParent);
                                rightFPs[rightInt].transform.position = rightRay.point + (Vector3.up * 0.01f);
                                rightFPs[rightInt].transform.forward = this.transform.forward;
                                rightInt++;
                                if (rightInt >= rightFPs.Length)
                                    rightInt = 0;
                            }
                        }
                    }
                }
                else
                {
                    if (rightFoot.transform.localEulerAngles.y > 0f && rightFoot.transform.localEulerAngles.y < 180f)
                        rightAboveZero = true;
                }
            }
        }

        if (this.transform.localEulerAngles.x != 0f || this.transform.localEulerAngles.z != 0f)
        {
            this.transform.rotation = new Quaternion(0f, this.transform.rotation.y, 0f, this.transform.rotation.w);
        }

        if (rippleEM.enabled == true)
        {
            rippleParticles.transform.position = new Vector3(this.transform.position.x, waterFSHeight - 0.02f, this.transform.position.z);
            rippleCParticles.transform.position = rippleParticles.transform.position;
        }

        if (ps.enableDebugTab && input.GetButton("Levitate"))
        {
            if (onGround)
                onGround = false;
            rb.velocity = new Vector3(rb.velocity.x, 10f, rb.velocity.z);
        }

        if (this.transform.lossyScale != Vector3.one)
        {
            this.transform.SetParent(initialParent);
            this.transform.localScale = Vector3.one;
        }
        /*
        if (onGround && this.transform.parent != initialParent)
        {
            if (rb.interpolation != RigidbodyInterpolation.None)
                rb.interpolation = RigidbodyInterpolation.None;
        }
        else
        {
            if (rb.interpolation != RigidbodyInterpolation.Interpolate)
                rb.interpolation = RigidbodyInterpolation.Interpolate;
        }
        */
        /*
        if (noIntObj != null)
        {
            if (Input.GetKeyDown(KeyCode.B))
            {
                noIntObj.SetActive(false);
                yesIntObj.SetActive(false);
                rb.interpolation = RigidbodyInterpolation.None;
            }
            if (Input.GetKeyDown(KeyCode.N))
            {
                noIntObj.SetActive(true);
                yesIntObj.SetActive(false);
                rb.interpolation = RigidbodyInterpolation.Interpolate;
            }
            if (Input.GetKeyDown(KeyCode.M))
            {
                noIntObj.SetActive(false);
                yesIntObj.SetActive(true);
                rb.interpolation = RigidbodyInterpolation.Extrapolate;
            }
        }
        */
    }

    void OnCollisionEnter(Collision o)
    {
        foreach (ContactPoint cp in o.contacts)
        {
            if (cp.otherCollider.gameObject.layer == enemyCollisionLayer)
            {
                if (bbPower && cp.otherCollider.gameObject.name != "DyingGroundDown" && !cp.otherCollider.gameObject.name.ToLower().Contains("sea"))
                {
                    if (cp.otherCollider.gameObject.GetComponent<EnemyHP>() != null && cp.otherCollider.gameObject.GetComponent<EnemyHP>().icodescript != null)
                        cp.otherCollider.gameObject.GetComponent<EnemyHP>().SendFlying(true);
                    else
                        BounceCharacter(false, cp.otherCollider.gameObject, false, false);
                }
                else
                {

                    if ((!currentAnim.inSomeAttack || (Vector3.Angle(this.transform.forward, (cp.point - this.transform.position)) > 90f))
                        || (cp.otherCollider.gameObject.GetComponent<EnemyHP>() != null && (cp.otherCollider.gameObject.GetComponent<EnemyHP>().instantKill || cp.otherCollider.gameObject.GetComponent<EnemyHP>().damageOnly)))    //if yeti bugs, this is why//
                    {
                        if (cp.otherCollider.gameObject.GetComponent<EnemyHP>() == null || !cp.otherCollider.gameObject.GetComponent<EnemyHP>().hitByShield)
                        {
                            if ((cp.otherCollider.tag == "InstantKill" || (cp.otherCollider.gameObject.GetComponent<EnemyHP>() != null &&
                                cp.otherCollider.gameObject.GetComponent<EnemyHP>().instantKill)) && !defeated)
                            {
                                if (cp.otherCollider.tag == "InstantKill")
                                {
                                    killedByDark = true;
                                    StartCoroutine("DisableAnim");
                                }
                                if (pID == 0 && curLeafMat != null && leafMesh.material.name != greyLeaf.name + " (Instance)" && leafMesh.material.name != whiteLeaf.name + " (Instance)")
                                    curLeafMat = leafMesh.material;
                                anim.Play("Defeat", 0);
                                Defeat();
                                return;
                            }
                            else
                            {
                                if (acornPower && cp.otherCollider.gameObject.GetComponent<EnemyHP>() != null)
                                    cp.otherCollider.gameObject.GetComponent<EnemyHP>().SendFlying(true);
                            }
                            if (cp.otherCollider.gameObject.layer == enemyCollisionLayer && !frozen && !invinceFrames && !beenHit)
                                GetHurt(cp.otherCollider.gameObject, cp.otherCollider.gameObject.GetComponent<EnemyHP>().damageAmount);
                        }
                    }
                    else
                    {
                        if (currentAnim.inSomeAttack && !checkingAttackAnim)
                        {
                            checkingAttackAnim = true;
                            StartCoroutine("CheckAttackAnim");
                        }
                    }
                }
            }

            if (cp.otherCollider.tag == "TouchActivate")
            {
                if (cp.otherCollider.gameObject.GetComponentInChildren<Animator>())
                {
                    cp.otherCollider.gameObject.GetComponentInChildren<Animator>().SetBool("Activate", true);
                    //   BerrySpawnManager.SpawnABerry(cp.otherCollider.transform.position);
                }
            }

            if (!frozen && !freezeInvince && cp.otherCollider.tag == "Freezing")
                StartCoroutine("FreezeCharacter");

            if (cp.otherCollider.gameObject.GetComponentInParent<EnemyBarrier>() != null)
                GetHurt(cp.otherCollider.gameObject, 0);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "InstantKill")
        {
            if (pID == 0 && curLeafMat != null && leafMesh.material.name != greyLeaf.name + " (Instance)" && leafMesh.material.name != whiteLeaf.name + " (Instance)")
                curLeafMat = leafMesh.material;
            anim.Play("Defeat", 0);
            defeated = true;
            Defeat();
            StartCoroutine("DisableAnim");
            return;
        }
        if (other.gameObject.tag == "Boost")
        {
            isBoosting = true;
            boostRandom = (float)Random.Range(1, 3);
            runST.Emit = true;

            anim.SetBool("isBoosting", true);

            boostingSound.clip = boostSound;
            boostingSound.loop = true;
            boostingSound.PlayDelayed(0f);

            StopCoroutine("BoostLength");
            StartCoroutine("BoostLength");

            boostEM.enabled = true;
            boostParticles.GetComponent<ParticleSystem>().Play();
            StartCoroutine(DeactivateParticle(boostParticles, boostEM));

            boostFlowerParticles.transform.position = other.gameObject.transform.position;
            boostFlowerParticles.transform.SetParent(this.transform.parent);
            boostFlowerEM.enabled = true;
            boostFlowerParticles.GetComponent<ParticleSystem>().Play();
            StartCoroutine(DeactivateParticle(boostParticles, boostFlowerEM));

        }
        if (other.gameObject.layer == waterCollisionLayer && other.gameObject.tag == "Shallow" && !defeated)
        {
            if (!inWaterFS)
            {
                inWaterFS = true;

                waterFSHeight = other.ClosestPoint(this.transform.position + Vector3.up).y + 0.1f;

                if (other.gameObject.transform.localEulerAngles.x == 0f && other.gameObject.transform.localEulerAngles.z == 0f)
                {
                    rippleParticles.gameObject.SetActive(true);
                    rippleEM.enabled = true;
                    rippleCEM.enabled = true;
                }

                PlaySplashEffect(this.transform.position - (Vector3.up * 0.2f));
            }
        }

        if (other.gameObject.tag == "FallingPlatform")
        {
            if (other.gameObject.name.Contains("Pine"))
                other.gameObject.transform.parent.GetComponentInChildren<Animator>().SetTrigger("ActivateTrigger");
            else
                other.gameObject.transform.parent.GetComponentInChildren<Animator>().SetBool("Activate", true);
        }

        if (other.gameObject.tag == "Slow")
        {
            slowCount++;
            slowMovement = true;
        }

        if (other.gameObject.layer == bouncyLayer)
        {
            if (other.transform.parent.name.Contains("NinfeaFlower") || other.transform.parent.name.Contains("Cactus"))
                BounceCharacter(true, other.gameObject, false, true);
            else
            {
                if (other.gameObject.name.Contains("Drum"))
                {
                    other.gameObject.GetComponent<AudioSource>().PlayDelayed(0f);
                    bounceDrums = true;
                }
                BounceCharacter(true, other.gameObject, false, false);
            }
        }

        if (other.gameObject.layer == bouncySuperLayer)
            BounceCharacter(true, other.gameObject, true, false);

        if (other.gameObject.name == "Foliage")
        {
            TreeShakeManager.ShakeTree(other.gameObject, other.gameObject.GetComponent<MeshRenderer>());
        }

        if (other.gameObject.tag == "Border")
            StartCoroutine(ReverseCharacter(other.gameObject.transform.forward));

        if (other.tag == "TouchActivate")
        {
            if (other.gameObject.GetComponentInChildren<Animator>())
            {
                other.gameObject.GetComponentInChildren<Animator>().SetBool("Activate", true);
                //    BerrySpawnManager.SpawnABerry(other.transform.position);
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.name == "Stone Attack")
            Physics.IgnoreCollision(this.GetComponent<Collider>(), other, true);

        if ((!inWindCol || (currentWindCol != null && currentWindCol != other.GetComponent<BoxCollider>()) && other.gameObject.transform.position.y > currentWindCol.transform.position.y))
        {
            if (other.gameObject.layer == windColumnColLayer)
            {
                //    if (other.gameObject.tag == "WaterColumn")
                //        inWaterCol = true;
                //    else
                {
                    if (other.transform.parent.name == "Bamboo")
                        StartCoroutine("InTheBamboo");
                    inWindCol = true;
                    currentWindCol = other.GetComponent<BoxCollider>();
                }
            }
        }
        if (gliding)
        {
            if (other.tag == "WindBox")
                rb.AddForce(other.transform.forward * 50f * rb.mass, ForceMode.Force);
        }

        if (other.gameObject.layer == waterCollisionLayer && other.gameObject.GetComponent<RiverForce>() && (!inRiverForce || other.gameObject.GetComponent<RiverForce>() != currentRiverForce) && !defeated)
        {
            inRiverForce = true;
            currentRiverForce = other.gameObject.GetComponent<RiverForce>();
            if (!currentRiverForce.setToForward)
                riverForce = currentRiverForce.forceDirection * currentRiverForce.intensity;
            else
                riverForce = currentRiverForce.gameObject.transform.forward * currentRiverForce.intensity;
            anim.ResetTrigger("unDrag");
            anim.SetTrigger("drag");
        }

        if (onGround && other.gameObject.tag == "Quicksand")
        {
            if (!superslowMovement)
                superslowMovement = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == waterCollisionLayer && other.gameObject.tag == "Shallow")
        {
            if (inWaterFS)
                ExitWaterFS();
        }
        if (other.gameObject.tag == "FallingPlatform")
        {
            other.gameObject.transform.parent.GetComponentInChildren<Animator>().SetBool("Activate", false);
        }
        if (other.gameObject.layer == waterCollisionLayer && other.gameObject.GetComponent<RiverForce>())
        {
            if (currentRiverForce == other.gameObject.GetComponent<RiverForce>())
            {
                ExitRiverForce();
            }
        }

        if (other.gameObject.tag == "Slow")
        {
            slowCount--;
            if (slowCount == 0)
                slowMovement = false;
        }
        if (other.gameObject.tag == "Quicksand")
        {
            superslowMovement = false;
        }
        if (other.gameObject.layer == windColumnColLayer)
        {
            //    if (other.gameObject.tag == "WaterColumn")
            //        inWaterCol = false;
            //    else
            inWindCol = false;
        }
    }

    public void ExitRiverForce()
    {
        if (this.gameObject.activeInHierarchy && this.gameObject.activeSelf)
        {
            inRiverForce = false;
            currentRiverForce = null;
            riverForce = Vector3.zero;
            zeroVelocity = Vector3.zero;
        }
    }

    public void ExitWaterFS()
    {
        if (this.gameObject.activeInHierarchy && this.gameObject.activeSelf)
        {
            inWaterFS = false;
            rippleEM.enabled = false;
            rippleParticles.gameObject.SetActive(false);
            rippleCEM.enabled = false;
        }
    }

    public void CalculateBasicAnimation(float inputX, float inputY)
    {

        pushing = false;
        if (onGround)
        {
            if ((inputX != 0f || inputY != 0f))
            {
                if (!anim.GetBool("inLocomotion") && !inLeafSlide && !inRiverForce)
                    anim.SetBool("inLocomotion", true);
                else
                {
                    if (inWaterFS)
                    {
                        if (splashMoveEM.enabled == false)
                            splashMoveEM.enabled = true;
                    }
                    else
                    {
                        if (splashMoveEM.enabled == true)
                            splashMoveEM.enabled = false;

                        if (((Vector3.Magnitude(new Vector3(inputX, 0f, inputY)) <= .75f) || input.GetButton("Walk")))
                        {
                            if (walkEM.enabled == false)
                                walkEM.enabled = true;
                            if (runEM.enabled == true)
                            {
                                runEM.enabled = false;
                            }
                        }
                        else
                        {
                            if (runEM.enabled == false && !isBoosting && !inLeafSlide)
                            {
                                runEM.enabled = true;
                            }
                            if (walkEM.enabled == true)
                                walkEM.enabled = false;
                        }
                    }
                }
                if (!isBoosting)
                {
                    if (!runByDefault)
                    {
                        if (!input.GetButton("Run"))
                            locoSpeed = Vector3.Magnitude(new Vector3(inputX, 0f, inputY)) * 20f;
                        else
                            locoSpeed = Vector3.Magnitude(new Vector3(inputX, 0f, inputY).normalized) * 60f;
                    }
                    else
                    {
                        bool slowRunning = (new Vector3(inputX, 0f, inputY).magnitude <= 0.6f);
                        if (input.GetButton("Run") || slowRunning)
                        {
                            if (!slowRunning)
                                locoSpeed = Vector3.Magnitude(new Vector3(inputX, 0f, inputY)) * 20f;
                            else
                                locoSpeed = Vector3.Magnitude(new Vector3(inputX, 0f, inputY)) * 40f;
                        }
                        else
                            locoSpeed = Vector3.Magnitude(new Vector3(inputX, 0f, inputY).normalized) * 60f;
                    }
                }

                if (Physics.Raycast(this.transform.position, this.transform.forward, out pushRay, boxZ * 3f, whatIsMoveable))
                {
                    pushing = true;
                    if (sound.clip != boxPushing)
                    {
                        sound.clip = boxPushing;
                        sound.loop = true;
                        sound.PlayDelayed(0f);
                    }
                    curBox = pushRay.collider.gameObject;
                    if (curBox.GetComponent<Rigidbody>().isKinematic)
                        pushRay.collider.gameObject.GetComponent<Rigidbody>().isKinematic = false;

                    if (pushRay.collider.gameObject.GetComponent<Rigidbody>().velocity == Vector3.zero &&
                       (Physics.Raycast(curBox.transform.position, (relative.transform.forward * inputY) + (relative.transform.right * inputX),
                           curBox.transform.lossyScale.z * curBox.GetComponent<BoxCollider>().size.z)) ||
                       (Physics.Raycast(curBox.transform.position + (Vector3.up * 0.3f), (relative.transform.forward * inputY) + (relative.transform.right * inputX),
                           curBox.transform.lossyScale.z * curBox.GetComponent<BoxCollider>().size.z)) ||
                       (Physics.Raycast(curBox.transform.position - (Vector3.up * 0.3f), (relative.transform.forward * inputY) + (relative.transform.right * inputX),
                           curBox.transform.lossyScale.z * curBox.GetComponent<BoxCollider>().size.z)))
                    {
                        leftX = 0f;
                        leftY = 0f;
                        direction = Vector3.zero;
                        rb.velocity = Vector3.zero;
                    }
                }
            }
            else
            {
                if (anim.GetBool("inLocomotion") && !isBoosting)
                    anim.SetBool("inLocomotion", false);
                CheckWREM();
            }
        }
        else
        {
            if (anim.GetBool("inLocomotion") && rb.velocity.y < -0.1f && fallNow)
            {
                anim.SetTrigger("falling");
                anim.SetBool("inLocomotion", false);
            }
            CheckWREM();
        }

        if (isBoosting)
            locoSpeed = 100f * boostRandom;
        anim.SetFloat("Speed", Mathf.Lerp(anim.GetFloat("Speed"), locoSpeed, Time.deltaTime * 5f));

        if (locoSpeed > 2f)
        {
            if (!onSpline)
            {
                anim.SetFloat("turnAmount", Mathf.Lerp(anim.GetFloat("turnAmount"),
                    Vector3.Angle((relative.transform.forward * inputY) + (relative.transform.right * inputX), this.transform.forward) * Mathf.Sign(Vector3.Dot((relative.transform.forward * inputY) + (relative.transform.right * inputX), this.transform.right)),
                    Time.deltaTime * 20f));
            }
            else
                anim.SetFloat("turnAmount", Mathf.Lerp(anim.GetFloat("turnAmount"), -inputY * 50f, Time.deltaTime * 20f));
        }
        else
            anim.SetFloat("turnAmount", Mathf.Lerp(anim.GetFloat("turnAmount"), 0f, Time.deltaTime * 5f));

        if (pushing && !anim.GetBool("pushing"))
            anim.SetBool("pushing", true);
        if (!pushing && anim.GetBool("pushing"))
            anim.SetBool("pushing", false);
    }

    public void ToggleAbility(int itemID, bool activate)
    {
        if (itemID == 3)
            flowerPower = activate;

        if (itemID == 5)
            umbrellaPower = activate;

        if (itemID == 6)
            acornPower = activate;

        if (itemID == 7)
            elfPower = activate;

        if (itemID == 8)
            bandanaPower = activate;

        if (itemID == 24)
            bbPower = activate;
    }

    public void BounceCharacter(bool isBounceObj, GameObject other, bool superBounce, bool ninfea)
    {
        /*
        if (inStoneMode)
        {
            inStoneMode = false;
            disableControl = false;
            rb.isKinematic = false;
            stoneAttack.activeAttack = false;
        }*/
        if (gliding)
        {
            gliding = false;
            anim.SetTrigger("exitGlide");
            //    Debug.Log("EXIT GLIDE BECAUSE BOUNCING");
        }
        if (isBounceObj)
        {
            if (other.GetComponent<Animator>() != null)
                other.GetComponent<Animator>().SetTrigger("Bounce");
            bounceDir = other.transform.up;
        }
        else
            bounceDir = Vector3.up * 0.5f;

        bounceNinfea = ninfea;
        if (ninfea)
            bounceDir *= 1.4f;
        if (superBounce)
            bounceDir *= 1.2f;
        if (bandanaPower)
            bounceDir *= 1.2f;

        bounce = true;
        bounceSuper = superBounce;
        ledgeHopping = false;
        lockVelocity = false;
    }

    void IfHasDrop()
    {
        if (leafColObj.GetComponent<LeafCollision>().hasWaterDrop)
        {
            dropObj.gameObject.SetActive(true);
            dropObj.GetComponent<Rigidbody>().useGravity = true;
            dropObj.transform.position = waterDropMesh.transform.position;
            dropObj.GetComponent<Rigidbody>().velocity = rb.velocity + (this.transform.forward * 3f) + (Vector3.up * 2f);
        }
        leafColObj.GetComponent<LeafCollision>().DropWater();
    }

    void PlaySplashEffect(Vector3 placement)
    {
        if (splashParticles.transform.parent != this.transform.parent)
            splashParticles.transform.SetParent(this.transform.parent);

        splashParticles.transform.position = placement;

        splashEM.enabled = true;
        splashParticles.GetComponent<ParticleSystem>().Play();
        StartCoroutine(DeactivateParticle(splashParticles, splashEM));
        interactions.Stop();
        interactions.loop = false;
        interactions.clip = splashSounds[Random.Range(0, splashSounds.Length)];
        interactions.PlayDelayed(0f);
    }

    void LeafCreate()
    {
        leafColObj = new GameObject();
        leafColObj.gameObject.name = "leafColObj " + this.gameObject.name;
        leafColObj.AddComponent<BoxCollider>();
        BoxCollider boxCol = leafColObj.GetComponent<BoxCollider>();
        boxCol.isTrigger = true;
        boxCol.center = Vector3.zero;

        if (pID == 0)                                       //WOODLE
            boxCol.size = new Vector3(0.4f, 0.39f, 0.19f);

        if (pID == 1)                                       //FOX
            boxCol.size = new Vector3(0.5f, 0.35f, 0.4f);

        if (pID == 2)                                       //BEAVER
        {
            boxCol.center = new Vector3(0f, 0.3f, 0f);
            boxCol.size = new Vector3(1f, 0.6f, 1f);
        }

        if (pID == 3)                                       //BUSH
        {
            boxCol.center = new Vector3(0f, 0f, 0.5f);
            boxCol.size = new Vector3(1f, 1f, 1.2f);
        }
        leafColObj.AddComponent<Rigidbody>();
        Rigidbody leafRB = leafColObj.GetComponent<Rigidbody>();
        leafRB.useGravity = false;
        leafRB.isKinematic = true;
        leafColObj.gameObject.AddComponent<AudioSource>();
        leafColObj.gameObject.GetComponent<AudioSource>().clip = leafWallHit;
        leafColObj.gameObject.GetComponent<AudioSource>().outputAudioMixerGroup = sound.outputAudioMixerGroup;
        leafColObj.gameObject.layer = leafCollisionLayer;
        leafCol = leafColObj.AddComponent<LeafCollision>();
        LeafCollision lCol = leafColObj.GetComponent<LeafCollision>();
        lCol.curAnim = this.currentAnim;
        lCol.boneToFollow = leafBone.gameObject;
        if (pID == 0)
            lCol.waterDrop = waterDropMesh;
        lCol.tpc = this;
        lCol.waterLayer = waterfallCollisionLayer;
        lCol.enemyLayer = enemyCollisionLayer;
        lCol.collisionLayers = whatIsGround;
        lCol.buttonParticles = buttonParticles;
        leafColObj.AddComponent<AttackSettings>();
        leafAS = leafColObj.GetComponent<AttackSettings>();
        leafAS.attackAmount = 1;
        leafAS.activeAttack = false;
        leafAS.enemyLayer = enemyCollisionLayer;
        leafAS.tpc = this;

        int wbInt = 0;
        wbPar1 = new GameObject(this.gameObject.name + " Windballs 1 Parent");
        wbPar2 = new GameObject(this.gameObject.name + " Windballs 2 Parent");
        wbPar3 = new GameObject(this.gameObject.name + " Windballs 3 Parent");
        wbPar4 = new GameObject(this.gameObject.name + " Windballs 4 Parent");
        while (wbInt < windBalls.Length)
        {
            WBInst(wbInt, windBalls, windball1, wbPar1);
            wbInt++;
        }

        if (pID == 0)
        {
            wbInt = 0;
            while (wbInt < windBalls2.Length)
            {
                WBInst(wbInt, windBalls2, windball2, wbPar2);
                wbInt++;
            }
            wbInt = 0;
            while (wbInt < windBalls3.Length)
            {
                WBInst(wbInt, windBalls3, windball3, wbPar3);
                wbInt++;
            }
            wbInt = 0;
            while (wbInt < windBalls4.Length)
            {
                WBInst(wbInt, windBalls4, windball4, wbPar4);
                wbInt++;
            }

            dropObj = Instantiate(waterReleaseObj, Vector3.zero, this.transform.rotation) as GameObject;
            dropObj.gameObject.name = this.gameObject.name + " Water Release";
            dropObj.gameObject.SetActive(false);
        }
    }

    private void CancelAttack()
    {
        StopVibrate();
        startedAttack = false;
        attackPressOnly = false;
        isHoldingAttack = false;
        chargingAttack = false;
        fullyCharged = false;
        attackBuffer = false;
        attackBufferX = false;
        leafST.Emit = false;
        leafAS.activeAttack = false;
        chargedEM.enabled = false;
        atkSound.Stop();
        atkSound.loop = false;
    }

    void CancelLeafSlide()
    {
        if (hasLeafSlide)
        {
            inLeafSlide = false;
            leafSlideST.Emit = false;
            anim.SetTrigger("exitSlide");
            StopCoroutine("SlideSound");
            atkSound.loop = false;
            atkSound.Stop();
            interactions.Stop();
        }
    }

    public void CollectedWater()
    {
        sound.clip = waterCatchSound;
        sound.PlayDelayed(0f);
    }

    void WBInst(int index, GameObject[] wbArray, GameObject wBType, GameObject parentObj)
    {
        wbArray[index] = Instantiate(wBType, Vector3.zero, this.transform.rotation) as GameObject;
        wbArray[index].gameObject.name = this.gameObject.name + " Wind Ball Projectile " + index.ToString();
        wbArray[index].AddComponent<AttackSettings>();
        wbArray[index].GetComponent<AttackSettings>().attackAmount = 2;
        wbArray[index].GetComponent<AttackSettings>().activeAttack = true;
        wbArray[index].GetComponent<AttackSettings>().enemyLayer = enemyCollisionLayer;
        wbArray[index].GetComponent<AttackSettings>().tpc = this;
        wbArray[index].GetComponent<AttackSettings>().isWindball = true;
        wbArray[index].AddComponent<AudioSource>();
        wbArray[index].transform.SetParent(parentObj.transform);
        AudioSource wBAS = wbArray[index].GetComponent<AudioSource>();
        wBAS.spread = 190;
        wBAS.rolloffMode = AudioRolloffMode.Linear;
        wBAS.clip = windballSound;
        wBAS.outputAudioMixerGroup = sound.outputAudioMixerGroup;
        wbArray[index].gameObject.SetActive(false);
    }

    public void UpdateBerryHUDRed()
    {
        PlayerPrefs.SetInt("Berries", berryCount);

        //    PlayerPrefs.Save();
        berryText.text = berryCount.ToString();
        if (berryHUD.GetBool("function") == false)
        {
            berryHUD.SetBool("function", true);
            StartCoroutine("HUDWait");
        }
        else
        {
            StopCoroutine("HUDWait");
            StartCoroutine("HUDWait");
        }
        berryHUD.SetTrigger("bounceText");
    }

    public void UpdateBerryHUDBlue()
    {
        PlayerPrefs.SetInt("BlueBerries", blueberryCount);
        if (blueberryCount >= 100)
        {
#if !UNITY_EDITOR
        //    if (SteamManager.Initialized) {                
        //        SteamUserStats.SetAchievement("Blue Berry Lover");
        //        SteamUserStats.StoreStats();
        //    }
#endif
        }
        //    PlayerPrefs.Save();
        blueberryText.text = blueberryCount.ToString();
        if (blueberryHUD.GetBool("function") == false)
        {
            blueberryHUD.SetBool("function", true);
            StartCoroutine("HUDWait");
        }
        else
        {
            StopCoroutine("HUDWait");
            StartCoroutine("HUDWait");
        }
        blueberryHUD.SetTrigger("bounceText");
    }

    public void ChallengePortalWarpPFX()
    {
        portal1EM.enabled = true;
        portal2EM.enabled = true;
        portal1Particles.gameObject.SetActive(true);
        portal2Particles.gameObject.SetActive(true);
        portal1Particles.GetComponent<ParticleSystem>().Play();
        portal2Particles.GetComponent<ParticleSystem>().Play();
        StartCoroutine(DeactivateParticle(portal1Particles, portal1EM));
        StartCoroutine(DeactivateParticle(portal2Particles, portal2EM));
    }

    IEnumerator DefeatWait()
    {
        defeated = true;
        anim.SetTrigger("fail");
        StartCoroutine("DisableAnim");
        yield return new WaitForSeconds(1f);
        StartCoroutine("Invincibility");
        Defeat();
    }

    void Defeat()
    {
        StartCoroutine("DefeatCoRo");
    }

    IEnumerator DisableAnim()
    {
        while (!anim.GetCurrentAnimatorStateInfo(0).IsName("Defeat"))
            yield return null;
        yield return new WaitForSeconds(1.95f);
        anim.enabled = false;
    }

    IEnumerator DefeatCoRo()
    {
        capcol.enabled = false;
        rb.useGravity = false;
        rb.velocity = Vector3.zero;
        rb.drag = 1000f;
        if (pID == 0)
            cam.GetComponent<CameraFollower>().disableControl = true;
        HDRumbleMain.PlayVibrationPreset(pID, "B10_Boom3", 1f, 0, 0.5f);
        boostingSound.Stop();
        isBoosting = false;
        runST.Emit = false;
        if (leafSlideST)
            leafSlideST.Emit = false;
        anim.SetBool("isBoosting", false);
        CancelLeafSlide();
        CancelAttack();
        leafAS.activeAttack = false;
        invinceFrames = true;
        this.transform.SetParent(initialParent);
        while (inButtonCutscene)
            yield return null;
        if (killedByDark)
            StartCoroutine("TurnDark");
        StartCoroutine("ResetWoodle");
        sound.clip = defeatSound;
        sound.PlayDelayed(0f);
    }

    public void PlayBlockStar()
    {
        blockStarEM.enabled = true;
        blockStarParticles.gameObject.SetActive(true);
        blockStarParticles.GetComponent<ParticleSystem>().Play();
        StartCoroutine(DeactivateParticle(blockStarParticles, blockStarEM));
    }

    public void GetHurt(GameObject enemyObj, int damageAmount)
    {
        disableControl = false;

        isHoldingLeaf = false;
        IfHasDrop();
        anim.SetTrigger("quickLeafDown");

        if (telescope != null && telescope.usingTelescope)
            telescope.ExitTelescope();

        if (leafCol.hasWaterDrop)
            leafCol.DropWater();

        if (gliding)
        {
            gliding = false;
            //   Debug.Log("EXIT GLIDE BECAUSE HURT");
        }

        isHoldingLeaf = false;
        //    anim.SetTrigger("quickLeafDown");

        //    if (!inStoneMode)
        {
            if (damageAmount != 0)
            {
                if (leafNo == 1)
                {
                    hitGEM.enabled = true;
                    hitGParticles.gameObject.SetActive(true);
                    hitGParticles.GetComponent<ParticleSystem>().Play();
                    StartCoroutine(DeactivateParticle(hitGParticles, hitGEM));
                }
                if (leafNo == 2)
                {
                    hitYEM.enabled = true;
                    hitYParticles.gameObject.SetActive(true);
                    hitYParticles.GetComponent<ParticleSystem>().Play();
                    StartCoroutine(DeactivateParticle(hitYParticles, hitYEM));
                }
                if (leafNo == 3)
                {
                    hitREM.enabled = true;
                    hitRParticles.gameObject.SetActive(true);
                    hitRParticles.GetComponent<ParticleSystem>().Play();
                    StartCoroutine(DeactivateParticle(hitRParticles, hitREM));
                }
                if (leafNo == 4)
                {
                    hitWEM.enabled = true;
                    hitWParticles.gameObject.SetActive(true);
                    hitWParticles.GetComponent<ParticleSystem>().Play();
                    StartCoroutine(DeactivateParticle(hitWParticles, hitWEM));
                }
            }
            else
            {
                knockedEM.enabled = true;
                knockedParticles.gameObject.SetActive(true);
                knockedParticles.GetComponent<ParticleSystem>().Play();
                StartCoroutine(DeactivateParticle(knockedParticles, knockedEM));
            }

            CancelLeafSlide();
            killedByDark = false;

            if (enemyObj.GetComponent<EnemyHP>() != null && (enemyObj.GetComponent<EnemyHP>().instantKill) || (pID != 0 && damageAmount != 0))
            {
                health = 0;
                killedByDark = enemyObj.GetComponent<EnemyHP>().isDark;
                if (!killedByDark)
                    killedByDark = enemyObj.GetComponent<EnemyHP>().instantKill;
                if (pID == 0 && leafMesh.material.name != greyLeaf.name + " (Instance)" && leafMesh.material.name != whiteLeaf.name + " (Instance)")
                    curLeafMat = leafMesh.material;
                anim.SetTrigger("fail");
                if (!killedByDark)
                {
                    StopCoroutine("HitFlash");
                    StartCoroutine("HitFlash");
                }
                if (defeatCollideSound != null)
                {
                    atkSound.clip = defeatCollideSound;
                    atkSound.PlayDelayed(0f);
                }
                Defeat();
            }
            else
            {
                if (damageAmount != 0)
                {
                    if (!PlayerPrefs.HasKey("GotHurt") || PlayerPrefs.GetInt("GotHurt", 0) == 0)
                    {
                        PlayerPrefs.SetInt("GotHurt", 1);
                        ps.gameObject.GetComponentInChildren<TextTriggerMain>().SetText(6);
                    }
                }

                HDRumbleMain.PlayVibrationPreset(pID, "B07_Bump2", 1f, 0, 0.3f);

                beenHit = true;
                if (damageAmount != 0)
                    LoseHealth(1);
                if (health == 0)
                {
                    if (defeatCollideSound != null)
                    {
                        atkSound.clip = defeatCollideSound;
                        atkSound.PlayDelayed(0f);
                    }
                    if (enemyObj.GetComponent<EnemyHP>() != null)
                        killedByDark = enemyObj.GetComponent<EnemyHP>().isDark;
                    if (!killedByDark)
                    {
                        StopCoroutine("HitFlash");
                        StartCoroutine("HitFlash");
                    }
                    StopCoroutine("RefillLife");
                    defeated = true;
                    boostingSound.Stop();
                    isBoosting = false;
                    runST.Emit = false;
                    if (leafSlideST)
                        leafSlideST.Emit = false;
                    anim.SetBool("isBoosting", false);
                    if (pID == 0)
                    {
                        anim.SetLayerWeight(2, 1f);
                        anim.SetBool("damaged", true);
                    }
                    disableControl = true;
                    rb.useGravity = true;
                    StartCoroutine("Invincibility");
                    StartCoroutine("DefeatWait");
                }
                else
                {
                    if (pID == 0 && damageAmount != 0)
                    {
                        anim.SetLayerWeight(2, 0.7f);
                        anim.SetBool("damaged", true);
                        if (curLeafMat != null && leafMesh.material.name != greyLeaf.name + " (Instance)" && leafMesh.material.name != whiteLeaf.name + " (Instance)")
                            curLeafMat = leafMesh.material;
                        leafMesh.material = greyLeaf;
                        leafMesh.material.SetColor("_MainColor", Color.grey);
                        interactions.clip = leafShrink;
                        interactions.PlayDelayed(0f);
                        StartCoroutine("LeafShrink");
                        StartCoroutine("RefillLife");
                    }
                    if (damageAmount != 0)
                    {
                        StopCoroutine("HitFlash");
                        StartCoroutine("HitFlash");
                    }

                    StartCoroutine("Invincibility");
                }
                anim.SetTrigger("hurt");
                hitIterate++;
                if (hitIterate > 2)
                {
                    hitIterate = 0;
                    sound.clip = hit1Sound;
                }
                if (hitIterate == 1)
                    sound.clip = hit2Sound;
                if (hitIterate == 2)
                    sound.clip = hit3Sound;
                sound.PlayDelayed(0f);

                lockVelocity = true;
                onGround = true;
                ledgeHopping = false;
                HurtVelo(enemyObj);
            }
        }
    }

    IEnumerator HitFlash()
    {
        f = 1;
        reverse = false;
        iterator = 0f;
        mCount = 0;
        while (f >= 0)
        {
            iterator = (float)f / 20f;

            mCount = 0;
            foreach (Renderer r in renderers)
            {
                if (!r.name.Contains("eaf"))
                {
                    foreach (Material m in r.materials)
                    {
                        if (m.HasProperty("_RimColor") && m.HasProperty("_RimIntensityF"))
                        {
                            m.SetColor("_RimColor", Color.Lerp(origRimCol[mCount], endCol, iterator));
                            //    if (m.HasProperty("_RimIntensityF"))
                            m.SetFloat("_RimIntensityF", Mathf.Lerp(origRimInt[mCount], 10f, iterator));
                            mCount++;
                        }
                    }
                }
            }

            if (f == 20)
                reverse = true;
            if (!reverse)
                f++;
            else
                f--;
            yield return null;
        }
    }

    void HurtVelo(GameObject enemyObj)
    {
        rb.velocity = Vector3.zero;
        this.transform.LookAt(new Vector3(enemyObj.transform.position.x, this.transform.position.y, enemyObj.transform.position.z));
        rb.velocity = -(this.transform.forward * 5f) + (Vector3.up * 8f);
        StartCoroutine(PersistSpeed(enemyObj));
    }

    public void AddHealth(int amount)
    {
        health += amount;
        if (health > 2)
            health = 2;
    }

    public void LoseHealth(int amount)
    {
        health -= amount;
        if (health < 0)
            health = 0;
    }

    void CycleMaterials(string[] matList, string matName)
    {
        int groundIT = 0;
        while (groundIT < matList.Length && !foundMat)
        {
            if (matList[groundIT] == matName || matList[groundIT] + " (Instance)" == matName)
                foundMat = true;
            groundIT++;
        }
        if (foundMat)
        {
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

    void CheckWREM()
    {
        if (walkEM.enabled == true)
            walkEM.enabled = false;
        if (runEM.enabled == true)
            runEM.enabled = false;
        if (splashMoveEM.enabled == true)
            splashMoveEM.enabled = false;
    }

    public bool CanWallJumpOnMaterial(string matName)
    {
        foreach (string s in nonWallJumpMaterials)
        {
            if (s == matName || s + " (Instance)" == matName)
                return false;
        }
        return true;
    }

    public void Vibrate(float level, float duration, int motorIndex)
    {
        if (Time.timeScale != 0f && PlayerPrefs.GetInt("Vibration", 1) == 1)
        {
            foreach (Joystick j in input.controllers.Joysticks)
            {
                if (pID != 0 || j.GetLastTimeActive() == ReInput.time.unscaledTime)
                {
                    if (!j.supportsVibration)
                        continue;
                    if (j.vibrationMotorCount > 0)
                        j.SetVibration(motorIndex, level, duration);
                }
            }
        }
    }

    public void StopVibrate()
    {
        if (PlayerPrefs.GetInt("Vibration", 1) == 1)
        {
            foreach (Joystick j in input.controllers.Joysticks)
            {
                if (!j.supportsVibration)
                    continue;
                if (j.vibrationMotorCount > 0)
                    j.StopVibration();
            }
        }
    }

    public void HitPushBack(Vector3 direct)
    {
        StopCoroutine("AttackSpeedStop");
        lockVelocity = true;
        rb.velocity = Vector3.zero;
        this.transform.forward = new Vector3(-direct.x, 0f, -direct.z);
        rb.AddForce((direct * doubleJumpImpulse * 0.5f), ForceMode.Impulse);
        anim.SetTrigger("leafBounce");
        EmitWallHitParticles();
        StartCoroutine("Locking2", 5f);
    }

    void EmitWallHitParticles()
    {
        if (leafNo == 1)
        {
            wallGEM.enabled = true;
            wallGParticles.gameObject.SetActive(true);
            wallGParticles.GetComponent<ParticleSystem>().Play();
            StartCoroutine(DeactivateParticle(wallGParticles, wallGEM));
        }
        if (leafNo == 2)
        {
            wallYEM.enabled = true;
            wallYParticles.gameObject.SetActive(true);
            wallYParticles.GetComponent<ParticleSystem>().Play();
            StartCoroutine(DeactivateParticle(wallYParticles, wallYEM));
        }
        if (leafNo == 3)
        {
            wallREM.enabled = true;
            wallRParticles.gameObject.SetActive(true);
            wallRParticles.GetComponent<ParticleSystem>().Play();
            StartCoroutine(DeactivateParticle(wallRParticles, wallREM));
        }
        if (leafNo == 4)
        {
            wallWEM.enabled = true;
            wallWParticles.gameObject.SetActive(true);
            wallWParticles.GetComponent<ParticleSystem>().Play();
            StartCoroutine(DeactivateParticle(wallWParticles, wallWEM));
        }
    }

    public void HitAnEnemy(Vector3 pos, bool wasWindball, bool infiniteHealth, Animator enemyAnim, bool defeatingHit, bool bigEnemy)
    {
        enemyEM.enabled = true;
        if (!wasWindball)
        {
            HDRumbleMain.PlayVibrationPreset(pID, "B03_Bu3", 1f, 1, 0.2f);
        }

        enHit1Sound.Stop();

        if (attackAnimIterate == 0)
        {
            if (!infiniteHealth)
                enHit1Sound.clip = enemyHitSound;
            else
                enHit1Sound.clip = enemyDefendSound;
            enHit1Sound.PlayDelayed(0f);
        }
        if (attackAnimIterate == 1)
        {
            if (!infiniteHealth)
                enHit2Sound.clip = enemyHitSound2;
            else
                enHit2Sound.clip = enemyDefendSound2;
            enHit2Sound.PlayDelayed(0f);
        }
        if (attackAnimIterate == 2)
        {
            if (!infiniteHealth)
                enHit3Sound.clip = enemyHitSound3;
            else
                enHit3Sound.clip = enemyDefendSound3;
            enHit3Sound.PlayDelayed(0f);
        }
        enemyParticles.transform.position = pos;
        enemyParticles.transform.LookAt(cam.transform.position);
        enemyParticles.transform.localEulerAngles = new Vector3(enemyParticles.transform.localEulerAngles.x, enemyParticles.transform.localEulerAngles.y, Random.Range(0f, 360f));
        enemyParticles.GetComponent<ParticleSystem>().Play();
        if (pID == 0)
            StartCoroutine(HitFreeze(enemyAnim, defeatingHit, wasWindball, bigEnemy));
        StartCoroutine(DeactivateParticle(enemyParticles, enemyEM));
    }

    IEnumerator Charging()
    {
        StartCoroutine("LoopWindballVib");
        if (chargingAttack)
        {
            fullyCharged = true;
            if (chargedEM.enabled == false)
                chargedEM.enabled = true;
        }
        while (input.GetButton("Attack"))
            yield return null;
        if (chargingAttack)
        {
            anim.SetTrigger("releaseAttack");
            chargingAttack = false;
            StopVibrate();
            atkSound.Stop();
            atkSound.loop = false;
            sound.clip = attackChargedSound;
            sound.PlayDelayed(0f);
            if (chargedEM.enabled == true)
                chargedEM.enabled = false;
            attackEM.enabled = true;
            attackParticles.GetComponent<ParticleSystem>().Play();
            StartCoroutine(DeactivateParticle(attackParticles, attackEM));
            LaunchWindball();
            CancelAttack();
        }
    }

    void LaunchWindball()
    {
        int wb = 0;
        bool found = false;
        if ((leafNo == 1 && pID == 0) || pID >= 1)
            WindballSet(windBalls, wb, found, windBall1Lifetime, windBall1Speed);
        if (leafNo == 2 && pID == 0)
            WindballSet(windBalls2, wb, found, windBall2Lifetime, windBall2Speed);
        if (leafNo == 3 && pID == 0)
            WindballSet(windBalls3, wb, found, windBall3Lifetime, windBall3Speed);
        if (leafNo == 4 && pID == 0)
            WindballSet(windBalls4, wb, found, windBall4Lifetime, windBall4Speed);
    }

    void WindballSet(GameObject[] wbs, int wb, bool found, float lifetime, float wbspeed)
    {
        while (wb < wbs.Length && !found)
        {
            if (wbs[wb].gameObject.activeSelf == false)
                found = true;
            else
                wb++;
        }
        if (found)
        {
            wbs[wb].gameObject.SetActive(true);
            foreach (Renderer r in wbs[wb].GetComponentsInChildren<Renderer>())
                r.enabled = true;
            wbs[wb].transform.position = leafBone.transform.position;
            wbs[wb].GetComponent<Rigidbody>().velocity = this.transform.forward * wbspeed;
            StartCoroutine(WindBall(wb, wbs, lifetime));
            wbs[wb].GetComponent<AudioSource>().Play();
            wbs[wb].GetComponent<AttackSettings>().activeAttack = true;
        }
    }

    public void SetDefaultLeafMat()
    {
        if (pID == 0)
        {
            curLeafMat = leafMesh.material;

            dissolves[2].origMaterials[0] = curLeafMat;

            if (leafNo == 1)
                dissolves[2].dissolveMaterials[0] = greenLeafDissolve;
            if (leafNo == 2)
                dissolves[2].dissolveMaterials[0] = yellowLeafDissolve;
            if (leafNo == 3)
                dissolves[2].dissolveMaterials[0] = redLeafDissolve;
            if (leafNo == 4)
                dissolves[2].dissolveMaterials[0] = blueLeafDissolve;
        }
    }

    IEnumerator SetLookAt()
    {
        if (currentAnim != null)
        {
            if (!lockVelocity)
            {
                if (!onSpline)
                    this.transform.rotation = Quaternion.Lerp(this.transform.rotation, lookAt.transform.rotation, Time.deltaTime * 5f);
                else
                    this.transform.rotation = Quaternion.Lerp(this.transform.rotation, relative.transform.rotation, Time.deltaTime * 5f);
            }
            else
                lookAt.transform.rotation = this.transform.rotation;
        }
        yield return null;
        StartCoroutine("SetLookAt");
    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(0.1f);
        delayed = true;
    }

    IEnumerator JumpWait(float time)
    {
        yield return new WaitForSeconds(0.15f);
        fallNow = true;
        yield return new WaitForSeconds(time - 0.15f);
        jumpChance = false;
        jumped = true;
    }

    IEnumerator AttackNext()
    {
        float an = 0f;
        while (an <= 0.5f)
        {
            if (!inHitFreeze && !ps.inPause)
                an += Time.deltaTime * Time.timeScale;
            yield return null;
        }
        attackAnimIterate = 0;
    }

    IEnumerator WindBall(int index, GameObject[] wbArray, float lifetime)
    {
        HDRumbleMain.PlayVibrationPreset(pID, "P05_DampedFm2", 1.3f, 0, 0.3f);
        yield return new WaitForSeconds(lifetime + 0.3f);
        wbArray[index].gameObject.SetActive(false);
    }

    IEnumerator LoopWindballVib()
    {
        while (chargingAttack)
        {
            HDRumbleMain.PlayVibrationPreset(pID, "P05_DampedFm2", 1f, 1, 0.2f);
            yield return new WaitForSeconds(0.5f);
        }
    }

    IEnumerator Buffer()
    {
        int a = 0;
        while (a < 28 && !letGoOfAttack)
        {
            yield return new WaitForEndOfFrame();
            a++;
        }
        attackBufferX = true;
        if (input.GetButton("Attack") && !letGoOfAttack && onGround)
        {
            if (input.GetButton("Attack") && !letGoOfAttack)
                isHoldingAttack = true;
            else
                attackPressOnly = true;
        }
        else
            attackPressOnly = true;
    }

    IEnumerator DeactiveAttack(float activeLength)
    {
        yield return new WaitForSeconds(activeLength);
        leafAS.activeAttack = false;
        CancelAttack();
        yield return new WaitForSeconds(0.25f);
    }

    IEnumerator BoostLength()
    {
        yield return new WaitForSeconds(4.5f);
        if (bandanaPower)
            yield return new WaitForSeconds(1f);
        boostingSound.Stop();
        isBoosting = false;
        runST.Emit = false;
        anim.SetBool("isBoosting", false);
    }

    IEnumerator ResetWoodle()
    {
        beingReset = true;
        while (inCutscene)
            yield return null;
        disableControl = true;
        if (pID == 0)
            cam.GetComponent<CameraFollower>().disableControl = true;
        rb.isKinematic = true;
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(Checkpoint());
        rb.drag = 0f;
        yield return new WaitForSeconds(1.3f);
        killedEM.enabled = true;
        killedParticles.GetComponent<ParticleSystem>().Play();
        StartCoroutine(DeactivateParticle(killedParticles, killedEM));
        yield return new WaitForSeconds(3.3f);
        if (pID != 0)
        {
            sound.clip = respawnSound;
            sound.loop = false;
            sound.Stop();
            sound.PlayDelayed(0f);
        }
        ExitWaterFS();
        inWindCol = false;
        //   inWaterCol = false;

        while (!anim.enabled)
            yield return null;
        anim.Play("Idle", 0);
        anim.SetBool("inLocomotion", false);
        anim.SetFloat("Speed", 0f);
        anim.SetFloat("turnAmount", 0f);

        interactions.PlayDelayed(0f);
        anim.SetBool("damaged", false);
        if (pID == 0)
            leafMesh.material = curLeafMat;
        capcol.enabled = true;
        if (challengePortal == null)
        {
            rb.isKinematic = false;
            disableControl = false;
            if (pID == 0)
                cam.GetComponent<CameraFollower>().disableControl = false;
        }
        health = 2;
        defeated = false;
        StartCoroutine("Invincibility");
        lockVelocity = false;
        anim.Play("Idle", 0);
        if (pID == 0)
        {
            StopCoroutine("Blink");
            StartCoroutine("Blink");
            anim.SetBool("damaged", false);
        }
        beingReset = false;
    }

    public IEnumerator Invincibility()
    {
        beenHit = false;
        Physics.IgnoreLayerCollision(characterCollisionLayer, enemyCollisionLayer, false);
        invinceFrames = true;
        if (health <= 1)
            yield return new WaitForSeconds(1f);
        else
            yield return null;
        invinceFrames = false;
    }

    IEnumerator Checkpoint()
    {
        yield return new WaitForSeconds(3f);
        if (pID == 0)
        {
            if (challengePortal != null)
                StartCoroutine(challengePortal.ResetCharacter(this));
            else
            {
                int lcp = PlayerPrefs.GetInt("LastCheckpoint", 0);
                if (lcp == -1)
                {
                    this.transform.position = new Vector3(0f, 1.5f, 0f);
                    if (PlayerManager.GetPlayer(1) != null)
                        PlayerManager.GetPlayer(1).transform.position = this.transform.position + (Vector3.up * 2f);
                    if (PlayerManager.GetPlayer(2) != null)
                        PlayerManager.GetPlayer(2).transform.position = this.transform.position + (Vector3.up * 2f);
                    if (PlayerManager.GetPlayer(3) != null)
                        PlayerManager.GetPlayer(3).transform.position = this.transform.position + (Vector3.up * 2f);
                }
                else
                    ps.Warp(lcp);
            }
        }
        else
            this.transform.position = PlayerManager.GetMainPlayer().transform.position + (Vector3.up * 2f);
    }

    public IEnumerator RespawnCharacterWait()
    {
        startedCharacterRespawn = true;
        yield return new WaitForSeconds(3f);
        while (!PlayerManager.GetMainPlayer().onGround)
            yield return null;
        RespawnCharacter();
    }

    void RespawnCharacter()
    {
        sound.clip = respawnSound;
        sound.loop = false;
        sound.Stop();
        sound.PlayDelayed(0f);
        this.transform.position += Vector3.up * 1000f;
        StartCoroutine(RespawnRoutine());
    }

    IEnumerator RespawnRoutine()
    {
        yield return new WaitForSeconds(0.4f);
        this.transform.position = PlayerManager.GetMainPlayer().gameObject.transform.position + (Vector3.up * 2f);
        startedCharacterRespawn = false;
    }

    IEnumerator RefillLife()
    {
        yield return new WaitForSeconds(4.3f);
        isHoldingLeaf = false;
        interactions.clip = leafGrow;
        AddHealth(1);
        interactions.PlayDelayed(0f);
        if (leafNo == 1)
        {
            refilGEM.enabled = true;
            leafRefilGParticles.gameObject.SetActive(true);
            leafRefilGParticles.GetComponent<ParticleSystem>().Play();
            StartCoroutine(DeactivateParticle(leafRefilGParticles, refilGEM));
        }
        if (leafNo == 2)
        {
            refilYEM.enabled = true;
            leafRefilYParticles.gameObject.SetActive(true);
            leafRefilYParticles.GetComponent<ParticleSystem>().Play();
            StartCoroutine(DeactivateParticle(leafRefilYParticles, refilYEM));
        }
        if (leafNo == 3)
        {
            refilREM.enabled = true;
            leafRefilRParticles.gameObject.SetActive(true);
            leafRefilRParticles.GetComponent<ParticleSystem>().Play();
            StartCoroutine(DeactivateParticle(leafRefilRParticles, refilREM));
        }
        if (leafNo == 4)
        {
            refilWEM.enabled = true;
            leafRefilWParticles.gameObject.SetActive(true);
            leafRefilWParticles.GetComponent<ParticleSystem>().Play();
            StartCoroutine(DeactivateParticle(leafRefilWParticles, refilWEM));
        }
        leafMesh.material = curLeafMat;
    }

    IEnumerator PersistSpeed(GameObject enemyObj)
    {
        Quaternion startRot = this.transform.rotation;
        this.transform.RotateAround(this.transform.position, Vector3.up, 180f);
        Quaternion endRot = this.transform.rotation;
        this.transform.rotation = startRot;
        for (float ps = 0f; ps <= 0.5f; ps += Time.deltaTime)
        {
            this.transform.rotation = Quaternion.Lerp(startRot, endRot, ps * 2f);
            yield return null;
        }
        onGround = true;
        ledgeHopping = false;
        lockVelocity = false;
    }

    IEnumerator Locking()
    {
        yield return new WaitForSeconds(Time.deltaTime * 30f);
        lockVelocity = false;
    }

    IEnumerator Locking2(float amount)
    {
        yield return new WaitForSeconds(Time.deltaTime * amount);
        lockVelocity = false;
    }

    IEnumerator LeafShrink()
    {
        StartCoroutine("LeafFlash");
        yield return new WaitForSeconds(5f);
        anim.SetBool("damaged", false);
    }

    IEnumerator LeafFlash()
    {
        float overall = 0f;
        float pingpong = 0f;
        bool goForward = true;
        while (overall <= 5f)
        {
            leafMesh.material.SetColor("_TintColor", Color.Lerp(Color.grey, Color.white, pingpong));
            if (goForward)
                pingpong += 0.1f;
            else
                pingpong -= 0.1f;
            if (goForward && pingpong >= 1f)
                goForward = false;
            if (!goForward && pingpong <= 0f)
                goForward = true;
            overall += Time.deltaTime;
            yield return null;
        }
        leafMesh.material.SetColor("_TintColor", Color.white);
    }

    IEnumerator PlayingInstrument()
    {
        yield return new WaitForSeconds(3.6f);
        isPlaying = false;
        cancelInstrument = true;
        instrumentEM.enabled = false;
        StartCoroutine("CancelInstrument");
    }

    IEnumerator CancelInstrument()
    {
        anim.SetTrigger("cancelMusic");
        instrumentEM.enabled = false;
        yield return new WaitForSeconds(0.5f);
        cancelInstrument = false;
    }

    IEnumerator ClipDelay()
    {
        if (isBoosting)
            yield return new WaitForSeconds(0.15f);
        else
            yield return new WaitForSeconds(Mathf.Lerp(0.2f, 0.6f, 1f - (movementSpeed / 10f)));
        stepDelay = false;
    }

    public IEnumerator DeactivateParticle(GameObject pObj, ParticleSystem.EmissionModule tempEM)
    {
        yield return new WaitForSeconds(pObj.GetComponent<ParticleSystem>().duration + 0.1f);
        tempEM.enabled = false;
    }

    IEnumerator EndSliding()
    {
        endingSlide = true;
        yield return new WaitForSeconds(0.5f);
        onSlide = false;
        if (sound.clip == sliding)
            sound.Stop();
        endingSlide = false;
    }

    IEnumerator WaitAttack()
    {
        yield return new WaitForSeconds(0.2f);
        canAttackNext = true;
    }

    IEnumerator LedgeHop()
    {
        Vector3 startPos = this.transform.position;
        Vector3 endPos = this.transform.position + (Vector3.up * 0.25f) + (this.transform.forward * 0.75f);

        Vector3 nextPos = Vector3.zero;

        float iter = 0f;
        for (float i = 0f; i <= 0.5f; i += (Time.deltaTime))
        {
            nextPos = Vector3.Lerp(startPos, endPos, i * 2f);
            iter += (Mathf.PI * Time.deltaTime * 2f);
            nextPos += Vector3.up * Mathf.Sin(iter);

            rb.velocity = Vector3.Lerp(rb.velocity, (nextPos - rb.position).normalized * 5f, Time.deltaTime * 30f);
            yield return null;
        }
        rb.isKinematic = false;
    }

    IEnumerator HopFailSafe()
    {
        yield return new WaitForSeconds(1f);
        StopCoroutine("LedgeHop");
        ledgeHopping = false;
        lockVelocity = false;
    }

    IEnumerator SlideSound()
    {
        if (atkSound.clip != enterLeafSlide)
        {
            atkSound.loop = false;
            atkSound.clip = enterLeafSlide;
            atkSound.PlayDelayed(0f);
        }
        yield return new WaitForSeconds(0.5f);
        if (atkSound.clip != leafSliding)
        {
            atkSound.loop = true;
            atkSound.clip = leafSliding;
            atkSound.PlayDelayed(0f);
        }
    }

    IEnumerator HUDWait()
    {
        yield return new WaitForSeconds(3f);
        if (berryHUD != null)
            berryHUD.SetBool("function", false);
        if (blueberryHUD != null)
            blueberryHUD.SetBool("function", false);
        if (compassHUD != null)
            compassHUD.SetBool("function", false);
        if (tearHUD != null)
            tearHUD.SetBool("function", false);
        if (itemHUD != null)
            itemHUD.SetBool("function", false);
    }

    IEnumerator VelocityDelay()
    {
        yield return new WaitForSeconds(0.3f);
        lockVelocity = false;
    }

    IEnumerator AttackSpeedStop()
    {
        for (float aaa = 0f; aaa < 1f / 6f; aaa += (Time.deltaTime))
            yield return null;
        rb.velocity = new Vector3(0f, rb.velocity.y, 0f);
    }

    IEnumerator Blink()
    {
        while (disableControl)
            yield return null;

        yield return new WaitForSeconds(Random.Range(2f, 4f));
        eyesRenderer.materials[1].mainTexture = eyesClosed;

        while (disableControl)
            yield return null;

        yield return new WaitForSeconds(0.3f);
        eyesRenderer.materials[1].mainTexture = eyesOpen;

        StartCoroutine("Blink");
    }

    public void PlayHitFreeze(Animator enemyAnim, bool defeatingHit, bool wasWindball, bool bigEnemy)
    {
        StartCoroutine(HitFreeze(enemyAnim, defeatingHit, wasWindball, bigEnemy));
    }

    IEnumerator HitFreeze(Animator enemyAnim, bool defeatingHit, bool wasWindball, bool bigEnemy)
    {
        inHitFreeze = (defeatingHit || wasWindball);
        if (defeatingHit)
        {
            if (camF == null)
                camF = cam.GetComponent<CameraFollower>();

            camF.AttackZoom();
        }

        yield return new WaitForSeconds(0.15f);
        if (defeatingHit)
            yield return new WaitForSeconds(0.05f);
        if (!ps.inPause)
        {
            float speedToUse = 0.5f;
            float timeOfEffect = 0.25f;
            if (wasWindball)
                speedToUse = 0.2f;
            if (defeatingHit)
            {
                speedToUse = 0.1f;
                timeOfEffect = 0.75f;
            }
            if (wasWindball && !defeatingHit)
                timeOfEffect = 0.2f;

            //    anim.speed = speedToUse;
            //    if(enemyAnim != null)
            //        enemyAnim.speed = speedToUse;
            //    anim.speed = 0f;
            //    if (enemyAnim != null)
            //        enemyAnim.speed = 0f;
            //    for (float t = 0; t < 0.2f; t += (Time.deltaTime / (Time.timeScale + 0.0001f)))
            //        yield return null;

            //    anim.speed = speedToUse;
            //    if(enemyAnim != null)
            //        enemyAnim.speed = speedToUse;
            //    anim.speed = 1f;
            //    if (enemyAnim != null)
            //        enemyAnim.speed = 1f;

            bool setSlow = (defeatingHit || wasWindball);
            if (setSlow)
                Time.timeScale = 0.1f;

            if (defeatingHit && bigEnemy)
            {
                Time.timeScale = 0.07f;
                timeOfEffect *= 1.25f;
            }

            for (float t = 0; t < timeOfEffect; t += (Time.deltaTime / (Time.timeScale + 0.0001f)))
                yield return null;

            inHitFreeze = false;

            while (ps.inPause)
                yield return null;

            anim.speed = 1f;
            if (enemyAnim != null)
                enemyAnim.speed = 1f;
            if (setSlow)
                Time.timeScale = 1f;
        }
        inHitFreeze = false;
    }

    IEnumerator TurnDark()
    {
        List<Color> origColors = new List<Color>();
        int smrCounter = 0;
        for (float t = 0f; t <= 2f; t += (Time.deltaTime))
        {
            if (t <= 1.5f)
            {
                smrCounter = 0;
                foreach (SkinnedMeshRenderer smr in bodyRenderers)
                {
                    if (t == 0f)
                        origColors.Add(smr.material.GetColor("_TintColor"));
                    smr.material.SetColor("_TintColor", Color.Lerp(origColors[smrCounter], Color.black, t / 2f));
                    smrCounter++;
                }
            }
            yield return null;
        }
        smrCounter = 0;
        foreach (SkinnedMeshRenderer smr in bodyRenderers)
        {
            smr.material.SetColor("_TintColor", origColors[smrCounter]);
            smrCounter++;
        }
    }


    IEnumerator FreezeCharacter()
    {
        frozen = true;
        ps.cantPause = true;
        disableControl = true;
        rb.isKinematic = true;
        anim.enabled = false;

        sound.clip = freezeSound;
        sound.PlayDelayed(0f);
        yield return new WaitForSeconds(0.1f);

        smrColors.Clear();
        foreach (SkinnedMeshRenderer smr in bodyRenderers)
        {
            SMRColors sMRC = new SMRColors();
            sMRC.tint = smr.material.GetColor("_TintColor");
            sMRC.rim = smr.material.GetColor("_RimColor");
            sMRC.sharpness = smr.material.GetFloat("_RimSharpnessF");
            sMRC.intensity = smr.material.GetFloat("_RimIntensityF");
            smrColors.Add(sMRC);

            smr.material.SetColor("_TintColor", Color.cyan);
            smr.material.SetColor("_RimColor", Color.cyan);
            smr.material.SetFloat("_RimSharpnessF", 0.4f);
            smr.material.SetFloat("_RimIntensityF", 1f);
        }

        yield return new WaitForSeconds(2.7f);

        sound.clip = unfreezeSound;
        sound.PlayDelayed(0f);
        yield return new WaitForSeconds(0.2f);

        int listCounter = 0;
        foreach (SkinnedMeshRenderer smr in bodyRenderers)
        {
            smr.material.SetColor("_TintColor", smrColors[listCounter].tint);
            smr.material.SetColor("_RimColor", smrColors[listCounter].rim);
            smr.material.SetFloat("_RimSharpnessF", smrColors[listCounter].sharpness);
            smr.material.SetFloat("_RimIntensityF", smrColors[listCounter].intensity);
            listCounter++;
        }

        anim.enabled = true;
        rb.isKinematic = false;
        disableControl = false;
        ps.cantPause = false;
        frozen = false;
        freezeInvince = true;

        yield return new WaitForSeconds(2f);

        freezeInvince = false;
    }

    IEnumerator IdleRoutine()
    {
        anim.Play("Idle");
        yield return null;
        anim.SetInteger("nextIdle", PlayRandomIdle());
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length - Time.deltaTime);
        yield return null;
        anim.SetInteger("nextIdle", PlayRandomIdle());
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length - Time.deltaTime);
        yield return null;
        anim.SetInteger("nextIdle", 4);
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length - Time.deltaTime);
        yield return null;
        if (pID == 0)
        {
            anim.SetInteger("nextIdle", PlayRandomIdle());
            yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length - Time.deltaTime);
            yield return null;
            anim.SetInteger("nextIdle", PlayRandomIdle());
            yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length - Time.deltaTime);
            yield return null;
            anim.SetInteger("nextIdle", 4);
            yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length - Time.deltaTime);
            yield return null;
            anim.SetInteger("nextIdle", PlayRandomIdle());
            yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length - Time.deltaTime);
            yield return null;
            anim.SetInteger("nextIdle", PlayRandomIdle());
            yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length - Time.deltaTime);
            yield return null;
            anim.SetInteger("nextIdle", 4);
            yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length - Time.deltaTime);
            yield return null;
            anim.SetInteger("nextIdle", PlayRandomIdle());
            yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length - Time.deltaTime);
            yield return null;
            anim.SetInteger("nextIdle", PlayRandomIdle());
            yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length - Time.deltaTime);
            yield return null;
            anim.SetInteger("nextIdle", 5);
            yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length - Time.deltaTime);
            yield return null;
        }
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length - Time.deltaTime);
        StartCoroutine("IdleRoutine");
    }

    int PlayRandomIdle()
    {
        int randomIdle = Random.Range(1, 4);
        if (randomIdle == anim.GetInteger("nextIdle"))
            randomIdle++;
        if (randomIdle >= 4)
            randomIdle = 1;

        return randomIdle;

    }

    IEnumerator LeafInDoubleJump()
    {
        if (pID == 0 && curLeafMat != null && leafMesh.material.name != greyLeaf.name + " (Instance)" && leafMesh.material.name != whiteLeaf.name + " (Instance)")
            curLeafMat = leafMesh.material;
        for (float f = 0f; (f <= (1f / 3f) && !onGround); f += (Time.deltaTime))
        {
            //    leafMesh.material.SetColor("_TintColor", Color.Lerp(Color.grey, Color.white, f / 20f)); 
            leafMesh.material.SetFloat("_RimSharpnessF", Mathf.Lerp(1.25f, 0f, f * 3f));
            yield return null;
        }
        leafMesh.material = whiteLeaf;
        while (!onGround && !bounce)
            yield return null;
        leafMesh.material = curLeafMat;
        for (float f = 0f; f <= (1f / 3f); f += (Time.deltaTime))
        {
            //    leafMesh.material.SetColor("_TintColor", Color.Lerp(Color.white, Color.grey, f / 20f));
            leafMesh.material.SetFloat("_RimSharpnessF", Mathf.Lerp(0f, 1.25f, f * 3f));
            yield return null;
        }
    }

    IEnumerator CheckAttackAnim()
    {
        while (currentAnim.inSomeAttack)
            yield return null;
        checkingAttackAnim = false;
        capcol.enabled = false;
        capcol.enabled = true;
    }

    IEnumerator GlideDelay()
    {
        yield return new WaitForSeconds(0.7f);
        glideDelay = false;
    }

    IEnumerator GroundDelay()
    {
        startedGroundDelay = true;
        yield return new WaitForSeconds(0.2f);
        canGlideFromGround = true;
        jumped = true;
        jumpChance = false;
    }

    IEnumerator InTheBamboo()
    {
        inBamboo = true;
        curBambooSpeed = bambooSpeed;

        for (float bb = 0f; bb <= 1f; bb += Time.deltaTime)
        {
            while (Time.timeScale != 1f)
                yield return null;

            curBambooSpeed = Mathf.Lerp(curBambooSpeed, 0f, bb * bb);
            yield return null;
        }

        inBamboo = false;
    }

    IEnumerator WallCheck()
    {
        startedWallCheck = true;
        yield return null;
        //    yield return new WaitForSeconds(0.1f);
        anim.ResetTrigger("falling");
        if (gliding)
        {
            gliding = false;
            anim.SetTrigger("exitGlide");
            //   Debug.Log("EXIT GLIDE BECAUSE CHECKING FOR WALL JUMP");
        }
        anim.SetTrigger("onWall");
        yield return null;
        //    yield return new WaitForSeconds(0.2f);
        HDRumbleMain.PlayVibrationPreset(pID, "K02_Patter2", 1f, 1, 0.15f);
        againstWall = true;
    }


    IEnumerator GlideTimer()
    {
        startedGlideTimer = true;
        gTimer = 0f;
        while (gTimer <= 20f)
        {
            if (gliding)
                gTimer += Time.deltaTime;
            yield return null;
        }
        glidedTooLong = true;
        gliding = false;
        //    Debug.Log("EXIT GLIDE BECAUSE TIMER RAN OUT");
        anim.SetTrigger("exitGlide");
    }

    IEnumerator ReverseCharacter(Vector3 direction)
    {
        if (!ps.GetComponentInChildren<TextTriggerMain>().currentlyDisplaying)
            ps.GetComponentInChildren<TextTriggerMain>().SetText(11);

        noDirectional = true;

        yield return null;

        rb.velocity = new Vector3(0f, rb.velocity.y, 0f);

        while (!onGround)
            yield return null;
        yield return null;

        disableControl = true;

        anim.SetBool("inLocomotion", true);
        anim.SetFloat("Speed", 20f);

        for (float rc = 0f; rc <= 2f; rc += (Time.deltaTime))
        {
            this.transform.forward = Vector3.Lerp(this.transform.forward, direction, rc / 2f);
            rb.velocity = this.transform.forward * groundSpeed;
            yield return null;
        }

        anim.SetBool("inLocomotion", false);
        anim.SetFloat("Speed", 0f);
        anim.SetFloat("turnAmount", 0f);
        yield return null;

        disableControl = false;
        noDirectional = false;
    }
}
