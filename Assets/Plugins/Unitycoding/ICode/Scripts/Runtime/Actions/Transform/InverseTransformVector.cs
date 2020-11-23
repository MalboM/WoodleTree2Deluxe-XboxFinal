using UnityEngine;
using System.Collections;

namespace ICode.Actions.UnityTransform{
	[Category(Category.Transform)]    
	[Tooltip("Transforms a vector from world space to local space. The opposite of Transform.TransformVector.")]
	[HelpUrl("http://docs.unity3d.com/ScriptReference/Transform.InverseTransformVector.html")]
	[System.Serializable]
	public class InverseTransformVector : TransformAction {
		[Tooltip("Vector to transform.")]
		public FsmVector3 vector;
		[Shared]
		[Tooltip("Store the result.")]
		public FsmVector3 store;
		[Tooltip("Execute the action every frame.")]
		public bool everyFrame;

		public override void OnEnter ()
		{
			base.OnEnter ();
			store.Value = transform.InverseTransformVector (vector.Value);
			if (!everyFrame) {
				Finish ();
			}
		}

		public override void OnUpdate ()
		{
			store.Value = transform.InverseTransformVector (vector.Value);
		}
	}
}