using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using UnityEngine.UI;

public class Telescope : MonoBehaviour {

	public Camera cam;
	public GameObject camPos;
	public GameObject woodleMesh;
	public GameObject lenseImage;

	public RectTransform controlsParent;

    public AudioSource audioSource;
    public AudioClip openSound;
    public AudioClip closeSound;

    public PauseScreen ps;

	Player input;
	TPC tpc;
	CameraFollower camF;

	[HideInInspector] public bool usingTelescope;
    [HideInInspector] public bool transitioning;

	float origFOV;
	Vector3 startPos;
	float yRot;
	float xRot;
	float tempX;
	float fovlerper;
	Quaternion prevRot;
    bool justEntered;

	void Start () {
		input = ReInput.players.GetPlayer (0);
		tpc = this.gameObject.GetComponent<TPC> ();
		camF = cam.gameObject.GetComponent<CameraFollower> ();
		fovlerper = 50f;
	}

	void Update(){
		if ((input.GetButtonDown ("Telescope") || (usingTelescope && input.GetButtonDown("Attack"))) && !transitioning && tpc.onGround && !ps.inPause && !tpc.challengeWarping && !camF.stationaryMode1 && !camF.stationaryMode2) {
			usingTelescope = !usingTelescope;
			transitioning = true;
			StartCoroutine ("Transition");
        }
	}

    public void ExitTelescope()
    {
        StopCoroutine("Transition");
        usingTelescope = false;
        transitioning = true;
        StartCoroutine("Transition");
    }

	void LateUpdate () {
		if (!transitioning) {
			if (usingTelescope) {
				yRot = input.GetAxis ("RH");
				xRot = input.GetAxis ("RV");

				cam.transform.RotateAround (cam.transform.position, Vector3.up, yRot * 0.8f);

                if (xRot < -0.1f) {
					if(cam.transform.localEulerAngles.x + xRot > 270f || cam.transform.localEulerAngles.x - xRot < 88f)
						cam.transform.RotateAround (cam.transform.position, cam.transform.right, xRot * -0.8f);
				}
				if (xRot > 0.1f) {
					if(cam.transform.localEulerAngles.x + xRot > 272f || cam.transform.localEulerAngles.x + xRot < 90f)
						cam.transform.RotateAround (cam.transform.position, cam.transform.right, xRot * -0.8f);
				}
                if (justEntered) {
                    if (cam.transform.localEulerAngles.x < 90f && xRot >= -0.1f)
                        cam.transform.RotateAround(cam.transform.position, cam.transform.right, Mathf.Lerp(0f, -0.8f, cam.transform.localEulerAngles.x / 90f));
                    else
                        justEntered = false;
                }
                //    if (cam.transform.localEulerAngles.x >= 0f && cam.transform.localEulerAngles.x <= 90f)
                //        cam.transform.position = Vector3.Lerp(camPos.transform.position, camPos.transform.position + (Vector3.up*5f), cam.transform.localEulerAngles.x/90f);

                if (cam.transform.localEulerAngles.z != 0f)
                    cam.transform.localEulerAngles = new Vector3(cam.transform.localEulerAngles.x, cam.transform.localEulerAngles.y, 0f);

				fovlerper = Mathf.Clamp (fovlerper - input.GetAxis ("LV"), 10f, 50f);

				cam.fieldOfView = fovlerper;
			}
		}
	}

	IEnumerator Transition()
    {
        if (usingTelescope)
        {
            origFOV = cam.fieldOfView;
            //                tpc.rb.isKinematic = true;

            if (tpc.isHoldingLeaf)
            {
                tpc.cancelLeafHold = true;
                yield return null;
                yield return null;
            }
            tpc.rb.velocity = Vector3.zero;
            tpc.anim.Play("Idle");
            tpc.disableControl = true;
            camF.disableControl = true;
            startPos = cam.transform.position;
            cam.transform.SetParent(camPos.transform);
            prevRot = cam.transform.localRotation;
        }
        float iterator = 0f;
		if (usingTelescope) {
            justEntered = true;
            audioSource.Stop();
            audioSource.clip = openSound;
            audioSource.PlayDelayed(0f);
            for (int t = 1; t <= 60; t++) {
				iterator = (t * 1f) / 60f;
				cam.fieldOfView = Mathf.Lerp (origFOV, fovlerper, iterator);
            //    cam.transform.position = Vector3.Lerp(startPos, Vector3.Lerp(camPos.transform.position, camPos.transform.position + (Vector3.up * 5f), cam.transform.localEulerAngles.x / 90f), iterator);
                cam.transform.position = Vector3.Lerp (startPos, camPos.transform.position, iterator);
			//	cam.transform.forward = Vector3.Lerp (cam.transform.forward, this.transform.forward, iterator);
				lenseImage.transform.localScale = Vector3.Lerp (Vector3.one*3f, Vector3.one, iterator);
				controlsParent.localPosition = Vector3.Lerp (Vector3.zero, new Vector3 (0f, 450f, 0f), iterator * iterator);
				yield return null;
			}
            if(woodleMesh != null) 
    			woodleMesh.gameObject.SetActive (false);
		} else{
            justEntered = false;
            audioSource.Stop();
            audioSource.clip = closeSound;
            audioSource.PlayDelayed(0f);
            cam.transform.SetParent (null);
			woodleMesh.gameObject.SetActive (true);
			Quaternion startRot = cam.transform.localRotation;
			Vector3 origPos = this.transform.position;
			camF.disableControl = false;
			for (int t = 59; t >= 0; t--) {
				iterator = (t * 1f) / 60f;
				cam.fieldOfView = Mathf.Lerp (origFOV, fovlerper, iterator);
				lenseImage.transform.localScale = Vector3.Lerp (Vector3.one*3f, Vector3.one, iterator);
		//		cam.transform.position = Vector3.Lerp (startPos, origPos, (1f * t) / 60f);
				//		cam.transform.localRotation = Quaternion.Lerp(prevRot, startRot, (t * 1f) / 60f);
				controlsParent.localPosition = Vector3.Lerp (Vector3.zero, new Vector3 (0f, 450f, 0f), iterator * iterator);
				yield return null;
			}
			tpc.disableControl = false;
            tpc.rb.isKinematic = false;
            fovlerper = 50f;
        }
		transitioning = false;
	}
}
