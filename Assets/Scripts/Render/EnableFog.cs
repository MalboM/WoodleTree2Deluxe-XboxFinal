using UnityEngine;
using System.Collections;

public class EnableFog : MonoBehaviour
{

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && other.gameObject.name == "Woodle Character")
        {

            RenderSettings.fog = true;
        }
    }
}
