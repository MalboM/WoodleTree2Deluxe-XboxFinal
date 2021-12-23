using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBarrier : MonoBehaviour
{
    public EnemyHP[] enemieHPScripts;
    int enemyCount;

    public GameObject cameraPosition;
    public GameObject barrier;
    public GameObject pfx;
    private ParticleSystem.EmissionModule pfxEM;
    TPC tpc;
    CameraFollower camF;

    public Collider protectedCollider;

    void Start()
    {
        pfxEM = pfx.GetComponent<ParticleSystem>().emission;
        pfxEM.enabled = false;

        enemyCount = 0;
        if (enemieHPScripts.Length > 0)
        {
            foreach (EnemyHP ehp in enemieHPScripts)
            {
                if (ehp != null)
                {
                    ehp.enemyBarrier = this;
                    enemyCount++;
                }
            }
        }

        if(protectedCollider)
            protectedCollider.enabled = false;

    //   Debug.Log(this.gameObject.name + ": " + enemyCount);
    }

    public void EnemyKilled()
    {
        enemyCount--;
    //    Debug.Log(this.gameObject.name + " left: " + enemyCount);
        if (enemyCount == 0)
            RemoveBarrier();
    }

    void RemoveBarrier()
    {
        //    Debug.Log(this.gameObject.name + " all gone.");
        StartCoroutine(PlayCutscene());
    }


    IEnumerator PlayCutscene()
    {
        yield return new WaitForSeconds(3f);

        GameObject.FindWithTag("Pause").transform.Find("Bars").GetComponent<Animator>().SetBool("barsToggle", true);

        tpc = PlayerManager.GetMainPlayer();
        camF = tpc.cam.GetComponent<CameraFollower>();

        tpc.disableControl = true;
        tpc.rb.isKinematic = true;
        tpc.inCutscene = true;

        camF.disableControl = true;
        Vector3 origPos = camF.transform.position;
        Quaternion origRot = camF.transform.rotation;

        if (cameraPosition == null)
        {
            camF.transform.position = barrier.transform.position - (barrier.transform.forward * 8f) + (Vector3.up * 6f);
            camF.transform.LookAt(barrier.transform);
        }
        else
        {
            camF.transform.position = cameraPosition.transform.position;
            camF.transform.rotation = cameraPosition.transform.rotation;
        }
        this.GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(0.8f);
        pfxEM.enabled = true;
        pfx.GetComponent<ParticleSystem>().Play();
        StartCoroutine(tpc.DeactivateParticle(pfx, pfxEM));
        yield return null;
        barrier.SetActive(false);
        if(protectedCollider)
            protectedCollider.enabled = true;

        GameObject.FindWithTag("Pause").transform.Find("Bars").GetComponent<Animator>().SetBool("barsToggle", false);

        yield return new WaitForSeconds(0.5f);

        for (int f = 0; f <= 60; f++)
        {
            camF.transform.position = Vector3.Lerp(camF.transform.position, origPos, f / 60f);
            camF.transform.rotation = Quaternion.Lerp(camF.transform.rotation, origRot, f / 60f);
            yield return null;
        }

        tpc.disableControl = false;
        tpc.rb.isKinematic = false;
        tpc.inCutscene = false;
        camF.disableControl = false;
    }
}
