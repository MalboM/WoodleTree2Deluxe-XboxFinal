using UnityEngine;
using System.Collections;

#if UNITY_5
namespace ICode{
	public class SendEvent : StateMachineBehaviour {
		public string enterEvent;
		public string exitEvent;
		public bool includeChildren;

		public override void OnStateEnter (Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			if(!string.IsNullOrEmpty(enterEvent))
				DoSend (animator.gameObject, enterEvent);
		}

		public override void OnStateExit (Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			if(!string.IsNullOrEmpty(exitEvent))
				DoSend (animator.gameObject, exitEvent);

		}

		private void DoSend(GameObject behaviorGameObject,string eventName){
			if (behaviorGameObject != null) {
				// Find the correct behavior tree based on the grouping
				var behaviorComponents = behaviorGameObject.GetBehaviours (includeChildren);
				if (behaviorComponents != null && behaviorComponents.Length > 0) {
					for (int i = 0; i < behaviorComponents.Length; ++i) {
						behaviorComponents [i].SendEvent (eventName, null);
					}
					
				}
			}
		}
	}
}
#endif