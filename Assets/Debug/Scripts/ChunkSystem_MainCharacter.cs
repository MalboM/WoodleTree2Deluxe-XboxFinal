using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkSystem_MainCharacter : MonoBehaviour
{
    [SerializeField] float speed;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
    //    Vector3 direction = transform.forward * Time.deltaTime * speed;
    //    transform.position += direction;
    }

    void OnTriggerEnter(Collider other)
    {
        ChunkManager cm = other.GetComponent<ChunkManager>();
        if (cm != null)
            cm.Activate();
    }

    void OnTriggerExit(Collider other)
    {
        ChunkManager cm = other.GetComponent<ChunkManager>();
        if (cm != null)
            cm.Deactivate();
    }
}
