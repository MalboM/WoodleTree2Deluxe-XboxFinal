using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableControlForCutscenes : MonoBehaviour
{
    public List<TPC> tpcs = new List<TPC>();
    public CameraFollower cameraFollower;

    private void OnEnable()
    {
        EnableFunction();
    }

    public void EnableFunction()
    {
        foreach (TPC t in tpcs)
        {
            t.inCutscene = true;
            t.disableControl = true;
        }
        cameraFollower.disableControl = true;
    }

    private void OnDisable() {
        DisableFunction();
    }

    public void DisableFunction()
    {
        foreach (TPC t in tpcs)
        {
            t.inCutscene = false;
            t.disableControl = false;
        }
        cameraFollower.disableControl = false;
    }
}
