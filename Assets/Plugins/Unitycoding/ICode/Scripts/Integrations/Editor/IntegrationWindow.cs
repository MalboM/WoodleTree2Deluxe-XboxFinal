using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Collections;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;

namespace ICode.FSMEditor{
	public class IntegrationWindow : EditorWindow {

		public static void ShowWindow()
		{
			EditorWindow.GetWindow<IntegrationWindow>(true, "Third Party Integrations", true);
		}

		private List<BuildTargetGroup> buildTargets;
		private Vector2 scrollPosition;
		[SerializeField]
		private List<string> queue=new List<string>();

		private void OnEnable(){
			buildTargets = Enum.GetValues(typeof(BuildTargetGroup)).Cast<BuildTargetGroup>().Where(x=>!IsObsolete(x)).ToList();
			buildTargets.Remove (BuildTargetGroup.Unknown);
		}

		private void OnGUI(){
			scrollPosition = GUILayout.BeginScrollView (scrollPosition);
			ShowIntegration ("DOTween","DOTween is a fast, efficient, fully type-safe object-oriented animation engine, optimized for C#.","/content/27676","","DOTWEEN",null,null);
			ShowIntegration ("Energy Bar Toolkit","Energy Bar Toolkit is a set of scripts to help you create progress or health bars that you need.","/content/7515","","ENERGY_BAR_TOOLKIT",null,null);
			ShowIntegration ("PoolManager","PoolManager, the original and best instance pooling solution for Unity, manages instances more efficiently to increase performance…","/content/1010","","POOL_MANAGER",null,null);
			ShowIntegration ("Easy Touch 4","EasyTouch allows you to quickly and easily develop actions based on a touchscreen gesture, joystick , button, DPadn TouchPad with EasyTouch controls.","/content/3322","","EASY_TOUCH_4",null,null);
			ShowIntegration ("UFPS","Ultimate FPS is a next-gen first person framework featuring procedural motion of first person controllers, cameras and weapons.","/content/2943","http://zerano-unity3d.com/docs/home/icode/examples/","UFPS",null,null);
			ShowIntegration ("Photon Unity Networking","Photon Unity Networking (PUN) is a Unity package for multiplayer games. It provides authentication options, matchmaking and fast, reliable in-game communication through Photon backend.","/content/1786","http://zerano-unity3d.com/docs/home/icode/examples/","PUN",null,null);
			ShowIntegration ("Master Audio","Master Audio is by far the most popular audio plugin on the Asset Store.","/content/5607","","MASTER_AUDIO",null,null);
			ShowIntegration ("NGUI","NGUI is a powerful UI system and event notification framework for Unity (both Pro and Free) written in C#.","/content/2413","","NGUI",null,null);
			ShowIntegration ("Apex Path","Apex Path is a high performance, easy to use dynamic pathfinding solution developed by Apex Game Tools.","/content/17943","","APEX_PATH",null,null);
			ShowIntegration ("UMA","UMA – Unity Multipurpose Avatar, is an open source avatar creation framework, it provides both base code and example content to create avatars. Using the UMA pack, it´s possible to customize the code and content for your own projects, and share or sell your creations through Unity Asset Store.","/content/13930","","UMA",null,null);
			ShowIntegration ("A* Pathfinding Project","A* Pathfinding Project is a lightning fast pathfinding for Unity3D. Whether you write a TD, RTS, FPS or RPG game, this package is for you.","/content/1876","http://zerano-unity3d.com/docs/home/icode/examples/","A_PATHFINDING_PROJECT",null,null);
			ShowIntegration ("CoreGameKit","Pooling & combat systems, pickup support, player stats, enemy waves etc.","/content/6640","","CORE_GAME_KIT",null,null);
			ShowIntegration ("PlayMaker","PlayMaker is an event driven visual scripting tool for unity.","/content/368","","PLAYMAKER",null,null);
			ShowIntegration ("Behavior Designer","Behavior Designer is a behavior tree implementation designed for everyone – programmers, artists, designers.","/content/15277","","BEHAVIOR_DESIGNER",null,null);
			ShowIntegration ("MySQL","MySql actions similiar to PlayerPrefs using php scripts.","","","MYSQL",null,null);
			ShowIntegration ("Highlighting System","Highlighting System package allows you to easily integrate outline glow effect similar to those used in top AAA games.","/content/41508","","HIGHLIGHTING_SYSTEM",null,null);
			ShowIntegration ("Photon Bolt","Networking middleware for Unity 3D and realtime multiplayer games and applications.","/content/41330","","PHOTON_BOLT",null,null);
			GUILayout.EndScrollView ();
		}

