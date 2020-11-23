using UnityEngine;
using System.Collections;

namespace ICode.Actions.UnityTransform{
	[Category(Category.Transform)]  
	[Tooltip("Gets the sibling index.")]
	[HelpUrl("http://docs.unity3d.com/ScriptReference/Transform.SetSiblingIndex.html")]
	[System.Serializable]
	public class SetSiblingIndex : TransformAction {
		[Tooltip("Index to set.")]
		public FsmInt index;

		public override void OnEnter ()
		{
			base.OnEnter ();
			 transform.SetSiblingIndex (index.Value);
		}
	}
}