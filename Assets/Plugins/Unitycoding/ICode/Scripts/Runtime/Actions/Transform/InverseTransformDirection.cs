using UnityEngine;
using System.Collections;

namespace ICode.Actions.UnityTransform{
	[Category(Category.Transform)]    
	[Tooltip("Transforms a direction from world space to local space. The opposite of Transform.TransformDirection.")]
	[HelpUrl("http://docs.unity3d.com/ScriptReference/Transform.InverseTransformDirection.html")]
	[System.Serializable]
	public class InverseTransformDirection : TransformAction {
		[Tooltip("Direction to transform.")]
		public FsmVector3 direction;
		[Shared]
		[Tooltip("Store the result.")]
		public FsmVector3 store;
		[Tooltip("Execute the action every frame.")]
		public bool everyFrame;

		public override void OnEnter ()
		{
			base.OnEnter ();
			store.Value = transform.InverseTransformDirection (direction.Value);
			if (!everyFrame) {
				Finish ();
			}
		}

		public override void OnUpdate ()
		{
			store.Value = transform.InverseTransformDirection (direction.Value);
		}
	}
}