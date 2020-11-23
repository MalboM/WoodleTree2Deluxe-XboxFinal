using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformRockMovement : MonoBehaviour
{
    public Animator anim;

    float curDistance;
    int charactersUsingThis;

    float curTime = 0f;

    private void OnTriggerStay(Collider col)
    {
        if (col.tag == "Player")
        {
            col.transform.SetParent(this.transform);

            if (Time.timeSinceLevelLoad != curTime)
            {
                charactersUsingThis = 1;
                curDistance = 0f;
                curTime = Time.timeSinceLevelLoad;
            }

            curDistance += col.transform.localPosition.z;
            charactersUsingThis++;
        }
    }

    private void OnTriggerExit(Collider col)
    {
        if (col.tag == "Player")
        {
            col.transform.SetParent(PlayerManager.singleton.transform);
        }
    }

    private void LateUpdate()
    {
        curDistance /= (float)Mathf.Clamp(charactersUsingThis, 1, 4);
        anim.SetFloat("Blend", Mathf.Lerp(anim.GetFloat("Blend"), curDistance, Time.deltaTime * 2f));
    }
}
