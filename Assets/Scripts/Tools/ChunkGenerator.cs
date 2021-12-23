using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
public class ChunkGenerator : MonoBehaviour {

    public GameObject[] parentObjects;

    public float sizeOfChunk = 30f;

    public bool excludeLargeBlocks;
    public float largestSizeAllowed;
    public bool dontParentToToactivate;

    float xMin;
    float xMax;
    float yMin;
    float yMax;
    float zMin;
    float zMax;

    float xPos;
    float yPos;
    float zPos;

#if UNITY_EDITOR
    public void ChunkItUp()
    {

        //FINDING BOUNDS
        xMin = 0;
        xMax = 0;
        yMin = 0;
        yMax = 0;
        zMin = 0;
        zMax = 0;

        foreach (GameObject g in parentObjects)
        {
            if (g != null)
            {
                foreach (Transform t in g.transform.GetComponentsInChildren<Transform>(true))
                {
                    if (t.transform.parent == g.transform)
                    {

                        if (t.transform.position.x < xMin)
                            xMin = t.transform.position.x;

                        if (t.transform.position.x > xMax)
                            xMax = t.transform.position.x;

                        if (t.transform.position.y < yMin)
                            yMin = t.transform.position.y;

                        if (t.transform.position.y > yMax)
                            yMax = t.transform.position.y;

                        if (t.transform.position.z < zMin)
                            zMin = t.transform.position.z;

                        if (t.transform.position.z > zMax)
                            zMax = t.transform.position.z;
                    }
                }
            }
        }


        //ALLOCATE OBJECTS TO CHUNKS
        GameObject chunksParent = new GameObject("Chunks Distance Checker");
        
        ChunkDistanceChecker cdc = chunksParent.AddComponent<ChunkDistanceChecker>();

        float halfSize = sizeOfChunk / 2f;

        for (xPos = xMin; xPos < (xMax + sizeOfChunk); xPos += sizeOfChunk)
        {
            for (yPos = yMin; yPos < (yMax + sizeOfChunk); yPos += sizeOfChunk)
            {
                for (zPos = zMin; zPos < (zMax + sizeOfChunk); zPos += sizeOfChunk)
                {
                    ChunkDistanceChecker.ChunkChild curChunkChild = new ChunkDistanceChecker.ChunkChild();

                    curChunkChild.chunkPos = new Vector3(xPos, yPos, zPos);

                    foreach (GameObject g in parentObjects)
                    {
                        if (g != null)
                        {
                            foreach (Transform t in g.transform.GetComponentsInChildren<Transform>(true))
                            {
                                if (t.transform.parent == g.transform)
                                {
                                    if (t.transform.position.x >= (xPos - halfSize) && t.transform.position.x < (xPos + halfSize)
                                        && t.transform.position.y >= (yPos - halfSize) && t.transform.position.y < (yPos + halfSize)
                                        && t.transform.position.z >= (zPos - halfSize) && t.transform.position.z < (zPos + halfSize))
                                    {
                                        if (excludeLargeBlocks)
                                        {
                                            if(t.GetComponentInChildren<BoxCollider>() != null)
                                            {
                                                BoxCollider col = t.GetComponentInChildren<BoxCollider>();
                                                if (col.size.x <= largestSizeAllowed && col.size.y <= largestSizeAllowed && col.size.z <= largestSizeAllowed)
                                                    curChunkChild.chunkObjects.Add(t.gameObject);
                                            }
                                        }else
                                            curChunkChild.chunkObjects.Add(t.gameObject);
                                    }
                                }
                            }
                        }
                    }

                    if (curChunkChild.chunkObjects.Count > 0)
                        cdc.chunkChildren.Add(curChunkChild);
                }
            }
        }

        if(!dontParentToToactivate)
            chunksParent.transform.SetParent(GameObject.FindWithTag("ToActivate").transform);
    }

    public void ActiveToggle(bool activate)
    {
        foreach (GameObject g in parentObjects)
        {
            if (g != null)
            {
                foreach (Transform t in g.transform.GetComponentsInChildren<Transform>(true))
                {
                    if ((activate || t.gameObject.tag != "OneWay") && t.transform.parent == g.transform)
                        t.gameObject.SetActive(activate);
                }
            }
        }
    }

#endif

}

#if UNITY_EDITOR
[CustomEditor(typeof(ChunkGenerator))]
public class ChunkGeneratorEdit : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        ChunkGenerator myScript = (ChunkGenerator)target;
        if (GUILayout.Button("Create Chunks"))
        {
            myScript.ChunkItUp();
        }
        if (GUILayout.Button("DEACTIVATE all objects"))
        {
            myScript.ActiveToggle(false);
        }
        if (GUILayout.Button("ACTIVATE all objects"))
        {
            myScript.ActiveToggle(true);
        }
    }
}
#endif