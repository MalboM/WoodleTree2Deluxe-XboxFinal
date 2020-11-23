using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;

namespace ICode.FSMEditor{
	public class ReorderableObjectList {
		public delegate void HeaderCallbackDelegate(Rect rect);
		public ReorderableObjectList.HeaderCallbackDelegate drawHeaderCallback;
		public delegate void AddCallbackDelegate(ReorderableObjectList list);
		public ReorderableObjectList.AddCallbackDelegate onAddCallback;
		public delegate void RemoveCallbackDelegate(ReorderableObjectList list);
		public ReorderableObjectList.RemoveCallbackDelegate onRemoveCallback;
		public delegate void ElementCallbackDelegate(int index,bool selected);
		public ReorderableObjectList.ElementCallbackDelegate drawElementCallback;
		public delegate void ContextCallbackDelegate(int index);
		public ReorderableObjectList.ContextCallbackDelegate onContextClick;
		public delegate void SelectCallbackDelegate(int index);
		public ReorderableObjectList.SelectCallbackDelegate onSelectCallback;
		public delegate void ReorderCallbackDelegate(ReorderableObjectList list);
		public ReorderableObjectList.ReorderCallbackDelegate onReorderCallback;
		public delegate void OnHeaderContextClick();
		public ReorderableObjectList.OnHeaderContextClick onHeaderContextClick;

		public float headerHeight = 18f;
		public float footerHeight = 13f;
		private int selectedIndex;
		public int index{
			get{return this.selectedIndex;}
			set{

				this.selectedIndex = value;
				if(onSelectCallback != null){
					onSelectCallback(selectedIndex);
				}
			}
		}
		public int count
		{
			get{
				return this.elements.arraySize;
			}
		}
		
		private SerializedObject serializedObject;
		private SerializedProperty elements;
		private bool draggable;
		private bool displayHeader;
		private bool isDragging;
		private Rect draggingLineRect;
		private int dragIndex=-1;
		private int swapIndex=-1;
		private bool selectFirst;

		private ReorderableObjectList.Styles styles;

		public ReorderableObjectList(SerializedObject serializedObject,SerializedProperty elements){
			this.serializedObject = serializedObject;
			this.elements = elements;
			this.draggable = true;
			this.displayHeader = true;
		}
		
		public ReorderableObjectList(SerializedObject serializedObject,SerializedProperty elements,bool draggable,bool displayHeader){
			this.serializedObject = serializedObject;
			this.elements = elements;
			this.draggable = draggable;
			this.displayHeader = displayHeader;
		}
		
		public void DoLayoutList(){
			if (serializedObject.targetObject == null) {
				return;
			}
			if (styles == null) {
				styles= new ReorderableObjectList.Styles();
			}
			if (!selectFirst && onSelectCallback != null) {
				onSelectCallback(index);			
				selectFirst=true;		
			}
			serializedObject.Update ();
			DoListHeader ();
			DoListElements ();
			DoListFooter ();
			serializedObject.ApplyModifiedProperties ();
		}
		
		private void DoListHeader()
		{
			if (displayHeader) {
				Rect headerRect = GUILayoutUtility.GetRect (0f, 18f, new GUILayoutOption[] { GUILayout.ExpandWidth (true) });
				if (Event.current.type == EventType.Repaint)
				{
					styles.headerBackground.Draw(headerRect, false, false, false, false);
				}
				headerRect.xMin = headerRect.xMin + 6f;
				headerRect.xMax = headerRect.xMax - 6f;
				headerRect.height = headerRect.height - 2f;
				headerRect.y = headerRect.y + 1f;
				if (this.drawHeaderCallback != null)
				{
					this.drawHeaderCallback(headerRect);
				}

				if(headerRect.Contains(Event.current.mousePosition) && Event.current.type== EventType.MouseDown && Event.current.button == 1 && onHeaderContextClick != null){
					onHeaderContextClick();
				}
			}
		}

		private void DoListFooter()
		{
			Rect rect = GUILayoutUtility.GetRect(4f, this.footerHeight, new GUILayoutOption[] { GUILayout.ExpandWidth(true) });
			float single = rect.xMax;
			float single1 = single - 8f;
			if (onAddCallback != null)
			{
				single1 = single1 - 25f;
			}
			if (onRemoveCallback != null)
			{
				single1 = single1 - 25f;
			}
			rect = new Rect(single1, rect.y, single - single1, rect.height);
			Rect rect1 = new Rect(single1 + 4f, rect.y - 3f, 25f, 13f);
			Rect rect2 = new Rect(single - 29f, rect.y - 3f, 25f, 13f);
			if (Event.current.type == EventType.Repaint)
			{
				styles.footerBackground.Draw(rect, false, false, false, false);
			}
			if (onAddCallback != null)
			{
				if (GUI.Button(rect1, styles.iconToolbarPlus, styles.preButton))
				{
					onAddCallback(this);
				}
			}
			if (onRemoveCallback != null)
			{
				EditorGUI.BeginDisabledGroup(index < 0 || index >= count);
				if (GUI.Button(rect2, styles.iconToolbarMinus, styles.preButton))
				{
					onRemoveCallback(this);
				}
				EditorGUI.EndDisabledGroup();
			}
		}
		
