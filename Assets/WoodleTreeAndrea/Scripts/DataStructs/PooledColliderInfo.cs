using System;
using UnityEngine;

[Serializable]
public class PooledColliderInfo
{
    public PreferredColliderType ColliderType { get { return colliderType; } }
    public GameObject ColliderGO { get { return go; } }
    public Collider Collider { get { return c; } }

    PreferredColliderType colliderType;
    GameObject go;
    Collider c;

    public PooledColliderInfo(PreferredColliderType _colliderType, GameObject _go)
    {
        colliderType = _colliderType;
        go = _go;
        c = _go.GetComponent<Collider>();
    }
}
