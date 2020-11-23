#if BEHAVIOR_DESIGNER
using UnityEngine;
using System.Collections;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

namespace ICode.Actions.BehaviorDesigner{
	[Category("BehaviorDesigner")]   
	[Tooltip("Starts a Behaviour Tree.")]
	[System.Serializable]
	public class StartBehaviorTree : StateAction {
		[SharedPersistent]
		[Tooltip("The GameObject to use.")]
		public FsmGameObject gameObject;
		[Tooltip("The group of the behavior tree")]
		public FsmInt behaviorTreeGroup;
		[Shared]
		[NotRequired]
		[Tooltip("The results after the behavior tree has finished running.")]
		public FsmBool storeSuccess;
		private BehaviorTree behaviorTree;

		public override void OnEnter ()
		{
			GameObject behaviorTreeGameObject = (GameObject)gameObject.Value;
			// Find the correct behavior tree based on the grouping
			var behaviorTreeComponents = behaviorTreeGameObject.GetComponents<BehaviorTree>();
			if (behaviorTreeComponents != null && behaviorTreeComponents.Length > 0) {
				behaviorTree = behaviorTreeComponents[0];
				//  We don't need the behaviorTreeGroup if there is only one behavior tree component
				if (behaviorTreeComponents.Length > 1) {
					for (int i = 0; i < behaviorTreeComponents.Length; ++i) {
						if (behaviorTreeComponents[i].Group == behaviorTreeGroup.Value) {
							// Cache the result when we have a match and stop looping.
							behaviorTree = behaviorTreeComponents[i];
							break;
						}
					}
				}
			}
			if (behaviorTree.ExecutionStatus == TaskStatus.Inactive) {
				behaviorTree.EnableBehavior ();
			}
		}

		public override void OnUpdate ()
		{
			if (behaviorTree != null && behaviorTree.ExecutionStatus != TaskStatus.Running) {
				storeSuccess.Value = behaviorTree.ExecutionStatus == TaskStatus.Success;
				Finish ();
			} 
		}
	}
}
#endif