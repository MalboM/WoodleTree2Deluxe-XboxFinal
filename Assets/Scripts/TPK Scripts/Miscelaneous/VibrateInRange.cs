using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VibrateInRange : MonoBehaviour {
    
    TPC tpc;
    bool entered;

    public float distanceToStart = 30f;
    public float intensityAtFurthest = 0.1f;
    public float intensityAtNearest = 0.35f;
    public AnimationCurve intensityCurve;

    public float collectIntensity = 0.6f;
    public float collectDuration = 0.2f;
    
    float curIntensity;
    float curDist;

	void Start () {
        entered = false;
        StartCoroutine("CheckDistance");
	}
	
    IEnumerator Vibrate()
    {
        curDist = Vector3.Distance(this.transform.position, tpc.transform.position);
        curIntensity = Mathf.Lerp(intensityAtNearest, intensityAtFurthest, intensityCurve.Evaluate(curDist / distanceToStart));

        tpc.Vibrate(curIntensity, Time.deltaTime, 1);
    //    HDRumblePlayer.PlayVibrationPreset("L01_RumbleLoop1", curIntensity);
    //    yield return null;

    //    yield return new WaitForSeconds(HDRumblePlayer.singleton.vibrationTimer * 60f);
        yield return null;
        StartCoroutine("Vibrate");
    }

    IEnumerator CheckDistance()
    {
        if (tpc == null)
            tpc = PlayerManager.GetMainPlayer();
        if(tpc != null)
        {
            if(Vector3.Distance(this.transform.position, tpc.transform.position) <= distanceToStart)
            {
                if (!entered)
                {
                    entered = true;
                    if(this.gameObject.activeInHierarchy)
                        StartCoroutine("Vibrate");
                }
            }
            else
            {
                if (entered)
                {
                    entered = false;
                    StopCoroutine("Vibrate");
                }
            }
        }

        for (float f = 0f; f < 1f; f += Time.deltaTime * Time.timeScale)
        {
            while (DataManager.isSuspended)
                yield return null;

            yield return null;
        }

        StartCoroutine("CheckDistance");
    }

    public void CollectedTear()
    {
        StopCoroutine("Vibrate");
        tpc.StopVibrate();
        StartCoroutine("DelayVib");
    }

    IEnumerator DelayVib()
    {
        yield return new WaitForSeconds(0.1f);
        HDRumbleMain.PlayVibrationPreset(0, "P04_DampedFm1", collectIntensity * 4f, 1, collectIntensity * 2f);
     //   tpc.Vibrate(collectIntensity * 2f, collectDuration * 2f, 1);
    }
}
