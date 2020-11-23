using UnityEngine;
using System.Collections;

namespace ICode.Actions.UnityTransform{
	[Category(Category.Transform)]  
	[Tooltip("Gets the sibling index.")]
	[HelpUrl("http://docs.unity3d.com/ScriptReference/Transform.GetSiblingIndex.html")]
	[System.Serializable]
	public class GetSiblingIndex : TransformAction {
		[Shared]
		[Tooltip("Store the result.")]
		public FsmInt store;

		public override void OnEnter ()
		{
			base.OnEnter ();
			 transform.GetSiblingIndex ();
		}
	}
}