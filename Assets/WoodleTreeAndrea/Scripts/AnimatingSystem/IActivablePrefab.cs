using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DynamicShadowProjector.Sample;
using UnityEngine.AI;
using ICode;

public class IActivablePrefab : MonoBehaviour, IActivable
{

    public bool testetstst;

    public float timeToWait; //Leave as zero to default to 3 seconds
    public float fullObjDistance;   //Leave as zero to default to 30
    public float lowObjDistance;    //Leave as zero to default to 60
    GameObject woodle;
    float curDistance;
    float fullDistToUse;
    float lowDistToUse;

    bool useChunkManager = false;
    public GameObject fullObj;
    public GameObject lowObj;
    public GameObject fullObjCharacter;
    public GameObject shadowObj;

    bool checkedRenderers;
    Renderer[] fullObjRenderers;
    SkinnedMeshRenderer[] fullObjSMR;
    Renderer[] lowObjRenderers;
    SkinnedMeshRenderer[] lowObjSMR;
    bool fullRenderersOn;
    bool lowRenderersOn;
    [HideInInspector] public bool insideChunk = true;


    public Animation animationComponent;

    public float distanceMultiplier = 1.3f;

    //public Component[] components;

    void OnEnable()
    {
        if (!useChunkManager)
        {
            if (timeToWait == 0f)
                timeToWait = 2.5f;
            if (fullObjDistance == 0f)
                fullObjDistance = 30f;
            if (lowObjDistance == 0f)
                lowObjDistance = fullObjDistance + 10f;

            fullDistToUse = fullObjDistance * fullObjDistance;
            lowDistToUse = lowObjDistance * lowObjDistance;

            /*
            if (fullObj != null && fullObj.GetComponentsInChildren<Animator>() != null)
            {
                foreach (Animator a in fullObj.GetComponentsInChildren<Animator>())
                    a.keepAnimatorControllerStateOnDisable = true;
            }
            */

            /*
            if (!checkedRenderers)
            {
                checkedRenderers = true;
                if (fullObj)
                {
                    fullObj.SetActive(true);
                    if (fullObjRenderers == null && fullObj.gameObject.GetComponentsInChildren<Renderer>(true) != null)
                        fullObjRenderers = fullObj.gameObject.GetComponentsInChildren<Renderer>(true);
                    if (fullObjSMR == null && fullObj.gameObject.GetComponentsInChildren<SkinnedMeshRenderer>(true) != null)
                        fullObjSMR = fullObj.gameObject.GetComponentsInChildren<SkinnedMeshRenderer>(true);
                }
                if (lowObj)
                {
                    lowObj.SetActive(true);
                    if (lowObjRenderers == null && lowObj.gameObject.GetComponentsInChildren<Renderer>(true) != null)
                        lowObjRenderers = lowObj.gameObject.GetComponentsInChildren<Renderer>(true);
                    if (lowObjSMR == null && lowObj.gameObject.GetComponentsInChildren<SkinnedMeshRenderer>(true) != null)
                        lowObjSMR = lowObj.gameObject.GetComponentsInChildren<SkinnedMeshRenderer>(true);
                }
            }
            fullRenderersOn = false;
            lowRenderersOn = false;
            */

            InvokeRepeating("CheckDistance", 0.1f, timeToWait);
        }
    }

    void OnDisable()
    {
        CancelRepeat();
    }

    void IActivable.Activate()
    {
        if (useChunkManager)
        {
            fullObj.SetActive(true);
            if (shadowObj != null)
                shadowObj.SetActive(true);
            lowObj.SetActive(false);
        }
    }

    void IActivable.Deactivate()
    {
        if (useChunkManager)
        {
            lowObj.transform.position = fullObj.transform.position;
            if (fullObjCharacter != null)
                lowObj.transform.rotation = fullObjCharacter.transform.rotation;
            fullObj.SetActive(false);
            if (shadowObj != null)
                shadowObj.SetActive(false);
            lowObj.SetActive(true);
        }
    }

    public void CancelRepeat()
    {
        CancelInvoke("CheckDistance");
        if (shadowObj != null)
            shadowObj.SetActive(false);
        if (fullObj != null)
            fullObj.SetActive(false);
        if (lowObj != null)
            lowObj.SetActive(false);
    }

    public void FullRenderersToggle(bool enabled)
    {
        fullRenderersOn = enabled;
        foreach (Renderer r in fullObjRenderers)
            r.enabled = enabled;
        foreach (SkinnedMeshRenderer smr in fullObjSMR)
            smr.enabled = enabled;
    }

