using UnityEngine;
using System.Collections;

public class ActivateAtDistanceAries : MonoBehaviour
{

    public GameObject object1;
    public GameObject shadowprojector;
    public Animator anim;
 
    public bool deactivate;
        void Start()
    {
        
       // anim = object1.GetComponent<Animator>();
     
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && other.gameObject.name == "Woodle Character")
        {

            object1.transform.localPosition = new Vector3(0, 0, 0);
            object1.transform.localEulerAngles = new Vector3(0, 0, 0);
      
          //  anim.Play("Run", -1, 0f);
            //anim.Play("Walk", -1, 0f);
            anim.SetBool("Deactivate", false);

            // object1.transform.localRotation = Quaternion.identity;

            anim.enabled = true;
            
            deactivate = false;
            object1.SetActive(true);
            shadowprojector.SetActive(true);


        }


    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player" && other.gameObject.name == "Woodle Character")
        {

            object1.transform.localPosition = new Vector3(0, 0, 0);
            object1.transform.localEulerAngles = new Vector3(0, 0, 0);
        
            anim.SetBool("Deactivate", true);

         //   anim.enabled = false;
            deactivate = true;
            object1.SetActive(false);
            shadowprojector.SetActive(false);
        }


    }

   // void Update()
   // {

      //  if(deactivate ==true)
       //     anim.SetBool("Deactivate", true);

       // if (deactivate == false)
        //    anim.SetBool("Deactivate", false);
   // }



}