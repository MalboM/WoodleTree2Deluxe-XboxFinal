using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BlobShadowRaycaster))]
public class BlobShadowRaycasterEditor : Editor
{
    public override void OnInspectorGUI()
    {
        BlobShadowRaycaster blob = (BlobShadowRaycaster)target;

        serializedObject.Update();

        DrawDefaultInspector();

        if (GUILayout.Button("Bake Shadow Position"))
            blob.RaycastShadow();

        serializedObject.ApplyModifiedProperties();
    }
}
