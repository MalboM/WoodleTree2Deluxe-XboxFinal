using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BerryPFX : MonoBehaviour
{
    public GameObject[] redPFX;
    private ParticleSystem.EmissionModule[] redPFXEMs;

    public GameObject[] bluePFX;
    private ParticleSystem.EmissionModule[] blueEMs;

    public GameObject[] purplePFX;
    private ParticleSystem.EmissionModule[] purpleEMs;

    public GameObject[] enemyKilledPFX;
    private ParticleSystem.EmissionModule[] enemyKilledEMs;

    GameObject[] objToFollow;

    void Start()
    {
        redPFXEMs = new ParticleSystem.EmissionModule[10];

        for (int x = 0; x < 10; x++)
        {
            redPFXEMs[x] = redPFX[x].GetComponent<ParticleSystem>().emission;
            redPFXEMs[x].enabled = false;
        }

        blueEMs = new ParticleSystem.EmissionModule[6];

        for (int x = 0; x < 6; x++)
        {
            blueEMs[x] = bluePFX[x].GetComponent<ParticleSystem>().emission;
            blueEMs[x].enabled = false;
        }

        purpleEMs = new ParticleSystem.EmissionModule[10];

        for (int x = 0; x < 10; x++)
        {
            purpleEMs[x] = purplePFX[x].GetComponent<ParticleSystem>().emission;
            purpleEMs[x].enabled = false;
        }

        enemyKilledEMs = new ParticleSystem.EmissionModule[16];

        for (int x = 0; x < 16; x++)
        {
            enemyKilledEMs[x] = enemyKilledPFX[x].GetComponent<ParticleSystem>().emission;
            enemyKilledEMs[x].enabled = false;
        }
        objToFollow = new GameObject[16];
    }

    public void PlayEffect(int col, Vector3 pos, GameObject obj, Vector3 cutsomScale, bool hasICodeScript)
    {
        if (col == 0) //red
        {
            for(int y = 0; y < redPFX.Length; y++)
            {
                if (!redPFXEMs[y].enabled)
                {
                    redPFX[y].transform.position = pos;
                    redPFXEMs[y].enabled = true;
                    redPFX[y].GetComponent<ParticleSystem>().Play();
                    StartCoroutine("DeactivateAfterR", y);
                    return;
                }
            }
        }
        if(col == 1) //blue
        {
            for (int y = 0; y < bluePFX.Length; y++)
            {
                if (!blueEMs[y].enabled)
                {
                    bluePFX[y].transform.position = pos;
                    blueEMs[y].enabled = true;
                    bluePFX[y].GetComponent<ParticleSystem>().Play();
                    StartCoroutine("DeactivateAfterB", y);
                    return;
                }
            }
        }
        if (col == 2) //purple
        {
            for (int y = 0; y < purplePFX.Length; y++)
            {
                if (!purpleEMs[y].enabled)
                {
                    purpleEMs[y].enabled = true;
                    purplePFX[y].GetComponent<ParticleSystem>().Play();
                    StartCoroutine("DeactivateAfterP", y);
                    return;
                }
            }
        }
        if (col == 3) //Enemy Killed
        {
            for (int y = 0; y < enemyKilledPFX.Length; y++)
            {
                if (!enemyKilledEMs[y].enabled)
                {
                    objToFollow[y] = obj;
                    enemyKilledEMs[y].enabled = true;
                    enemyKilledPFX[y].GetComponent<ParticleSystem>().Play();
                    if(cutsomScale != Vector3.zero)
                        enemyKilledPFX[y].transform.localScale = cutsomScale;
                    else
                        enemyKilledPFX[y].transform.localScale = obj.transform.lossyScale;
                    StartCoroutine("DeactivateAfterEK", y);
                    StartCoroutine(EKFXFollow(y, hasICodeScript));
                    return;
                }
            }
        }
    }

    IEnumerator DeactivateAfterR(int index)
    {
        yield return new WaitForSeconds(1.2f);
        redPFXEMs[index].enabled = false;
    }

    IEnumerator DeactivateAfterB(int index)
    {
        yield return new WaitForSeconds(1.2f);
        blueEMs[index].enabled = false;
    }

    IEnumerator DeactivateAfterP(int index)
    {
        yield return new WaitForSeconds(1.2f);
        purpleEMs[index].enabled = false;
    }

    IEnumerator EKFXFollow(int index, bool hasICodeScript)
    {
        float yPos = objToFollow[index].transform.position.y;
        while (enemyKilledEMs[index].enabled)
        {
            if (objToFollow[index] != null)
            {
                if(hasICodeScript)
                    enemyKilledPFX[index].transform.position = objToFollow[index].transform.position;
                else
                    enemyKilledPFX[index].transform.position = new Vector3(objToFollow[index].transform.position.x, yPos, objToFollow[index].transform.position.z);
            }
            yield return null;
        }
    }

    IEnumerator DeactivateAfterEK(int index)
    {
        yield return new WaitForSeconds(2f);
        enemyKilledEMs[index].enabled = false;
    }
}
