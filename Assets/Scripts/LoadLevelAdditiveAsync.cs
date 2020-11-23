using UnityEngine;
using System.Collections;
//using UnityEngine.SceneManagement;

public class LoadLevelAdditiveAsync : MonoBehaviour
{

    public string level;
    public bool enterlevel;

    Animator animator;
    public GameObject loadinggui;

    private string loadProgress = "Loading...";
    private string lastLoadProgress = null;

    void Awake()
    {
        //Application.LoadLevelAdditiveAsync(2); //To reactivate!!!
      // Application.LoadLevelAdditiveAsync(3);
       // Application.LoadLevelAdditiveAsync(4);
       // Application.LoadLevelAdditiveAsync(5);
       // Application.LoadLevelAdditiveAsync(6);
       // Application.LoadLevelAdditiveAsync(7);
       

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && other.gameObject.name == "Woodle Character")
        {
           // animator.SetBool("Loading", true);


            enterlevel = true;
            this.GetComponent<Collider>().enabled = false;

            //StartCoroutine(InitializeScene(level));
        }

    }

   

    void Update()
    {
        if (enterlevel == true)
        {
            Application.LoadLevelAdditiveAsync(3);
            enterlevel = false;
        }
        /*     if (enterlevel == true) {
                 Application.LoadLevelAdditiveAsync(level);
                 Time.timeScale = 0.21f;

                 enterlevel = false;
                 StartCoroutine( loadinglevel());
             }

           */

    }

    /*

     IEnumerator loadinglevel()
     {


         yield return new WaitForSeconds(0.011f );

         animator.SetBool("Loading", false);

         Time.timeScale = 1.0f;



     }
     */

  //  public IEnumerator InitializeScene(string scene)
 //   {
//




        /*      AsyncOperation op = Application.LoadLevelAdditiveAsync(2);
              op.allowSceneActivation = false;
              while (!op.isDone)
              {
                  if (op.progress < 0.9f)
                  {
                      loadProgress = "Loading: " + (op.progress * 100f).ToString("F0") + "%";
                  }
                  else // if progress >= 0.9f the scene is loaded and is ready to activate.
                  {
                      if (Input.anyKeyDown)
                      {
                          op.allowSceneActivation = true;
                      }
                      loadProgress = "Loading ready for activation, Press any key to continue";
                  }
                  if (lastLoadProgress != loadProgress) { lastLoadProgress = loadProgress; Debug.Log(loadProgress); } // Don't spam console.
                 // yield return null;
              }
              loadProgress = "Load complete.";
              Debug.Log(loadProgress);


              //  Application.LoadLevelAdditiveAsync(level);

          }
          */


   // }

}

