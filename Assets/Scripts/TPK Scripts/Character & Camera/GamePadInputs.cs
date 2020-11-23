/// <summary>
/// CURRENT VERSION: 1.5 (Nov '15)
/// This script was originally written by Yan Dawid of Zelig Games.
/// 
/// KEY (for the annotations in this script):
/// -The one referred to as 'User' is the person who uses/edits this asset
/// -The one referred to as 'Player' is the person who plays the project build
/// -The one referred to as 'Character' is the in-game character that the player controls
/// 
/// This script is to NOT be redistributed and can only be used by the person that has purchased this asset.
/// Editing or altering this script does NOT make redistributing this script valid.
/// This asset can be used in both personal and commercial projects.
/// The user is free to edit/alter this script to their hearts content.
/// You can contact the author for support via the Zelig Games official website: http://zeliggames.weebly.com/contact.html
/// </summary>

using UnityEngine;
using System;
using System.Collections;

public class GamePadInputs : MonoBehaviour {

    public bool triggersAreButtons;                 //Are the triggers on the gamepad buttons (as opposed to Axes)
    [HideInInspector] public Func<bool> pressAction;
    [HideInInspector] public Func<bool> holdAction;
    [HideInInspector] public Func<bool> pressRightB;
    [HideInInspector] public Func<bool> holdRightB;
    [HideInInspector] public Func<bool> pressLeftB;
    [HideInInspector] public Func<bool> holdLeftB;
    [HideInInspector] public Func<bool> pressTopB;
    [HideInInspector] public Func<bool> holdTopB;
    [HideInInspector] public Func<bool> pressLBump;
    [HideInInspector] public Func<bool> holdLBump;
    [HideInInspector] public Func<bool> pressRBump;
    [HideInInspector] public Func<bool> holdRBump;
    [HideInInspector] public Func<bool> pressSelect;
    [HideInInspector] public Func<bool> holdSelect;
    [HideInInspector] public Func<bool> pressStart;
    [HideInInspector] public Func<bool> holdStart;
    [HideInInspector] public Func<bool> pressLStick;
    [HideInInspector] public Func<bool> holdLStick;
    [HideInInspector] public Func<bool> pressRStick;
    [HideInInspector] public Func<bool> holdRStick;

    public bool triggersAreButtons2;
    [HideInInspector] public Func<bool> pressAction2;
    [HideInInspector] public Func<bool> holdAction2;
    [HideInInspector] public Func<bool> pressRightB2;
    [HideInInspector] public Func<bool> holdRightB2;
    [HideInInspector] public Func<bool> pressLeftB2;
    [HideInInspector] public Func<bool> holdLeftB2;
    [HideInInspector] public Func<bool> pressTopB2;
    [HideInInspector] public Func<bool> holdTopB2;
    [HideInInspector] public Func<bool> pressLBump2;
    [HideInInspector] public Func<bool> holdLBump2;
    [HideInInspector] public Func<bool> pressRBump2;
    [HideInInspector] public Func<bool> holdRBump2;
    [HideInInspector] public Func<bool> pressSelect2;
    [HideInInspector] public Func<bool> holdSelect2;
    [HideInInspector] public Func<bool> pressStart2;
    [HideInInspector] public Func<bool> holdStart2;
    [HideInInspector] public Func<bool> pressLStick2;
    [HideInInspector] public Func<bool> holdLStick2;
    [HideInInspector] public Func<bool> pressRStick2;
    [HideInInspector] public Func<bool> holdRStick2;

