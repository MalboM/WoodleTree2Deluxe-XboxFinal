using UnityEngine;
using System.Collections;

namespace ICode.Actions.UnityVector2{
	[Category(Category.Vector2)] 
	[Tooltip("Subtracts one vector from another.")]
	[HelpUrl("https://docs.unity3d.com/Documentation/ScriptReference/Vector2-operator_subtract.html")]
	[System.Serializable]
	public class Subtract : StateAction {
		[Tooltip("Vector2 value.")]
		public FsmVector2 a;
		[Tooltip("Vector2 value to subtract.")]
		public FsmVector2 b;
		[Shared]
		[Tooltip("Store the result.")]
		public FsmVector2 store;
		[Tooltip("Execute the action every frame.")]
		public bool everyFrame;
		
		public override void OnEnter ()
		{
			store.Value = a.Value-b.Value;
			if (!everyFrame) {
				Finish ();
			}
		}
		
		public override void OnUpdate ()
		{
			store.Value = a.Value-b.Value;
		}
	}
}