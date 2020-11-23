using UnityEngine;
using System.Collections;

public class EnemyStanding : MonoBehaviour {


    Animator animator;
    public bool hit;
    public float speedfloat;
    void Awake()
    {

        animator = GetComponent<Animator>();

    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.layer == 10)

        {

    
                 hit = true;
                StartCoroutine(Hit());
            //this.GetComponent<Collider>().enabled = false;
            // animator.SetFloat("Speed", 0.3f);


            //StartCoroutine(Killed());


        }
    }

    void OnCollisionExit(Collision other)
    {
        if (other.gameObject.layer == 10)

        {

         //   hit = false;
            
            
        }
    }

    void Update()
    {

        if (hit == true && speedfloat <= 0.4f)
        {



            speedfloat += 0.8f * Time.deltaTime;
            animator.SetFloat("Speed", speedfloat);
         
        }
        else if (hit == false && speedfloat >=0)
        {

            speedfloat -= 0.8f * Time.deltaTime;
            animator.SetFloat("Speed", speedfloat);
        }

        
        
    }

    private IEnumerator Hit()
    {
        yield return new WaitForSeconds(0.7f);
        hit = false;

       
    }
}
