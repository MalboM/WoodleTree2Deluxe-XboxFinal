using UnityEngine;
using System.Collections;

public class FPS : MonoBehaviour
{
    [SerializeField] bool limitFPSTo60;
    float deltaTime = 0.0f;

    void Start() {
		if (limitFPSTo60) {
			QualitySettings.vSyncCount = 0;
			Application.targetFrameRate = 60;
		}
    }
}
