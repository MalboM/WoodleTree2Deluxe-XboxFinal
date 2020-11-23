using UnityEngine;
using System.Collections;

namespace ICode.Actions.UnityVector2{
	[Category(Category.Vector2)] 
	[Tooltip("Returns a vector that is made from the largest components of two vectors.")]
	[HelpUrl("http://docs.unity3d.com/ScriptReference/Vector2.Min.html")]
	[System.Serializable]
	public class Min : StateAction {
		[Tooltip("Vector2 value.")]
		public FsmVector2 a;
		[Tooltip("Vector2 value.")]
		public FsmVector2 b;
		[Shared]
		[Tooltip("Store the result.")]
		public FsmVector2 store;
		[Tooltip("Execute the action every frame.")]
		public bool everyFrame;
		
		public override void OnEnter ()
		{
			store.Value = Vector2.Min(a.Value,b.Value);
			if (!everyFrame) {
				Finish ();
			}
		}
		
		public override void OnUpdate ()
		{
			store.Value = Vector2.Min(a.Value,b.Value);
		}
	}
}