    public bool triggersAreButtons3;
    [HideInInspector] public Func<bool> pressAction3;
    [HideInInspector] public Func<bool> holdAction3;
    [HideInInspector] public Func<bool> pressRightB3;
    [HideInInspector] public Func<bool> holdRightB3;
    [HideInInspector] public Func<bool> pressLeftB3;
    [HideInInspector] public Func<bool> holdLeftB3;
    [HideInInspector] public Func<bool> pressTopB3;
    [HideInInspector] public Func<bool> holdTopB3;
    [HideInInspector] public Func<bool> pressLBump3;
    [HideInInspector] public Func<bool> holdLBump3;
    [HideInInspector] public Func<bool> pressRBump3;
    [HideInInspector] public Func<bool> holdRBump3;
    [HideInInspector] public Func<bool> pressSelect3;
    [HideInInspector] public Func<bool> holdSelect3;
    [HideInInspector] public Func<bool> pressStart3;
    [HideInInspector] public Func<bool> holdStart3;
    [HideInInspector] public Func<bool> pressLStick3;
    [HideInInspector] public Func<bool> holdLStick3;
    [HideInInspector] public Func<bool> pressRStick3;
    [HideInInspector] public Func<bool> holdRStick3;

    public bool triggersAreButtons4;
    [HideInInspector] public Func<bool> pressAction4;
    [HideInInspector] public Func<bool> holdAction4;
    [HideInInspector] public Func<bool> pressRightB4;
    [HideInInspector] public Func<bool> holdRightB4;
    [HideInInspector] public Func<bool> pressLeftB4;
    [HideInInspector] public Func<bool> holdLeftB4;
    [HideInInspector] public Func<bool> pressTopB4;
    [HideInInspector] public Func<bool> holdTopB4;
    [HideInInspector] public Func<bool> pressLBump4;
    [HideInInspector] public Func<bool> holdLBump4;
    [HideInInspector] public Func<bool> pressRBump4;
    [HideInInspector] public Func<bool> holdRBump4;
    [HideInInspector] public Func<bool> pressSelect4;
    [HideInInspector] public Func<bool> holdSelect4;
    [HideInInspector] public Func<bool> pressStart4;
    [HideInInspector] public Func<bool> holdStart4;
    [HideInInspector] public Func<bool> pressLStick4;
    [HideInInspector] public Func<bool> holdLStick4;
    [HideInInspector] public Func<bool> pressRStick4;
    [HideInInspector] public Func<bool> holdRStick4;

    [HideInInspector] public Func<float> DH;              //The Horizontal Axis for the D-Pad
    [HideInInspector] public Func<float> DV;              //The Vertical Axis for the D-Pad
    [HideInInspector] public Func<float> RH;              //The Horizontal Axis for the RIGHT analoge stick
    [HideInInspector] public Func<float> RV;              //The Vertical Axis for the RIGHT analoge stick
    [HideInInspector] public Func<float> MX;              //The Horizontal Axis for the RIGHT analoge stick
    [HideInInspector] public Func<float> MY;              //The Vertical Axis for the RIGHT analoge stick
    [HideInInspector] public Func<float> LH;              //The Horizontal Axis for the LEFT analoge stick
    [HideInInspector] public Func<float> LV;              //The Vertical Axis for the LEFT analoge stick
    [HideInInspector] public Func<float> LT;              //The Axis for the LEFT Trigger
    [HideInInspector] public Func<float> RT;              //The Axis for the RIGHT Trigger
    [HideInInspector] public Func<bool> LTPress;
    [HideInInspector] public Func<bool> LTHold;
    [HideInInspector] public Func<bool> RTPress;
    [HideInInspector] public Func<bool> RTHold;

    [HideInInspector] public Func<float> DH2;
    [HideInInspector] public Func<float> DV2;
    [HideInInspector] public Func<float> RH2;
    [HideInInspector] public Func<float> RV2;
    [HideInInspector] public Func<float> MX2;
    [HideInInspector] public Func<float> MY2;
    [HideInInspector] public Func<float> LH2;
    [HideInInspector] public Func<float> LV2;
    [HideInInspector] public Func<float> LT2;
    [HideInInspector] public Func<float> RT2;
    [HideInInspector] public Func<bool> LTPress2;
    [HideInInspector] public Func<bool> LTHold2;
    [HideInInspector] public Func<bool> RTPress2;
    [HideInInspector] public Func<bool> RTHold2;
    
