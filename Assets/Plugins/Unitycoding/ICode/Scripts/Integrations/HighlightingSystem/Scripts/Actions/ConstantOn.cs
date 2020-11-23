#if HIGHLIGHTING_SYSTEM
using UnityEngine;
using System.Collections;
using HighlightingSystem;

namespace ICode.Actions.HighlightingSystem{
	[System.Serializable]
	[Category("Highlighting System")]
	[Tooltip("Fade in constant highlighting with given color.")]
	public class ConstantOn : HighlighterAction {
		[NotRequired]
		public FsmColor color;

		public override void OnEnter ()
		{
			base.OnEnter ();
			if (color.IsNone) {
				highlighter.ConstantOn ();
			} else {
				highlighter.ConstantOn(color.Value);
			}
			Finish ();
		}
		
	}
}
#endif