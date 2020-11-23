#if PUN
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

namespace ICode{
	public class PrefabCacheEditor : EditorWindow {
		[MenuItem ("Tools/Unitycoding/ICode/Photon/PrefabCache")]
		static void Init () {
			PrefabCacheEditor editor= (PrefabCacheEditor)EditorWindow.GetWindow (typeof (PrefabCacheEditor));
			#if UNITY_4_6 || UNITY_4_7 || UNITY_5_0 || UNITY_5_1
			editor.title="Prefab Cache";
			#else
			editor.titleContent=new GUIContent("Prefab Cache");
			#endif
		}
		
		private PrefabCache cache;
		private Vector2 scroll;
		
		private void Update(){
			if (cache == null && !EditorApplication.isCompiling) {
				cache = (PrefabCache)Resources.Load ("PrefabCache");
				if (cache == null) {
					if (!System.IO.Directory.Exists(Application.dataPath + "/ICode/Scripts/Photon Support/Resources")) {
						AssetDatabase.CreateFolder("Assets/ICode/Scripts/Photon Support", "Resources");
					}	
					cache= ICode.FSMEditor.AssetCreator.CreateAsset<PrefabCache>("Assets/ICode/Scripts/Photon Support/Resources/PrefabCache.asset");
					EditorUtility.DisplayDialog("Created PrefabCache!",
					                            "Do not delete or rename the Resource folder and the PrefabCache asset.", "Ok");
				}
				return;
			}
			
			if (cache != null) {
				if (cache.prefabs == null) {
					cache.prefabs = new List<PrefabCache.PrefabInstance> ();			
				}
				
				if (cache.prefabs.Count == 0 || cache.prefabs [cache.prefabs.Count - 1].prefab != null) {
					cache.prefabs.Add (new PrefabCache.PrefabInstance ());
				}
			}
		}
		
		private void OnGUI(){
			if (cache == null) {
				return;			
			}
			scroll = GUILayout.BeginScrollView(scroll);
			GUI.backgroundColor=new Color(0.5f, 0.5f, 0.5f);
			GUILayout.BeginHorizontal ("AS TextArea",GUILayout.Height(21));
			GUILayout.Space (2);
			GUILayout.Label ("Name");
			GUILayout.Label ("Prefab");
			GUILayout.EndHorizontal ();
			GUI.backgroundColor = Color.white;
			Draw ();
			GUILayout.EndScrollView ();
		}
		
		private void Draw(){
			SerializedObject cacheObject = new SerializedObject (cache);
			cacheObject.Update();
			SerializedProperty property = cacheObject.FindProperty ("prefabs");
			if (property != null) {
				int removeIndex=-1;
				for(int i=0;i< property.arraySize;i++){
					GUILayout.BeginHorizontal();
					SerializedProperty nameProperty=property.GetArrayElementAtIndex(i).FindPropertyRelative("name");
					EditorGUILayout.PropertyField(nameProperty,GUIContent.none);
					
					SerializedProperty prefabProperty=property.GetArrayElementAtIndex(i).FindPropertyRelative("prefab");
					bool isNull=prefabProperty.objectReferenceValue==null;
					EditorGUILayout.PropertyField(prefabProperty,GUIContent.none);
					if(isNull && prefabProperty.objectReferenceValue!=null){
						nameProperty.stringValue=prefabProperty.objectReferenceValue.name;
					}  
					if(GUILayout.Button(EditorGUIUtility.FindTexture("Toolbar Minus"),"label",GUILayout.Width(20))){
						removeIndex=i;
					}
					GUILayout.EndHorizontal();
				}			
				
				if(removeIndex != -1){
					property.DeleteArrayElementAtIndex(removeIndex);
				}
			}
			cacheObject.ApplyModifiedProperties();
		}
	}
}
#endif