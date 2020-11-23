#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class AddObjectsToLOD : MonoBehaviour {

    public void AddIt() {
        LODGroup lODGroup = this.gameObject.GetComponent<LODGroup>();

        LOD[] Lods = new LOD[1];

        // Fill up Renderer[] arrays.  This is the list of drawables per LOD level
        Lods[0].renderers = this.GetComponentsInChildren<Renderer>();

        // Make it live!
        lODGroup.SetLODS(Lods);
        lODGroup.RecalculateBounds();
    }
}


[CustomEditor(typeof(AddObjectsToLOD))]
public class AddItAll : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        AddObjectsToLOD myScript = (AddObjectsToLOD)target;
        if (GUILayout.Button("Replace"))
        {
            myScript.AddIt();
        }
    }
}
#endif
