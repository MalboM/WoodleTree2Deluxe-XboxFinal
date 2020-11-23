#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class KeepFirstChildren : MonoBehaviour
{
    public void RemoveThem()
    {
        foreach(Transform t in this.gameObject.GetComponentsInChildren<Transform>(true))
        {
            if (t != null && t != this.gameObject.transform && t.parent != this.gameObject.transform)
                DestroyImmediate(t.gameObject);
        }
    }
}

[CustomEditor(typeof(KeepFirstChildren))]
public class KeepFirstChildrenEdit : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        KeepFirstChildren myScript = (KeepFirstChildren)target;
        if (GUILayout.Button("Keep First Children"))
        {
            myScript.RemoveThem();
        }
    }
}
#endif