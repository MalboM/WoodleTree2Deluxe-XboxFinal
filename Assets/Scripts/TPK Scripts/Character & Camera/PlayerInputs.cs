using UnityEngine;
using System;
using System.Collections;

public class PlayerInputs : MonoBehaviour {

    private GamePadInputs gPI;

    [HideInInspector] public float xAxisL;
    [HideInInspector] public float yAxisL;
    [HideInInspector] public float xAxisR;
    [HideInInspector] public float yAxisR;
    [HideInInspector] public float instrumentTrig;

    [HideInInspector] public float xAxisL2;
    [HideInInspector] public float yAxisL2;
    [HideInInspector] public float xAxisR2;
    [HideInInspector] public float yAxisR2;
    [HideInInspector] public float instrumentTrig2;

    [HideInInspector] public float xAxisL3;
    [HideInInspector] public float yAxisL3;
    [HideInInspector] public float xAxisR3;
    [HideInInspector] public float yAxisR3;
    [HideInInspector] public float instrumentTrig3;

    [HideInInspector] public float xAxisL4;
    [HideInInspector] public float yAxisL4;
    [HideInInspector] public float xAxisR4;
    [HideInInspector] public float yAxisR4;
    [HideInInspector] public float instrumentTrig4;
    

    [HideInInspector] public bool attackPress;
    [HideInInspector] public bool jumpPress;
    [HideInInspector] public bool glidePress;
    [HideInInspector] public bool leafPress;
    [HideInInspector] public bool runPress;
    [HideInInspector] public bool startPress;
    [HideInInspector] public bool attackHold;
    [HideInInspector] public bool jumpHold;
    [HideInInspector] public bool glideHold;
    [HideInInspector] public bool leafHold;
    [HideInInspector] public bool runHold;
    [HideInInspector] public bool startHold;

    [HideInInspector] public bool attackPress2;
    [HideInInspector] public bool jumpPress2;
    [HideInInspector] public bool glidePress2;
    [HideInInspector] public bool leafPress2;
    [HideInInspector] public bool runPress2;
    [HideInInspector] public bool startPress2;
    [HideInInspector] public bool attackHold2;
    [HideInInspector] public bool jumpHold2;
    [HideInInspector] public bool glideHold2;
    [HideInInspector] public bool leafHold2;
    [HideInInspector] public bool runHold2;
    [HideInInspector] public bool startHold2;

    [HideInInspector] public bool attackPress3;
    [HideInInspector] public bool jumpPress3;
    [HideInInspector] public bool glidePress3;
    [HideInInspector] public bool leafPress3;
    [HideInInspector] public bool runPress3;
    [HideInInspector] public bool startPress3;
    [HideInInspector] public bool attackHold3;
    [HideInInspector] public bool jumpHold3;
    [HideInInspector] public bool glideHold3;
    [HideInInspector] public bool leafHold3;
    [HideInInspector] public bool runHold3;
    [HideInInspector] public bool startHold3;

    [HideInInspector] public bool attackPress4;
    [HideInInspector] public bool jumpPress4;
    [HideInInspector] public bool glidePress4;
    [HideInInspector] public bool leafPress4;
    [HideInInspector] public bool runPress4;
    [HideInInspector] public bool startPress4;
    [HideInInspector] public bool attackHold4;
    [HideInInspector] public bool jumpHold4;
    [HideInInspector] public bool glideHold4;
    [HideInInspector] public bool leafHold4;
    [HideInInspector] public bool runHold4;
    [HideInInspector] public bool startHold4;


    Func<float> activeLXaxis;
    Func<float> activeLYaxis;
    Func<float> activeRXaxis;
    Func<float> activeRYaxis;
    Func<float> activeMXaxis;
    Func<float> activeMYaxis;
    Func<float> activeInstrument;

    Func<float> activeLXaxis2;
    Func<float> activeLYaxis2;
    Func<float> activeRXaxis2;
    Func<float> activeRYaxis2;
    Func<float> activeMXaxis2;
    Func<float> activeMYaxis2;
    Func<float> activeInstrument2;

    Func<float> activeLXaxis3;
    Func<float> activeLYaxis3;
    Func<float> activeRXaxis3;
    Func<float> activeRYaxis3;
    Func<float> activeMXaxis3;
    Func<float> activeMYaxis3;
    Func<float> activeInstrument3;

    Func<float> activeLXaxis4;
    Func<float> activeLYaxis4;
    Func<float> activeRXaxis4;
    Func<float> activeRYaxis4;
    Func<float> activeMXaxis4;
    Func<float> activeMYaxis4;
    Func<float> activeInstrument4;


    Func<bool> activeAttackPress;
    Func<bool> activeJumpPress;
    Func<bool> activeGlidePress;
    Func<bool> activeLeafPress;
    Func<bool> activeRunPress;
    Func<bool> activeStartPress;

