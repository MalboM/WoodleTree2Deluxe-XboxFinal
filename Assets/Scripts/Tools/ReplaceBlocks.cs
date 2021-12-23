using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
public class ReplaceBlocks : MonoBehaviour {

    public float theScale;
    public GameObject newParent;
    public GameObject prefab;
    public GameObject smallPrefab;
    bool smallBlock;
    GameObject cube1;

    
    public GameObject newprefab;
    public GameObject newsmallPrefab;


    //S: 2.461947. M: 15.884. L: 216.7947.
#if UNITY_EDITOR
    public void Replace() {
        if (theScale == 0f)
            theScale = 2f;

        foreach (Transform t in this.transform)
        {
            if (t.gameObject.GetComponent<BoxCollider>() != null)
            {
                Quaternion initRot = t.transform.localRotation;
                t.transform.localRotation = Quaternion.identity;

                t.transform.localScale /= 2f;
                Vector3 newScale = t.transform.localScale;

                if (newScale.x < 0.37f || newScale.y < 0.37f || newScale.z < 0.37f)
                    smallBlock = true;
                else
                    smallBlock = false;

                Object obj;
                if (!smallBlock)
                    obj = prefab;
                else
                    obj = smallPrefab;

                GameObject cube1 = PrefabUtility.InstantiatePrefab(obj as GameObject) as GameObject;
                cube1.tag = t.gameObject.tag;
                cube1.transform.SetParent(t);
                cube1.transform.localPosition = Vector3.zero;

                foreach(MeshRenderer mr in cube1.GetComponentsInChildren<MeshRenderer>())
                    mr.sharedMaterial = t.gameObject.GetComponent<MeshRenderer>().sharedMaterial;
                cube1.transform.SetParent(newParent.transform);

                //   newScale = new Vector3(Mathf.Clamp(newScale.x, 0.37f, Mathf.Infinity), Mathf.Clamp(newScale.y, 0.37f, Mathf.Infinity), Mathf.Clamp(newScale.z, 0.37f, Mathf.Infinity));

                if(smallBlock)
                    newScale = new Vector3(Mathf.Clamp(newScale.x, 0.19f, Mathf.Infinity), Mathf.Clamp(newScale.y, 0.19f, Mathf.Infinity), Mathf.Clamp(newScale.z, 0.19f, Mathf.Infinity));

                Vector3 resultScale = new Vector3(newScale.x - 1f, newScale.y - 1f, newScale.z - 1f);
                if (smallBlock)
                    resultScale = new Vector3(newScale.x - (1f/ theScale), newScale.y - (1f / theScale), newScale.z - (1f / theScale));

                float extraScale = 0.59f;

                cube1.gameObject.GetComponentInChildren<BoxCollider>().size = new Vector3(newScale.x, newScale.y, newScale.z) * 2f;

                if (!smallBlock)
                {
                    //CORNERS
                    GameObject cftl = cube1.transform.Find("BevelledCornerFrontTopLeft").gameObject;
                    GameObject cftr = cube1.transform.Find("BevelledCornerFrontTopRight").gameObject;
                    GameObject cfbl = cube1.transform.Find("BevelledCornerFrontBottomLeft").gameObject;
                    GameObject cfbr = cube1.transform.Find("BevelledCornerFrontBottomRight").gameObject;
                    GameObject cbtl = cube1.transform.Find("BevelledCornerBackTopLeft").gameObject;
                    GameObject cbtr = cube1.transform.Find("BevelledCornerBackTopRight").gameObject;
                    GameObject cbbl = cube1.transform.Find("BevelledCornerBackBottomLeft").gameObject;
                    GameObject cbbr = cube1.transform.Find("BevelledCornerBackBottomRight").gameObject;

                    cftl.transform.localPosition = new Vector3(resultScale.x, resultScale.y, resultScale.z);
                    cftr.transform.localPosition = new Vector3(resultScale.x, resultScale.y, -resultScale.z);
                    cfbl.transform.localPosition = new Vector3(resultScale.x, -resultScale.y, resultScale.z);
                    cfbr.transform.localPosition = new Vector3(resultScale.x, -resultScale.y, -resultScale.z);
                    cbtl.transform.localPosition = new Vector3(-resultScale.x, resultScale.y, resultScale.z);
                    cbtr.transform.localPosition = new Vector3(-resultScale.x, resultScale.y, -resultScale.z);
                    cbbl.transform.localPosition = new Vector3(-resultScale.x, -resultScale.y, resultScale.z);
                    cbbr.transform.localPosition = new Vector3(-resultScale.x, -resultScale.y, -resultScale.z);


                    //EDGES
                    GameObject eft = cube1.transform.Find("BevelledEdgeFrontTop").gameObject;
                    GameObject efb = cube1.transform.Find("BevelledEdgeFrontBottom").gameObject;
                    GameObject efl = cube1.transform.Find("BevelledEdgeFrontLeft").gameObject;
                    GameObject efr = cube1.transform.Find("BevelledEdgeFrontRight").gameObject;
                    GameObject elt = cube1.transform.Find("BevelledEdgeLeftTop").gameObject;
                    GameObject elb = cube1.transform.Find("BevelledEdgeLeftBottom").gameObject;
                    GameObject ert = cube1.transform.Find("BevelledEdgeRightTop").gameObject;
                    GameObject erb = cube1.transform.Find("BevelledEdgeRightBottom").gameObject;
                    GameObject ebt = cube1.transform.Find("BevelledEdgeBackTop").gameObject;
                    GameObject ebb = cube1.transform.Find("BevelledEdgeBackBottom").gameObject;
                    GameObject ebl = cube1.transform.Find("BevelledEdgeBackLeft").gameObject;
                    GameObject ebr = cube1.transform.Find("BevelledEdgeBackRight").gameObject;

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

                    eft.transform.localScale = new Vector3(1f, 1f, newScale.z + (resultScale.z * extraScale));
                    efb.transform.localScale = new Vector3(1f, 1f, newScale.z + (resultScale.z * extraScale));
                    efl.transform.localScale = new Vector3(1f, newScale.y + (resultScale.y * extraScale), 1f);
                    efr.transform.localScale = new Vector3(1f, newScale.y + (resultScale.y * extraScale), 1f);
                    elt.transform.localScale = new Vector3(newScale.x + (resultScale.x * extraScale), 1f, 1f);
                    elb.transform.localScale = new Vector3(newScale.x + (resultScale.x * extraScale), 1f, 1f);
                    ert.transform.localScale = new Vector3(newScale.x + (resultScale.x * extraScale), 1f, 1f);
                    erb.transform.localScale = new Vector3(newScale.x + (resultScale.x * extraScale), 1f, 1f);
                    ebt.transform.localScale = new Vector3(1f, 1f, newScale.z + (resultScale.z * extraScale));
                    ebb.transform.localScale = new Vector3(1f, 1f, newScale.z + (resultScale.z * extraScale));
                    ebl.transform.localScale = new Vector3(1f, newScale.y + (resultScale.y * extraScale), 1f);
                    ebr.transform.localScale = new Vector3(1f, newScale.y + (resultScale.y * extraScale), 1f);

                    //

                    //FACES
                    GameObject ff = cube1.transform.Find("BevelledFaceFront").gameObject;
                    GameObject fb = cube1.transform.Find("BevelledFaceBack").gameObject;
                    GameObject fl = cube1.transform.Find("BevelledFaceLeft").gameObject;
                    GameObject fr = cube1.transform.Find("BevelledFaceRight").gameObject;
                    GameObject ft = cube1.transform.Find("BevelledFaceTop").gameObject;
                    GameObject fbt = cube1.transform.Find("BevelledFaceBottom").gameObject;

                    ff.transform.localScale = new Vector3(newScale.x, newScale.y + (resultScale.y * extraScale), newScale.z + (resultScale.z * extraScale));
                    fb.transform.localScale = new Vector3(newScale.x, newScale.y + (resultScale.y * extraScale), newScale.z + (resultScale.z * extraScale));
                    fl.transform.localScale = new Vector3(newScale.x + (resultScale.x * extraScale), newScale.y + (resultScale.y * extraScale), newScale.z);
                    fr.transform.localScale = new Vector3(newScale.x + (resultScale.x * extraScale), newScale.y + (resultScale.y * extraScale), newScale.z);
                    ft.transform.localScale = new Vector3(newScale.x + (resultScale.x * extraScale), newScale.y, newScale.z + (resultScale.z * extraScale));
                    fbt.transform.localScale = new Vector3(newScale.x + (resultScale.x * extraScale), newScale.y, newScale.z + (resultScale.z * extraScale));
                }
                else
                {
                    //CORNERS
                    GameObject cftl = cube1.transform.Find("BevelledCornerFrontTopLeft").gameObject;
                    GameObject cftr = cube1.transform.Find("BevelledCornerFrontTopRight").gameObject;
                    GameObject cfbl = cube1.transform.Find("BevelledCornerFrontBottomLeft").gameObject;
                    GameObject cfbr = cube1.transform.Find("BevelledCornerFrontBottomRight").gameObject;
                    GameObject cbtl = cube1.transform.Find("BevelledCornerBackTopLeft").gameObject;
                    GameObject cbtr = cube1.transform.Find("BevelledCornerBackTopRight").gameObject;
                    GameObject cbbl = cube1.transform.Find("BevelledCornerBackBottomLeft").gameObject;
                    GameObject cbbr = cube1.transform.Find("BevelledCornerBackBottomRight").gameObject;

                    cftl.transform.localPosition = new Vector3(resultScale.x, resultScale.y, resultScale.z);
                    cftr.transform.localPosition = new Vector3(resultScale.x, resultScale.y, -resultScale.z);
                    cfbl.transform.localPosition = new Vector3(resultScale.x, -resultScale.y, resultScale.z);
                    cfbr.transform.localPosition = new Vector3(resultScale.x, -resultScale.y, -resultScale.z);
                    cbtl.transform.localPosition = new Vector3(-resultScale.x, resultScale.y, resultScale.z);
                    cbtr.transform.localPosition = new Vector3(-resultScale.x, resultScale.y, -resultScale.z);
                    cbbl.transform.localPosition = new Vector3(-resultScale.x, -resultScale.y, resultScale.z);
                    cbbr.transform.localPosition = new Vector3(-resultScale.x, -resultScale.y, -resultScale.z);


                    //EDGES
                    GameObject eft = cube1.transform.Find("BevelledEdgeFrontTop").gameObject;
                    GameObject efb = cube1.transform.Find("BevelledEdgeFrontBottom").gameObject;
                    GameObject efl = cube1.transform.Find("BevelledEdgeFrontLeft").gameObject;
                    GameObject efr = cube1.transform.Find("BevelledEdgeFrontRight").gameObject;
                    GameObject elt = cube1.transform.Find("BevelledEdgeLeftTop").gameObject;
                    GameObject elb = cube1.transform.Find("BevelledEdgeLeftBottom").gameObject;
                    GameObject ert = cube1.transform.Find("BevelledEdgeRightTop").gameObject;
                    GameObject erb = cube1.transform.Find("BevelledEdgeRightBottom").gameObject;
                    GameObject ebt = cube1.transform.Find("BevelledEdgeBackTop").gameObject;
                    GameObject ebb = cube1.transform.Find("BevelledEdgeBackBottom").gameObject;
                    GameObject ebl = cube1.transform.Find("BevelledEdgeBackLeft").gameObject;
                    GameObject ebr = cube1.transform.Find("BevelledEdgeBackRight").gameObject;

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

                    eft.transform.localScale = new Vector3(1f, 1f, (newScale.z + (resultScale.z * extraScale)) * theScale);
                    efb.transform.localScale = new Vector3(1f, 1f, (newScale.z + (resultScale.z * extraScale)) * theScale);
                    efl.transform.localScale = new Vector3(1f, (newScale.y + (resultScale.y * extraScale)) * theScale, 1f);
                    efr.transform.localScale = new Vector3(1f, (newScale.y + (resultScale.y * extraScale)) * theScale, 1f);
                    elt.transform.localScale = new Vector3((newScale.x + (resultScale.x * extraScale)) * theScale, 1f, 1f);
                    elb.transform.localScale = new Vector3((newScale.x + (resultScale.x * extraScale)) * theScale, 1f, 1f);
                    ert.transform.localScale = new Vector3((newScale.x + (resultScale.x * extraScale)) * theScale, 1f, 1f);
                    erb.transform.localScale = new Vector3((newScale.x + (resultScale.x * extraScale)) * theScale, 1f, 1f);
                    ebt.transform.localScale = new Vector3(1f, 1f, (newScale.z + (resultScale.z * extraScale)) * theScale);
                    ebb.transform.localScale = new Vector3(1f, 1f, (newScale.z + (resultScale.z * extraScale)) * theScale);
                    ebl.transform.localScale = new Vector3(1f, (newScale.y + (resultScale.y * extraScale)) * theScale, 1f);
                    ebr.transform.localScale = new Vector3(1f, (newScale.y + (resultScale.y * extraScale)) * theScale, 1f);
                    

                    //FACES
                    GameObject ff = cube1.transform.Find("BevelledFaceFront").gameObject;
                    GameObject fb = cube1.transform.Find("BevelledFaceBack").gameObject;
                    GameObject fl = cube1.transform.Find("BevelledFaceLeft").gameObject;
                    GameObject fr = cube1.transform.Find("BevelledFaceRight").gameObject;
                    GameObject ft = cube1.transform.Find("BevelledFaceTop").gameObject;
                    GameObject fbt = cube1.transform.Find("BevelledFaceBottom").gameObject;

                    ff.transform.localScale = new Vector3(newScale.x, newScale.y + (resultScale.y * extraScale), newScale.z + (resultScale.z * extraScale)) * theScale;
                    fb.transform.localScale = new Vector3(newScale.x, newScale.y + (resultScale.y * extraScale), newScale.z + (resultScale.z * extraScale)) * theScale;
                    fl.transform.localScale = new Vector3(newScale.x + (resultScale.x * extraScale), newScale.y + (resultScale.y * extraScale), newScale.z) * theScale;
                    fr.transform.localScale = new Vector3(newScale.x + (resultScale.x * extraScale), newScale.y + (resultScale.y * extraScale), newScale.z) * theScale;
                    ft.transform.localScale = new Vector3(newScale.x + (resultScale.x * extraScale), newScale.y, newScale.z + (resultScale.z * extraScale)) * theScale;
                    fbt.transform.localScale = new Vector3(newScale.x + (resultScale.x * extraScale), newScale.y, newScale.z + (resultScale.z * extraScale)) * theScale;
                }
                t.transform.localScale *= 2f;

                t.transform.localRotation = initRot;
                cube1.transform.localRotation = t.transform.localRotation;
            }
            else
                Debug.Log(t.gameObject.name);
        }
    }