		private void Update(){
			if (queue.Count > 0 && !EditorApplication.isCompiling) {
				MethodInfo method=GetType().GetMethod(queue[0]);
				method.Invoke(this,null);
				queue.RemoveAt(0);
			}
		}

		private void ShowIntegration(string name,string description,string link,string exampleLink, string defName, Action onEnable, Action onDisable){
			GUILayout.BeginVertical ("box");
			if (GUILayout.Toggle(IsIntegrationEnabled(defName), name)){
				EnableIntegration(defName, onEnable);
			}else{
				DisableIntegration(defName,onDisable);
			}
			GUILayout.Label (description,EditorStyles.wordWrappedLabel);
			GUILayout.BeginHorizontal ();
			GUILayout.FlexibleSpace ();
			Color color = GUI.contentColor;
			GUI.contentColor = new Color(0,0,0.7f,0.9f);
			if (!string.IsNullOrEmpty (exampleLink) && GUILayout.Button ("Example","label")) {
				Application.OpenURL(exampleLink);
			}
			GUILayout.Space (3);
			if (!string.IsNullOrEmpty(link) && GUILayout.Button ("Asset Store","label")) {
				AssetStore.Open(link);
			}
			GUI.contentColor = color;
			GUILayout.EndHorizontal ();
			GUILayout.EndVertical ();
		}
		
		private void DisableIntegration(string name, Action onDisable)
		{
			if (IsIntegrationEnabled (name) == false) {
				return;
			}
			if (onDisable != null) {
				onDisable.Invoke();
			}
			foreach (BuildTargetGroup group in buildTargets)
			{
				string symbols = UnityEditor.PlayerSettings.GetScriptingDefineSymbolsForGroup(group);
				string[] split = symbols.Split(';');
				var list = new List<string>(split);
				list.Remove(name);
				if(group != BuildTargetGroup.Unknown){
					UnityEditor.PlayerSettings.SetScriptingDefineSymbolsForGroup(group, string.Join(";", list.ToArray()));
				}
			}
		}
		
		private void EnableIntegration(string name, Action onEnable)
		{
			if (IsIntegrationEnabled (name)) {
				return;
			}
			foreach (BuildTargetGroup group in buildTargets)
			{
				string symbols = UnityEditor.PlayerSettings.GetScriptingDefineSymbolsForGroup(group);
				string[] split = symbols.Split(';');
				var list = new List<string>(split);
				list.Add(name);
				if(group != BuildTargetGroup.Unknown){
					UnityEditor.PlayerSettings.SetScriptingDefineSymbolsForGroup(group, string.Join(";", list.ToArray()));
				}
			}
			if (onEnable != null) {
				queue.Add(onEnable.Method.Name);
			}
		}
		
		private bool IsIntegrationEnabled(string name)
		{
			return UnityEditor.PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone).Contains(name);
		}

		private static bool IsObsolete(Enum value)
		{
			var fi = value.GetType().GetField(value.ToString());
			var attributes = (ObsoleteAttribute[])
				fi.GetCustomAttributes(typeof(ObsoleteAttribute), false);
			
			return (attributes != null && attributes.Length > 0);
		}
	}
}