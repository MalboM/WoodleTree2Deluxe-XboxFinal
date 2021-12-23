using UnityEngine;
using System.Collections;
using Rewired;

public class LeafCollision : MonoBehaviour {

    [HideInInspector] public GameObject boneToFollow;
    [HideInInspector] public TPC tpc;
    [HideInInspector] public int waterLayer;
	[HideInInspector] public int enemyLayer;
	[HideInInspector] public LayerMask collisionLayers;
    [HideInInspector] public GameObject waterDrop;
    [HideInInspector] public Animations curAnim;

    [HideInInspector] public bool hasWaterDrop;

    [HideInInspector] public GameObject buttonParticles;
    [HideInInspector] public ParticleSystem.EmissionModule buttonEM;

    int playerI;
	RaycastHit checkCol;

	LeafBoxObstacle lbo;
    AudioSource sound;

    void Start() {
		playerI = int.Parse (tpc.playerID);
        if (playerI == 0)
        {
            this.transform.localScale = Vector3.one * 1.5f;
            waterDrop.gameObject.SetActive(false);
        }
        if(playerI == 1)
            this.transform.localScale = Vector3.one * 2f;

        hasWaterDrop = false;
		checkCol = new RaycastHit ();
        sound = this.gameObject.GetComponent<AudioSource>();
        sound.Stop();
        sound.loop = false;
        sound.spatialBlend = 0f;
        sound.dopplerLevel = 0f;
        sound.rolloffMode = AudioRolloffMode.Linear;
        sound.minDistance = 2f;
        sound.maxDistance = 30f;

        buttonEM = buttonParticles.GetComponent<ParticleSystem>().emission;
        buttonEM.enabled = false;
    }

    // Update is called once per frame
    void Update () {
        this.transform.position = boneToFollow.transform.position;
        this.transform.rotation = boneToFollow.transform.rotation;
    //    this.transform.localScale = boneToFollow.transform.lossyScale;
	}

	void OnTriggerEnter(Collider other){
		if ((curAnim.inAttack || curAnim.inAttack2 || curAnim.inAttack3) ) {
            if(playerI == 0) { 
			    if (((collisionLayers & 1 << other.gameObject.layer) != 0)) {
                    if (other.gameObject.GetComponent<ActivateButton>() == null && (other.gameObject.name != "LeafBox"))
                    {
                        if (Physics.Raycast(this.transform.position, tpc.transform.forward, out checkCol, 3f, collisionLayers) && !checkCol.collider.isTrigger && checkCol.normal.y <= 0.1f)
                            BounceChara(-checkCol.normal, true);
                    }
			    }

                if (other.gameObject.name == "LeafBox")
                {
                    lbo = other.gameObject.GetComponentInParent<LeafBoxObstacle>();
                    if (tpc.leafNo >= lbo.boxType)
                    {
                        tpc.PlayHitFreeze(null, false, false, false);
                        HDRumbleMain.PlayVibrationPreset(playerI, "D06_Thumpp4", 1f, 0, 0.2f);
                        lbo.DestroyBox();
                    }
                    else
                    {
                        if (lbo.boxType == 5)
                        {
                            if (PlayerPrefs.GetInt("WrongLeafBlockBlack", 0) == 0)
                            {
                                PlayerPrefs.SetInt("WrongLeafBlockBlack", 1);
                                tpc.ps.gameObject.GetComponentInChildren<TextTriggerMain>().SetText(8);
                            }
                        }
                        else
                        {
                            if (PlayerPrefs.GetInt("WrongLeafBlock", 0) == 0)
                            {
                                PlayerPrefs.SetInt("WrongLeafBlock", 1);
                                tpc.ps.gameObject.GetComponentInChildren<TextTriggerMain>().SetText(7);
                            }
                        }
                        lbo.Wrong();
                        LeafStarKnockback(other.transform.position);
                    }
                }
                if(other.gameObject.GetComponent<NPCCage>())
                    LeafStarKnockback(other.transform.position);
            }
            if (other.tag == "LeafHit" || other.tag == "TouchActivate")
            {
                if (other.gameObject.GetComponentInChildren<Animator>())
                {
                    other.gameObject.GetComponentInChildren<Animator>().SetBool("Activate", true);
                    if(other.tag == "LeafHit")
                        BerrySpawnManager.SpawnABerry(other.transform.position);

                    tpc.PlayHitFreeze(null, false, false, false);
                }
            }
        }
    }

    void OnTriggerStay(Collider other){
		if (playerI == 0 && curAnim.inLeafHold && !hasWaterDrop && tpc.isHoldingLeaf && tpc.onGround && tpc.health > 1) {
			if (other.gameObject.layer == waterLayer) {
                tpc.currentWaterfall = other;
				GetWater ();
			}
		} 
    }

    void LeafStarKnockback(Vector3 pos)
    {
        tpc.PlayBlockStar();
        BounceChara((pos - this.transform.position).normalized, false);
    }

    void GetWater() {
        hasWaterDrop = true;
        tpc.CollectedWater();
        waterDrop.gameObject.SetActive(true);
    }

    public void DropWater() {
        hasWaterDrop = false;
		if (playerI == 0)
            waterDrop.gameObject.SetActive(false);
    }

	void BounceChara(Vector3 direct, bool usualVib){
		direct = new Vector3 (direct.x, 0f, direct.z);
		direct.Normalize ();
		tpc.HitPushBack (-direct);
        if (usualVib)
        {
            //   tpc. Vibrate(0.3f, 1f, 0);
            HDRumbleMain.PlayVibrationPreset(playerI, "B06_Bump1", 1f, 0, 0.2f);
        }
        else
            HDRumbleMain.PlayVibrationPreset(playerI, "D03_Thumpp1", 1f, 0, 0.2f);
        sound.Stop();
        sound.PlayDelayed(0f);
	}
}
