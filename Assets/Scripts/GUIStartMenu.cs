using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GUIStartMenu : MonoBehaviour {

    public float slideSpeed;

    private CameraFollower camF;

    public bool paused;
    public GameObject pausemenu;

    public GameObject woodle;
    public GameObject woodleGUI;
    public GameObject woodleGUI2;
    public GameObject canvasGUI;

    public float woodleXposition;
    public float woodleZposition;

    public float finalguiwoodlepositionx;
    public float finalguiwoodlepositionz;

    public float centerpositionx;
    public float centerpositionz;


    public GameObject resume;
    public GameObject quit;

    public GameObject mousesensitivity;

    public GameObject mousesensitivitybar;

    public GameObject tick;

    public bool arrowonquit;
    public bool mousesensitivityboolean;

    public bool togglebool;

    public bool qualitybool;
    public int qualityint;
    public GameObject qualitygameobject;
    public int togglequality;

    public GameObject best;
    public GameObject high;
    public GameObject medium;
    public GameObject low;

    public int changeoption;
    bool coDelay;

    float vert;
    float horz;

    //enum QualityOptions { low, medium, best};

    void Start() {
        camF = Camera.main.GetComponent<CameraFollower>();
        qualityint = 4;
        changeoption = 1;
        togglequality = 5;
    }

    // Update is called once per frame
    void Update() {

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            vert = 1f;
        else {
            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
                vert = -1f;
            else
                vert = Input.GetAxis("Vertical");
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            horz = -1f;
        else {
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
                horz = 1f;
            else
                horz = Input.GetAxis("Horizontal");
        }


        if (Input.GetButtonDown("Start") || Input.GetKeyDown("escape")) { 

            if (paused)
                paused = false;

                 else
                paused = true;

        }


        if (paused)
        {
            //set time
          //  Time.timeScale = 0.1f;
            woodleXposition = woodle.transform.position.x;
            woodleZposition = woodle.transform.position.z;

            // finalguiwoodlepositionx = -woodleXposition * 0.5f + 562;
            // finalguiwoodlepositionz = -woodleZposition * 0.5f + 350;

            finalguiwoodlepositionx = -woodleXposition * 4.5f + 562;
            finalguiwoodlepositionz = -woodleZposition * 4.5f + 350;

            woodleGUI2.GetComponent<RectTransform>().anchoredPosition = new Vector2(-woodleXposition/4, -woodleZposition/4 + 40);

            // woodleGUI.transform.position = new Vector3( 0, 0 , 0);
            if(!coDelay && (vert < -0.9f || vert > 0.9f))
                StartCoroutine(menupaused(vert));
            MenuPause();

        }


        if (!paused)
        {
            StartCoroutine(menunotpaused());
        }
    }

    void MenuPause()
    {
        if(changeoption == 1) { 
            if (horz > 0.9f && mousesensitivityboolean == false)
            {
                
                quit.GetComponent<Animator>().SetBool("Activate", true);
                resume.GetComponent<Animator>().SetBool("Activate", false);
                mousesensitivity.GetComponent<Animator>().SetBool("Activate", false);
                print("right");
                arrowonquit = true;
            }

            if (arrowonquit == true)
            {

                if (Input.GetButtonDown("ActionButton") || Input.GetKeyDown("enter"))
                {

                    print("quit game");
                    Application.Quit();
                }
            }

            if(arrowonquit == false)
            {
                resume.GetComponent<Animator>().SetBool("Activate", true);
            }


            if (horz < -0.9f && mousesensitivityboolean == false)
            {
                quit.GetComponent<Animator>().SetBool("Activate", false);
                resume.GetComponent<Animator>().SetBool("Activate", true);
                mousesensitivity.GetComponent<Animator>().SetBool("Activate", false);
                print("left");
                arrowonquit = false;

            }
            if (resume.GetComponent<Animator>().GetBool("Activate") == true)
            {
                if (Input.GetButtonDown("ActionButton") || Input.GetKeyDown("enter"))
                {
                    paused = false;
                }
            }
        }

        //MOUSE SENSITIVITY
        if (changeoption == 0 )
        {
            qualitygameobject.GetComponent<Animator>().SetBool("Activate", false);
            mousesensitivity.GetComponent<Animator>().SetBool("Activate", true);
            quit.GetComponent<Animator>().SetBool("Activate", false);
            resume.GetComponent<Animator>().SetBool("Activate", false);
            mousesensitivityboolean = true;
            arrowonquit = false;



        }

        if (changeoption == 1 )
        {
            qualitygameobject.GetComponent<Animator>().SetBool("Activate", false);
            mousesensitivity.GetComponent<Animator>().SetBool("Activate", false);
           // quit.GetComponent<Animator>().SetBool("Activate", false);
          //  resume.GetComponent<Animator>().SetBool("Activate", true);
            mousesensitivityboolean = false;
           // arrowonquit = false;
        }

        if(changeoption ==2 )
        {

          
            mousesensitivity.GetComponent<Animator>().SetBool("Activate", false);
            quit.GetComponent<Animator>().SetBool("Activate", false);
            resume.GetComponent<Animator>().SetBool("Activate", false);
            arrowonquit = false;
        }



        //QUALITY------

       
        if (changeoption == 2)
        {

            qualitygameobject.GetComponent<Animator>().SetBool("Activate", true);

            if (horz > 0.9f)
            {
                if (togglequality <= 5)
                    togglequality++;

            }


            if (horz < -0.9f)
            {
                if (togglequality >= -5)
                    togglequality--;

            }

            if (togglequality <= -1 && togglequality >= -5)
                qualityint = 1;

            if (togglequality >= 0 && togglequality <= 2)
                qualityint = 3;

            if (togglequality >= 3 && togglequality <= 5)
                qualityint = 4;





            if (qualityint == 1)
            {
                QualitySettings.SetQualityLevel(0, true);

                best.SetActive(false);
                medium.SetActive(false);
                low.SetActive(true);
            }

            if (qualityint == 2)
            {
                QualitySettings.SetQualityLevel(2, true);
            }

            if (qualityint == 3)
            {
                QualitySettings.SetQualityLevel(4, true);
                best.SetActive(false);
                medium.SetActive(true);
                low.SetActive(false);
            }

            if (qualityint == 4)
            {
                QualitySettings.SetQualityLevel(6, true);
                best.SetActive(true);
                medium.SetActive(false);
                low.SetActive(false);
            }
        }

        //MOUSE SENSITIVITY CHANGING
        if (horz < -0.9f && mousesensitivityboolean == true)
        {
            if (camF.freeRotateSpeed >= 0.8f) 
            {
                camF.freeRotateSpeed -= 0.3f;
                camF.zoomSensitivity = camF.freeRotateSpeed / 11.25f;
                mousesensitivitybar.transform.Translate(Vector3.right * (-slideSpeed));
            }

            //Change mouse sensitivity less

        }



        if (horz > 0.9f && mousesensitivityboolean == true)
        {
            if (camF.freeRotateSpeed <= 8.7f)
            {
                Camera.main.GetComponent<CameraFollower>().freeRotateSpeed += 0.3f;
                camF.zoomSensitivity = camF.freeRotateSpeed / 11.25f;
                mousesensitivitybar.transform.Translate(Vector3.right * slideSpeed);

            }
            //Change mouse sensitivity more

        }


        if (Input.GetButtonDown("ActionButton") || Input.GetKeyDown("enter"))
        {
            if (mousesensitivityboolean == true)
            {

                if (!togglebool)
                {
                    togglebool = true;
                    tick.SetActive(false);
                    camF.dampCamera = false;
                }

                else
                {

                    togglebool = false;
                    tick.SetActive(true);
                    camF.dampCamera = true;
                }
            }
        }
        
        canvasGUI.SetActive(true);
        pausemenu.SetActive(true);
    }

    IEnumerator menupaused(float axis)
    {
        coDelay = true;
        yield return new WaitForSeconds(0.2f);

        if (axis > 0f)
            changeoption++;
        if (axis < 0f)
            changeoption--;

        if (changeoption < 0)
            changeoption = 0;
        if (changeoption > 2)
            changeoption = 2;
        coDelay = false;
    }

    IEnumerator menunotpaused()
    {

        yield return new WaitForSeconds(0);
        canvasGUI.SetActive(false);
        pausemenu.SetActive(false);
      //  Time.timeScale = 1.0f;


    }
}