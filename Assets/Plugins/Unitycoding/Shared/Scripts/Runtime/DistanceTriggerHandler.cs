using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

namespace Unitycoding{
	public class DistanceTriggerHandler : MonoBehaviour {
		[Tag]
		public string triggerTag="Player";
        public string triggerName = "Woodle Character";
        public TriggerType triggerType;
		public KeyCode key=KeyCode.None;
		public float distance = 2f;
		protected GameObject triggerGameObject;
		
		protected virtual void Start(){
			GameObject trigger = new GameObject ("Distance Trigger");
			trigger.transform.parent = gameObject.transform;
			trigger.transform.localPosition = Vector3.zero;
			DistanceTrigger distanceTrigger = trigger.AddComponent<DistanceTrigger> ();
			distanceTrigger.triggerTag = triggerTag;
			distanceTrigger.distance = distance;
			distanceTrigger.onTriggerEnter = new DistanceTrigger.TriggerEvent ();
			distanceTrigger.onTriggerEnter.AddListener (delegate(GameObject go) {
				triggerGameObject=go;
				enabled=true;
				if(triggerType == TriggerType.OnTriggerEnter || triggerType == TriggerType.All){
					Execute();
				}
			});
			distanceTrigger.onTriggerExit = new DistanceTrigger.TriggerEvent ();
			distanceTrigger.onTriggerExit.AddListener (delegate(GameObject go) {
				triggerGameObject=go;
				enabled=false;
				
			});
			enabled = false;
		}
		
		protected virtual void Execute(){}
		
		
		private void Update(){
			if (!this.enabled) {
				return;
			}
			if (triggerType == TriggerType.Key && Input.GetKeyDown (key) || triggerType == TriggerType.All) {
				Execute ();
			}
		}
		
		private void OnMouseDown(){
			if (!this.enabled || EventSystem.current != null && EventSystem.current.IsPointerOverGameObject ())
			{
				OutOfRange();
				return;
			}
			if (triggerType == TriggerType.OnMouseDown || triggerType == TriggerType.All) {
				Execute ();
			}
		}
		
		protected virtual void OutOfRange(){}
		
		public enum TriggerType{
			OnMouseDown,
			OnTriggerEnter,
			Key,
			All
		}
	}
}