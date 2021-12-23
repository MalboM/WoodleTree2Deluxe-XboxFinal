using UnityEngine;
using System.Collections;

public class ActivateButton : MonoBehaviour
{
    public string animationobject;
    Animator animator;
    TPC tpc;
    CameraFollower camF;
    public Animator objectanimated;
    public GameObject cameraPosition;

	public int buttonID;	//123 existing
    LeafCollision lCol;
    public bool activated;

    void OnEnable() {
        
        if(animator == null)
            animator = GetComponent<Animator>();

    //    if (buttonID == 22 || buttonID == 30)
     //       PlayerPrefs.SetInt("Button" + buttonID.ToString(), 0);
        
        if ((buttonID == -1 && activated) || (buttonID != -1 && PlayerPrefs.GetInt("Button" + buttonID.ToString(), 0) == 1))
        {
        //    objectanimated.Play("New State", 0);
            ActivateIt();
        }
        if (activated && objectanimated.GetBool("Activated") == false)
            objectanimated.SetBool("Activated", true);
    }

	void Update(){
		if (buttonID != -1 && PlayerPrefs.GetInt ("Button" + buttonID.ToString (), 0) == 0 && animator.GetBool ("ButtonActivated") == true)
        {
            animator.SetBool("ButtonActivated", false);
			objectanimated.SetBool(animationobject, false);
			animator.Play ("New State");
			objectanimated.Play ("New State");

            activated = false;
        }
	}

    void OnTriggerEnter(Collider other) {
        if (!PlayerManager.GetMainPlayer().inButtonCutscene && other.gameObject.layer == 14 && ((buttonID == -1 && !activated) || (buttonID != -1 && PlayerPrefs.GetInt("Button" + buttonID.ToString(), 0) == 0))) {
			if (other.gameObject.GetComponent<AttackSettings>().activeAttack == true) {
                tpc = PlayerManager.GetMainPlayer();
                tpc.PlayHitFreeze(null, false, false, false);
                camF = tpc.cam.GetComponent<CameraFollower>();
                if(cameraPosition != null || objectanimated != null)
                    StartCoroutine("PlayCutscene");
                if(buttonID != -1)
    				PlayerPrefs.SetInt ("Button"+buttonID.ToString (), 1);
            //    PlayerPrefs.Save();
                if(lCol == null)
                    lCol = other.gameObject.GetComponent<LeafCollision>();
                if (lCol != null)
                {
                    lCol.buttonParticles.transform.SetParent(lCol.tpc.transform.parent);
                    lCol.buttonParticles.transform.position = other.ClosestPoint(this.transform.position);
                    lCol.buttonParticles.transform.LookAt(other.transform.position);
                    lCol.buttonEM.enabled = true;
                    lCol.buttonParticles.GetComponent<ParticleSystem>().Play();
                    StartCoroutine(lCol.tpc.DeactivateParticle(lCol.buttonParticles, lCol.buttonEM));
                    //    lCol.tpc.Vibrate(0.4f, 1f, 0);
                    HDRumbleMain.PlayVibrationPreset(0, "D06_Thumpp4", 1f, 0, 0.2f);
                }
                ActivateIt ();
            }
        }
    }

	public void ActivateIt(){
        activated = true;

        if (animator == null)
            animator = GetComponent<Animator>();

        animator.SetBool("ButtonActivated", true);
        if (objectanimated != null)
        {
            if (!objectanimated.transform.parent.gameObject.activeInHierarchy)
                objectanimated.transform.parent.gameObject.SetActive(true);
            if(objectanimated.GetComponentInParent<IActivablePrefab>() != null)
                objectanimated.GetComponentInParent<IActivablePrefab>().enabled = false;

            if (!objectanimated.gameObject.activeInHierarchy)
                objectanimated.gameObject.SetActive(true);
            
            objectanimated.SetBool(animationobject, true);
        }
		// anim.SetBool(ButtonActivated, true);
	}

    IEnumerator PlayCutscene()
    {
        tpc.inButtonCutscene = true;

        yield return new WaitForSeconds(2.5f);

        GameObject.FindWithTag("Pause").transform.Find("Bars").GetComponent<Animator>().SetBool("barsToggle", true);

        tpc.disableControl = true;
        tpc.rb.isKinematic = true;
        tpc.inCutscene = true;

        if (tpc.onGround)
            tpc.anim.Play("Idle", 0);

        camF.disableControl = true;
        Vector3 origPos = camF.transform.position - tpc.transform.position;
        Quaternion origRot = camF.transform.rotation;

        if (cameraPosition == null)
        {
            camF.transform.position = objectanimated.transform.position - (objectanimated.transform.forward * 8f) + (Vector3.up * 6f);
            camF.transform.LookAt(objectanimated.transform);
        }
        else
        {
            camF.transform.position = cameraPosition.transform.position;
            camF.transform.rotation = cameraPosition.transform.rotation;
        }

        if (buttonID == 82)
            yield return new WaitForSeconds(1f);
        else
        {
            yield return new WaitForSeconds(2.5f);

            for (float f = 0f; f < 1f; f += (1f / 60f))
            {
                camF.transform.position = Vector3.Lerp(camF.transform.position, tpc.transform.position + origPos, f);
                camF.transform.rotation = Quaternion.Lerp(camF.transform.rotation, origRot, f);
                yield return null;
            }
        }

        GameObject.FindWithTag("Pause").transform.Find("Bars").GetComponent<Animator>().SetBool("barsToggle", false);

        camF.transform.position = tpc.transform.position + origPos;
        camF.transform.rotation = origRot;

        tpc.disableControl = false;
        tpc.rb.isKinematic = false;
        tpc.inCutscene = false;
        camF.disableControl = false;

        tpc.inButtonCutscene = false;

        if (objectanimated != null)
            objectanimated.GetComponentInParent<IActivablePrefab>().enabled = true;
    }
}
