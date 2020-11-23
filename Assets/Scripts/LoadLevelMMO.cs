using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityStandardAssets.ImageEffects;
//using Steamworks;


public class LoadLevelMMO : MonoBehaviour
{

    public string level;
    public bool enterlevel;

    Animator animator;
    public GameObject loadinggui;
    public GameObject cameraobject;
    public Blur blurscript;
    public GameObject noncolliderzone;
    AsyncOperation loader;

    void Awake()
    {
        //
        enterlevel = false;
        //
        if (loadinggui != null)
            animator = loadinggui.GetComponent<Animator>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && other.gameObject.name == "Woodle Character")
        {
            //Achievement
           // SteamUserStats.SetAchievement("MMO mode");

            animator.SetBool("Loading", true);
            enterlevel = true;
            this.GetComponent<Collider>().enabled = false;
        }

    }

    void Update()
    {
        if (enterlevel == true)
        {
            blurscript = cameraobject.GetComponent<Blur>();
            blurscript.enabled = true;

            //Application.LoadLevelAdditive(level);
            //
            Debug.LogError("Load level " + level);
            //
            SceneManager.LoadScene(level, LoadSceneMode.Single);

           // loader = SceneManager.LoadSceneAsync(level, LoadSceneMode.Additive);

           // Time.timeScale = 0.01f;

            enterlevel = false;
            StartCoroutine(loadinglevel());
        }
    }
    
    IEnumerator loadinglevel()
    {


        // yield return new WaitForSeconds(0.001f );

        yield return new WaitForSeconds(0.011f);

        
       // if (loader.isDone) { 
           // Time.timeScale = 1.0f;

            //yield return new WaitForSeconds(6.0f);
            animator.SetBool("Loading", false);
            blurscript.enabled = false;

            noncolliderzone.SetActive(false);
       // StartCoroutine(waitseconds());

             
           
       // }
    }

    IEnumerator waitseconds()
    {
         yield return new WaitForSeconds(0.5f);
         noncolliderzone.SetActive(false);
    }

        
 }
