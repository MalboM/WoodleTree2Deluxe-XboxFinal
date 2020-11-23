using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LoadSceneAsync : MonoBehaviour
{
    IEnumerator Start()
    {
        //    AsyncOperation async = Application.LoadLevelAdditiveAsync(1);
        AsyncOperation async = SceneManager.LoadSceneAsync(1);
        yield return async;
        Debug.Log("Loading complete");
    }
}