using UnityEngine;
using System.Collections;

public class RotatingSkybox : MonoBehaviour {


    public float adjustment;

    // Use this for initialization
   

    void Update()
    {
        if(RenderSettings.skybox != null)
            RenderSettings.skybox.SetFloat("_Rotation", Time.time * adjustment);
    }
}