    public void MergeBlocks()
    {
        foreach (Transform t in this.transform)
        {
            GameObject cb = t.GetChild(0).gameObject;
            cb.transform.SetParent(t.parent);

            cb.transform.localScale = t.transform.localScale;
        }
    }

    public void CleanUp()
    {
        foreach (Transform t in this.transform)
        {
            Vector3 bc = t.GetComponent<BoxCollider>().size;
            Vector3 ls = t.transform.localScale;
            t.transform.localScale = new Vector3(Mathf.Abs(ls.x * bc.x), Mathf.Abs(ls.y * bc.y), Mathf.Abs(ls.z * bc.z));
            t.GetComponent<BoxCollider>().size = Vector3.one;
        }
    }

    public void Rotate()
    {
        foreach (Transform t in this.transform)
        {
            if (t.localEulerAngles.x == 0f && t.localEulerAngles.y == 0f && t.localEulerAngles.z == 0f) { }
            else
            {
                Vector3 newScale = t.localRotation * t.localScale;
                t.localEulerAngles = Vector3.zero;
                t.localScale = newScale;
            }

            Vector3 ls = t.transform.localScale;
            t.transform.localScale = new Vector3(Mathf.Abs(ls.x), Mathf.Abs(ls.y), Mathf.Abs(ls.z));
        }
    }

