#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class ReplaceBlockPrefabs : MonoBehaviour {

    public Vector3 newScale;

    private void Start()
    {
        
    }

    public void Replace() {
        newScale = new Vector3(Mathf.Clamp(newScale.x, 0.37f, Mathf.Infinity), Mathf.Clamp(newScale.y, 0.37f, Mathf.Infinity), Mathf.Clamp(newScale.z, 0.37f, Mathf.Infinity));
        Vector3 resultScale = new Vector3(newScale.x - 1f, newScale.y - 1f, newScale.z - 1f);

        this.gameObject.GetComponent<BoxCollider>().size = new Vector3(newScale.x, newScale.y, newScale.z) * 2f;
        float offset = 1f;

        //CORNERS
        GameObject cftl = this.transform.Find("BevelledCornerFrontTopLeft").gameObject;
        GameObject cftr = this.transform.Find("BevelledCornerFrontTopRight").gameObject;
        GameObject cfbl = this.transform.Find("BevelledCornerFrontBottomLeft").gameObject;
        GameObject cfbr = this.transform.Find("BevelledCornerFrontBottomRight").gameObject;
        GameObject cbtl = this.transform.Find("BevelledCornerBackTopLeft").gameObject;
        GameObject cbtr = this.transform.Find("BevelledCornerBackTopRight").gameObject;
        GameObject cbbl = this.transform.Find("BevelledCornerBackBottomLeft").gameObject;
        GameObject cbbr = this.transform.Find("BevelledCornerBackBottomRight").gameObject;

        cftl.transform.localPosition = new Vector3(resultScale.x, resultScale.y, resultScale.z);
        cftr.transform.localPosition = new Vector3(resultScale.x, resultScale.y, -resultScale.z);
        cfbl.transform.localPosition = new Vector3(resultScale.x, -resultScale.y, resultScale.z);
        cfbr.transform.localPosition = new Vector3(resultScale.x, -resultScale.y, -resultScale.z);
        cbtl.transform.localPosition = new Vector3(-resultScale.x, resultScale.y, resultScale.z);
        cbtr.transform.localPosition = new Vector3(-resultScale.x, resultScale.y, -resultScale.z);
        cbbl.transform.localPosition = new Vector3(-resultScale.x, -resultScale.y, resultScale.z);
        cbbr.transform.localPosition = new Vector3(-resultScale.x, -resultScale.y, -resultScale.z);

        //EDGES
        GameObject eft = this.transform.Find("BevelledEdgeFrontTop").gameObject;
        GameObject efb = this.transform.Find("BevelledEdgeFrontBottom").gameObject;
        GameObject efl = this.transform.Find("BevelledEdgeFrontLeft").gameObject;
        GameObject efr = this.transform.Find("BevelledEdgeFrontRight").gameObject;
        GameObject elt = this.transform.Find("BevelledEdgeLeftTop").gameObject;
        GameObject elb = this.transform.Find("BevelledEdgeLeftBottom").gameObject;
        GameObject ert = this.transform.Find("BevelledEdgeRightTop").gameObject;
        GameObject erb = this.transform.Find("BevelledEdgeRightBottom").gameObject;
        GameObject ebt = this.transform.Find("BevelledEdgeBackTop").gameObject;
        GameObject ebb = this.transform.Find("BevelledEdgeBackBottom").gameObject;
        GameObject ebl = this.transform.Find("BevelledEdgeBackLeft").gameObject;
        GameObject ebr = this.transform.Find("BevelledEdgeBackRight").gameObject;

        eft.transform.localPosition = new Vector3(resultScale.x, resultScale.y, 0f);
        efb.transform.localPosition = new Vector3(resultScale.x, -resultScale.y, 0f);
        efl.transform.localPosition = new Vector3(resultScale.x, 0f, resultScale.z);
        efr.transform.localPosition = new Vector3(resultScale.x, 0f, -resultScale.z);
        elt.transform.localPosition = new Vector3(0f, resultScale.y, resultScale.z);
        elb.transform.localPosition = new Vector3(0f, -resultScale.y, resultScale.z);
        ert.transform.localPosition = new Vector3(0f, resultScale.y, -resultScale.z);
        erb.transform.localPosition = new Vector3(0f, -resultScale.y, -resultScale.z);
        ebt.transform.localPosition = new Vector3(-resultScale.x, resultScale.y, 0f);
        ebb.transform.localPosition = new Vector3(-resultScale.x, -resultScale.y, 0f);
        ebl.transform.localPosition = new Vector3(-resultScale.x, 0f, resultScale.z);
        ebr.transform.localPosition = new Vector3(-resultScale.x, 0f, -resultScale.z);

        eft.transform.localScale = new Vector3(1f, 1f, newScale.z + (resultScale.z*0.59f));
        efb.transform.localScale = new Vector3(1f, 1f, newScale.z + (resultScale.z * 0.59f));
        efl.transform.localScale = new Vector3(1f, newScale.y + (resultScale.y * 0.59f), 1f);
        efr.transform.localScale = new Vector3(1f, newScale.y + (resultScale.y * 0.59f), 1f);
        elt.transform.localScale = new Vector3(newScale.x + (resultScale.x * 0.59f), 1f, 1f);
        elb.transform.localScale = new Vector3(newScale.x + (resultScale.x * 0.59f), 1f, 1f);
        ert.transform.localScale = new Vector3(newScale.x + (resultScale.x * 0.59f), 1f, 1f);
        erb.transform.localScale = new Vector3(newScale.x + (resultScale.x * 0.59f), 1f, 1f);
        ebt.transform.localScale = new Vector3(1f, 1f, newScale.z + (resultScale.z * 0.59f));
        ebb.transform.localScale = new Vector3(1f, 1f, newScale.z + (resultScale.z * 0.59f));
        ebl.transform.localScale = new Vector3(1f, newScale.y + (resultScale.y * 0.59f), 1f);
        ebr.transform.localScale = new Vector3(1f, newScale.y + (resultScale.y * 0.59f), 1f);

        //

        //FACES
        GameObject ff = this.transform.Find("BevelledFaceFront").gameObject;
        GameObject fb = this.transform.Find("BevelledFaceBack").gameObject;
        GameObject fl = this.transform.Find("BevelledFaceLeft").gameObject;
        GameObject fr = this.transform.Find("BevelledFaceRight").gameObject;
        GameObject ft = this.transform.Find("BevelledFaceTop").gameObject;
        GameObject fbt = this.transform.Find("BevelledFaceBottom").gameObject;

        ff.transform.localScale = new Vector3(newScale.x, newScale.y + (resultScale.y * 0.59f), newScale.z+ (resultScale.z*0.59f));
        fb.transform.localScale = new Vector3(newScale.x, newScale.y + (resultScale.y * 0.59f), newScale.z + (resultScale.z * 0.59f));
        fl.transform.localScale = new Vector3(newScale.x + (resultScale.x * 0.59f), newScale.y + (resultScale.y * 0.59f), newScale.z);
        fr.transform.localScale = new Vector3(newScale.x + (resultScale.x * 0.59f), newScale.y + (resultScale.y * 0.59f), newScale.z);
        ft.transform.localScale = new Vector3(newScale.x + (resultScale.x * 0.59f), newScale.y, newScale.z + (resultScale.z * 0.59f));
        fbt.transform.localScale = new Vector3(newScale.x + (resultScale.x * 0.59f), newScale.y, newScale.z + (resultScale.z * 0.59f));

        //
    }
}

[CustomEditor(typeof(ReplaceBlockPrefabs))]
public class ReplaceThemAll : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        ReplaceBlockPrefabs myScript = (ReplaceBlockPrefabs)target;
        if (GUILayout.Button("Replace"))
        {
            myScript.Replace();
        }
    }
}
#endif