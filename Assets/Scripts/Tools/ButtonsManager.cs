using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonsManager : MonoBehaviour
{
    public ActivateButton[] buttons;

    private void OnEnable()
    {
        StartCoroutine(Wait());
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(1f);
        foreach (ActivateButton ab in buttons)
        {
            if (PlayerPrefs.GetInt("Button" + ab.buttonID.ToString(), 0) == 1)
                ab.ActivateIt();
        }
    }
}
