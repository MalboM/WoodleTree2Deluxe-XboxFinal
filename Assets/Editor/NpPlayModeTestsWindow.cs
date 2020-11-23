using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

public class NpPlayModeTestWindow : EditorWindow
{

    [MenuItem("SCE Publishing Utils/Configure PlayMode Tests")]
    static void CreateWindow()
    {
        NpPlayModeTestWindow window = EditorWindow.GetWindow(typeof(NpPlayModeTestWindow), false, "Tests", false) as NpPlayModeTestWindow;

        window.minSize = new Vector2(400, 400);

        window.ReadTestState();
        window.BuildDisplayNames();
    }

    enum PlayModeTests
    {
        Friends,
        Matching,
        Network_Utils,
        NpUtils,
        Presence,
        Ranking,
        Trophy,
        TSS,
        TUS,
        User_Profile,
        Word_Filter,

        MaxTests, // Must be last in the list
    }

    const string initTestsDefine = "INIT";
    const string prefixDefine = "NPT2_";
    const string suffixDefine = "_TESTS";

    bool[] testsEnabled = null;
    string[] displayNames = null;

    void BuildDisplayNames()
    {
        int count = (int)PlayModeTests.MaxTests;

        displayNames = new string[count];

        for (int i = 0; i < count; i++)
        {
            string testName = ((PlayModeTests)i).ToString();
            displayNames[i] = testName.Replace("_", " ");
        }
    }

    void ReadTestState()
    {
        testsEnabled = new bool[(int)PlayModeTests.MaxTests];

        string defineSettings = PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.PS4);

        for (int i = 0; i < testsEnabled.Length; i++)
        {
            testsEnabled[i] = false;

            string testName = ((PlayModeTests)i).ToString();
            testName = testName.ToUpper();

            testName = prefixDefine + testName + suffixDefine;

            if (defineSettings.Contains(testName) == true)
            {
                testsEnabled[i] = true;
            }
        }
    }

    void SetTestState(int index)
    {
        string testName = ((PlayModeTests)index).ToString();
        testName = testName.ToUpper();
        testName = prefixDefine + testName + suffixDefine;

        if (testsEnabled[index] == false)
        {
            RemoveDefine(testName);
        }
        else
        {
            SetDefine(testName);
        }

        CheckForInitialiseTest();
    }

    void RemoveDefine(string define)
    {
        string defineSettings = PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.PS4);

        string defineNamePlusSemiColon = define + ";";
        // Disable test
        if (defineSettings.Contains(defineNamePlusSemiColon) == true)
        {
            defineSettings = defineSettings.Replace(defineNamePlusSemiColon, "");
        }
        else if (defineSettings.Contains(define) == true)
        {
            defineSettings = defineSettings.Replace(define, "");
        }

        PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.PS4, defineSettings);
    }

    void SetDefine(string define)
    {
        string defineSettings = PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.PS4);

        // Does the current define settings end with a semicolon, if not add one.
        if (defineSettings.EndsWith(";") == false)
        {
            defineSettings += ";";
        }

        // Enable test
        defineSettings += define + ";";
        PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.PS4, defineSettings);
    }

    void GetTestsState(out bool atLeastOneTestEnabled, out bool allTestsEnable)
    {
        atLeastOneTestEnabled = false;
        allTestsEnable = true;

        for (int i = 0; i < testsEnabled.Length; i++)
        {
            if (testsEnabled[i] == true)
            {
                atLeastOneTestEnabled = true;
            }
            else
            {
                allTestsEnable = false;
            }
        }
    }

    /// <summary>
    /// Check it any test is initialised and if so enable the NPINITTEST define, otherwise remove it.
    /// </summary>
    void CheckForInitialiseTest()
    {
        bool atLeastOneTestEnabled = false;
        bool allTestsEnable = false;

        GetTestsState(out atLeastOneTestEnabled, out allTestsEnable);

        string defineSettings = PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.PS4);

        string initDefine = prefixDefine + initTestsDefine + suffixDefine;

        if (atLeastOneTestEnabled == true)
        {
            // Must make sure NPINITTEST is set
            if(defineSettings.Contains(initDefine) == false)
            {
                SetDefine(initDefine);
            }
        }
        else
        {
            // Must remove NPINITTEST is there are no tests defined
            if (defineSettings.Contains(initDefine) == true)
            {
                RemoveDefine(initDefine);
            }
        }
    }

    void OnGUI ()
	{
        EditorStyles.label.wordWrap = true;

        EditorGUILayout.LabelField("Enable PlayMode Tests");

        bool atLeastOneTestEnabled = false;
        bool allTestsEnable = false;

        GetTestsState(out atLeastOneTestEnabled, out allTestsEnable);

        bool enableAll = GUILayout.Toggle(allTestsEnable, new GUIContent("All"));

        if (allTestsEnable != enableAll)
        {
            if (enableAll == false)
            {
                // Disable all tests
                for (int i = 0; i < testsEnabled.Length; i++)
                {
                    if (testsEnabled[i] == true)
                    {
                        testsEnabled[i] = false;
                        SetTestState(i);
                    }
                }
            }
            else
            {
                // Enable all tests
                for (int i = 0; i < testsEnabled.Length; i++)
                {
                    if (testsEnabled[i] == false)
                    {
                        testsEnabled[i] = true;
                        SetTestState(i);
                    }
                }
            }
        }

        EditorGUILayout.Space();

        for (int i = 0; i < testsEnabled.Length; i++)
        {
            bool oldValue = testsEnabled[i];
            testsEnabled[i] = GUILayout.Toggle(testsEnabled[i], new GUIContent(displayNames[i]));

            if (oldValue != testsEnabled[i])
            {
                // Test value changed. Either remove the test or enable it
                SetTestState(i);
            }
        }

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Warning: When enabling/disabling any of these test groups they will only be displayed in the PlayMode window once the editor has finished compiling the script code.");

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Remember to enable PlayMode tests from the 'Test Runner' window. See 'Test Runner' documentation on how to run PlayMode tests.");
    }
}
