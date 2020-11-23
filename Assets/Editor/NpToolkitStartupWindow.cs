using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

[InitializeOnLoad]
public class StartupWindow : EditorWindow
{
	static int announcementVersion = 3;

    static StartupWindow ()
	{
		EditorApplication.update += CreateWindow;      
    }

	static void CreateWindow()
	{
		EditorApplication.update -= CreateWindow;

		// This will get called when the editor is loaded and also when any script is recompiled, so it uses a temporary editor pref to avoid showing the
		// window whenever scripts are compiled.
		bool skipAnnoucement = EditorPrefs.GetBool("NPT2_Announcement"+announcementVersion);
        
		if ( skipAnnoucement == false)
		{
			StartupWindow window = EditorWindow.GetWindow(typeof(StartupWindow), false, "NpToolkit2 Info", false ) as StartupWindow;

			window.minSize = new Vector2(400, 400);
		}
	}

    static Vector2 scrollPos = Vector2.zero;

    void OnGUI ()
	{
		var standardFontStyle = EditorStyles.label.fontStyle;
		EditorStyles.label.wordWrap = true;

		//EditorGUI.DrawRect(new Rect(5, 5, this.position.width - 10, this.position.height - 10), new Color(0.027f,0.094f,0.207f));

		GUILayout.BeginArea(new Rect (10,10,this.position.width-20,this.position.height-20));

        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

        EditorStyles.label.fontStyle = FontStyle.Bold;
		EditorGUILayout.LabelField("Unity NpToolkit2 Sample");
		EditorGUILayout.Space();

        EditorStyles.label.fontStyle = FontStyle.Bold;
        EditorGUILayout.LabelField("PlayMode Tests");
        EditorStyles.label.fontStyle = standardFontStyle;

        EditorGUILayout.LabelField("PlayMode tests have been added to the sample app. See the Assets/Editor/NpToolkit2_Readme.txt for more details.");

        EditorStyles.label.fontStyle = FontStyle.Bold;
        EditorGUILayout.LabelField("SDK 5.5 Update");
        EditorStyles.label.fontStyle = standardFontStyle;

        EditorGUILayout.LabelField("The Plugin has been compiled against SDK 5.5.");

        EditorStyles.label.fontStyle = FontStyle.Bold;
        EditorGUILayout.LabelField("SDK 5.0 Update");
        EditorStyles.label.fontStyle = standardFontStyle;

        EditorGUILayout.LabelField("The C# API has been updated to match the SDK 5.0 API. Some existing methods and types have been marked as Obsolete.");
        EditorGUILayout.LabelField("New methods have been added to TUS service: Tus.TryAndSetVariable, Tus.GetFriendsVariable, Tus.GetUsersVariable, Tus.GetUsersDataStatus and Tus.GetFriendsDataStatus");
        EditorGUILayout.LabelField("The plugin can be compiled using SDK 4.5 which is backward compatible with the C# SDK 5.0 API.");
        EditorGUILayout.LabelField("Note: The Unity Sample App displays the SDK of the native plugin when started.");
        EditorGUILayout.Space();

        EditorStyles.label.fontStyle = FontStyle.Bold;
		EditorGUILayout.LabelField("ReadMe");
		EditorStyles.label.fontStyle = standardFontStyle;
		EditorGUILayout.LabelField("Setup instructions for the sample and plugin usage can be found in Assets/Editor/NpToolkit2_Readme.txt.");
		EditorGUILayout.Space();

		EditorStyles.label.fontStyle = FontStyle.Bold;
		EditorGUILayout.LabelField("Source Code");
		EditorStyles.label.fontStyle = standardFontStyle;
		EditorGUILayout.LabelField("All source code for the native (C++) and managed DLL (C#) is included in the project. The source code and Visual Studio solutions can be found inside Assets/Plugins/PS4/UnityNpToolkit2_Source.zip.");
		EditorGUILayout.LabelField("You can rebuild the native C++ plugin code and set breakpoints to debug the native code if you encounter problems.");
		EditorGUILayout.LabelField("Many of the C# methods match the same C++ native calls in the Sony NpToolkit library so you can search for these on DevNet to find out more detailed documentation.");
		EditorGUILayout.Space();

		EditorStyles.label.fontStyle = FontStyle.Bold;
		EditorGUILayout.LabelField("Additional");
		EditorStyles.label.fontStyle = standardFontStyle;
		EditorGUILayout.LabelField("To remove this message it is safe to delete the Assets/Editor/NpToolkitStartUpWindow.cs file.");
		EditorGUILayout.LabelField("When importing the sample package into your own project you only require the SonyNP.dll, SonyNP.XML (Intellisense Documentation), and the UnityNpToolkit2.prx files.");
		EditorGUILayout.Space();

		EditorGUILayout.Space();
		EditorGUILayout.BeginHorizontal();
		bool skipAnnoucementCurrent = EditorPrefs.GetBool("NPT2_Announcement"+announcementVersion);
		bool skipAnnoucementNew = EditorGUILayout.Toggle("Don't Show Again ", skipAnnoucementCurrent);

		if ( skipAnnoucementNew != skipAnnoucementCurrent )
		{
			EditorPrefs.SetBool("NPT2_Announcement"+announcementVersion, skipAnnoucementNew);
		}

		if ( GUILayout.Button("Close") == true )
		{
			this.Close();
		}

		EditorGUILayout.EndHorizontal();

        EditorGUILayout.EndScrollView();

        GUILayout.EndArea();
	}
}