    Func<bool> activeAttackPress2;
    Func<bool> activeJumpPress2;
    Func<bool> activeGlidePress2;
    Func<bool> activeLeafPress2;
    Func<bool> activeRunPress2;
    Func<bool> activeStartPress2;

    Func<bool> activeAttackPress3;
    Func<bool> activeJumpPress3;
    Func<bool> activeGlidePress3;
    Func<bool> activeLeafPress3;
    Func<bool> activeRunPress3;
    Func<bool> activeStartPress3;

    Func<bool> activeAttackPress4;
    Func<bool> activeJumpPress4;
    Func<bool> activeGlidePress4;
    Func<bool> activeLeafPress4;
    Func<bool> activeRunPress4;
    Func<bool> activeStartPress4;


    Func<bool> activeAttackHold;
    Func<bool> activeJumpHold;
    Func<bool> activeGlideHold;
    Func<bool> activeLeafHold;
    Func<bool> activeRunHold;
    Func<bool> activeStartHold;

    Func<bool> activeAttackHold2;
    Func<bool> activeJumpHold2;
    Func<bool> activeGlideHold2;
    Func<bool> activeLeafHold2;
    Func<bool> activeRunHold2;
    Func<bool> activeStartHold2;

    Func<bool> activeAttackHold3;
    Func<bool> activeJumpHold3;
    Func<bool> activeGlideHold3;
    Func<bool> activeLeafHold3;
    Func<bool> activeRunHold3;
    Func<bool> activeStartHold3;

    Func<bool> activeAttackHold4;
    Func<bool> activeJumpHold4;
    Func<bool> activeGlideHold4;
    Func<bool> activeLeafHold4;
    Func<bool> activeRunHold4;
    Func<bool> activeStartHold4;

    void Start() {
        gPI = this.GetComponent<GamePadInputs>();

        activeLXaxis = () => { return gPI.LH(); };
        activeLYaxis = () => { return gPI.LV(); };
        activeRXaxis = () => { return gPI.RH(); };
        activeRYaxis = () => { return gPI.RV(); };
        activeMXaxis = () => { return gPI.MX(); };
        activeMYaxis = () => { return gPI.MY(); };
        activeInstrument = () => { return gPI.RT(); };

        activeLXaxis2 = () => { return gPI.LH2(); };
        activeLYaxis2 = () => { return gPI.LV2(); };
        activeRXaxis2 = () => { return gPI.RH2(); };
        activeRYaxis2 = () => { return gPI.RV2(); };
        activeMYaxis2 = () => { return gPI.MY2(); };
        activeInstrument2 = () => { return gPI.RT2(); };

        activeLXaxis3 = () => { return gPI.LH3(); };
        activeLYaxis3 = () => { return gPI.LV3(); };
        activeRXaxis3 = () => { return gPI.RH3(); };
        activeRYaxis3 = () => { return gPI.RV3(); };
        activeMYaxis3 = () => { return gPI.MY3(); };
        activeInstrument3 = () => { return gPI.RT3(); };

        activeLXaxis4 = () => { return gPI.LH4(); };
        activeLYaxis4 = () => { return gPI.LV4(); };
        activeRXaxis4 = () => { return gPI.RH4(); };
        activeRYaxis4 = () => { return gPI.RV4(); };
        activeMYaxis4 = () => { return gPI.MY4(); };
        activeInstrument4 = () => { return gPI.RT4(); };


        activeAttackPress = () => { return gPI.pressRightB(); };
        activeJumpPress = () => { return gPI.pressAction(); };
        activeGlidePress = () => { return gPI.pressTopB(); };
        activeLeafPress = () => { return gPI.pressTopB(); };
        activeRunPress = () => { return gPI.pressLeftB(); };
        activeStartPress = () => { return gPI.pressStart(); };
        activeAttackHold = () => { return gPI.holdRightB(); };
        activeJumpHold = () => { return gPI.holdAction(); };
        activeGlideHold = () => { return gPI.holdTopB(); };
        activeLeafHold = () => { return gPI.holdTopB(); };
        activeRunHold = () => { return gPI.holdLeftB(); };
        activeStartPress = () => { return gPI.holdStart(); };

        activeAttackPress2 = () => { return gPI.pressRightB2(); };
        activeJumpPress2 = () => { return gPI.pressAction2(); };
        activeGlidePress2 = () => { return gPI.pressTopB2(); };
        activeLeafPress2 = () => { return gPI.pressTopB2(); };
        activeRunPress2 = () => { return gPI.pressLeftB2(); };
        activeStartPress2 = () => { return gPI.pressStart2(); };
        activeAttackHold2 = () => { return gPI.holdRightB2(); };
        activeJumpHold2 = () => { return gPI.holdAction2(); };
        activeGlideHold2 = () => { return gPI.holdTopB2(); };
        activeLeafHold2 = () => { return gPI.holdTopB2(); };
        activeRunHold2 = () => { return gPI.holdLeftB2(); };
        activeStartPress2 = () => { return gPI.holdStart2(); };

        activeAttackPress3 = () => { return gPI.pressRightB3(); };
        activeJumpPress3 = () => { return gPI.pressAction3(); };
        activeGlidePress3 = () => { return gPI.pressTopB3(); };
        activeLeafPress3 = () => { return gPI.pressTopB3(); };
        activeRunPress3 = () => { return gPI.pressLeftB3(); };
        activeStartPress3 = () => { return gPI.pressStart3(); };
        activeAttackHold3 = () => { return gPI.holdRightB3(); };
        activeJumpHold3 = () => { return gPI.holdAction3(); };
        activeGlideHold3 = () => { return gPI.holdTopB3(); };
        activeLeafHold3 = () => { return gPI.holdTopB3(); };
        activeRunHold3 = () => { return gPI.holdLeftB3(); };
        activeStartPress3 = () => { return gPI.holdStart3(); };

        activeAttackPress4 = () => { return gPI.pressRightB4(); };
        activeJumpPress4 = () => { return gPI.pressAction4(); };
        activeGlidePress4 = () => { return gPI.pressTopB4(); };
        activeLeafPress4 = () => { return gPI.pressTopB4(); };
        activeRunPress4 = () => { return gPI.pressLeftB4(); };
        activeStartPress4 = () => { return gPI.pressStart4(); };
        activeAttackHold4 = () => { return gPI.holdRightB4(); };
        activeJumpHold4 = () => { return gPI.holdAction4(); };
        activeGlideHold4 = () => { return gPI.holdTopB4(); };
        activeLeafHold4 = () => { return gPI.holdTopB4(); };
        activeRunHold4 = () => { return gPI.holdLeftB4(); };
        activeStartPress4 = () => { return gPI.holdStart4(); };
    }

