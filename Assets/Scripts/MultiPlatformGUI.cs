using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MultiPlatformGUI : MonoBehaviour
{
    public MultiPlatfromGUIManager.IconType iconType;

    public Image image;
    bool assigned = false;

    void Start()
    {
        MultiPlatfromGUIManager.changeEvent += ControllerChanged;

        if(!assigned)
            ControllerChanged(MultiPlatfromGUIManager.singleton.curControllerType);
    }

    void ControllerChanged(MultiPlatfromGUIManager.ControllerType newControllerType)
    {
        if (!assigned)
            assigned = true;

        foreach(MultiPlatfromGUIManager.SpriteCollection sc in MultiPlatfromGUIManager.singleton.spriteCollections)
        {
            if (sc.type == iconType)
            {
                if(image != null)
                    image.sprite = sc.sprites[(int)newControllerType];
            }
        }
    }
}
