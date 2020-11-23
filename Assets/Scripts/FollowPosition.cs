using UnityEngine;
using System.Collections;

public class FollowPosition : MonoBehaviour {


    public GameObject objecttofollow;

    public float positionx;
    public float positiony;
    public float positionz;
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        positionx = objecttofollow.transform.position.x;
        positiony = objecttofollow.transform.position.y;
        positionz = objecttofollow.transform.position.z;

        transform.position = new Vector3(positionx, positiony, positionz);
    }
}