    [HideInInspector] public Func<float> DH3;
    [HideInInspector] public Func<float> DV3;
    [HideInInspector] public Func<float> RH3;
    [HideInInspector] public Func<float> RV3;
    [HideInInspector] public Func<float> MX3;
    [HideInInspector] public Func<float> MY3;
    [HideInInspector] public Func<float> LH3;
    [HideInInspector] public Func<float> LV3;
    [HideInInspector] public Func<float> LT3;
    [HideInInspector] public Func<float> RT3;
    [HideInInspector] public Func<bool> LTPress3;
    [HideInInspector] public Func<bool> LTHold3;
    [HideInInspector] public Func<bool> RTPress3;
    [HideInInspector] public Func<bool> RTHold3;
    
    [HideInInspector] public Func<float> DH4;
    [HideInInspector] public Func<float> DV4;
    [HideInInspector] public Func<float> RH4;
    [HideInInspector] public Func<float> RV4;
    [HideInInspector] public Func<float> MX4;
    [HideInInspector] public Func<float> MY4;
    [HideInInspector] public Func<float> LH4;
    [HideInInspector] public Func<float> LV4;
    [HideInInspector] public Func<float> LT4;
    [HideInInspector] public Func<float> RT4;
    [HideInInspector] public Func<bool> LTPress4;
    [HideInInspector] public Func<bool> LTHold4;
    [HideInInspector] public Func<bool> RTPress4;
    [HideInInspector] public Func<bool> RTHold4;
    
