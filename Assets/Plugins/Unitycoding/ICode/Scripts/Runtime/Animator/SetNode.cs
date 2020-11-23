using UnityEngine;
using System.Collections;

#if UNITY_5
namespace ICode{
	public class SetNode : StateMachineBehaviour {
		public int group = 0;
		public string nodeName;

		public override void OnStateEnter (Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			// Find the correct behavior tree based on the grouping
			var behaviorComponents = animator.gameObject.GetComponents<ICodeBehaviour>();
			if (behaviorComponents != null && behaviorComponents.Length > 0) {
				for (int i = 0; i < behaviorComponents.Length; ++i) {
					if(behaviorComponents[i].group==group){
						behaviorComponents[i].SetNode(nodeName);
					}
				}
			}
		}
	}
}
#endif