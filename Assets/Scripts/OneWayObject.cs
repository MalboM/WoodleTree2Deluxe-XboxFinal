using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneWayObject : MonoBehaviour {

	GameObject chara;
    GameObject beaver;
	BoxCollider box;
	float yAmount;
	float charaY;

	void Start(){
		if (this.gameObject.GetComponent<BoxCollider> () != null) {
			box = this.gameObject.GetComponent<BoxCollider> ();
			yAmount = this.transform.position.y + ((box.size.y * this.transform.lossyScale.y) / 2f);
		}
	}

	void Update () {
        if (beaver == null && PlayerManager.GetPlayer(1).gameObject != null)
            beaver = PlayerManager.GetPlayer(1).gameObject;


        if (beaver != null && beaver.activeInHierarchy)
        {
            if (box != null && box.isTrigger)
                box.isTrigger = false;
        }
        else
        {
            if (box != null)
            {
                if (chara == null && GameObject.Find("Woodle Character") != null)
                {
                    chara = GameObject.Find("Woodle Character");
                    charaY = chara.gameObject.GetComponent<CapsuleCollider>().height;
                }
                if (chara != null)
                {
                    if ((chara.transform.position.y - (charaY / 2f)) >= yAmount)
                    {
                        if (box.isTrigger)
                            box.isTrigger = false;
                    }
                    else
                    {
                        if (!box.isTrigger)
                            box.isTrigger = true;
                    }
                }
            }
        }
	}
}
