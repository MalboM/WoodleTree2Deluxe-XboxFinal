using UnityEngine;
using System.Collections;

public class RedBallToHIt : MonoBehaviour
{

    public Animator redball;
    public bool hit;

    public GameObject watercollider;
    public GameObject watermesh;


    void OnTriggerEnter(Collider other)
    {
        if (!hit && other.GetComponent<Collider>().gameObject.layer == LayerMask.NameToLayer("WaterRelease"))
        {
            this.gameObject.GetComponent<AudioSource>().Play();
            //this.transform.localScale -= new Vector3(0.1F, 0, 0);
            hit = true;
            redball.SetBool("Activate", true);

           // watercollider.SetActive(false);
           // watermesh.SetActive(false);

            StartCoroutine(destroiedball());
        }
    }


    IEnumerator destroiedball()
    {
        yield return new WaitForSeconds(3);
        this.gameObject.transform.localScale = Vector3.one * 0.00001f;
    }
}
