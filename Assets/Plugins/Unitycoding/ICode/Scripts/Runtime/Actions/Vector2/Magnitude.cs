using UnityEngine;
using System.Collections;

namespace ICode.Actions.UnityVector2{
	[Category(Category.Vector2)]    
	[Tooltip("Returns the length of this vector.")]
	[HelpUrl("http://docs.unity3d.com/Documentation/ScriptReference/Vector2-magnitude.html")]
	[System.Serializable]
	public class Magnitude : StateAction {
		[Tooltip("Vector2 value.")]
		public FsmVector2 vector;
		[Shared]
		[Tooltip("Store the result.")]
		public FsmFloat store;
		[Tooltip("Execute the action every frame.")]
		public bool everyFrame;

		public override void OnEnter ()
		{
			store.Value = vector.Value.magnitude;
			if (!everyFrame) {
				Finish ();
			}
		}

		public override void OnUpdate ()
		{
			store.Value = vector.Value.magnitude;
		}
	}
}