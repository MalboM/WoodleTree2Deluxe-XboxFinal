using UnityEngine;
using System.Collections;

public class ChangeWoodlePositionOnTrigger : MonoBehaviour {


    public GameObject objectposition;
    GameObject woodle;


    public float positionx;
    public float positiony;
    public float positionz;

    public bool enterarea;
 
    void Start() {
    }

    void OnTriggerEnter(Collider col) {
		if (col.gameObject.tag == "Player"){
			if (enterarea == false) {
				enterarea = true;
				woodle = col.gameObject;
				StartCoroutine (teleport ());
			}
        }
    }


    IEnumerator teleport() {
        yield return new WaitForSeconds(3);
		woodle.transform.position = objectposition.transform.position;
    }
}
