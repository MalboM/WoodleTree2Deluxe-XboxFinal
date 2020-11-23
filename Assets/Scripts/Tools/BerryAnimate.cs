using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BerryAnimate : MonoBehaviour {

    Transform[] allBerries;
    float newYPos;
    float yPosIterator;
    
    public float movementSpeed;
    public float movementHeight;
    public float rotationSpeed;
    float rotateSpeed;
    /*
	void Start () {
        allBerries = new Transform[this.transform.childCount];
        for (int i = 0; i < this.transform.childCount; i++)
            allBerries[i] = this.transform.GetChild(i);
	}
	
	void Update () {
        yPosIterator += (movementSpeed * Time.deltaTime);
        newYPos = (Mathf.Sin(yPosIterator) * movementHeight) + movementHeight;

        rotateSpeed = rotationSpeed * Time.deltaTime;

		foreach(Transform t in allBerries)  
        {
            if (t.GetChild(1).GetChild(0).gameObject.activeInHierarchy)
            {
                t.GetChild(1).localPosition = new Vector3(0f, newYPos, 0f);
                t.GetChild(1).Rotate(Vector3.up * rotateSpeed, Space.Self);
            }
        }
	}*/
}
