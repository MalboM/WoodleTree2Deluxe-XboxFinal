using UnityEngine;
using System.Collections;

public class AnimationStartTrigger : MonoBehaviour
{

    public string trigger;
   
   
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == trigger || other.tag == "NPC" || other.gameObject.layer == 14 || other.gameObject.layer == 15)
        {            GetComponent<Animation>().Play();
        }
    }
  
}
