#if BEHAVIOR_DESIGNER
using UnityEngine;
using System.Collections;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

namespace ICode.Actions.BehaviorDesigner{
	[Category("BehaviorDesigner")]  
	[Tooltip("Synchronize a variable from or to Behavior Designer.")]
	[System.Serializable]
	public class SynchronizeVariable : StateAction {
		[SharedPersistent]
		[Tooltip("The GameObject to use.")]
		public FsmGameObject gameObject;
		[Tooltip("The group of the behavior tree")]
		public FsmInt behaviorTreeGroup;
		public Direction direction;
		[Tooltip("Name of the variable to use.")]
		public FsmString variableName;
		[Tooltip("Execute the action every frame.")]
		public bool everyFrame;

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
			DoSync ();
			if (!everyFrame) {
				Finish();			
			}
		}
		
		public override void OnUpdate ()
		{
			DoSync ();
		}

		private void DoSync(){
			FsmVariable variable = this.Root.GetVariable (variableName.Value);
			SharedVariable sharedVariable = behaviorTree.GetVariable (variableName.Value);
			if (direction == Direction.FromBehaviourTree) {
				variable.SetValue (sharedVariable.GetValue ());			
			} else {
				sharedVariable.SetValue(variable.GetValue());
			}
			
		}
		
		public enum Direction{
			FromBehaviourTree,
			ToBehaviourTree
		}
	}
}
#endif