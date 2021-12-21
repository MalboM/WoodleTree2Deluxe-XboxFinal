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


    private void Start()
    {
        
    }

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


        if (!PlayerManager.GetMainPlayer().inButtonCutscene && other.gameObject.layer == 14 && ((buttonID == -1 && !activated) || (buttonID != -1 && PlayerPrefs.GetInt("Button" + buttonID.ToString(), 0) == 0))) 
        {
            Debug.Log("A");

			if (other.gameObject.GetComponent<AttackSettings>().activeAttack == true) 
            {
                Debug.Log("A-1");

                tpc = PlayerManager.GetMainPlayer();
                camF = tpc.cam.GetComponent<CameraFollower>();

                if(cameraPosition != null || objectanimated != null)
                {
                    Debug.Log("B");
                    StartCoroutine(PlayCutscene());
                }
                if (buttonID != -1)
                {
                    Debug.Log("C");
                    PlayerPrefs.SetInt("Button" + buttonID.ToString(), 1);

                }

                if (lCol == null)
                {
                    Debug.Log("D");
                    lCol = other.gameObject.GetComponent<LeafCollision>();
                }
                else if (lCol != null)
                {
                    Debug.Log("E");
                    lCol.buttonParticles.transform.SetParent(lCol.tpc.transform.parent);
                    lCol.buttonParticles.transform.position = other.ClosestPoint(this.transform.position);
                    lCol.buttonParticles.transform.LookAt(other.transform.position);
                    lCol.buttonEM.enabled = true;
                    lCol.buttonParticles.GetComponent<ParticleSystem>().Play();
                    StartCoroutine(lCol.tpc.DeactivateParticle(lCol.buttonParticles, lCol.buttonEM));
                //    lCol.tpc.Vibrate(0.4f, 1f, 0);
                    HDRumbleMain.PlayVibrationPreset(0, "D06_Thumpp4", 1f, 0, 0.2f);
                }

                Debug.Log("F");
                ActivateIt();
            }
        }
    }

	public void ActivateIt()
    {
        Debug.Log("F-1");

        activated = true;

        if (animator == null)
        {
            Debug.Log("F-2 : ANIMATOR WAS NULL");

            animator = GetComponent<Animator>();

            if(animator != null)
            {
                Debug.Log("F-3 : WE HAVE ANIMATOR");

                animator.SetBool("ButtonActivated", true);
            }
            else
            {
                Debug.Log("F-4 : WE DON'T HAVE THE ANIMATOR");

            }
        }
        else
        {
            Debug.Log("F-5 : WE ALREADY HAD THE ANIMATOR");
            animator.SetBool("ButtonActivated", true);
            //PERCHE'???
        }

        if (objectanimated != null)
        {
            Debug.Log("F-6 : OBJANIMATED WASN'T NULL");

            if (!objectanimated.transform.parent.gameObject.activeInHierarchy)
            {
                Debug.Log("F-7 : WE ACTIVATE OBJANIMATED");

                objectanimated.transform.parent.gameObject.SetActive(true);
            }

            if (objectanimated.GetComponentInParent<IActivablePrefab>() != null)
            {
                Debug.Log("F-8 : WE GOT THE COMPONENT IN PARENT");
                objectanimated.GetComponentInParent<IActivablePrefab>().enabled = false;
            }

            if (!objectanimated.gameObject.activeInHierarchy)
            {
                Debug.Log("F-9 : OBJANIMATED WASN'T ACTIVE IN THE SCENE");
                objectanimated.gameObject.SetActive(true);
            }

            Debug.Log("F-10 : WE SET THE BOOL TRUE ");
            objectanimated.SetBool(animationobject, true);
        }
        else
        {
            Debug.Log("F-11 : WE HAVE NO ANIMATED OBJECT");

        }
        // anim.SetBool(ButtonActivated, true);
    }

    IEnumerator PlayCutscene()
    {
        Debug.Log("B-1 : PLAY CUTSCENE");

        tpc.inButtonCutscene = true;

        yield return new WaitForSeconds(2.5f);

        GameObject.FindWithTag("Pause").transform.Find("Bars").GetComponent<Animator>().SetBool("barsToggle", true);

        tpc.disableControl = true;
        tpc.rb.isKinematic = true;
        tpc.inCutscene = true;

        if (tpc.onGround)
        {
            Debug.Log("B-2 : PLAYER'S ON GROUND");
            tpc.anim.Play("Idle", 0);
        }

        camF.disableControl = true;
        Vector3 origPos = camF.transform.position;
        Quaternion origRot = camF.transform.rotation;

        if (cameraPosition == null)
        {
            Debug.Log("B-3 : CAMERA POSITION IS NULL");

            camF.transform.position = objectanimated.transform.position - (objectanimated.transform.forward * 8f) + (Vector3.up * 6f);
            camF.transform.LookAt(objectanimated.transform);
        }
        else
        {
            Debug.Log("B-4 : CAMERA POSISTION ISN'T NULL");

            camF.transform.position = cameraPosition.transform.position;
            camF.transform.rotation = cameraPosition.transform.rotation;
        }

        if (buttonID == 82)
        {
            Debug.Log("B-5 : BUTTON ID IS 82");

            yield return new WaitForSeconds(1f);
        }
        else
        {
            Debug.Log("B-6 : BUTTON ID ISN'T 82");

            yield return new WaitForSeconds(2.5f);

            for (float f = 0f; f < 1f; f += (1f / 60f))
            {
                camF.transform.position = Vector3.Lerp(camF.transform.position, origPos, f);
                camF.transform.rotation = Quaternion.Lerp(camF.transform.rotation, origRot, f);
                yield return null;
            }

            Debug.Log("B-7 : FOR LOOP PASSED");

        }

        GameObject.FindWithTag("Pause").transform.Find("Bars").GetComponent<Animator>().SetBool("barsToggle", false);

        camF.transform.position = origPos;
        camF.transform.rotation = origRot;

        tpc.disableControl = false;
        tpc.rb.isKinematic = false;
        tpc.inCutscene = false;
        camF.disableControl = false;

        tpc.inButtonCutscene = false;

        if (objectanimated != null)
        {
            Debug.Log("B-8 : OBJANIMATED ISN'T NULL");

            objectanimated.GetComponentInParent<IActivablePrefab>().enabled = true;

        }

        Debug.Log("B-9 : END OF CUTSCENE");

    }
}
