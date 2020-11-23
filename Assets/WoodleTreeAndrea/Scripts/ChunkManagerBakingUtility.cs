#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

public class ChunkManagerBakingUtility
{
    [MenuItem("WoodleTree/BakeChunkManagers")]
    public static void BakeChunkManagers()
    {
        BakeChunkReferences();
    }

    public static void BakeChunkReferences()
    {
        List<ChunkManager> managerList = new List<ChunkManager>();
        managerList.AddRange(GameObject.FindObjectsOfType<ChunkManager>());

        List<Animable> animableList = new List<Animable>();
        animableList.AddRange(GameObject.FindObjectsOfType<Animable>());

        foreach (Animable an in animableList)
        {
            foreach (ChunkManager cm in managerList)
            {
                if (cm.ChunkCollider.bounds.Contains(an.transform.position))
                {
                    an.SetOwner(cm);
                    cm.AddAnimable_EDITOR(an);
                }
            }
        }

        List<MonoBehaviour> _temp = new List<MonoBehaviour>();
        _temp.AddRange(GameObject.FindObjectsOfType<MonoBehaviour>());

        foreach (MonoBehaviour b in _temp)
        {
            if (b is IActivable)
            {
                foreach (ChunkManager cm in managerList)
                {
                    if (cm.ChunkCollider.bounds.Contains(b.transform.position))
                        cm.AddActivable_EDITOR(b.gameObject);
                }
            }
        }

        foreach (ChunkManager cm in managerList)
        {
            cm.BakeReferences();
            cm.Deactivate();
        }

        EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
    }
}
#endif