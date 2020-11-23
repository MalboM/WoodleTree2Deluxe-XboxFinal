using UnityEngine;
using System.Collections;

namespace ICode.Actions.UnityVector3{
	[Category(Category.Vector3)] 
	[Tooltip("Divides a vector by a number.")]
	[HelpUrl("http://docs.unity3d.com/ScriptReference/Vector3-operator_divide.html")]
	[System.Serializable]
	public class Divide : StateAction {
		[Tooltip("Vector3 value.")]
		public FsmVector3 vector;
		[Tooltip("Float value to divide with.")]
		public FsmFloat a;
		[Shared]
		[Tooltip("Store the result.")]
		public FsmVector3 store;
		[Tooltip("Execute the action every frame.")]
		public bool everyFrame;

		public override void OnEnter ()
		{
			store.Value = vector.Value/a.Value;
			if (!everyFrame) {
				Finish ();
			}
		}

		public override void OnUpdate ()
		{
			store.Value = vector.Value/a.Value;
		}
	}
}