		private void DoListElements(){
			GUILayout.BeginVertical (styles.boxBackground);
			List<Rect> rects = new List<Rect> ();
			if (count > 0) {
				for (int i=0; i< count; i++) {
					GUILayout.BeginVertical();
					bool flag=GUI.enabled;
					if(doDrag && i==index){
						GUI.enabled=false;
					}
					if(drawElementCallback != null){
						drawElementCallback(i,i==index);
					}
					GUI.enabled=flag;
					GUILayout.EndVertical();

					Rect lastRect=GUILayoutUtility.GetLastRect();
					if(Event.current.type== EventType.Repaint){
						if(doDrag){
							styles.selectBackground.Draw(draggingLineRect,false,false,false,false);
							EditorGUIUtility.AddCursorRect (lastRect, MouseCursor.Pan);	
						}
					}
					rects.Add(lastRect);
				}
			} else {
				GUILayout.Label("List is Empty");
			}
			GUILayout.Space (3f);
			GUILayout.EndVertical ();

			HandleEvents (rects);
		}

		private bool doDrag;
		private void HandleEvents(List<Rect> rects){
			for (int i=0; i< rects.Count; i++) {
				Rect elementRect=rects[i];
				
				switch (Event.current.rawType) {
				case EventType.MouseDown:
					if (elementRect.Contains (Event.current.mousePosition)) {
						if(Event.current.button == 0){
							index=i;
							dragIndex = i;
							isDragging = draggable?true:false;
							draggingLineRect = new Rect (elementRect.x, elementRect.y, elementRect.width, 2);
							GUI.FocusControl ("");
							Event.current.Use();
						}else if(Event.current.button == 1 && onContextClick != null){
							onContextClick(i);
						}
					}
					break;
				case EventType.MouseUp:
					if(isDragging){
						isDragging = false;
						doDrag=false;
						Event.current.Use();
					}
					break;
				case EventType.MouseDrag:

					if (elementRect.Contains (Event.current.mousePosition) && Event.current.button == 0 && count > 1 && draggable && isDragging && Event.current.delta.magnitude>0.1f) {
						doDrag=true;
						if (Event.current.mousePosition.y < elementRect.y + elementRect.height * 0.5f) {
							draggingLineRect = new Rect (elementRect.x, elementRect.y-2f, elementRect.width, 2);
							swapIndex = (dragIndex > i ? i : i - 1);
						}
						if (Event.current.mousePosition.y > elementRect.y + elementRect.height * 0.5f) {
							draggingLineRect = new Rect (elementRect.x, elementRect.y + elementRect.height, elementRect.width, 2);
							swapIndex = (dragIndex > i ? i + 1 : i);
						}
						Event.current.Use();
					}
					break;
				}
			}
			
			if (swapIndex != -1 && !isDragging && dragIndex != -1 && draggable) {
				
				if (count > dragIndex && dragIndex>-1) {
					elements.MoveArrayElement(dragIndex,swapIndex);
					index=swapIndex;
					serializedObject.ApplyModifiedProperties();
					if(onReorderCallback != null){
						onReorderCallback(this);
					}

				}
				swapIndex=-1;
			}
			
			if (!isDragging) {
				dragIndex=-1;	
			}
		}
		
		private class Styles{
			public GUIContent iconToolbarPlus;
			public GUIContent iconToolbarPlusMore;
			public GUIContent iconToolbarMinus;
			
			public readonly GUIStyle draggingHandle;
			public readonly GUIStyle headerBackground;
			public readonly GUIStyle footerBackground;
			public readonly GUIStyle boxBackground;
			public readonly GUIStyle preButton;
			public readonly GUIStyle selectBackground;

			public Styles(){
				iconToolbarPlus=new GUIContent(EditorGUIUtility.FindTexture("Toolbar Plus"));
				iconToolbarPlusMore=new GUIContent(EditorGUIUtility.FindTexture("Toolbar Plus More"));
				iconToolbarMinus=new GUIContent(EditorGUIUtility.FindTexture("Toolbar Minus"));
				
				draggingHandle=new GUIStyle("RL DragHandle");
				headerBackground=new GUIStyle("RL Header");
				footerBackground=new GUIStyle("RL Footer");
				boxBackground= new GUIStyle("RL Background"){
					stretchHeight=false,
				};
				preButton  =new GUIStyle("RL FooterButton");
				selectBackground= new GUIStyle("MeTransitionSelectHead"){
					stretchHeight=false,

				};
				selectBackground.overflow= new RectOffset(-1,-2,-2,2);
			//	selectBackground.fixedHeight=20;
			}
		}
	}
}