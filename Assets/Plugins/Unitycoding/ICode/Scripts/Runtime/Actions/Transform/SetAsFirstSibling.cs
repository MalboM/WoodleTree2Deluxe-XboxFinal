using UnityEngine;
using System.Collections;

namespace ICode.Actions.UnityTransform{
	[Category(Category.Transform)]  
	[Tooltip("Move the transform to the start of the local transform list.")]
	[HelpUrl("http://docs.unity3d.com/ScriptReference/Transform.SetAsFirstSibling.html")]
	[System.Serializable]
	public class SetAsFirstSibling : TransformAction {
		public override void OnEnter ()
		{
			base.OnEnter ();
			 transform.SetAsFirstSibling ();
		}
	}
}