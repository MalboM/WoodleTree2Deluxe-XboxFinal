using UnityEngine;
using System.Collections;

public class WaterTearCatch : MonoBehaviour {

    public Transform woodle;
    public bool catched;
    public GameObject soundNearby;

    public float distance;

    void Start()
    {
        
            woodle = GameObject.Find("Woodle Character/WoodleSon6").transform;
    }
	
	void Update () {
	    if (catched == true)
        {

            distance = Vector3.Distance(woodle.position, this.transform.position);

            if (distance >=10 )
                this.GetComponent<SmoothFollowWoodle>().positionDamping = 30.1f;

            else {
                this.GetComponent<SmoothFollowWoodle>().positionDamping = 3.2f;
            }
        }

        
    }


 
    void OnTriggerEnter(Collider other)
    {
		if (other.gameObject.tag == "Player")

        {
            if (catched == false) { 
                catched = true;
                AudioSource audio = GetComponent<AudioSource>();

                audio.Play();
                soundNearby.GetComponent<AudioSource>().Stop();
                this.GetComponent<SmoothFollowWoodle>().enabled = true;
            }
        }

    }
}
