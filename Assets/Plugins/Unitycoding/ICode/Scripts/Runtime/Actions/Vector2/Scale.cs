using UnityEngine;
using System.Collections;

namespace ICode.Actions.UnityVector2{
	[Category(Category.Vector2)] 
	[Tooltip("Multiplies two vectors component-wise.")]
	[HelpUrl("http://docs.unity3d.com/ScriptReference/Vector2.Scale.html")]
	[System.Serializable]
	public class Scale : StateAction {
		[Tooltip("Vector2 value.")]
		public FsmVector2 a;
		[Tooltip("Vector2 value to add.")]
		public FsmVector2 b;
		[Shared]
		[Tooltip("Store the result.")]
		public FsmVector2 store;
		[Tooltip("Execute the action every frame.")]
		public bool everyFrame;
		
		public override void OnEnter ()
		{
			store.Value = Vector2.Scale (a.Value, b.Value);
			if (!everyFrame) {
				Finish ();
			}
		}
		
		public override void OnUpdate ()
		{
			store.Value = Vector2.Scale (a.Value, b.Value);
		}
	}
}