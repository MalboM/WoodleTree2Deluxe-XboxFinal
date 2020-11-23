using UnityEngine;
using System.Collections;

public class ActivateElevatorBoss : MonoBehaviour {
	
    public GameObject water1;
    public GameObject water2;
    public GameObject water3;
    public GameObject water4;
    public GameObject water5;
    public GameObject water6;
    public GameObject water7;
    public GameObject water8;

    public GameObject elevator;

    public GameObject watermainfoundtain1;
    public GameObject watermainfoundtain2;

    public GameObject maincamera;
    public GameObject cameracinematic;

    void OnTriggerEnter(Collider other) {
        if (other.tag == "Player" && other.gameObject.name == "Woodle Character")
        {
			if (water1.gameObject.activeInHierarchy && water2.gameObject.activeInHierarchy && water3.gameObject.activeInHierarchy && 
					water4.gameObject.activeInHierarchy && water5.gameObject.activeInHierarchy && water6.gameObject.activeInHierarchy &&
					water7.gameObject.activeInHierarchy && water8.gameObject.activeInHierarchy) {
                elevator.gameObject.GetComponent<Animation>().Play();
                watermainfoundtain1.gameObject.SetActive(true);
                watermainfoundtain2.gameObject.SetActive(true);
                StartCoroutine(camerachange());
            }
        }
    }
		
    IEnumerator camerachange() {
		cameracinematic.GetComponent<Camera>().enabled = true;
        maincamera.GetComponent<Camera>().enabled = false;

        yield return new WaitForSeconds(3);

        cameracinematic.GetComponent<Camera>().enabled = false;
        maincamera.GetComponent<Camera>().enabled = true;
    }
}
