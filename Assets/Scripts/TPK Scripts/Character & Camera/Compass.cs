using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Compass : MonoBehaviour {

    public GameObject arrow;
    public GameObject woodle;
    RectTransform rectT;
    
	void Start () {
        rectT = this.gameObject.GetComponent<RectTransform>();
	}
	
	void Update () {
		if(rectT.localPosition.y < 620f){
            arrow.transform.localEulerAngles = new Vector3(0f, 0f, woodle.transform.localEulerAngles.y-180f);
        }
    }
}
