#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class CombineSelected : MonoBehaviour
{
    [Header("COMBINE")]
    public string nameOfNewParent;
    public GameObject[] objectsToCombine;

    [Header("UN-COMBINE")]
    public GameObject parentOfCombinedObjs;

    public void Combine()
    {
        if (nameOfNewParent == null || nameOfNewParent == "")
            nameOfNewParent = "CombinedObjectsParent";

        GameObject newParent = new GameObject(nameOfNewParent);
        newParent.transform.SetParent(objectsToCombine[0].transform.parent);

    //    Vector3 newParentPos = objectsToCombine[0].transform.position;
        foreach (GameObject g in objectsToCombine)
        {
        //    newParentPos = Vector3.Lerp(newParentPos, g.transform.position, 0.5f);
            g.transform.SetParent(newParent.transform);
        }

        MeshFilter[] meshFilters = new MeshFilter[newParent.gameObject.GetComponentsInChildren<MeshFilter>().Length];
        meshFilters = newParent.gameObject.GetComponentsInChildren<MeshFilter>();

        newParent.gameObject.AddComponent<MeshFilter>();
        MeshRenderer mRender = newParent.gameObject.AddComponent<MeshRenderer>();
        mRender.lightProbeUsage = UnityEngine.Rendering.LightProbeUsage.Off;
        mRender.reflectionProbeUsage = UnityEngine.Rendering.ReflectionProbeUsage.Off;
        mRender.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        mRender.receiveShadows = false;
        mRender.motionVectorGenerationMode = MotionVectorGenerationMode.ForceNoMotion;
        mRender.allowOcclusionWhenDynamic = true;

        CombineInstance[] combine = new CombineInstance[meshFilters.Length];

        int i = 0;
        while (i < meshFilters.Length)
        {
            if (i == 0)
                mRender.sharedMaterial = meshFilters[i].gameObject.GetComponent<MeshRenderer>().sharedMaterial;
            combine[i].mesh = meshFilters[i].sharedMesh;

            Matrix4x4 mfT = meshFilters[i].transform.localToWorldMatrix;
            //instead of pivoting every MeshCombiner in (0,0,0) world,
            //offset each meshFilter by parent's position, thus keeping the pivot where is now
            mfT.m03 -= newParent.transform.position.x;
            mfT.m13 -= newParent.transform.position.y;
            mfT.m23 -= newParent.transform.position.z;

            combine[i].transform = mfT;

            //Disabling old gameObjects.
            //  meshFilters[i].gameObject.SetActive(false);

            //Destroy Components Only
            //   DestroyImmediate(meshFilters[i].gameObject.GetComponent<MeshRenderer>());
            //   DestroyImmediate(meshFilters[i]);

            //Destroy old gameObjects
            //    DestroyImmediate(meshFilters[i].gameObject);

            i++;
        }


        newParent.transform.GetComponent<MeshFilter>().mesh = new Mesh();
        newParent.transform.GetComponent<MeshFilter>().sharedMesh.CombineMeshes(combine, true, true, false);

        newParent.gameObject.isStatic = true;

        /*
        foreach (Transform t in newParent.transform) {
            if (t.gameObject != newParent)
                t.gameObject.SetActive(false);
        }
        */

        foreach (MeshRenderer mr in newParent.gameObject.GetComponentsInChildren<MeshRenderer>())
        {
            if(mr.gameObject != newParent)
                mr.enabled = false;
        }
    }

    public void UnCombine() {
        if (parentOfCombinedObjs == null)
            Debug.LogError("Please drag in the parent of the combined objects you wish to un-combine into 'Parent Of Combined Objs'.");
        else
        {
            foreach (MeshRenderer mr in parentOfCombinedObjs.gameObject.GetComponentsInChildren<MeshRenderer>())
                mr.enabled = true;
            
            
            for (int x = parentOfCombinedObjs.transform.childCount-1; x >= 0 ; x--)
            {
                parentOfCombinedObjs.transform.GetChild(x).gameObject.SetActive(true);
                parentOfCombinedObjs.transform.GetChild(x).SetParent(parentOfCombinedObjs.transform.parent);
            }

            DestroyImmediate(parentOfCombinedObjs);
        }
    }
}

[CustomEditor(typeof(CombineSelected))]
public class CombineSelectedEdit : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        CombineSelected myScript = (CombineSelected)target;
        if (GUILayout.Button("Combine"))
        {
            myScript.Combine();
        }
        if (GUILayout.Button("Un-Combine"))
        {
            myScript.UnCombine();
        }
    }
}
#endif
