#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlobShadowRaycaster : MonoBehaviour
{
    [SerializeField] GameObject shadowGO;
    [SerializeField] LayerMask hitMask;

    public void RaycastShadow()
    {
        RaycastHit hitInfo;
        if (Physics.Raycast(transform.position, -transform.up, out hitInfo, 100f, hitMask))
            shadowGO.transform.position = hitInfo.point + (transform.up * 0.01f);
        else
            shadowGO.transform.position = transform.position - (transform.up * 100f);
    }
}

#endif
