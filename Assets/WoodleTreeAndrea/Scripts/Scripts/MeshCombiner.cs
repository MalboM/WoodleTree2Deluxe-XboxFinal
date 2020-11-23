#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Linq;

[ExecuteInEditMode]
public class MeshCombiner : MonoBehaviour
{
    public Material forMaterial;
    public string saveInFolder;
    public GameObject cubeRef;

    public void CreateSinglePoly()
    {
        MeshRenderer[] allBevelled = this.transform.GetComponentsInChildren<MeshRenderer>();

        List<MeshFilter> filtersOfSame = new List<MeshFilter>();

        foreach(MeshRenderer mr in allBevelled)
        {
            if(mr.sharedMaterial.name == forMaterial.name + " (Instance)" || mr.sharedMaterial.name == forMaterial.name)
                filtersOfSame.Add(mr.GetComponent<MeshFilter>());
        }

        GameObject newObj = new GameObject("Combined Blocks For " + forMaterial.name);
        newObj.transform.parent = this.transform.parent;

        newObj.gameObject.AddComponent<MeshFilter>();
        MeshRenderer mRender = newObj.gameObject.AddComponent<MeshRenderer>();
        mRender.lightProbeUsage = UnityEngine.Rendering.LightProbeUsage.Off;
        mRender.reflectionProbeUsage = UnityEngine.Rendering.ReflectionProbeUsage.Off;
        mRender.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        mRender.receiveShadows = false;
        mRender.motionVectorGenerationMode = MotionVectorGenerationMode.ForceNoMotion;
        mRender.allowOcclusionWhenDynamic = true;

        CombineInstance[] combine = new CombineInstance[filtersOfSame.Count];

        int i = 0;
        while (i < filtersOfSame.Count)
        {
            if (i == 0)
                mRender.sharedMaterial = forMaterial;
            combine[i].mesh = filtersOfSame[i].sharedMesh;

            Matrix4x4 mfT = filtersOfSame[i].transform.localToWorldMatrix;
            mfT.m03 -= newObj.transform.position.x;
            mfT.m13 -= newObj.transform.position.y;
            mfT.m23 -= newObj.transform.position.z;

            combine[i].transform = mfT;
            i++;
        }


        newObj.transform.GetComponent<MeshFilter>().mesh = new Mesh();
        newObj.transform.GetComponent<MeshFilter>().sharedMesh.CombineMeshes(combine, true, true, false);

        Vector3[] vertices = newObj.transform.GetComponent<MeshFilter>().sharedMesh.vertices;
        for (int v = 0; v < vertices.Length; v++)
        {
            Vector3 curVert = vertices[v];
            for (int j = v + 1; j < vertices.Length; j++)
            {
                if (Vector3.Distance(curVert, vertices[j]) < 0.0005f)
                {
                    vertices[j] = curVert;
                }
            }
        }
        newObj.transform.GetComponent<MeshFilter>().sharedMesh.vertices = vertices;

        AssetDatabase.CreateAsset(newObj.transform.GetComponent<MeshFilter>().sharedMesh, "Assets/Resources/CombinedBlocks/" + saveInFolder +"/" + this.gameObject.scene.name + forMaterial.name +".fbx");
        AssetDatabase.SaveAssets();

        newObj.transform.gameObject.SetActive(true);

        foreach(MeshFilter mf in filtersOfSame)
        {
            if (PrefabUtility.GetPrefabAssetType(mf.gameObject) != PrefabAssetType.NotAPrefab && PrefabUtility.GetPrefabInstanceStatus(mf.gameObject) != PrefabInstanceStatus.NotAPrefab)
                PrefabUtility.UnpackPrefabInstance(mf.gameObject, PrefabUnpackMode.Completely, InteractionMode.AutomatedAction);
            DestroyImmediate(mf.gameObject);
        }
    }

