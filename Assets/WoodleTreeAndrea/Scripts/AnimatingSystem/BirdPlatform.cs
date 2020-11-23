using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdPlatform : MonoBehaviour
{
    [System.Serializable]
    public class BirdPosition { public GameObject bird;[Range(0f, 1f)] public float startingOffset; public bool invertMovement; public float speed; public GameObject leftWing; public GameObject rightWing;[HideInInspector] public Vector3 startPos; }
    public BirdPosition[] birdPositions;
    float[] positioner;
    float[] sign;

    float[] rotationer;
    float[] curRotations;
    float[] rSign;

    public float overallDistance;

    bool charaInDistance;
    GameObject chara;

    private void OnEnable()
    {
        if (positioner == null)
        {
            positioner = new float[birdPositions.Length];
            sign = new float[positioner.Length];
            rotationer = new float[positioner.Length];
            curRotations = new float[positioner.Length];
            rSign = new float[positioner.Length];

            for (int a = 0; a < birdPositions.Length; a++)
            {
                if (birdPositions[a] != null && birdPositions[a].bird != null)
                {
                    birdPositions[a].startPos = birdPositions[a].bird.transform.position;
                    sign[a] = 1f;
                    if (birdPositions[a].invertMovement)
                    {
                        sign[a] = -1f;
                        birdPositions[a].startingOffset *= -1f;
                    }
                    positioner[a] = birdPositions[a].startingOffset * overallDistance;

                    rotationer[a] = 0f;
                    curRotations[a] = 0f;
                    rSign[a] = 1f;
                }
            }
        }

        InvokeRepeating("CheckDist", 1f, 2f);
    }

    private void OnDisable()
    {
        CancelInvoke("CheckDist");
    }

    private void LateUpdate()
    {
        if (charaInDistance)
        {
            for (int b = 0; b < birdPositions.Length; b++)
            {
                if (birdPositions[b] != null && birdPositions[b].bird != null)
                {
                    positioner[b] += Time.deltaTime * birdPositions[b].speed * sign[b];

                    if (!birdPositions[b].invertMovement)
                    {
                        if ((sign[b] > 0f && positioner[b] > overallDistance) || (sign[b] < 0f && positioner[b] < 0f))
                            sign[b] *= -1f;
                    }
                    else
                    {
                        if ((sign[b] > 0f && positioner[b] > 0f) || (sign[b] < 0f && positioner[b] < -overallDistance))
                            sign[b] *= -1f;
                    }
                    //    if (positioner[b] < 0f)
                    //        sign[b] = 1f;

                    rotationer[b] += Time.deltaTime * 300f * Mathf.Sign(rSign[b]);
                    if (rotationer[b] > 60f)
                        rSign[b] = -1f;
                    if (rotationer[b] < -40f)
                        rSign[b] = 1f;

                    curRotations[b] = Mathf.Lerp(curRotations[b], rotationer[b], Time.deltaTime * 2f);
                    birdPositions[b].rightWing.transform.localEulerAngles = new Vector3(0f, 0f, curRotations[b]);
                    birdPositions[b].leftWing.transform.localEulerAngles = new Vector3(0f, 0f, curRotations[b] * -1f);

                    Vector3 curPos = birdPositions[b].bird.transform.position;
                    Vector3 endPos = birdPositions[b].startPos + (Vector3.up * positioner[b]) + (Vector3.up * (curRotations[b] / 100f));

                    birdPositions[b].bird.transform.position = Vector3.Lerp(curPos, endPos, Time.deltaTime);
                }
            }
        }
    }

    void CheckDist()
    {
        if (chara == null)
        {
            if (PlayerManager.GetMainPlayer() != null)
                chara = PlayerManager.GetMainPlayer().gameObject;
        }
        else
        {
            if (Vector3.Distance(birdPositions[0].bird.transform.position, chara.transform.position) < 200f)
                charaInDistance = true;
            else
                charaInDistance = false;
        }
    }
}
