using UnityEngine;
using System.Collections;

public class AnimatedObject : MonoBehaviour {

	public float speed = 1;

	private Quaternion _startRotation;

	// Use this for initialization
	void Start () {

		_startRotation = gameObject.transform.rotation;
	}
	
	// Update is called once per frame
	void Update () {
	
//		if(demo.RotationEnalbed){
			Vector3 v = gameObject.transform.localRotation.eulerAngles;
			v.y += speed;
			_startRotation.eulerAngles=v;
			gameObject.transform.localRotation =_startRotation;
//		}
	}
}
