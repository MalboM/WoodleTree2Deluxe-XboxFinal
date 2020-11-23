using UnityEngine;
using System.Collections;

public class DeactivateExtraItem : MonoBehaviour {

    public bool activateInstead = false;

    public string playerprefstocheck;
    GameObject firstChild;


	void OnEnable () {
        if (firstChild == null)
        {
            foreach(Transform t in this.GetComponentsInChildren<Transform>(true))
            {
                if(t.transform.parent == this.transform)
                    firstChild = t.gameObject;
            }
        }
        if (PlayerPrefs.GetInt(playerprefstocheck, 0) == 1)
        {
            if (firstChild.activeInHierarchy == !activateInstead)
                firstChild.SetActive(activateInstead);
        }

        StartCoroutine("CheckIfCollected");
	}

    IEnumerator CheckIfCollected()
    {
        for (float f = 0f; f < 5f; f += Time.deltaTime * Time.timeScale)
        {
            while (DataManager.isSuspended)
                yield return null;

            yield return null;

            if (!this.enabled)
                StopCoroutine("CheckIfCollected");
        }

        if (PlayerPrefs.GetInt(playerprefstocheck, 0) == 0)
        {
            if (firstChild.activeInHierarchy == activateInstead)
                firstChild.SetActive(!activateInstead);
        }

        StartCoroutine("CheckIfCollected");
    }
}
