using System;
using UnityEngine;

public enum PreferredColliderType { BOX_COLLIDER, SPHERE_COLLIDER, CAPSULE_COLLIDER }

[Serializable]
public class BakedColliderInfo
{
    public Vector3 Position { get { return position; } }
    public Quaternion Rotation { get { return rotation; } }
    public Bounds Extents { get { return extents; } }

    public PreferredColliderType ColliderType { get { return colliderType; } }

    [SerializeField] Vector3 position;
    [SerializeField] Quaternion rotation;
    [SerializeField] Bounds extents;

    [SerializeField] PreferredColliderType colliderType;

    public BakedColliderInfo(Transform _colliderT, PreferredColliderType _colliderType)
    {
        position = _colliderT.position;
        rotation = _colliderT.rotation;
        extents = _colliderT.GetComponent<Renderer>().bounds;

        colliderType = _colliderType;
    }
}