    public void CreateCubes()
    {
        MeshRenderer[] allBevelled = this.transform.GetComponentsInChildren<MeshRenderer>();
        GameObject newParent = new GameObject("Cubes Parent");

        foreach(MeshRenderer mr in allBevelled)
        {
            GameObject newCube = Instantiate(cubeRef, newParent.transform) as GameObject;
            newCube.transform.position = mr.gameObject.transform.position;
            newCube.transform.localScale = new Vector3(mr.transform.lossyScale.x * mr.GetComponent<BoxCollider>().size.x, mr.transform.lossyScale.y * mr.GetComponent<BoxCollider>().size.y, mr.transform.lossyScale.z * mr.GetComponent<BoxCollider>().size.z);
            newCube.GetComponent<MeshRenderer>().sharedMaterial = mr.sharedMaterial;
        }
    }

    public void CombineMesh()
    {
        foreach (Transform t in transform)
        {
            Quaternion initRot = t.transform.localRotation;
            t.transform.localRotation = Quaternion.identity;
            MeshFilter[] meshFilters = new MeshFilter[t.gameObject.GetComponentsInChildren<MeshFilter>().Length];
            meshFilters = t.gameObject.GetComponentsInChildren<MeshFilter>();

            t.gameObject.AddComponent<MeshFilter>();
            MeshRenderer mRender = t.gameObject.AddComponent<MeshRenderer>();
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
                mfT.m03 -= t.transform.position.x;
                mfT.m13 -= t.transform.position.y;
                mfT.m23 -= t.transform.position.z;

                combine[i].transform = mfT;

                //disabling old gameObjects.
                meshFilters[i].gameObject.SetActive(false);

                //Destroy Components Only
            //   DestroyImmediate(meshFilters[i].gameObject.GetComponent<MeshRenderer>());
            //   DestroyImmediate(meshFilters[i]);

                //Destroy old gameObjects
            //    DestroyImmediate(meshFilters[i].gameObject);

                i++;
            }


            t.transform.GetComponent<MeshFilter>().mesh = new Mesh();
            t.transform.GetComponent<MeshFilter>().sharedMesh.CombineMeshes(combine, true, true, false);

            Vector3[] vertices = t.transform.GetComponent<MeshFilter>().sharedMesh.vertices;
            for (int v = 0; v < vertices.Length; v++)
            {
                Vector3 curVert = vertices[v];
                for (int j = v + 1; j < vertices.Length; j++)
                {
                    if (Vector3.Distance(curVert, vertices[j]) < 0.0005f)
                    {
                        vertices[j] = curVert;
                    }
                }
            }
            t.transform.GetComponent<MeshFilter>().sharedMesh.vertices = vertices;

            t.transform.gameObject.SetActive(true);

            t.transform.localRotation = initRot;
        }
    }

    public void UncombineMesh() {
        foreach(Transform child in transform)
        {
            DestroyImmediate(child.GetChild(0).gameObject.GetComponent<MeshRenderer>());
            DestroyImmediate(child.GetChild(0).gameObject.GetComponent<MeshFilter>());
            foreach (Transform t in child.GetChild(0)) {
                if (!t.gameObject.activeInHierarchy)
                    t.gameObject.SetActive(true);
            }
        }
    }

    public void FixBlocks()
    {
        foreach (Transform child in transform)
        {
            child.Find("Bevelled").gameObject.GetComponent<MeshFilter>().sharedMesh = child.gameObject.GetComponent<MeshFilter>().sharedMesh;
            child.Find("Bevelled").gameObject.GetComponent<MeshRenderer>().sharedMaterial = child.gameObject.GetComponent<MeshRenderer>().sharedMaterial;
            DestroyImmediate(child.gameObject.GetComponent<MeshRenderer>());
            DestroyImmediate(child.gameObject.GetComponent<MeshFilter>());
        }
    }
}

[CustomEditor(typeof(MeshCombiner))]
public class CombineBlocks : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        MeshCombiner myScript = (MeshCombiner)target;
        if (GUILayout.Button("Combine Segmented Blocks"))
        {
            myScript.CombineMesh();
        }
        if (GUILayout.Button("Undo"))
        {
            myScript.UncombineMesh();
        }
        if (GUILayout.Button("Fix"))
        {
            myScript.FixBlocks();
        }
        if (GUILayout.Button("Create Low Poly For Material"))
        {
            myScript.CreateSinglePoly();
        }
        if (GUILayout.Button("Create Cube Version For LowPoly"))
        {
            myScript.CreateCubes();
        }
    }
}
#endif