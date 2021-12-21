using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
public class BerryShadowPlacer : MonoBehaviour {

    public LayerMask whatToHit;

#if UNITY_EDITOR    
    public void ChangeThem()
    {
        foreach (Transform t in this.transform) {
            if(t.gameObject != this.gameObject)
                CheckChild(t);
        }
    }

    void CheckChild(Transform t) {
        if (t.gameObject.name != "NewBlobShadow")
        {
            if (t.childCount > 0)
            {
                int x = t.childCount - 1;
                while(x >= 0)
                {
                    CheckChild(t.GetChild(x));
                    x--;
                }
            }
        }
        else
            PositionIt(t.parent);
    }

    void PositionIt(Transform t)
    {
        RaycastHit heh = new RaycastHit();
        if (Physics.Raycast(t.position, -Vector3.up, out heh, 100f, whatToHit) && t.tag != "Elevator")
        {
            t.GetChild(0).up = heh.normal;
            t.GetChild(0).position = heh.point + (heh.normal * 0.03f);
        }
        else
            t.GetChild(0).position = t.position - (Vector3.up*500f);
    }

    public void Count()
    {
        int c = 0;
        foreach (Transform t in this.transform)
        {
            if (t.transform.parent == this.transform)
                c++;
        }
        Debug.Log(c.ToString());
    }
#endif
}

#if UNITY_EDITOR
[CustomEditor(typeof(BerryShadowPlacer))]
public class BerryShadowPlacerEdit : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        BerryShadowPlacer myScript = (BerryShadowPlacer)target;
        if (GUILayout.Button("Update Shadows"))
        {
            myScript.ChangeThem();
        }
        if (GUILayout.Button("Count"))
        {
            myScript.Count();
        }
    }
}
#endif