    void Start() {
        pressAction = () => { return  Input.GetButtonDown("ActionButton"); };
        holdAction = () => { return  Input.GetButton("ActionButton"); };
        pressLeftB = () => { return  Input.GetButtonDown("LeftButton"); };
        holdLeftB = () => { return  Input.GetButton("LeftButton"); };
        pressRightB = () => { return  Input.GetButtonDown("RightButton"); };
        holdRightB = () => { return  Input.GetButton("RightButton"); };
        pressTopB = () => { return  Input.GetButtonDown("TopButton"); };
        holdTopB = () => { return  Input.GetButton("TopButton"); };
        pressLBump = () => { return  Input.GetButtonDown("LBump"); };
        holdLBump = () => { return  Input.GetButton("LBump"); };
        pressRBump = () => { return  Input.GetButtonDown("RBump"); };
        holdRBump = () => { return  Input.GetButton("RBump"); };
        pressStart = () => { return  Input.GetButtonDown("Start"); };
        holdStart = () => { return  Input.GetButton("Start"); };
        pressSelect = () => { return  Input.GetButtonDown("Select"); };
        holdSelect = () => { return  Input.GetButton("Select"); };
        pressLStick = () => { return  Input.GetButtonDown("LStickPress"); };
        holdLStick = () => { return  Input.GetButton("LStickPress"); };
        pressRStick = () => { return  Input.GetButtonDown("RStickPress"); };
        holdRStick = () => { return  Input.GetButton("RStickPress"); };

        pressAction2 = () => { return Input.GetButtonDown("ActionButton2"); };
        holdAction2 = () => { return Input.GetButton("ActionButton2"); };
        pressLeftB2 = () => { return Input.GetButtonDown("LeftButton2"); };
        holdLeftB2 = () => { return Input.GetButton("LeftButton2"); };
        pressRightB2 = () => { return Input.GetButtonDown("RightButton2"); };
        holdRightB2 = () => { return Input.GetButton("RightButton2"); };
        pressTopB2 = () => { return Input.GetButtonDown("TopButton2"); };
        holdTopB2 = () => { return Input.GetButton("TopButton2"); };
        pressLBump2 = () => { return Input.GetButtonDown("LBump2"); };
        holdLBump2 = () => { return Input.GetButton("LBump2"); };
        pressRBump2 = () => { return Input.GetButtonDown("RBump2"); };
        holdRBump2 = () => { return Input.GetButton("RBump2"); };
        pressStart2 = () => { return Input.GetButtonDown("Start2"); };
        holdStart2 = () => { return Input.GetButton("Start2"); };
        pressSelect2 = () => { return Input.GetButtonDown("Select2"); };
        holdSelect2 = () => { return Input.GetButton("Select2"); };
        pressLStick2 = () => { return Input.GetButtonDown("LStickPress2"); };
        holdLStick2 = () => { return Input.GetButton("LStickPress2"); };
        pressRStick2 = () => { return Input.GetButtonDown("RStickPress2"); };
        holdRStick2 = () => { return Input.GetButton("RStickPress2"); };

        pressAction3 = () => { return Input.GetButtonDown("ActionButton3"); };
        holdAction3 = () => { return Input.GetButton("ActionButton3"); };
        pressLeftB3 = () => { return Input.GetButtonDown("LeftButton3"); };
        holdLeftB3 = () => { return Input.GetButton("LeftButton3"); };
        pressRightB3 = () => { return Input.GetButtonDown("RightButton3"); };
        holdRightB3 = () => { return Input.GetButton("RightButton3"); };
        pressTopB3 = () => { return Input.GetButtonDown("TopButton3"); };
        holdTopB3 = () => { return Input.GetButton("TopButton3"); };
        pressLBump3 = () => { return Input.GetButtonDown("LBump3"); };
        holdLBump3 = () => { return Input.GetButton("LBump3"); };
        pressRBump3 = () => { return Input.GetButtonDown("RBump3"); };
        holdRBump3 = () => { return Input.GetButton("RBump3"); };
        pressStart3 = () => { return Input.GetButtonDown("Start3"); };
        holdStart3 = () => { return Input.GetButton("Start3"); };
        pressSelect3 = () => { return Input.GetButtonDown("Select3"); };
        holdSelect3 = () => { return Input.GetButton("Select3"); };
        pressLStick3 = () => { return Input.GetButtonDown("LStickPress3"); };
        holdLStick3 = () => { return Input.GetButton("LStickPress3"); };
        pressRStick3 = () => { return Input.GetButtonDown("RStickPress3"); };
        holdRStick3 = () => { return Input.GetButton("RStickPress3"); };

        pressAction4 = () => { return Input.GetButtonDown("ActionButton4"); };
        holdAction4 = () => { return Input.GetButton("ActionButton4"); };
        pressLeftB4 = () => { return Input.GetButtonDown("LeftButton4"); };
        holdLeftB4 = () => { return Input.GetButton("LeftButton4"); };
        pressRightB4 = () => { return Input.GetButtonDown("RightButton4"); };
        holdRightB4 = () => { return Input.GetButton("RightButton4"); };
        pressTopB4 = () => { return Input.GetButtonDown("TopButton4"); };
        holdTopB4 = () => { return Input.GetButton("TopButton4"); };
        pressLBump4 = () => { return Input.GetButtonDown("LBump4"); };
        holdLBump4 = () => { return Input.GetButton("LBump4"); };
        pressRBump4 = () => { return Input.GetButtonDown("RBump4"); };
        holdRBump4 = () => { return Input.GetButton("RBump4"); };
        pressStart4 = () => { return Input.GetButtonDown("Start4"); };
        holdStart4 = () => { return Input.GetButton("Start4"); };
        pressSelect4 = () => { return Input.GetButtonDown("Select4"); };
        holdSelect4 = () => { return Input.GetButton("Select4"); };
        pressLStick4 = () => { return Input.GetButtonDown("LStickPress4"); };
        holdLStick4 = () => { return Input.GetButton("LStickPress4"); };
        pressRStick4 = () => { return Input.GetButtonDown("RStickPress4"); };
        holdRStick4 = () => { return Input.GetButton("RStickPress4"); };

        DH = () => { return Input.GetAxis("DH"); };
        DV = () => { return Input.GetAxis("DV"); };
        RH = () => { return Input.GetAxis("ZHorizontal"); };
        RV = () => { return Input.GetAxis("ZVertical"); };
        MX = () => { return Input.GetAxis("Mouse X"); };
        MY = () => { return Input.GetAxis("Mouse Y"); };
        LH = () => { return Input.GetAxis("Horizontal"); };
        LV = () => { return Input.GetAxis("Vertical"); };

        LTPress = () => { return Input.GetButtonDown("LTrig"); };
        LTHold = () => { return Input.GetButton("LTrig"); };
        RTPress = () => { return Input.GetButtonDown("RTrig"); };
        RTHold = () => { return Input.GetButton("RTrig"); };
        LT = () => { return Input.GetAxis("LTrig"); };
        RT = () => { return Input.GetAxis("RTrig"); };

        DH2 = () => { return Input.GetAxis("DH2"); };
        DV2 = () => { return Input.GetAxis("DV2"); };
        RH2 = () => { return Input.GetAxis("ZHorizontal2"); };
        RV2 = () => { return Input.GetAxis("ZVertical2"); };
        LH2 = () => { return Input.GetAxis("Horizontal2"); };
        LV2 = () => { return Input.GetAxis("Vertical2"); };

        LTPress2 = () => { return Input.GetButtonDown("LTrig2"); };
        LTHold2 = () => { return Input.GetButton("LTrig2"); };
        RTPress2 = () => { return Input.GetButtonDown("RTrig2"); };
        RTHold2 = () => { return Input.GetButton("RTrig2"); };
        LT2 = () => { return Input.GetAxis("LTrig2"); };
        RT2 = () => { return Input.GetAxis("RTrig2"); };

        DH3 = () => { return Input.GetAxis("DH3"); };
        DV3 = () => { return Input.GetAxis("DV3"); };
        RH3 = () => { return Input.GetAxis("ZHorizontal3"); };
        RV3 = () => { return Input.GetAxis("ZVertical3"); };
        LH3 = () => { return Input.GetAxis("Horizontal3"); };
        LV3 = () => { return Input.GetAxis("Vertical3"); };

        LTPress3 = () => { return Input.GetButtonDown("LTrig3"); };
        LTHold3 = () => { return Input.GetButton("LTrig3"); };
        RTPress3 = () => { return Input.GetButtonDown("RTrig3"); };
        RTHold3 = () => { return Input.GetButton("RTrig3"); };
        LT3 = () => { return Input.GetAxis("LTrig3"); };
        RT3 = () => { return Input.GetAxis("RTrig3"); };

        DH4 = () => { return Input.GetAxis("DH4"); };
        DV4 = () => { return Input.GetAxis("DV4"); };
        RH4 = () => { return Input.GetAxis("ZHorizontal4"); };
        RV4 = () => { return Input.GetAxis("ZVertical4"); };
        LH4 = () => { return Input.GetAxis("Horizontal4"); };
        LV4 = () => { return Input.GetAxis("Vertical4"); };

        LTPress4 = () => { return Input.GetButtonDown("LTrig4"); };
        LTHold4 = () => { return Input.GetButton("LTrig4"); };
        RTPress4 = () => { return Input.GetButtonDown("RTrig4"); };
        RTHold4 = () => { return Input.GetButton("RTrig4"); };
        LT4 = () => { return Input.GetAxis("LTrig4"); };
        RT4 = () => { return Input.GetAxis("RTrig4"); };
    }

    void Update() {
        if (triggersAreButtons) {
            if (LTPress() || LTHold())
                LT = () => { return 1f; };
            else
                LT = () => { return 0f; };
            if (RTPress() || RTHold())
                RT = () => { return 1f; };
            else
                RT = () => { return 0f; };
        }

        if (triggersAreButtons2) {
            if (LTPress2() || LTHold2())
                LT2 = () => { return 1f; };
            else
                LT2 = () => { return 0f; };
            if (RTPress2() || RTHold2())
                RT2 = () => { return 1f; };
            else
                RT2 = () => { return 0f; };
        }

        if (triggersAreButtons3) {
            if (LTPress3() || LTHold3())
                LT3 = () => { return 1f; };
            else
                LT3 = () => { return 0f; };
            if (RTPress3() || RTHold3())
                RT3 = () => { return 1f; };
            else
                RT3 = () => { return 0f; };
        }

        if (triggersAreButtons4) {
            if (LTPress4() || LTHold4())
                LT4 = () => { return 1f; };
            else
                LT4 = () => { return 0f; };
            if (RTPress4() || RTHold4())
                RT4 = () => { return 1f; };
            else
                RT4 = () => { return 0f; };
        }
    }
}
