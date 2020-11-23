using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBlobShadow : MonoBehaviour {

	public GameObject newShadow;
	public LayerMask whatToHit;

	void Start ()
    {
        //    StartCoroutine ("WaitToProject");
        RaycastHit heh = new RaycastHit();
        if (Physics.Raycast(this.transform.position, -Vector3.up, out heh, 100f, whatToHit))
            newShadow.transform.position = heh.point + (Vector3.up * 0.01f);
        else
            newShadow.transform.position = this.transform.position - (Vector3.up * 100f);
    }



    IEnumerator WaitToProject()
    {
        for (int w = 0; w < 600; w++)
        {
            if (this.gameObject.name == "HAHA")
                Debug.Log(w +"  "+ this.gameObject.activeInHierarchy +"  "+ this.gameObject.activeSelf);
            yield return null;
        }
        //	GameObject newShadow = Instantiate (shadowPrefab, this.transform.position, this.transform.rotation) as GameObject;
        //	newShadow.transform.SetParent (this.transform);
        //	newShadow.transform.localEulerAngles = new Vector3 (-90f, 0f, 0f);
        RaycastHit heh = new RaycastHit ();
		if (Physics.Raycast (this.transform.position, -Vector3.up, out heh, 100f, whatToHit))
			newShadow.transform.position = heh.point + (Vector3.up * 0.01f);
		else
			newShadow.transform.position = this.transform.position - (Vector3.up * 100f);
        if (this.gameObject.name == "HAHA")
            Debug.Log(heh.point + "  " + heh.collider.gameObject.name);
	}
}