	void Update () {
        xAxisL = activeLXaxis();
        yAxisL = activeLYaxis();

        xAxisL2 = activeLXaxis2();
        yAxisL2 = activeLYaxis2();

        xAxisL3 = activeLXaxis3();
        yAxisL3 = activeLYaxis3();

        xAxisL4 = activeLXaxis4();
        yAxisL4 = activeLYaxis4();


        if (activeRXaxis() != 0f)
            xAxisR = activeRXaxis();
        else
            xAxisR = activeMXaxis();
        if (activeRYaxis() != 0f)
            yAxisR = activeRYaxis();
        else
            yAxisR = activeMYaxis();

        instrumentTrig = activeInstrument();
        instrumentTrig2 = activeInstrument2();
        instrumentTrig3 = activeInstrument3();
        instrumentTrig4 = activeInstrument4();

        attackPress = activeAttackPress();
        attackHold = activeAttackHold();
        attackPress2 = activeAttackPress2();
        attackHold2 = activeAttackHold2();
        attackPress3 = activeAttackPress3();
        attackHold3 = activeAttackHold3();
        attackPress4 = activeAttackPress4();
        attackHold4 = activeAttackHold4();

        jumpPress = activeJumpPress();
        jumpHold = activeJumpHold();
        jumpPress2 = activeJumpPress2();
        jumpHold2 = activeJumpHold2();
        jumpPress3 = activeJumpPress3();
        jumpHold3 = activeJumpHold3();
        jumpPress4 = activeJumpPress4();
        jumpHold4 = activeJumpHold4();

        glidePress = activeGlidePress();
        glideHold = activeGlideHold();
        glidePress2 = activeGlidePress2();
        glideHold2 = activeGlideHold2();
        glidePress3 = activeGlidePress3();
        glideHold3 = activeGlideHold3();
        glidePress4 = activeGlidePress4();
        glideHold4 = activeGlideHold4();

        leafPress = activeLeafPress();
        leafHold = activeLeafHold();
        leafPress2 = activeLeafPress2();
        leafHold2 = activeLeafHold2();
        leafPress3 = activeLeafPress3();
        leafHold3 = activeLeafHold3();
        leafPress4 = activeLeafPress4();
        leafHold4 = activeLeafHold4();

        runPress = activeRunPress();
        runHold = activeRunHold();
        runPress2 = activeRunPress2();
        runHold2 = activeRunHold2();
        runPress3 = activeRunPress3();
        runHold3 = activeRunHold3();
        runPress4 = activeRunPress4();
        runHold4 = activeRunHold4();

        startPress = activeStartPress();
        startHold = activeStartPress();
        startPress2 = activeStartPress2();
        startHold2 = activeStartPress2();
        startPress3 = activeStartPress3();
        startHold3 = activeStartPress3();
        startPress4 = activeStartPress4();
        startHold4 = activeStartPress4();

        //SYNTAX TO CHANGE BUTTON //e.g.//    ChangeButton(ref activeStartPress, gPI.pressLBump);
    }

    void ChangeButton(ref Func<bool> boolToChange, Func<bool> newValue) {
        boolToChange = () => { return newValue(); };
    }

}
