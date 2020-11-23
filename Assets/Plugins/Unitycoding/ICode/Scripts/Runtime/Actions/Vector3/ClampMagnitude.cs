using UnityEngine;
using System.Collections;

namespace ICode.Actions.UnityVector3{
	[Category(Category.Vector3)]  
	[Tooltip("Returns a vector with its magnitude clamped to maxLength.")]
	[HelpUrl("http://docs.unity3d.com/ScriptReference/Vector3.ClampMagnitude.html")]
	[System.Serializable]
	public class ClampMagnitude : StateAction {
		[Tooltip("The vector to clamp.")]
		public FsmVector3 vector;
		[Tooltip("Max length of the vector")]
		public FsmFloat maxLength;
		[Shared]
		[Tooltip("Store the result.")]
		public FsmVector3 store;
		[Tooltip("Execute the action every frame.")]
		public bool everyFrame;
		
		public override void OnEnter ()
		{
			store.Value = Vector3.ClampMagnitude (vector.Value, maxLength.Value);
			if (!everyFrame) {
				Finish ();
			}
		}
		
		public override void OnUpdate ()
		{
			store.Value = Vector3.ClampMagnitude (vector.Value, maxLength.Value);
		}
	}
}