    public void AdjustScales()
    {
        foreach (Transform t in this.transform)
        {
            t.localScale *= 2f;
            t.GetComponent<BoxCollider>().size /= 2f;
        }
    }

    public void FinalClean()
    {
        foreach (Transform t in this.transform)
        {
            if (t.name.Contains("mall"))
            {
                GameObject cube = PrefabUtility.InstantiatePrefab(newsmallPrefab as GameObject) as GameObject;
                cube.transform.SetParent(newParent.transform);
                cube.transform.position = t.transform.position;
                cube.transform.localScale = t.transform.localScale;
                cube.transform.localRotation = t.transform.localRotation;

                cube.GetComponentInChildren<MeshFilter>().sharedMesh = t.GetComponent<MeshFilter>().sharedMesh;
                cube.GetComponentInChildren<MeshRenderer>().sharedMaterials = t.GetComponent<MeshRenderer>().sharedMaterials;
                cube.GetComponentInChildren<BoxCollider>().size = t.GetComponent<BoxCollider>().size;
            }
            else
            {
                GameObject cube = PrefabUtility.InstantiatePrefab(newprefab as GameObject) as GameObject;
                cube.transform.SetParent(newParent.transform);
                cube.transform.position = t.transform.position;
                cube.transform.localScale = t.transform.localScale;
                cube.transform.localRotation = t.transform.localRotation;

                cube.GetComponentInChildren<MeshFilter>().sharedMesh = t.GetComponent<MeshFilter>().sharedMesh;
                cube.GetComponentInChildren<MeshRenderer>().sharedMaterials = t.GetComponent<MeshRenderer>().sharedMaterials;
                cube.GetComponentInChildren<BoxCollider>().size = t.GetComponent<BoxCollider>().size;
            }
        }
    }
#endif
}

#if UNITY_EDITOR
[CustomEditor(typeof(ReplaceBlocks))]
public class BlocksEdit : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        ReplaceBlocks myScript = (ReplaceBlocks)target;
        if (GUILayout.Button("Replace"))
        {
            myScript.Replace();
        }
        if (GUILayout.Button("Merge"))
        {
            myScript.MergeBlocks();
        }
        if (GUILayout.Button("Clean Up"))
        {
            myScript.CleanUp();
        }
        if (GUILayout.Button("Rotate"))
        {
            myScript.Rotate();
        }
        if (GUILayout.Button("Scales"))
        {
            myScript.AdjustScales();
        }
        if (GUILayout.Button("Final"))
        {
            myScript.FinalClean();
        }
    }
}
#endif