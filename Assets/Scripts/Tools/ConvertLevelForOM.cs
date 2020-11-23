#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class ConvertLevelForOM : MonoBehaviour
{
    public LevelObjectManager lom;

    void Start()
    {
        if (lom == null)
            lom = this.gameObject.GetComponent<LevelObjectManager>();
    }

    public void Convert()
    {
        foreach(LevelObjectManager.ParentPrefab pp in lom.parentPrefabs)
        {
            foreach(Transform t in pp.parent.GetComponentsInChildren<Transform>(true))
            {
                if (t != null && t != pp.parent.transform)
                {
                    if(PrefabUtility.GetPrefabAssetType(t.gameObject) != PrefabAssetType.NotAPrefab && PrefabUtility.GetPrefabInstanceStatus(t.gameObject) != PrefabInstanceStatus.NotAPrefab) 
                        PrefabUtility.UnpackPrefabInstance(t.gameObject, PrefabUnpackMode.Completely, InteractionMode.AutomatedAction);
                    if(t.parent != pp.parent.transform)
                        DestroyImmediate(t.gameObject);
                    else
                    {
                        Component[] components = t.gameObject.GetComponents(typeof(Component));

                        foreach (Component comp in components)
                        {
                            if (comp.GetType() != typeof(Transform))
                                DestroyImmediate(comp);
                        }
                    }
                }
            }
        }
    }
}


[CustomEditor(typeof(ConvertLevelForOM))]
public class ConvertLevelForOMEdit : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        ConvertLevelForOM myScript = (ConvertLevelForOM)target;
        if (GUILayout.Button("Convert"))
        {
            myScript.Convert();
        }
    }
}
#endif
