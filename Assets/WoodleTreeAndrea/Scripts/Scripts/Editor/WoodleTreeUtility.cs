#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class WoodleTreeUtility
{
    /*
     * working on it, disabled for now
    [MenuItem("WoodleTree/BakeChunkManagers")]
    public static void BakeChunkManagers()
    {
        foreach (ChunkManager cm in GameObject.FindObjectsOfType<ChunkManager>())
            cm.BakeChunkReferences();
    }
    */
    [MenuItem("WoodleTree/CombineMeshes")]
    public static void CombineMeshes()
    {
        foreach (MeshCombiner cm in GameObject.FindObjectsOfType<MeshCombiner>())
            cm.CombineMesh();
    }

    [MenuItem("WoodleTree/PositionShadows")]
    public static void PositionShadows()
    {
        foreach (BlobShadowRaycaster sr in GameObject.FindObjectsOfType<BlobShadowRaycaster>())
            sr.RaycastShadow();
    }

}
#endif