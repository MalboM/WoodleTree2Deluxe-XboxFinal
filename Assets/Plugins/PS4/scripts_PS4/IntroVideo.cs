using UnityEngine;
#if UNITY_PS4
using UnityEngine.PS4;
#endif
using UnityEngine.SceneManagement;
using System.Collections;

public class IntroVideo : MonoBehaviour
{

#if UNITY_PS4 && !UNITY_EDITOR

    PS4VideoPlayer video;
    UnityEngine.PS4.PS4ImageStream lumaTex;
    UnityEngine.PS4.PS4ImageStream chromaTex;
    PS4VideoPlayer.Looping looping = PS4VideoPlayer.Looping.None;

    GameObject plane = null;
    bool initialized = false; 

    //qua ci pensa il modulo PS4

    void Start()
    {
        //disabilitiamo il modulo non PS4    
        video = new PS4VideoPlayer();			// this sets up a VideoDecoderType.DEFAULT system

        bool useNewDecoder = true;

        if (useNewDecoder == false)
        {
            //	 this setting allows you to player more than one video at the same time
            video.PerformanceLevel = PS4VideoPlayer.Performance.Default;
            video.demuxVideoBufferSize = 2 * 1024 * 1024;	// change the demux buffer from it's 1mb default	
            video.numOutputVideoFrameBuffers = 3;		// increasing this can stop frame stuttering
        }
        else
        {
            video.PerformanceLevel = PS4VideoPlayer.Performance.Optimal;
            video.demuxVideoBufferSize = 2 * 1024 * 1024;		// change the demux buffer from it's 1mb default
            video.numOutputVideoFrameBuffers = 2;	// increasing this can stop frame stuttering
        }

        lumaTex = new UnityEngine.PS4.PS4ImageStream();
        lumaTex.Create(1920, 1080, PS4ImageStream.Type.R8, 0);
        chromaTex = new UnityEngine.PS4.PS4ImageStream();
        chromaTex.Create(1920 / 2, 1080 / 2, PS4ImageStream.Type.R8G8, 0);
        video.Init(lumaTex, chromaTex);

        plane = GameObject.Find("PS4Video");
        plane.GetComponent<Renderer>().material.mainTexture = lumaTex.GetTexture();
        plane.GetComponent<Renderer>().material.SetTexture("_CromaTex", chromaTex.GetTexture());

        initialized = false;

        string assetLocation = System.IO.Path.Combine(Application.streamingAssetsPath, "VelocityImprovedLogo.mp4");
        Play(assetLocation);
    }


    public bool isPlaying
    {
        get
        {
            if (video == null)
                return false;

            if ((video.playerState == PS4VideoPlayer.VidState.STOP))
            {
                return false;
            }
            else
                return true;
        }
    }

    private void Init()
    {
        initialized = true;
    }


    public void Play(string videoPath)
    {
        if (!initialized)
            Init();

        PS4VideoPlayer.PlayParams pp = new PS4VideoPlayer.PlayParams();
        pp.loopSetting = PS4VideoPlayer.Looping.None;
        pp.volume = 0.0f;
        video.PlayVideoEx(videoPath, pp);
        return;
    }

    public void Stop()
    {
        if (!initialized) return;
        video.Stop();
    }

    float timeVideo;
    void Update()
    {
        if (initialized)
        {
            if (isPlaying)
            {
                video.Update();
                CropVideo();
                timeVideo += Time.deltaTime;
                if (timeVideo > 5)
                {
                    //
                    if (video != null)
                        video.Stop();
                    //             
                    AudioSource asrc = GetComponent<AudioSource>();
                    if (asrc != null) asrc.Stop();
                    this.gameObject.SetActive(false);
                    //
                    /*if (PS4Manager.loadingScreen != null)
                        PS4Manager.loadingScreen.Display(true);*/
                    //
                    SceneManager.LoadScene(1);
                }
            }
        }
    }

    void OnPreRender()
    {

    }

    private void OnMovieEvent(int FMVevent)
    {
        Debug.LogError("script has received FMV event " + FMVevent);
    }


    private void CropVideo()
    {
        //PS4Input.PadSetLightBar(0, UnityEngine.Random.Range(0, 255), UnityEngine.Random.Range(0, 255), UnityEngine.Random.Range(0, 255));
        int cropleft, cropright, croptop, cropbottom, width, height;
        video.GetVideoCropValues(out cropleft, out cropright, out croptop, out cropbottom, out width, out height);
        float scalex = 1.0f;
        float scaley = 1.0f;
        float offx = 0.0f;
        float offy = 0.0f;

        if ((width > 0) && (height > 0))
        {
            int fullwidth = width + cropleft + cropright;
            scalex = (float)width / (float)fullwidth;
            offx = (float)cropleft / (float)fullwidth;
            int fullheight = height + croptop + cropbottom;
            scaley = (float)height / (float)fullheight;
            offy = (float)croptop / (float)fullheight;
        }
        plane.GetComponent<Renderer>().material.SetVector("_MainTex_ST", new Vector4(scalex, scaley * -1, offx, 1 - offy)); // typically we want to invert the Y on the video because thats how planes UV's are layed out
        //Debug.LogError("PLAYING VIDEO");
    }




#endif

#if UNITY_XBOXONE
    //qua ci pensa il modulo XBOXOne
    void Start()
    {
        GameObject ps4 = GameObject.Find("PS4_PlayVideo");
        if (ps4 != null) ps4.SetActive(false);
    }
#endif

#if UNITY_STANDALONE_WIN || UNITY_STANDALONE_MAC

    //WINDOWS / MAC ----------------------------------------

    TextMesh text, textShadow;
    MeshRenderer mText, mShadow;
    MovieTexture video;
    AudioSource vaudio;
    Renderer vRender;
    float videoTime;
  
    bool isLoadingMainMenu = false;
    float timerStartVideo = 0;

    // Use this for initialization
    void Start()
    {
        GameObject.Find("PS4_PlayVideo").SetActive(false);
        video = (MovieTexture)Resources.Load("Videos/Opening") as MovieTexture;
        vRender = GetComponent<MeshRenderer>();
        vaudio = gameObject.GetComponent<AudioSource>();
        video.loop = false;        
        vRender.material.mainTexture = video;
        mText = GameObject.Find("TextLoad").GetComponent<MeshRenderer>();
        mText.enabled = false;
        //vRender.transform.localScale = new Vector3(1280, 720, 1);
        /*video.Play();
        //vaudio.clip = video.audioClip;
        vaudio.Play();*/
    }

    // Update is called once per frame
    void Update()
    {
        if (isLoadingMainMenu)
        {
                foreach (GameObject o in GameObject.FindObjectsOfType<GameObject>())
                {
                //    Destroy(o);
                }
                SceneManager.LoadScene("MainMenuGameScreen");
            
            return;
        }
        else
        {
            videoTime += Time.deltaTime;
            if (videoTime > 28)
            {
                mText.enabled = true;
                vRender.enabled = false;
                video.Stop();
                vaudio.Stop();
                isLoadingMainMenu = true;
            }
            else
            {
                timerStartVideo += Time.deltaTime;
                if (timerStartVideo > 0.5f && !video.isPlaying)
                {
                    video.Play();
                    //vaudio.Play();
                }
            }
        }
    }
#endif
}