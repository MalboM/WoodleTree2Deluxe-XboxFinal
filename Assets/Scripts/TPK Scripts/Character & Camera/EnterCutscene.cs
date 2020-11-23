using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnterCutscene : MonoBehaviour {

	[SerializeField] private GameObject cutsceneObject;
    [SerializeField] private GameObject objectToFollow;
    [SerializeField] private bool deactivateAfterCutscene;

    private GameObject camera;
    private CameraFollower camF;
    private Animation anim;
    private string[] animations = new string[2];
    private bool inThisCutscene;
    private bool thisIsDisabled;
    private float lerpCounter;

    private Vector3 beginPos;
    private Vector3 startPos;
    private Vector3 startForward;
    private Vector3 beginForward;
    private Vector3 exitPos;
    private bool exiting;

    void Start() {
        camera = GameObject.FindWithTag("MainCamera");
        camF = camera.GetComponent<CameraFollower>();
        if (cutsceneObject)
            anim = cutsceneObject.GetComponent<Animation>();
        if (anim)
        {
            int a = 0;
            foreach (AnimationState state in anim)
            {
                animations[a] = state.name;
                a++;
            }
        }
        inThisCutscene = false;
        lerpCounter = 0f;
        exiting = false;
    }

    void LateUpdate() {
        if (inThisCutscene) {
            camera.transform.localPosition = Vector3.Lerp(startPos, exitPos, lerpCounter);
            camera.transform.forward = Vector3.Lerp(camera.transform.forward, startForward, lerpCounter);
            lerpCounter += Time.deltaTime;
            if (lerpCounter >= 1f){
                if (!exiting) {
                    camera.transform.localPosition = Vector3.zero;
                    if(anim != null)
                        anim.Play(animations[0]);
                    StartCoroutine("CutsceneEnd");
                    if (camF != null)
                        camF.transitionBackFromCutscene = false;
                    this.inThisCutscene = false;
                }else {
                    if (camF != null)
                    {
                        camF.transitionBackFromCutscene = false;
                        camF.cutsceneMode = false;
                        camF.behindMode = true;
                    }
                    if (deactivateAfterCutscene)
                        thisIsDisabled = true;
                    this.inThisCutscene = false;
                }
            }
        }
    }

    void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Player" && other.gameObject.name == "Woodle Character" && !camF.cutsceneMode && !camF.transitionBackFromCutscene && !thisIsDisabled) {
            camF.cutsceneMode = true;
            camF.behindMode = false;
            if(objectToFollow)
                camera.transform.SetParent(objectToFollow.transform);
            this.inThisCutscene = true;
            lerpCounter = 0f;
            beginPos = camera.transform.position;
            startPos = camera.transform.localPosition;
            exitPos = Vector3.zero;
            beginForward = camera.transform.forward;
            startForward = this.transform.forward;
            exiting = false;
            camF.transitionBackFromCutscene = true;
        }
    }

    IEnumerator CutsceneEnd() {
        yield return new WaitForSeconds(anim.GetClip(animations[0]).length);
        anim.Stop(animations[0]);
        camera.transform.SetParent(null);
        exitPos = beginPos;
        startPos = camera.transform.position;
        startForward = beginForward;
        lerpCounter = 0f;
        exiting = true;
        this.inThisCutscene = true;
        camF.transitionBackFromCutscene = true;
    }
}
