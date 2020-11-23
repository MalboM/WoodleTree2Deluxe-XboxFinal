using UnityEngine;
using System.Collections;

namespace ICode.Actions.UnityVector3{
	[Category(Category.Vector3)] 
	[Tooltip("Multiplies two vectors component-wise.")]
	[HelpUrl("http://docs.unity3d.com/ScriptReference/Vector3.Scale.html")]
	[System.Serializable]
	public class Scale : StateAction {
		[Tooltip("Vector3 value.")]
		public FsmVector3 a;
		[Tooltip("Vector3 value to add.")]
		public FsmVector3 b;
		[Shared]
		[Tooltip("Store the result.")]
		public FsmVector3 store;
		[Tooltip("Execute the action every frame.")]
		public bool everyFrame;
		
		public override void OnEnter ()
		{
			store.Value = Vector3.Scale (a.Value, b.Value);
			if (!everyFrame) {
				Finish ();
			}
		}
		
		public override void OnUpdate ()
		{
			store.Value = Vector3.Scale (a.Value, b.Value);
		}
	}
}