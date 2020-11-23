using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableItem : MonoBehaviour
{


    public GameObject itemdisable;

    // Start is called before the first frame update
    void Start()
    {

        itemdisable.SetActive(false);    

    }

  
}
