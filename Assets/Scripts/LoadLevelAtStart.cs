using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.Video;
//using nn;

public class LoadLevelAtStart : MonoBehaviour
{
    public bool dontLoad;

    public string level;
    public TextMesh texty;

    public float waitTime;

    public VideoPlayer vPlayer;
    public float vDelay;

    public AudioSource mPlayer;
    public float aDelay;

    public AudioMixer[] audioMixers;

    void Awake()
    {
        int currentResolution = PlayerPrefs.GetInt("Resolution", Screen.resolutions.Length - 1);
        if (currentResolution >= Screen.resolutions.Length)
            currentResolution = Screen.resolutions.Length - 1;
        Resolution curRes = Screen.resolutions[currentResolution];
        PlayerPrefs.SetInt("Resolution", currentResolution);
        Screen.SetResolution(curRes.width, curRes.height, Screen.fullScreenMode);
        
        foreach (AudioMixer am in audioMixers)
            am.SetFloat("effectsVol", -80f + ((PlayerPrefs.GetFloat("effectsVolume", 8f)) * 10f));
        foreach (AudioMixer am in audioMixers)
            am.SetFloat("musicVol", -80f + ((PlayerPrefs.GetFloat("musicVolume", 8f)) * 10f));

#if (!UNITY_EDITOR && UNITY_SWITCH)
        PlayerPrefsSwitch.PlayerPrefsSwitch.Init();
        
        //
        if (PlayerPrefs.GetInt("FirstRun", 0) == 0)
        {           
            //
            string slang = nn.oe.Language.GetDesired();

            //
            if (slang.Length >= 2)
            {
                //
                switch (slang.Substring(0, 2).ToUpper())
                {                
                    //
                    case "EN":                  
                    PlayerPrefs.SetInt("Language", 0);
                    break;
                    //
                    case "RU":                  
                    PlayerPrefs.SetInt("Language", 1);
                    break;
                    //
                    case "ES":                  
                    PlayerPrefs.SetInt("Language", 2);
                    break;
                    //
                    case "IT":                    
                    PlayerPrefs.SetInt("Language", 3);
                    break;
                    //
                    case "ZH":                    
                    PlayerPrefs.SetInt("Language", 4);
                    break;
                    //
                    case "FR":                   
                    PlayerPrefs.SetInt("Language", 5);
                    break;
                    //
                    case "PT":                   
                    PlayerPrefs.SetInt("Language", 6);
                    break;
                    //
                    case "NL":                   
                    PlayerPrefs.SetInt("Language", 7);
                    break;
                    //
                    case "DE":                    
                    PlayerPrefs.SetInt("Language", 8);
                    break;
                    //
                    case "JA":
                    PlayerPrefs.SetInt("Language", 9);
                    break;
                    //
                    case "TR":
                    PlayerPrefs.SetInt("Language", 10);
                    break;
                    //
                    case "KO":                   
                    PlayerPrefs.SetInt("Language", 14);
                    break;
                    //
                    default:                    
                    PlayerPrefs.SetInt("Language", 0);
                    break;
                }                
            }       
        }
#endif
        /*    if (!PlayerPrefs.HasKey("Berries")){
                PlayerPrefs.SetInt("LastCheckpoint", 0);
                PlayerPrefs.SetInt("Berries", 0);
                PlayerPrefs.SetInt("BlueBerries", 0);
                for (int lvl = 1; lvl <= 8; lvl++){
                    for (int tearNo = 1; tearNo <= 3; tearNo++)
                        PlayerPrefs.SetInt("Vase" + tearNo.ToString() + "Level" + lvl.ToString(), 0);
                }
                for (int cp = 0; cp <= 37; cp++)
                    PlayerPrefs.SetInt("Checkpoint" + cp.ToString(), 0);
                for (int xyz = 0; xyz < 21; xyz++){
                    PlayerPrefs.SetInt("UsingItem" + xyz.ToString(), 0);
                    PlayerPrefs.SetInt("PaidForItem" + xyz.ToString(), 0);
                }
                for (int ab = 0; ab <= 123; ab++)
                    PlayerPrefs.SetInt("Button" + ab.ToString(), 0);
                PlayerPrefs.SetInt("marmellade1", 0);
                PlayerPrefs.SetInt("marmellade2", 0);
                PlayerPrefs.SetInt("marmellade3", 0);
                PlayerPrefs.SetInt("plantjar", 0);
                PlayerPrefs.SetInt("plantjar2", 0);
                PlayerPrefs.SetInt("book1", 0);
                PlayerPrefs.SetInt("book2", 0);
                PlayerPrefs.SetInt("book3", 0);
                PlayerPrefs.SetInt("paint", 0);
                PlayerPrefs.SetInt("paint2", 0);
                PlayerPrefs.SetInt("gameboy", 0);
                PlayerPrefs.SetInt("bell", 0);
                PlayerPrefs.SetInt("heater", 0);
                PlayerPrefs.SetInt("globe", 0);
                PlayerPrefs.SetInt("cupbear", 0);
                PlayerPrefs.SetInt("compass", 0);
                PlayerPrefs.SetInt("carpet", 0);
                PlayerPrefs.SetInt("candle", 0);
                PlayerPrefs.SetInt("statue1", 0);
                PlayerPrefs.SetInt("statue2", 0);
                PlayerPrefs.SetInt("statue3", 0);
                PlayerPrefs.SetInt("mask1", 0);
                PlayerPrefs.SetInt("mask2", 0);
                PlayerPrefs.SetInt("mask3", 0);
                PlayerPrefs.SetInt("map", 0);
                PlayerPrefs.SetInt("jukebox", 0);
                PlayerPrefs.SetInt("inbox", 0);
                PlayerPrefs.SetInt("IntroWatched", 0);
                PlayerPrefs.SetInt("Intro2Watched", 0);
                PlayerPrefs.SetInt("SeenLogo", 0);
            }*/





        StartCoroutine(LoadinglevelX());


    }


    IEnumerator LoadinglevelX()
    {
        mPlayer.PlayDelayed(aDelay);

        yield return new WaitForSeconds(vDelay);

        vPlayer.Play();

        yield return new WaitForSeconds(waitTime);

        if(!dontLoad)
            SceneManager.LoadSceneAsync(level);
    }
}
