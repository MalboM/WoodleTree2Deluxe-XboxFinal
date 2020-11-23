using UnityEngine;
using System.Collections;

public class ActivateAtDistanceNewDouble2 : MonoBehaviour
{

    GameObject chara;
    public GameObject obj;
    public GameObject obj2;
    public float distance;
	public float timePerCheck;
    float dist;
	float checker;

    void Start() {
        foreach (GameObject g in GameObject.FindGameObjectsWithTag("Player"))
        {
            if (g.name == "Woodle Character")
                chara = g;
        }
        obj.SetActive(false);
        obj2.SetActive(false);
        if (timePerCheck == 0f)
			timePerCheck = 2f;
		checker = timePerCheck + 1f;
    }

    void Update() {
		checker += Time.deltaTime;
		if (checker > timePerCheck) {
			dist = Vector3.Distance (chara.transform.position, this.transform.position);
			if (obj.activeInHierarchy == true && dist >= distance) { 
				obj.SetActive (false);
                obj2.SetActive(false);

            }
            if (obj.activeInHierarchy == false && dist < distance) { 
				obj.SetActive (true);
                obj2.SetActive(true);
            }
            checker = 0f;
		}
    }
}
