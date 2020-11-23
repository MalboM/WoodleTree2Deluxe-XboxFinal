using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkManagerPoolManager : MonoBehaviour
{
    static ChunkManagerPoolManager singleton;
    static List<PooledColliderInfo> boxPool;
    static List<PooledColliderInfo> spherePool;
    static List<PooledColliderInfo> capsulePool;

    [SerializeField] int poolStartSize;

    void Awake()
    {
        if (singleton != null)
        {
            Destroy(this);
            enabled = false;
            return;
        }

        singleton = this;
        DontDestroyOnLoad(this);

        Initialize();
    }
	
	void Initialize()
    {
        boxPool = new List<PooledColliderInfo>();
        spherePool = new List<PooledColliderInfo>();
        capsulePool = new List<PooledColliderInfo>();
	}

    void RequestPoolEnlargement(PreferredColliderType cType)
    {
        GameObject go = new GameObject("collider_pooled_" + cType.ToString());
        switch (cType)
        {
            case PreferredColliderType.BOX_COLLIDER:
                go.AddComponent<BoxCollider>();
                PooledColliderInfo pInfo_b = new PooledColliderInfo(PreferredColliderType.BOX_COLLIDER, go);            
                boxPool.Add(pInfo_b);
                break;
            case PreferredColliderType.SPHERE_COLLIDER:
                go.AddComponent<SphereCollider>();
                PooledColliderInfo pInfo_s = new PooledColliderInfo(PreferredColliderType.SPHERE_COLLIDER, go);
                spherePool.Add(pInfo_s);
                break;
            case PreferredColliderType.CAPSULE_COLLIDER:
                go.AddComponent<CapsuleCollider>();
                PooledColliderInfo pInfo_c = new PooledColliderInfo(PreferredColliderType.CAPSULE_COLLIDER, go);
                capsulePool.Add(pInfo_c);
                break;
        }
    }

    public static void RequestPoolReposition(BakedColliderInfo[] singleChunkColliders)
    {
        if (singleton == null)
        {
            Debug.LogError("WARNING: ChunkManagerPoolManager.RequestPoolReposition(" + "singleChunkColliders [" + singleChunkColliders.Length + "elements]) was called but singleton was null. Aborting");
            return;
        }

        singleton._RequestPoolReposition(singleChunkColliders);
    }

    void _RequestPoolReposition(BakedColliderInfo[] singleChunkColliders)
    {
        int index = 0;

        for (int i = 0; i < singleChunkColliders.Length; i++)
        {
            index = i;

            switch (singleChunkColliders[i].ColliderType)
            {
                case PreferredColliderType.BOX_COLLIDER:

                    if (i >= boxPool.Count)
                    {
                        RequestPoolEnlargement(PreferredColliderType.BOX_COLLIDER);
                        index = boxPool.Count - 1;
                    }

                    boxPool[index].ColliderGO.transform.position = singleChunkColliders[i].Position;
                    (boxPool[index].Collider as BoxCollider).size = singleChunkColliders[i].Extents.extents * 2;

                    break;
                case PreferredColliderType.SPHERE_COLLIDER:

                    if (i >= spherePool.Count)
                    {
                        RequestPoolEnlargement(PreferredColliderType.SPHERE_COLLIDER);
                        index = spherePool.Count - 1;
                    }

                    spherePool[index].ColliderGO.transform.position = singleChunkColliders[i].Position;
                    (spherePool[index].Collider as SphereCollider).radius = singleChunkColliders[i].Extents.extents.magnitude;
                    
                    break;
                case PreferredColliderType.CAPSULE_COLLIDER:

                    if (i >= capsulePool.Count)
                    {
                        RequestPoolEnlargement(PreferredColliderType.CAPSULE_COLLIDER);
                        index = capsulePool.Count - 1;
                    }

                    capsulePool[index].ColliderGO.transform.position = singleChunkColliders[i].Position;
                    (capsulePool[index].Collider as CapsuleCollider).height = singleChunkColliders[i].Extents.extents.magnitude;

                    break;
            }
        }
    }
}