    public void LowRenderersToggle(bool enabled)
    {
        lowRenderersOn = enabled;
        foreach (Renderer r in lowObjRenderers)
            r.enabled = enabled;
        foreach (SkinnedMeshRenderer smr in lowObjSMR)
            smr.enabled = enabled;
    }

    void CheckDistanceX()
    {
        //    if (!this.gameObject.activeInHierarchy || !this.enabled)
        //        CancelRepeat();

        if (woodle == null)
            woodle = PlayerManager.GetMainPlayer().gameObject;

        if (insideChunk && fullObj != null)
        {
            curDistance = (woodle.transform.position - fullObj.transform.position).sqrMagnitude;
            if (curDistance <= (fullDistToUse * distanceMultiplier))
            {
                if (!fullRenderersOn)
                    FullRenderersToggle(true);
                if (lowRenderersOn)
                    LowRenderersToggle(false);
                if (animationComponent != null && !animationComponent.enabled)
                    animationComponent.enabled = true;
            }
            else
            {
                if (curDistance > (fullDistToUse * distanceMultiplier) && curDistance <= (lowDistToUse * distanceMultiplier))
                {
                    if (!lowRenderersOn)
                        LowRenderersToggle(true);
                    if (fullRenderersOn)
                        FullRenderersToggle(false);
                    if (animationComponent != null && animationComponent.enabled)
                        animationComponent.enabled = false;
                }
                else
                {
                    if (lowRenderersOn)
                        LowRenderersToggle(false);
                    if (fullRenderersOn)
                        FullRenderersToggle(false);
                    if (animationComponent != null && animationComponent.enabled)
                        animationComponent.enabled = false;
                }
            }
        }
    }


    void CheckDistance()
    {
        if (!this.gameObject.activeInHierarchy || !this.enabled)
            CancelRepeat();

        if (woodle == null)
            woodle = PlayerManager.GetMainPlayer().gameObject;
        if (fullObj != null)
        {
            curDistance = (woodle.transform.position - fullObj.transform.position).sqrMagnitude;
            distanceMultiplier = Mathf.Lerp(1f, 2f, (float)PlayerPrefs.GetInt("ObjectDetails",2)/8f);
            if (curDistance <= (fullDistToUse * distanceMultiplier))
            {
                if ((lowObj != null && lowObj.activeInHierarchy) || (!fullObj.activeInHierarchy))
                {
                    fullObj.SetActive(true);
                    /*    if (fullObj.GetComponentInChildren<Animation>())
                        {
                            fullObj.GetComponentInChildren<Animation>().enabled = true;
                        }*/
                    if (animationComponent != null)
                        animationComponent.enabled = true;
                    if (shadowObj != null)
                        shadowObj.SetActive(true);
                    if (lowObj != null)
                        lowObj.SetActive(false);
                }
            }
            else
            {
                if (curDistance > (fullDistToUse * distanceMultiplier) && curDistance <= (lowDistToUse * distanceMultiplier))
                {

                    if ((lowObj != null && !lowObj.activeInHierarchy) || (fullObj.activeInHierarchy))
                    {
                        //    fullObj.SetActive(false);
                        //    lowObj.SetActive(true);

                        if (lowObj)
                        {
                            if (fullObjCharacter != null)
                            {
                                lowObj.transform.position = fullObjCharacter.transform.position;
                                lowObj.transform.rotation = fullObjCharacter.transform.rotation;
                            }
                            else
                            {
                                lowObj.transform.position = fullObj.transform.position;
                                lowObj.transform.rotation = fullObj.transform.rotation;
                            }
                        }
                        /*  if (fullObj.GetComponentInChildren<Animation>())
                          {
                              fullObj.GetComponentInChildren<Animation>().enabled = false;
                          }*/
                        if (animationComponent != null)
                            animationComponent.enabled = false;

                        fullObj.SetActive(false);
                        if (shadowObj != null)
                            shadowObj.SetActive(false);
                        if (lowObj != null)
                            lowObj.SetActive(true);
                    }
                }
                else
                {

                    if ((fullObj.activeInHierarchy) || (lowObj != null && lowObj.activeInHierarchy))
                    {
                        fullObj.SetActive(false);
                        /*    if (fullObj.GetComponentInChildren<Animation>())
                            {
                                fullObj.GetComponentInChildren<Animation>().enabled = false;
                            }*/
                        if (animationComponent != null)
                            animationComponent.enabled = false;
                        if (shadowObj != null)
                            shadowObj.SetActive(false);
                        if (lowObj != null)
                            lowObj.SetActive(false);
                    }
                }
            }
        }
    }
}