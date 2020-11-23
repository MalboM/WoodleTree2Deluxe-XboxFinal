using UnityEngine;
using System.Collections;

namespace ICode.Actions.UnityVector3{
	[Category(Category.Vector3)] 
	[Tooltip("Moves a point current towards target. This is essentially the same as Vector2.Lerp but instead the function will ensure that the speed never exceeds maxDistanceDelta. Negative values of maxDistanceDelta pushes the vector away from target.")]
	[HelpUrl("http://docs.unity3d.com/ScriptReference/Vector3.MoveTowards.html")]
	[System.Serializable]
	public class MoveTowards : StateAction {
		[Tooltip("Vector3 value.")]
		public FsmVector3 current;
		[Tooltip("Vector3 value.")]
		public FsmVector3 target;
		public FsmFloat maxDistanceDelta;
		[Shared]
		[Tooltip("Store the result.")]
		public FsmVector3 store;
		[Tooltip("Execute the action every frame.")]
		public bool everyFrame;
		
		public override void OnEnter ()
		{
			store.Value = Vector3.MoveTowards (current.Value, target.Value, maxDistanceDelta.Value);
			if (!everyFrame) {
				Finish ();
			}
		}
		
		public override void OnUpdate ()
		{
			store.Value = Vector3.MoveTowards (current.Value, target.Value, maxDistanceDelta.Value);
		}
	}
}