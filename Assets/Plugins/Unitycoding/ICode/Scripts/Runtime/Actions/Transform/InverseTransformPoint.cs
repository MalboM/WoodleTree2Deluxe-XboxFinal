using UnityEngine;
using System.Collections;

namespace ICode.Actions.UnityTransform{
	[Category(Category.Transform)]    
	[Tooltip("Transforms position from world space to local space.")]
	[HelpUrl("http://docs.unity3d.com/ScriptReference/Transform.InverseTransformPoint.html")]
	[System.Serializable]
	public class InverseTransformPoint : TransformAction {
		[Tooltip("Position to transform.")]
		public FsmVector3 position;
		[Shared]
		[Tooltip("Store the result.")]
		public FsmVector3 store;
		[Tooltip("Execute the action every frame.")]
		public bool everyFrame;

		public override void OnEnter ()
		{
			base.OnEnter ();
			store.Value = transform.InverseTransformPoint (position.Value);
			if (!everyFrame) {
				Finish ();
			}
		}

		public override void OnUpdate ()
		{
			store.Value = transform.InverseTransformPoint (position.Value);
		}
	}
}