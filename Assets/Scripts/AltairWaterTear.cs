using UnityEngine;
using System.Collections;

public class AltairWaterTear : MonoBehaviour {

    // public Transform vase;

    public GameObject vase;
    public bool positioned;
    public string vasetoposition;
    public GameObject spherecollision;
    public GameObject watertear;

    void Start()
    {
        vase = GameObject.Find(vasetoposition);
    }

    void OnTriggerEnter(Collider other)
    {

        
        if (other.tag == "Altair")
        {

           
            positioned = true;

            this.GetComponent<SmoothFollowWoodle>().enabled = false;
            this.GetComponent<WaterTearCatch>().enabled = false;

        }


    }

    void Update()
    {

        if (positioned == true)
        {
           
            this.gameObject.SetActive(false);
           

            watertear = vase.transform.GetChild(0).gameObject;
            watertear.SetActive(true);
        }
    }

}
