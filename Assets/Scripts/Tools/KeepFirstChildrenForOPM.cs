#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class KeepFirstChildrenForOPM : MonoBehaviour
{
    public void Delete()
    {
        foreach (Transform child in this.transform)
        {
            foreach (Transform childX in child)
                DestroyImmediate(childX.gameObject);
        }
    }
}

[CustomEditor(typeof(KeepFirstChildrenForOPM))]
public class KeepFirstChildrenForOPMEdit : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        KeepFirstChildrenForOPM myScript = (KeepFirstChildrenForOPM)target;
        if (GUILayout.Button("Run"))
        {
            myScript.Delete();
        }
    }
}
#endif
