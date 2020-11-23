using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkDistanceChecker : MonoBehaviour
{

    [System.Serializable]
    public class ChunkChild { public Vector3 chunkPos; public List<GameObject> chunkObjects = new List<GameObject>(); public bool inChunk; }

    public List<ChunkChild> chunkChildren = new List<ChunkChild>();

    public float intervalBetweenChecks = 3f;

    public float activationDistance = 80f;
    float origAD;
    float activationDistanceToUse;

    TPC chara;
    OneWayManager owm;

    public float distanceMultiplier = 1.5f;

    /*
    private void Start()
    {
        if (objectPoolManager)
        {
            if (objectPoolManager.objectPools != null && objectPoolManager.objectPools.Length > 0)
            {
                foreach (ObjectPool op in objectPoolManager.objectPools)
                    op.maxObjects = op.pooledObjects.Count;
            }
        }
    }
    */

    void OnEnable()
    {
        StopAllCoroutines();
        if (chunkChildren.Count != 0)
        {
            activationDistanceToUse = activationDistance * activationDistance;
            origAD = activationDistance;

            StartCoroutine("InitialCheck");
        }
    }

    IEnumerator InitialCheck()
    {
        while (PlayerManager.GetMainPlayer() == null || PlayerManager.GetMainPlayer().GetComponentInParent<OneWayManager>() == null)
            yield return null;
        owm = PlayerManager.GetMainPlayer().GetComponentInParent<OneWayManager>();
        if (owm != null)
        {
            while (owm.currentlyChecking)
                yield return null;
        }
        else
            yield return null;

        float interval = intervalBetweenChecks / chunkChildren.Count;
        float totalInterval = 0f;
        foreach (ChunkChild c in chunkChildren)
        {
            c.inChunk = false;
            object[] paras = new object[3];
            paras[0] = c;
            paras[1] = true;
            paras[2] = totalInterval;
            totalInterval += interval;
            StartCoroutine("CheckDistance", paras);
            //    yield return new WaitForSeconds(interval);
        }
    }

    IEnumerator CheckDistance(object[] paras)
    {
        ChunkChild chunk = (ChunkChild)paras[0];

        if (chara == null && PlayerManager.GetMainPlayer() != null)
            chara = PlayerManager.GetMainPlayer();

        if (activationDistance != origAD)
        {
            activationDistanceToUse = activationDistance * activationDistance;
            origAD = activationDistance;
        }

        if (chara)
        {
            distanceMultiplier = Mathf.Lerp(1f, 2f, (float)PlayerPrefs.GetInt("ObjectDetails", 2) / 8f);

            if ((chunk.chunkPos - chara.transform.position).sqrMagnitude <= activationDistanceToUse * distanceMultiplier)
            {
                if (!chunk.inChunk)
                {
                    chunk.inChunk = true;
                    foreach (GameObject g in chunk.chunkObjects)
                    {
                        if (g)
                            g.SetActive(true);
                    }
                }
            }
            else
            {
                if ((bool)paras[1] == true || chunk.inChunk)
                {
                    chunk.inChunk = false;
                    foreach (GameObject g in chunk.chunkObjects)
                    {
                        if (g)
                            g.SetActive(false);
                    }
                }
            }
        }
        yield return new WaitForSeconds(intervalBetweenChecks + (float)paras[2]);
        paras[1] = false;
        paras[2] = 0f;
        StartCoroutine("CheckDistance", paras);
    }

    /*
    void ResetObject(ObjectPool pool, GameObject obj)
    {
        if(pool.objectName == "Berry Red")
        {
            foreach (Transform t in obj.GetComponentInChildren<Transform>(true))
                t.gameObject.SetActive(true);
            obj.transform.GetChild(1).transform.localScale = Vector3.one;
            obj.transform.GetChild(1).transform.localPosition = Vector3.zero;
        }
        if (pool.objectName == "Dark Enemy Wandering" || pool.objectName == "Dark Enemy Flying" || pool.objectName == "Dark Enemy Big")
        {
            EnemyHP enemyHP = obj.GetComponentInChildren<EnemyHP>(true);
            foreach (Renderer r in enemyHP.renderers)
                r.enabled = true;
            enemyHP.parentToDisable.SetActive(true);
            enemyHP.Reset();
        }
    }
    */

    public bool ShouldBeActivated(GameObject g)
    {
        foreach (ChunkChild c in chunkChildren)
        {
            if (c.inChunk)
            {
                if (c.chunkObjects.Contains(g))
                    return true;
            }
        }
        return false;
    }
}