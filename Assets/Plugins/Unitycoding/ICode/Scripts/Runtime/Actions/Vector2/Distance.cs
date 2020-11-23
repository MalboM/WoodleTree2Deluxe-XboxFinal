using UnityEngine;
using System.Collections;

namespace ICode.Actions.UnityVector2{
	[Category(Category.Vector2)]    
	[Tooltip("Distance between two Vector2 points.")]
	[HelpUrl("https://docs.unity3d.com/Documentation/ScriptReference/Vector2.Distance.html")]
	[System.Serializable]
	public class Distance : StateAction {
		[Tooltip("Vector2 value.")]
		public FsmVector2 a;
		[Tooltip("Vector3 value.")]
		public FsmVector2 b;
		[Shared]
		[Tooltip("Store the result.")]
		public FsmFloat store;
		[Tooltip("Execute the action every frame.")]
		public bool everyFrame;

		public override void OnEnter ()
		{
			base.OnEnter ();
			store.Value = Vector2.Distance ( a.Value, b.Value);
			if (!everyFrame) {
				Finish ();
			}
		}

		public override void OnUpdate ()
		{
			store.Value = Vector2.Distance ( a.Value, b.Value);
		}
	}
}