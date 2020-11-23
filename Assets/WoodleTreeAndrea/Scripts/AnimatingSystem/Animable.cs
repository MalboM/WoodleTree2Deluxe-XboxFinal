using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Animable : MonoBehaviour
{
    [SerializeField] protected ChunkManager owner;

    public abstract void Animate(float deltaTime);

#if UNITY_EDITOR

    public void SetOwner(ChunkManager owner)
    {
        this.owner = owner;
    }

#endif
}
