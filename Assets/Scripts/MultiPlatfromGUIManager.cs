using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Rewired;

public class MultiPlatfromGUIManager : MonoBehaviour
{
    [HideInInspector] public static MultiPlatfromGUIManager singleton;

    public enum ControllerType { mk = 0,  xb = 1,  ps = 2 };
     public ControllerType curControllerType;
    ControllerType prevType;

    public enum IconType { a, b, x, y, lb, rb, start, leftStick, rightStick, rightPress, back, xGUI, yGUI, aGUI, bGUI };

    public delegate void ChangeEvent(ControllerType controllerType);
    public static event ChangeEvent changeEvent;

    Player input;
    Controller curController;

    [System.Serializable] public class SpriteCollection { public IconType type; public List<Sprite> sprites; }
    public List<SpriteCollection> spriteCollections;

    void Awake()
    {
        if (singleton != null)
        {
            enabled = false;
            Destroy(this);
            return;
        }
        singleton = this;

        input = ReInput.players.GetPlayer(0);

        curControllerType = ControllerType.mk;
        ControllerChanged();
    }

    private void Update()
    {
        if(input.controllers.GetLastActiveController() != curController)
        {
            curController = input.controllers.GetLastActiveController();

            prevType = curControllerType;

            if(curController == null || curController.name == "Mouse" || curController.name == "Keyboard")
                curControllerType = ControllerType.mk;
            else
            {
                if(curController.name.ToUpper().Contains("SONY") || curController.name.ToUpper().Contains("PS"))
                    curControllerType = ControllerType.ps;
                else
                    curControllerType = ControllerType.xb;
            }

            if(curControllerType != prevType)
                ControllerChanged();
        }
    }

    public void ControllerChanged()
    {
        if (changeEvent != null)
            changeEvent(curControllerType);
    }
}
