using UnityEngine;
using System.Collections;

namespace ICode.Actions.UnityVector2{
	[Category(Category.Vector2)] 
	[Tooltip("Reflects a vector off the vector defined by a normal.")]
	[HelpUrl("http://docs.unity3d.com/ScriptReference/Vector2.Reflect.html")]
	[System.Serializable]
	public class Reflect : StateAction {
		[Tooltip("Vector2 value.")]
		public FsmVector2 inDirection;
		[Tooltip("Vector2 value.")]
		public FsmVector2 inNormal;
		[Shared]
		[Tooltip("Store the result.")]
		public FsmVector2 store;
		[Tooltip("Execute the action every frame.")]
		public bool everyFrame;
		
		public override void OnEnter ()
		{
			store.Value = (-2f * Vector2.Dot(inNormal.Value, inDirection.Value) * inNormal.Value) + inDirection.Value;
			if (!everyFrame) {
				Finish ();
			}
		}
		
		public override void OnUpdate ()
		{
			store.Value = (-2f * Vector2.Dot(inNormal.Value, inDirection.Value) * inNormal.Value) + inDirection.Value;
		}
	}
}