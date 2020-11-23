using UnityEngine;
using System.Collections;

public class ActivateAtDistanceStay : MonoBehaviour
{

    public GameObject object1;


    void Start()
    {
        if (object1 != null)
        {
            StartCoroutine("DeactivateIt");
        }
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && other.gameObject.name == "Woodle Character")
        {
            if (object1 != null)
            {
                StopCoroutine("DeactivateIt");
                StartCoroutine("ActivateIt");
            }

        }


    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player" && other.gameObject.name == "Woodle Character")
        {
            if (object1 != null)
            {
                StopCoroutine("ActivateIt");
                StartCoroutine("DeactivateIt");
            }

        }
    }

    IEnumerator ActivateIt()
    {
        foreach (Transform t in object1.transform)
        {
            if (t.gameObject.name != "CameraActions")
            {
                foreach (Transform tr in t)
                {
                    if (tr.gameObject != t.gameObject)
                    {
                        tr.gameObject.SetActive(true);
                        yield return null;
                    }
                }
            }
        }
    }

    IEnumerator DeactivateIt()
    {
        foreach (Transform t in object1.transform)
        {
            if (t.gameObject.name != "CameraActions")
            {
                foreach (Transform tr in t)
                {
                    if (tr.gameObject != t.gameObject)
                    {
                        tr.gameObject.SetActive(false);
                        yield return null;
                    }
                }
            }
        }
    }

}
