using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

[InitializeOnLoad]
public class SaveDataStartupWindow : EditorWindow
{
	static int announcementVersion = 0;

    static SaveDataStartupWindow()
	{
		EditorApplication.update += CreateWindow;      
    }

	static void CreateWindow()
	{
		EditorApplication.update -= CreateWindow;

		// This will get called when the editor is loaded and also when any script is recompiled, so it uses a temporary editor pref to avoid showing the
		// window whenever scripts are compiled.
		bool skipAnnoucement = EditorPrefs.GetBool("SaveData_Announcement"+announcementVersion);
        
		if ( skipAnnoucement == false)
		{
            SaveDataStartupWindow window = EditorWindow.GetWindow(typeof(SaveDataStartupWindow), false, "SaveData Info", false ) as SaveDataStartupWindow;

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
		EditorGUILayout.LabelField("Unity SaveData Sample");
		EditorGUILayout.Space();

        EditorStyles.label.fontStyle = FontStyle.Bold;
        EditorGUILayout.LabelField("Beta Release");
        EditorStyles.label.fontStyle = standardFontStyle;
        EditorGUILayout.LabelField("This is currently a Beta release for the new SaveData plugin. This means the API might need to change due to improvements and/or bug fixes.");
        EditorGUILayout.Space();

        EditorStyles.label.fontStyle = FontStyle.Bold;
		EditorGUILayout.LabelField("ReadMe");
		EditorStyles.label.fontStyle = standardFontStyle;
		EditorGUILayout.LabelField("Setup instructions for the sample and plugin usage can be found in Assets/Editor/SaveData_Readme.txt.");
		EditorGUILayout.Space();

		EditorStyles.label.fontStyle = FontStyle.Bold;
		EditorGUILayout.LabelField("Source Code");
		EditorStyles.label.fontStyle = standardFontStyle;
		EditorGUILayout.LabelField("All source code for the native (C++) and managed DLL (C#) is included in the project. The source code and Visual Studio solutions can be found inside Assets/Plugins/PS4/SaveData_Source.zip.");
		EditorGUILayout.LabelField("You can rebuild the native C++ plugin code and set breakpoints to debug the native code if you encounter problems.");
		EditorGUILayout.LabelField("Many of the C# methods match the same C++ native calls in the Sony SaveData library so you can search for these on DevNet to find out more detailed documentation.");
		EditorGUILayout.Space();

		EditorStyles.label.fontStyle = FontStyle.Bold;
		EditorGUILayout.LabelField("Additional");
		EditorStyles.label.fontStyle = standardFontStyle;
		EditorGUILayout.LabelField("To remove this message it is safe to delete the Assets/Editor/SaveDataStartUpWindow.cs file.");
		EditorGUILayout.LabelField("When importing the sample package into your own project you only require the SonyPS4SaveData.dll, SonyPS4SaveData.XML (Intellisense Documentation), and the SaveData.prx files.");
		EditorGUILayout.Space();

		EditorGUILayout.Space();
		EditorGUILayout.BeginHorizontal();
		bool skipAnnoucementCurrent = EditorPrefs.GetBool("SaveData_Announcement" + announcementVersion);
		bool skipAnnoucementNew = EditorGUILayout.Toggle("Don't Show Again ", skipAnnoucementCurrent);

		if ( skipAnnoucementNew != skipAnnoucementCurrent )
		{
			EditorPrefs.SetBool("SaveData_Announcement" + announcementVersion, skipAnnoucementNew);
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
