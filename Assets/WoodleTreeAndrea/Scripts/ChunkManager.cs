using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif

[RequireComponent(typeof(Collider))]
public class ChunkManager : MonoBehaviour
{
    public Collider ChunkCollider { get { if (chunkTrigger == null) chunkTrigger = GetComponent<Collider>(); return chunkTrigger; } }
    public bool IsActive { get { return isActive; } }

    [SerializeField] bool isActive;
    [SerializeField] Collider chunkTrigger;
    [SerializeField] List<Animable> animables;
    [SerializeField] List<GameObject> activables;

    public void Activate()
    {
        SetColliderEnabled(true);
    }

    public void Deactivate()
    {
        SetColliderEnabled(false);
    }

    void SetColliderEnabled(bool doEnable)
    {
        if (activables.Count > 0)
        {
            for (int i = 0; i < activables.Count; i++)
            {
                if (activables[i] != null && activables[i].GetComponent<IActivable>() != null)
                {
                    if (doEnable)
                        activables[i].GetComponent<IActivable>().Activate();
                    else
                        activables[i].GetComponent<IActivable>().Deactivate();
                }
            }
        }
        isActive = doEnable;
        enabled = doEnable;
    }

    void Update()
    {
        if (animables.Count > 0)
        {
            for (int i = 0; i < animables.Count; i++)
            {
                animables[i].Animate(Time.deltaTime);
            }
        }
    }

    public void AddAnimable(Animable obj)
    {
        if (!animables.Contains(obj))
            animables.Add(obj);
    }

    public void AddActivable(GameObject obj)
    {
        if (!activables.Contains(obj))
            activables.Add(obj);
    }

    public void RemoveAnimable(Animable obj)
    {
        if (animables.Contains(obj))
            animables.Remove(obj);
    }

    public void RemoveActivable(GameObject obj)
    {
        if (activables.Contains(obj))
            activables.Remove(obj);
    }

#if UNITY_EDITOR

    List<Animable> animablesToBake;
    List<GameObject> activablesToBake;

    public void AddAnimable_EDITOR(Animable a)
    {
        if (animablesToBake == null)
            animablesToBake = new List<Animable>();

        if (!animablesToBake.Contains(a))
            animablesToBake.Add(a);
    }

    public void AddActivable_EDITOR(GameObject a)
    {
        if (activablesToBake == null)
            activablesToBake = new List<GameObject>();

        if (!activablesToBake.Contains(a))
            activablesToBake.Add(a);
    }

    public void BakeReferences()
    {
        Undo.RecordObject(this, "bakeChunkRefereces_" + name);

        if (animablesToBake == null)
            animablesToBake = new List<Animable>();

        if (activablesToBake == null)
            activablesToBake = new List<GameObject>();

        animables = animablesToBake;
        activables = activablesToBake;

        animablesToBake = new List<Animable>();
        activablesToBake = new List<GameObject>();
    }

#endif
}
