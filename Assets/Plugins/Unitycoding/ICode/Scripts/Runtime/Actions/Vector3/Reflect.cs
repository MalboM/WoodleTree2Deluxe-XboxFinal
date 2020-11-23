using UnityEngine;
using System.Collections;

namespace ICode.Actions.UnityVector3{
	[Category(Category.Vector3)] 
	[Tooltip("Reflects a vector off the vector defined by a normal.")]
	[HelpUrl("http://docs.unity3d.com/ScriptReference/Vector3.Reflect.html")]
	[System.Serializable]
	public class Reflect : StateAction {
		[Tooltip("Vector3 value.")]
		public FsmVector3 inDirection;
		[Tooltip("Vector3 value.")]
		public FsmVector3 inNormal;
		[Shared]
		[Tooltip("Store the result.")]
		public FsmVector3 store;
		[Tooltip("Execute the action every frame.")]
		public bool everyFrame;
		
		public override void OnEnter ()
		{
			store.Value = (-2f * Vector3.Dot(inNormal.Value, inDirection.Value) * inNormal.Value) + inDirection.Value;
			if (!everyFrame) {
				Finish ();
			}
		}
		
		public override void OnUpdate ()
		{
			store.Value = (-2f * Vector3.Dot(inNormal.Value, inDirection.Value) * inNormal.Value) + inDirection.Value;
		}
	}
}