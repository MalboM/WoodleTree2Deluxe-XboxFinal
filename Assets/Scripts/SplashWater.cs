using UnityEngine;
using System.Collections;

public class SplashWater : MonoBehaviour
{

    public GameObject splashPrefab;
    public bool enterwater;

    public Vector3 positionsplash;

    GameObject[] splashes;
    
    void Start()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Shallow")
        {
            positionsplash = new Vector3(transform.position.x, transform.position.y - 0.7f, transform.position.z);
            Instantiate(splashPrefab, positionsplash, Quaternion.Euler(0, 0, 0));
            
        }
    }

}