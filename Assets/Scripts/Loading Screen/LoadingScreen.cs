using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingScreen : MonoBehaviour
{
    //
    public GameObject loadObj, loadImgProgression;
    public Font normalFont, chineseFont;

    //
    public int scenetoload;
    public Text loadingText;
    bool isLoading;


    private void Awake()
    {
        //
     //   SwitchStaticObjs.loadingScreen = this;
        DontDestroyOnLoad(this.gameObject);        
    }

    public void StartLoadScene(int _sceneToLoad = -1)
    {
        //
        if (isLoading)
            return;

        //
        isLoading = true;

        //
        if (_sceneToLoad != -1)
            scenetoload = _sceneToLoad;     
        //
        loadingText.text = "0";        
        //
        Display(true);
        //
        StartCoroutine(LoadSyncScene());
    }

    public AsyncOperation StartLoadASyncScene(int _sceneToLoad, LoadSceneMode mode)
    {   
        //
        isLoading = true;

        //
        if (_sceneToLoad != -1)
            scenetoload = _sceneToLoad;
        //
        loadingText.text = "0";
        //
        Display(true);
        //
        return SceneManager.LoadSceneAsync(_sceneToLoad, mode);
    }

    //
    public void TranslateLoading()
    {
        // 0 engFont;
        // 1 Font rusFont;
        // 2 Font espFont;
        // 3 Font itlFont;
        // 4 Font chsFont;
        // 5 Font frnFont;
        // 6 Font gerFont;
        // 7 Font japFont;
        // 8 Font trkFont;

        //
        int currentLanguage = PlayerPrefs.GetInt("Language", 0);
        loadingText.font = normalFont;

        //
        switch (currentLanguage)
        {
            // en
            case 0:
            loadingText.text = "Loading ...";
            break;

            // ru
            case 1:
            loadingText.text = "погрузка ...";
            break;

            // esp
            case 2:
            loadingText.text = "Cargando ...";
            break;

            // itl
            case 3:
            loadingText.text = "Caricamento ...";
            break;

            // chs
            case 4:
            loadingText.text = "装载 ...";
            loadingText.font = chineseFont;
            break;

            // frn
            case 5:
            loadingText.text = "Chargement ...";
            break;

            // ger
            case 6:
            loadingText.text = "Wird Geladen ...";
            break;

            // jap
            case 7:
            loadingText.text = "読み込み中 ...";
            break;

            // turk
            case 8:
            loadingText.text = "Yükleniyor ...";
            break;
        }
    }
    
    public void LoadSyncScene(int _sceneToLoad = -1)
    {        
        //
        isLoading = true;

        //
        if (_sceneToLoad != -1)
            scenetoload = _sceneToLoad;
        else
            _sceneToLoad = scenetoload;

        //
        SceneManager.LoadScene(scenetoload);
    }

    //
    IEnumerator LoadSyncScene()
    {
        //
        yield return new WaitForSeconds(0.1f);
        yield return new WaitForEndOfFrame();
        //
        Display(true);
        //
        isLoading = false;
        //
        loadingText.text = "0";
        SceneManager.LoadScene("NSwitchLoader");
    }

    //
    public void StartASyncLoadScene()
    {
        //
        if (isLoading)
            return;

        //
        loadingText.text = "0";
        //
        StartCoroutine(LoadScene(scenetoload));
    }


    //
    private AsyncOperation async = null; // When assigned, load is in progress.
    private IEnumerator LoadScene(int _scenetoload = -1)
    {     
        //
        Display(true);

        //
        if (_scenetoload == -1) _scenetoload = scenetoload;
        //
        isLoading = true;
        async = SceneManager.LoadSceneAsync(_scenetoload);
        yield return async;
    }

    //
    int prog;
    void OnGUI()
    {
        //
        if (async != null)
        {
            //
            float prog = async.progress;
            //
            // scaliamo la grafica
            loadImgProgression.transform.localScale = new Vector3(prog, 1, 1);
        }
    }
    
    //
    private void OnLevelWasLoaded(int level)
    {
        //
        Display(false);
        loadingText.text = "0";
        isLoading = false;
        StopCoroutine(LoadSyncScene());
        StopCoroutine(LoadScene());
    }

    private void Start()
    {
        //
        isLoading = false;
        Display(false);
    }

    public void Display(bool on)
    {
        //
        if (on)
            TranslateLoading();
        //
        loadObj.SetActive(on);       
    }


}

