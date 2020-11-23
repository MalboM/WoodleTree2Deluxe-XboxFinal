#if HIGHLIGHTING_SYSTEM
using UnityEngine;
using System.Collections;
using HighlightingSystem;

namespace ICode.Actions.HighlightingSystem{
	[System.Serializable]
	[Category("Highlighting System")]
	[Tooltip("Turn on one-frame highlighting.")]
	public class On : HighlighterAction {
		[NotRequired]
		public FsmColor color;

		public override void OnEnter ()
		{
			base.OnEnter ();
			if (color.IsNone) {
				highlighter.On ();
			} else {
				highlighter.On(color.Value);
			}
			Finish ();
		}
		
	}
}
#endif