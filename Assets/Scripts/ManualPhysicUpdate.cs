using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManualPhysicUpdate : MonoBehaviour
{
    void Awake()
    {
        Physics.autoSimulation = false;
    }

    /// <summary>
    /// Get's called at the end of the Update cycle.
    /// Defined in project settings under "Script Execution Order".
    /// </summary>
    void Update()
    {
        float safeDeltaTime =
            Time.deltaTime < Time.maximumDeltaTime ?
                Time.deltaTime :
                Time.maximumDeltaTime;

        Physics.Simulate(Mathf.Clamp(safeDeltaTime, 0f, Mathf.Infinity));
    }
}