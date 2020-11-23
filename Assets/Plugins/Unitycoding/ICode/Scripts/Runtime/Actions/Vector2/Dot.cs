using UnityEngine;
using System.Collections;

namespace ICode.Actions.UnityVector2{
	[Category(Category.Vector2)] 
	[Tooltip("Dot Product of two vectors.")]
	[HelpUrl("http://docs.unity3d.com/ScriptReference/Vector2.Dot.html")]
	[System.Serializable]
	public class Dot : StateAction {
		[Tooltip("Vector2 value.")]
		public FsmVector2 a;
		[Tooltip("Vector2 value.")]
		public FsmVector2 b;
		[Shared]
		[Tooltip("Store the result.")]
		public FsmFloat store;
		[Tooltip("Execute the action every frame.")]
		public bool everyFrame;
		
		public override void OnEnter ()
		{
			store.Value = Vector2.Dot(a.Value,b.Value);
			if (!everyFrame) {
				Finish ();
			}
		}
		
		public override void OnUpdate ()
		{
			store.Value = Vector2.Dot(a.Value,b.Value);
		}
	}
}