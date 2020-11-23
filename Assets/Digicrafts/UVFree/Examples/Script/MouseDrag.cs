using UnityEngine;
using System.Collections;

public class MouseDrag : MonoBehaviour {

	float xDeg = 0;
	float yDeg = 0;
	Quaternion fromRotation;
	Quaternion toRotation;
	Vector3 mouseLastPosition;

	void OnMouseDown () {
		mouseLastPosition=Input.mousePosition;
		fromRotation = transform.rotation;
	}

	void OnMouseDrag () {		
		Vector3 delta = Input.mousePosition-mouseLastPosition;
		xDeg = delta.x/2;
		yDeg = delta.y/2;
		transform.rotation = fromRotation*Quaternion.Euler(-yDeg,-xDeg,0);
	}

	void OnMouseUp () {
		
	}		

}
