using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ActivateAtDistanceAndDisableCollider : MonoBehaviour {

    public GameObject object1;
    public GameObject collider;
    public string sceneName;
    // public GameObject object2;
    // public GameObject object3;
    // public GameObject object4;

    void OnTriggerStay(Collider other)
    {
        if (!object1.activeInHierarchy && other.tag == "Player" && other.gameObject.name == "Woodle Character")
        {
            bool hasScene = false;
            foreach(Scene s in SceneManager.GetAllScenes())
            {
                if (s.name == sceneName)
                    hasScene = true;
            }
            if (!hasScene)
            {
                object1.SetActive(true);
                collider.SetActive(false);
            }
            //    object2.SetActive(true);
            //    object3.SetActive(true);
            //   object4.SetActive(true);
        }


    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player" && other.gameObject.name == "Woodle Character")
        {

            object1.SetActive(false);
            //   object2.SetActive(false);
            //   object3.SetActive(false);
            //   object4.SetActive(false);
        }


    }



}
