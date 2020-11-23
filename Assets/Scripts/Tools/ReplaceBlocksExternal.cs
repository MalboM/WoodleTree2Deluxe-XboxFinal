#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class ReplaceBlocksExternal : MonoBehaviour {
    
    public GameObject newParent;
    GameObject cube1;

    public GameObject block05x05x05;
    public GameObject block1x05x05;
    public GameObject block1x1x05;
    public GameObject block1x1x1;
    public GameObject block2x1x1;
    public GameObject block2x2x1;
    public GameObject block2x2x2;
    public GameObject block3x1x1;
    public GameObject block3x3x1;
    public GameObject block3x3x3;
    public GameObject block5x2x2;
    public GameObject block5x5x2;
    public GameObject block5x5x5;
    public GameObject block10x4x4;
    public GameObject block10x10x4;
    public GameObject block10x10x10;
    public GameObject block15x6x6;
    public GameObject block15x15x6;
    public GameObject block15x15x15;
    public GameObject block20x8x8;
    public GameObject block20x20x8;
    public GameObject block20x20x20;
    public GameObject block30x12x12;
    public GameObject block30x30x12;
    public GameObject block30x30x30;
    public GameObject block50x20x20;
    public GameObject block50x50x20;
    public GameObject block50x50x50;

    int whichValueIsHighest;
    int whichValueSecondHighest;
    int whichValueThirdHighest;

    enum BlockCase { block05, block1, block2, block3, block5, block10, block15, block20, block30, block50 }
    BlockCase blockCase;
    enum BlockShape { length, flat, cube }
    BlockShape blockShape;

    float bigScale;
    float smallScale;

    // Use this for initialization
    void Start()
    {

    }

    //S: 2.461947. M: 15.884. L: 216.7947.

    public void NewReplace()
    {
        foreach (Transform t in this.transform)
        {
            if (t.gameObject.GetComponent<BoxCollider>() != null)
            {
                t.gameObject.GetComponent<BoxCollider>().size = t.localScale;
                t.localScale = Vector3.one;
            }
        }
        Replace();
    }

    public void Replace()
    {
        foreach (Transform t in this.transform)
        {
            if (t.gameObject.GetComponent<BoxCollider>() != null)
            {
                Vector3 newScale = t.gameObject.GetComponent<BoxCollider>().size/2f;

                //FIGURE OUT SIZE HERE
                float[] values = new float[3];
                values[0] = newScale.x;
                values[1] = newScale.y;
                values[2] = newScale.z;
                float maxValue = newScale.y;
                whichValueIsHighest = 1;
                whichValueSecondHighest = 0;
                whichValueThirdHighest = 2;
                if (newScale.x > newScale.y)
                {
                    maxValue = newScale.x;
                    whichValueIsHighest = 0;
                    whichValueSecondHighest = 1;
                    whichValueThirdHighest = 2;
                    if (newScale.z > newScale.x)
                    {
                        maxValue = newScale.z;
                        whichValueIsHighest = 2;
                        whichValueSecondHighest = 0;
                        whichValueThirdHighest = 1;
                    }
                }
               

                if (values[whichValueSecondHighest] < values[whichValueThirdHighest]) {
                    int temp = whichValueThirdHighest;
                    whichValueThirdHighest = whichValueSecondHighest;
                    whichValueSecondHighest = temp;
                }

                Object obj;

                if (values[whichValueIsHighest] <= 0.5f)
                {
                    blockCase = BlockCase.block05;
                    blockShape = BlockShape.cube;
                    obj = block05x05x05;
                    bigScale = 0.5f;
                    smallScale = 0.5f;
                }
                else
                {
                    if (values[whichValueIsHighest] <= 1f)
                    {
                        blockCase = BlockCase.block1;
                        GetSize(values, 0.5f);
                        if (blockShape == BlockShape.length)
                        {
                            obj = block1x05x05;
                            bigScale = 1f;
                            smallScale = 0.5f;
                        }
                        else
                        {
                            if (blockShape == BlockShape.flat)
                            {
                                obj = block1x1x05;
                                bigScale = 1f;
                                smallScale = 0.5f;
                            }
                            else
                            {
                                obj = block1x1x1;
                                bigScale = 1f;
                                smallScale = 1f;
                            }
                        }
                    }
                    else
                    {
                        if (values[whichValueIsHighest] <= 2f)
                        {
                            blockCase = BlockCase.block2;
                            GetSize(values, 1f);
                            if (blockShape == BlockShape.length) { 
                                obj = block2x1x1;
                                bigScale = 2f;
                                smallScale = 1f;
                            }
                            else
                            {
                                if (blockShape == BlockShape.flat)
                                {
                                    obj = block2x2x1;
                                    bigScale = 2f;
                                    smallScale = 1f;
                                }
                                else
                                {
                                    obj = block2x2x2;
                                    bigScale = 2f;
                                    smallScale = 2f;
                                }
                            }
                        }
                        else
                        {
                            if (values[whichValueIsHighest] <= 3f)
                            {
                                blockCase = BlockCase.block3;
                                GetSize(values, 2f);
                                if (blockShape == BlockShape.length)
                                {
                                    obj = block3x1x1;
                                    bigScale = 3f;
                                    smallScale = 1f;
                                }
                                else
                                {
                                    if (blockShape == BlockShape.flat)
                                    {
                                        obj = block3x3x1;
                                        bigScale = 3f;
                                        smallScale = 1f;
                                    }
                                    else
                                    {
                                        obj = block3x3x3;
                                        bigScale = 3f;
                                        smallScale = 3f;
                                    }
                                }
                            }
                            else
                            {
                                if (values[whichValueIsHighest] <= 5f)
                                {
                                    blockCase = BlockCase.block5;
                                    GetSize(values, 3f);
                                    if (blockShape == BlockShape.length)
                                    {
                                        obj = block5x2x2;
                                        bigScale = 5f;
                                        smallScale = 2f;
                                    }
                                    else
                                    {
                                        if (blockShape == BlockShape.flat)
                                        {
                                            obj = block5x5x2;
                                            bigScale = 5f;
                                            smallScale = 2f;
                                        }
                                        else
                                        {
                                            obj = block5x5x5;
                                            bigScale = 5f;
                                            smallScale = 5f;
                                        }
                                    }
                                }
                                else
                                {
                                    if (values[whichValueIsHighest] <= 10f)
                                    {
                                        blockCase = BlockCase.block10;
                                        GetSize(values, 4f);
                                        if (blockShape == BlockShape.length)
                                        {
                                            obj = block10x4x4;
                                            bigScale = 10f;
                                            smallScale = 4f;
                                        }
                                        else
                                        {
                                            if (blockShape == BlockShape.flat)
                                            {
                                                obj = block10x10x4;
                                                bigScale = 10f;
                                                smallScale = 4f;
                                            }
                                            else
                                            {
                                                obj = block10x10x10;
                                                bigScale = 10f;
                                                smallScale = 10f;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (values[whichValueIsHighest] <= 15f)
                                        {
                                            blockCase = BlockCase.block15;
                                            GetSize(values, 6f);
                                            if (blockShape == BlockShape.length)
                                            {
                                                obj = block15x6x6;
                                                bigScale = 15f;
                                                smallScale = 6f;
                                            }
                                            else
                                            {
                                                if (blockShape == BlockShape.flat)
                                                {
                                                    obj = block15x15x6;
                                                    bigScale = 15f;
                                                    smallScale = 6f;
                                                }
                                                else
                                                {
                                                    obj = block15x15x15;
                                                    bigScale = 15f;
                                                    smallScale = 15f;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (values[whichValueIsHighest] <= 20f)
                                            {
                                                blockCase = BlockCase.block20;
                                                GetSize(values, 8f);
                                                if (blockShape == BlockShape.length)
                                                {
                                                    obj = block20x8x8;
                                                    bigScale = 20f;
                                                    smallScale = 8f;
                                                }
                                                else
                                                {
                                                    if (blockShape == BlockShape.flat)
                                                    {
                                                        obj = block20x20x8;
                                                        bigScale = 20f;
                                                        smallScale = 8f;
                                                    }
                                                    else
                                                    {
                                                        obj = block20x20x20;
                                                        bigScale = 20f;
                                                        smallScale = 20f;
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                if (values[whichValueIsHighest] <= 30f)
                                                {
                                                    blockCase = BlockCase.block30;
                                                    GetSize(values, 12f);
                                                    if (blockShape == BlockShape.length)
                                                    {
                                                        obj = block30x12x12;
                                                        bigScale = 30f;
                                                        smallScale = 12f;
                                                    }
                                                    else
                                                    {
                                                        if (blockShape == BlockShape.flat)
                                                        {
                                                            obj = block30x30x12;
                                                            bigScale = 30f;
                                                            smallScale = 12f;
                                                        }
                                                        else
                                                        {
                                                            obj = block30x30x30;
                                                            bigScale = 30f;
                                                            smallScale = 30f;
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    blockCase = BlockCase.block50;
                                                    GetSize(values, 20f * (values[whichValueIsHighest]/50f));
                                                    if (blockShape == BlockShape.length)
                                                    {
                                                        obj = block50x20x20;
                                                        bigScale = 50f;
                                                        smallScale = 20f;
                                                    }
                                                    else
                                                    {
                                                        if (blockShape == BlockShape.flat)
                                                        {
                                                            obj = block50x50x20;
                                                            bigScale = 50f;
                                                            smallScale = 20f;
                                                        }
                                                        else
                                                        {
                                                            obj = block50x50x50;
                                                            bigScale = 50f;
                                                            smallScale = 50f;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                GameObject cube1 = PrefabUtility.InstantiatePrefab(obj as GameObject) as GameObject;
                cube1.tag = t.gameObject.tag;
                cube1.transform.SetParent(t);
                cube1.transform.localPosition = Vector3.zero;

                foreach (MeshRenderer mr in cube1.GetComponentsInChildren<MeshRenderer>())
                    mr.sharedMaterial = t.GetChild(0).GetChild(0).gameObject.GetComponent<MeshRenderer>().sharedMaterial;
                cube1.transform.SetParent(newParent.transform);
                
                if(blockShape == BlockShape.length)
                {
                    if (whichValueIsHighest == 1)//y
                    {
                        if(whichValueSecondHighest == 0)//x
                            cube1.transform.localScale = new Vector3(values[whichValueSecondHighest] / smallScale, values[whichValueIsHighest] / bigScale, values[whichValueThirdHighest] / smallScale);
                        else
                            cube1.transform.localScale = new Vector3(values[whichValueThirdHighest] / smallScale, values[whichValueIsHighest] / bigScale, values[whichValueSecondHighest] / smallScale);
                    }
                    else {
                        if (whichValueIsHighest == 0)//x
                        {
                            cube1.transform.localEulerAngles = new Vector3(0f, 0f, 90f);
                            if (whichValueSecondHighest == 1)//y
                                cube1.transform.localScale = new Vector3(values[whichValueSecondHighest] / smallScale, values[whichValueIsHighest] / bigScale, values[whichValueThirdHighest] / smallScale);
                            else
                                cube1.transform.localScale = new Vector3(values[whichValueThirdHighest] / smallScale, values[whichValueIsHighest] / bigScale, values[whichValueSecondHighest] / smallScale);
                        }
                        else
                        {
                            if (whichValueIsHighest == 2)//z
                            {
                                cube1.transform.localEulerAngles = new Vector3(90f, 0f, 0f);
                                if (whichValueSecondHighest == 0)//x
                                    cube1.transform.localScale = new Vector3(values[whichValueSecondHighest] / smallScale, values[whichValueIsHighest] / bigScale, values[whichValueThirdHighest] / smallScale);
                                else
                                    cube1.transform.localScale = new Vector3(values[whichValueThirdHighest] / smallScale, values[whichValueIsHighest] / bigScale, values[whichValueSecondHighest] / smallScale);
                            }
                        }
                    }
                }
                else
                {
                    if (blockShape == BlockShape.flat)
                    {
                        if (whichValueThirdHighest == 0)//x
                        {
                            if(whichValueIsHighest == 1)//y
                                cube1.transform.localScale = new Vector3(values[whichValueThirdHighest] / smallScale, values[whichValueIsHighest] / bigScale, values[whichValueSecondHighest] / bigScale);
                            else
                                cube1.transform.localScale = new Vector3(values[whichValueThirdHighest] / smallScale, values[whichValueSecondHighest] / bigScale, values[whichValueIsHighest] / bigScale);
                        }
                        else
                        {
                            if (whichValueThirdHighest == 1)//y
                            {
                                cube1.transform.localEulerAngles = new Vector3(0f, 0f, 90f);
                                if (whichValueIsHighest == 0)//x
                                    cube1.transform.localScale = new Vector3(values[whichValueThirdHighest] / smallScale, values[whichValueIsHighest] / bigScale, values[whichValueSecondHighest] / bigScale);
                                else
                                    cube1.transform.localScale = new Vector3(values[whichValueThirdHighest] / smallScale, values[whichValueSecondHighest] / bigScale, values[whichValueIsHighest] / bigScale);
                            }
                            else
                            {
                                if (whichValueThirdHighest == 2)//z
                                {
                                    cube1.transform.localEulerAngles = new Vector3(0f, 90f, 0f);
                                    if (whichValueIsHighest == 1)//u
                                        cube1.transform.localScale = new Vector3(values[whichValueThirdHighest] / smallScale, values[whichValueIsHighest] / bigScale, values[whichValueSecondHighest] / bigScale);
                                    else
                                        cube1.transform.localScale = new Vector3(values[whichValueThirdHighest] / smallScale, values[whichValueSecondHighest] / bigScale, values[whichValueIsHighest] / bigScale);
                                }
                            }
                        }
                    }
                    else
                        cube1.transform.localScale = new Vector3(newScale.x / bigScale, newScale.y / bigScale, newScale.z / bigScale);
                }
            }
            else
                Debug.Log(t.gameObject.name);
        }
    }

    void GetSize(float[] vls, float limit)
    {
        blockShape = BlockShape.length;
        if (vls[whichValueSecondHighest] >= limit)
        {
            blockShape = BlockShape.flat;
            if (vls[whichValueThirdHighest] >= limit)
                blockShape = BlockShape.cube;
        }
    }
}


[CustomEditor(typeof(ReplaceBlocksExternal))]
public class BlocksExtEdit : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        ReplaceBlocksExternal myScript = (ReplaceBlocksExternal)target;
        if (GUILayout.Button("Replace"))
        {
            myScript.Replace();
        }
        if (GUILayout.Button("New Replace"))
        {
            myScript.NewReplace();
        }
    }
}
#endif
