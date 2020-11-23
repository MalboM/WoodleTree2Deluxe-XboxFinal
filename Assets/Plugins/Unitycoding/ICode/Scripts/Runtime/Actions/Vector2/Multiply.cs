using UnityEngine;
using System.Collections;

namespace ICode.Actions.UnityVector2{
	[Category(Category.Vector2)] 
	[Tooltip("Multiplies a vector by a number.")]
	[HelpUrl("https://docs.unity3d.com/Documentation/ScriptReference/Vector2-operator_multiply.html")]
	[System.Serializable]
	public class Multiply : StateAction {
		[Tooltip("Vector2 value.")]
		public FsmVector2 vector;
		[Tooltip("Float value to multiply with.")]
		public FsmFloat a;
		[Shared]
		[Tooltip("Store the result.")]
		public FsmVector2 store;
		[Tooltip("Execute the action every frame.")]
		public bool everyFrame;

		public override void OnEnter ()
		{
			store.Value = vector.Value*a.Value;
			if (!everyFrame) {
				Finish ();
			}
		}

		public override void OnUpdate ()
		{
			store.Value = vector.Value*a.Value;
		}
	}
}