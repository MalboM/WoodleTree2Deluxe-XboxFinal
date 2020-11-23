using UnityEngine;
using System.Collections;

namespace ICode.Actions.Math{
	[Category(Category.Math)] 
	[Tooltip("Clamps a value between a minimum int and maximum int value.")]
	[HelpUrl("http://docs.unity3d.com/ScriptReference/Mathf.Clamp.html")]
	[System.Serializable]
	public class ClampInt : StateAction {
		[Tooltip("The value to clamp")]
		public FsmInt value;
		[Tooltip("Min value")]
		public FsmInt min;
		[Tooltip("Max value")]
		public FsmInt max;
		[Shared]
		[Tooltip("Store the result.")]
		public FsmInt store;
		[Tooltip("Execute the action every frame.")]
		public bool everyFrame;

		public override void OnEnter ()
		{
			store.Value = Mathf.Clamp (value.Value, min.Value, max.Value);
			if (!everyFrame) {
				Finish ();
			}
		}

		public override void OnUpdate ()
		{
			store.Value = Mathf.Clamp (value.Value, min.Value, max.Value);
		}
	}
}