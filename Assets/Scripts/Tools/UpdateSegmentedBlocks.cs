#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class UpdateSegmentedBlocks : MonoBehaviour
{
    BoxCollider full;
    GameObject low;

    public void ChangeThem() {
        foreach(Transform t in transform)
        {
        //    t.Find("Bevelled").gameObject.GetComponent<MeshFilter>().sharedMesh = t.gameObject.GetComponent<MeshFilter>().sharedMesh;
        //    t.Find("Bevelled").gameObject.GetComponent<MeshRenderer>().sharedMaterial = t.gameObject.GetComponent<MeshRenderer>().sharedMaterial;

        //    DestroyImmediate(t.gameObject.GetComponent<MeshFilter>());
        //    DestroyImmediate(t.gameObject.GetComponent<MeshRenderer>());

            if(t.gameObject.GetComponent<MeshRenderer>() != null)
                t.Find("Low").gameObject.GetComponent<MeshRenderer>().sharedMaterial = t.gameObject.GetComponent<MeshRenderer>().sharedMaterial;
            else
            {
                if (t.Find("Bevelled").gameObject.GetComponent<MeshRenderer>() != null)
                    t.Find("Low").gameObject.GetComponent<MeshRenderer>().sharedMaterial = t.Find("Bevelled").gameObject.GetComponent<MeshRenderer>().sharedMaterial;
            }
            if (t.Find("Bevelled").gameObject.GetComponent<BoxCollider>() == null)
                Debug.Log(t.name);
            else
                t.Find("Low").localScale = t.Find("Bevelled").gameObject.GetComponent<BoxCollider>().size;
        }
    }

    public void ChangeCols()
    {
        foreach (Transform t in transform)
        {
            foreach (Transform tr in t)
            {
                if (tr.name == "Bevelled")
                    full = tr.gameObject.GetComponent<BoxCollider>();
                if (tr.name == "Low")
                    low = tr.gameObject;
            }
            full.size = low.transform.localScale;
        }
    }
}

[CustomEditor(typeof(UpdateSegmentedBlocks))]
public class UpdateSegmentedBlocksEdit : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        UpdateSegmentedBlocks myScript = (UpdateSegmentedBlocks)target;
        if (GUILayout.Button("Update Children"))
        {
            myScript.ChangeThem ();
        }

        if (GUILayout.Button("Update Colliders"))
        {
            myScript.ChangeCols();
        }
    }
}
#endif
