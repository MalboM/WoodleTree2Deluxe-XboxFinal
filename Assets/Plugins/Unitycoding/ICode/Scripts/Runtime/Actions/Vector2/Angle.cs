using UnityEngine;
using System.Collections;

namespace ICode.Actions.UnityVector2{
	[Category(Category.Vector2)]  
	[Tooltip("Returns the angle in degrees between from and to.")]
	[HelpUrl("https://docs.unity3d.com/Documentation/ScriptReference/Vector2.Angle.html")]
	[System.Serializable]
	public class Angle : StateAction {
		[Tooltip("The angle extends round from this vector.")]
		public FsmVector2 from;
		[Tooltip("The angle extends round to this vector.")]
		public FsmVector2 to;
		[Shared]
		[Tooltip("Store the result.")]
		public FsmFloat store;
		[Tooltip("Execute the action every frame.")]
		public bool everyFrame;

		public override void OnEnter ()
		{
			store.Value = Vector2.Angle (from.Value, to.Value);
			if (!everyFrame) {
				Finish ();
			}
		}

		public override void OnUpdate ()
		{
			store.Value = Vector2.Angle (from.Value, to.Value);
		}
	}
}