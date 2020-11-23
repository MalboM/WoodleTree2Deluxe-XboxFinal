using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextTrigger : MonoBehaviour {

    public int textID;
    TextTriggerMain main;
    bool activated;

    void OnTriggerEnter(Collider other){
        if (other.gameObject.tag == "Player" && other.gameObject.name == "Woodle Character" && !activated)
        {
            if (main == null)
                main = GameObject.FindWithTag("Pause").transform.Find("Event Text").gameObject.GetComponent<TextTriggerMain>();

            if (main != null)
            {
                activated = true;
                main.SetText(textID);
            }
        }
    }
}
