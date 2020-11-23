using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FPSCounter : MonoBehaviour
{
    public Text counter;
    float deltaTime = 0.0f;
    float fps;

    void Update()
    {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
        fps = 1.0f / deltaTime;
        counter.text = fps.ToString("F2");
    //    Time.fixedDeltaTime = 1f/fps;
    }
}