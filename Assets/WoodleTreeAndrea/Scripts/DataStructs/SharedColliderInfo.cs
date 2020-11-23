using System;
using UnityEngine;

[Serializable]
public class SharedColliderInfo
{
    public BakedColliderInfo SharedCollider { get { return sharedCollider; } }
    public ChunkManager[] ChunkManagers { get { return chunkManagers; } }

    [SerializeField] BakedColliderInfo sharedCollider;
    [SerializeField] ChunkManager[] chunkManagers;

    public SharedColliderInfo(BakedColliderInfo _sharedCollider, ChunkManager[] _chunkManagers)
    {
        sharedCollider = _sharedCollider;
        chunkManagers = _chunkManagers;
    }

    public bool DoDeactivate(ChunkManager petitioner)
    {
        bool result = true;

        for (int i = 0; i < chunkManagers.Length; i++)
        {
            if(chunkManagers[i] != petitioner)
                result &= !chunkManagers[i].IsActive;
        }

        return result;
    }
}
