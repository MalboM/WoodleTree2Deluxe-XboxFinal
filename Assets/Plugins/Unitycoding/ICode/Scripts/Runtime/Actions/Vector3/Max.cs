using UnityEngine;
using System.Collections;

namespace ICode.Actions.UnityVector3{
	[Category(Category.Vector3)] 
	[Tooltip("Returns a vector that is made from the largest components of two vectors.")]
	[HelpUrl("http://docs.unity3d.com/ScriptReference/Vector3.Max.html")]
	[System.Serializable]
	public class Max : StateAction {
		[Tooltip("Vector3 value.")]
		public FsmVector3 a;
		[Tooltip("Vector3 value.")]
		public FsmVector3 b;
		[Shared]
		[Tooltip("Store the result.")]
		public FsmVector3 store;
		[Tooltip("Execute the action every frame.")]
		public bool everyFrame;
		
		public override void OnEnter ()
		{
			store.Value = Vector3.Max(a.Value,b.Value);
			if (!everyFrame) {
				Finish ();
			}
		}
		
		public override void OnUpdate ()
		{
			store.Value = Vector3.Max(a.Value,b.Value);
		}
	}
}