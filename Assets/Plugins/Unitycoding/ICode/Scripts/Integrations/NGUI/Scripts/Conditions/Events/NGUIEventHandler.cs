#if NGUI
using UnityEngine;
using System.Collections;

namespace ICode{
	public class NGUIEventHandler : MonoBehaviour {
		public delegate void Trigger();
		public NGUIEventType type;
		public event Trigger onTrigger;

		private void OnClick(){
			if (onTrigger != null && type == NGUIEventType.OnClick) {
				onTrigger();			
			}
		}

		private void OnDoubleClick(){
			if (onTrigger != null && type == NGUIEventType.OnDoubleClick) {
				onTrigger();			
			}
		}

		private void OnHover(bool isOver){
			if (onTrigger != null && type == NGUIEventType.OnHover) {
				onTrigger();			
			}
		}

		private void OnPress(bool isDown){
			if (onTrigger != null && type == NGUIEventType.OnPress) {
				onTrigger();			
			}
		}

		private void OnTooltip(bool show){
			if (onTrigger != null && type == NGUIEventType.OnTooltip) {
				onTrigger();			
			}
		}
	}
}
#endif