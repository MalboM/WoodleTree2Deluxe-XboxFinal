using UnityEngine;
using System.Collections;

namespace ICode.Actions.UnityVector2{
	[Category(Category.Vector2)] 
	[Tooltip("Returns this vector with a magnitude of 1.")]
	[HelpUrl("http://docs.unity3d.com/Documentation/ScriptReference/Vector2-normalized.html")]
	[System.Serializable]
	public class Normalize : StateAction {
		[Tooltip("Vector2 to normalize.")]
		public FsmVector2 vector;
		[Shared]
		[Tooltip("Store the result.")]
		public FsmVector2 store;
		[Tooltip("Execute the action every frame.")]
		public bool everyFrame;

		public override void OnEnter ()
		{
			store.Value = vector.Value.normalized;
			if (!everyFrame) {
				Finish ();
			}
		}

		public override void OnUpdate ()
		{
			store.Value = vector.Value.normalized;
		}
	}
}