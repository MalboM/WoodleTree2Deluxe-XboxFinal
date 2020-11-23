using UnityEngine;
using System.Collections;

public class DarkBossAttacking : MonoBehaviour {


    public Animator boss;

	// Use this for initialization
	void Start () {
	   
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {

            boss.SetBool("Attack", true);


        }
    }


    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {

            boss.SetBool("Attack", false);


        }
    }


}
