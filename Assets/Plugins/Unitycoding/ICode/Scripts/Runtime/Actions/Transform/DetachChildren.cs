using UnityEngine;
using System.Collections;

namespace ICode.Actions.UnityTransform{
	[Category(Category.Transform)]  
	[Tooltip("Unparents all children. Useful if you want to destroy the root of a hierarchy without destroying the children.")]
	[HelpUrl("http://docs.unity3d.com/ScriptReference/Transform.DetachChildren.html")]
	[System.Serializable]
	public class DetachChildren : TransformAction {
		public override void OnEnter ()
		{
			base.OnEnter ();
			transform.DetachChildren ();
		}
	}
}