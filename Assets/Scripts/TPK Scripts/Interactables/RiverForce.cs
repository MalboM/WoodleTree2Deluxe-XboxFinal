using UnityEngine;
using System.Collections;

public class RiverForce : MonoBehaviour {

	public Vector3 forceDirection;
    public bool setToForward;
    public float intensity;
    public bool turnOn;
    TPC tpc;
    Rigidbody rb;
    float multiplier;
    bool inWater;

    private void OnEnable()
    {
        if (this.gameObject.layer != 4)
            this.gameObject.layer = 4;
    }

    /*
    private void FixedUpdate()
    {
        if (inWater)
        {
            multiplier = 1f;
            if (tpc.inLeafSlide)
                multiplier = 0.1f;
            tpc.zeroVelocity = forceDirection * intensity * 0.1f * multiplier;
            rb.AddForce(forceDirection * intensity * multiplier, ForceMode.Acceleration);
        }
    }

    void OnTriggerEnter(Collider other){
        if (turnOn){
            if (other.gameObject.tag == "Player")
            {
                if (tpc == null)
                    tpc = other.gameObject.GetComponent<TPC>();
                if (rb == null)
                    rb = other.gameObject.GetComponent<Rigidbody>();

                tpc.anim.SetTrigger("drag");

                inWater = true;
            }
        }
    }

    void OnTriggerExit(Collider other) {
        if (other.gameObject.tag == "Player")
        {
            inWater = false;
            tpc.zeroVelocity = Vector3.zero;
        }
    }*/
}
