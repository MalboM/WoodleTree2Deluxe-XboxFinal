using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateWithLOD : MonoBehaviour {

    Collider[] colliders;
    MeshRenderer[] rend;
    bool collidersActive;
    Telescope tele;
    float largestSide;

    void Start() { }

	void OnEnable () {
        colliders = this.gameObject.GetComponentsInChildren<Collider>();

     //   if(rend == null || rend.Length == 0)
     //       rend = this.gameObject.GetComponentsInChildren<MeshRenderer>();

        foreach(GameObject g in GameObject.FindGameObjectsWithTag("Player"))
        {
            if (g.name == "Woodle Character")
                tele = g.gameObject.GetComponent<Telescope>();
        }

     //   StartCoroutine(CheckRenderer());
        StartCoroutine(CheckColliders());
    }

    IEnumerator CheckColliders()
    {
        yield return new WaitForSeconds(2f);

        foreach (Collider c in colliders)
        {
            if (c.gameObject.transform.localScale.x > c.gameObject.transform.localScale.y) {
                if (c.gameObject.transform.localScale.x > c.gameObject.transform.localScale.z)
                    largestSide = c.gameObject.transform.localScale.x;
                else
                    largestSide = c.gameObject.transform.localScale.z;
            }
            else
            {
                if (c.gameObject.transform.localScale.y > c.gameObject.transform.localScale.z)
                    largestSide = c.gameObject.transform.localScale.y;
                else
                    largestSide = c.gameObject.transform.localScale.z;
            }
            if (Vector3.Distance(tele.gameObject.transform.position, c.gameObject.transform.position) < (100f * largestSide))
                    c.enabled = true;
                else
                    c.enabled = false;
        }
        StartCoroutine(CheckColliders());
    }

    IEnumerator CheckRenderer() {
        yield return new WaitForSeconds(2f);

        if (tele == null || !tele.usingTelescope)
        {
            foreach (MeshRenderer m in rend)
            {
                if (m.gameObject.GetComponent<Collider>())
                {
                    if(Vector3.Distance(tele.gameObject.transform.position, m.gameObject.transform.position) < 100f)
                        m.gameObject.GetComponent<Collider>().enabled = true;
                    else
                        m.gameObject.GetComponent<Collider>().enabled = false;
                }
            }
        }
        StartCoroutine(CheckRenderer());
    }
}
