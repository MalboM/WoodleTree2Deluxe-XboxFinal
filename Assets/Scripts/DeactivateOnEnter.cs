using UnityEngine;
using System.Collections;

public class DeactivateOnEnter : MonoBehaviour {

    public GameObject object1;


    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && other.gameObject.name == "Woodle Character")
        {

            // object1.SetActive(false);
            StartCoroutine(deactivatecollider());
        }


    }

    IEnumerator deactivatecollider()
    {

        yield return new WaitForSeconds(8.0f);
        object1.SetActive(false);
    }
}
