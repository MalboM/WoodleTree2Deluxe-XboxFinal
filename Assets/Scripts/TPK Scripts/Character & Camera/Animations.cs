using UnityEngine;
using System.Collections;

public class Animations : MonoBehaviour {

	//[HideInInspector]
    public Animator anim;
	private int iterator;
	private AnimatorStateInfo animSInfo;
    
	[HideInInspector] public bool inLeafSlide;
    [HideInInspector] public bool inLeafSlideEnter;
    [HideInInspector] public bool inLeafSliding;

    ////Box-Pushing Animations
    [HideInInspector] public bool inGrabPull;
	[HideInInspector] public bool inGrabPush;
	[HideInInspector] public bool inGrabStill;

	[HideInInspector] public bool inABoxPushingAnim;

////Airborne Animations
	[HideInInspector] public bool inFall;

////Jumping Animations
	[HideInInspector] public bool inJump;
    [HideInInspector] public bool inDoubleJump;
   
////Locomotion Animations
	[HideInInspector] public bool inIdle;
	[HideInInspector] public bool inRun;
	[HideInInspector] public bool inRunLeft;
	[HideInInspector] public bool inRunRight;
	[HideInInspector] public bool inSprint;
	[HideInInspector] public bool inSprintLeft;
	[HideInInspector] public bool inSprintRight;
	[HideInInspector] public bool inWalk;
	[HideInInspector] public bool inBrake;
	[HideInInspector] public bool inSliding;
    [HideInInspector] public bool inGlide;
    [HideInInspector] public bool inLand;
	[HideInInspector] public bool inRunLand;
    [HideInInspector] public bool onWall;

    [HideInInspector] public bool locomotionBlend;

////Leaf Animations
    [HideInInspector] public bool inLeafUp;
    [HideInInspector] public bool inLeafHold;
    [HideInInspector] public bool inLeafDown;
    [HideInInspector] public bool inQuickLeafDown;

    [HideInInspector] public bool inALeafAnim;

    ////Attack Animations
    [HideInInspector] public bool inSomeAttack;
    bool startedCoRot;
    [HideInInspector] public bool inAttack;
	[HideInInspector] public bool inAttack2;
	[HideInInspector] public bool inAttack3;
    [HideInInspector] public bool inAttackCharge;
    [HideInInspector] public bool inAttackRelease;

    [HideInInspector] public bool inAttackAnim;

////Misc Animations
    [HideInInspector] public bool inVictory;
    [HideInInspector] public bool inDefeated;
    [HideInInspector] public bool inHit;
    
	void Update () {

		if (anim != null && anim.gameObject.activeSelf && anim.gameObject.activeInHierarchy) {
			animSInfo = anim.GetCurrentAnimatorStateInfo (0);

			inLeafSlideEnter = animSInfo.IsName ("LeafSlideEnter") ? true : false;
            inLeafSliding = animSInfo.IsName("LeafSliding") ? true : false;

            inGrabPull = animSInfo.IsName ("boxPull") ? true : false;
			inGrabPush = animSInfo.IsName ("boxPush") ? true : false;
			inGrabStill = animSInfo.IsName ("BlockPushing") ? true : false;

			inFall = animSInfo.IsName ("Falling") ? true : false;

			inJump = (animSInfo.IsName("Jump") || animSInfo.IsName("Jump Spin")) ? true : false;
			inDoubleJump = animSInfo.IsName ("DoubleJump") ? true : false;
			inGlide = animSInfo.IsName ("Glide") ? true : false;

			inIdle = (animSInfo.IsName ("Idle") || animSInfo.IsName("Idle 2") || animSInfo.IsName("Idle 3") || animSInfo.IsName("Idle Special") || animSInfo.IsName("Idle Special 2")) ? true : false;
			locomotionBlend = animSInfo.IsName ("Locomotion") ? true : false;
			inBrake = animSInfo.IsName ("Brake") ? true : false;
			inSliding = animSInfo.IsName ("Sliding") ? true : false;
			inLand = animSInfo.IsName ("Land") ? true : false;
			inRunLand = animSInfo.IsName ("RunningLand") ? true : false;
            onWall = animSInfo.IsName("OnWall") ? true : false;

            inVictory = animSInfo.IsName ("Victory") ? true : false;
			inDefeated = animSInfo.IsName ("Defeated") ? true : false;
			inHit = animSInfo.IsName ("Hit") ? true : false;

			animSInfo = anim.GetCurrentAnimatorStateInfo (1);

			inLeafUp = animSInfo.IsName ("LeafUp") ? true : false;
			inLeafHold = animSInfo.IsName ("LeafHold") ? true : false;
			inLeafDown = animSInfo.IsName ("LeafDown") ? true : false;
			inQuickLeafDown = animSInfo.IsName ("QuickLeafDown") ? true : false;

			inAttack = animSInfo.IsName ("Attack") ? true : false;
			inAttack2 = animSInfo.IsName ("Attack2") ? true : false;
			inAttack3 = animSInfo.IsName ("Attack3") ? true : false;
			inAttackCharge = animSInfo.IsName ("AttackCharge") ? true : false;
			inAttackRelease = animSInfo.IsName ("AttackRelease") ? true : false;

            if (inAttack || inAttack2 || inAttack3 || inAttackRelease)
            {
                inSomeAttack = true;
                if (startedCoRot)
                {
                    StopCoroutine("FinishInSomeAttack");
                    startedCoRot = false;
                }
            }
            else
            {
                if (inSomeAttack && !startedCoRot)
                {
                    startedCoRot = true;
                    StartCoroutine("FinishInSomeAttack");
                }
            }
		}

        if ((inGrabStill || inGrabPull || inGrabPush))
            inABoxPushingAnim = true;
        else
            inABoxPushingAnim = false;

        if ((inLeafUp || inLeafHold || inLeafDown || inQuickLeafDown))
            inALeafAnim = true;
        else
            inALeafAnim = false;

		if ((inAttack || inAttack2 || inAttack3 || inAttackCharge || inAttackRelease))
            inAttackAnim = true;
        else
            inAttackAnim = false;

        if (inLeafSlideEnter || inLeafSliding)
            inLeafSlide = true;
        else
            inLeafSlide = false;
    }

    IEnumerator FinishInSomeAttack()
    {
        yield return new WaitForSeconds(0.5f);
        inSomeAttack = false;
        startedCoRot = false;
    }
}
