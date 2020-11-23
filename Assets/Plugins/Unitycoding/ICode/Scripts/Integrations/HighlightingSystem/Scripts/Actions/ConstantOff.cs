#if HIGHLIGHTING_SYSTEM
using UnityEngine;
using System.Collections;
using HighlightingSystem;

namespace ICode.Actions.HighlightingSystem{
	[System.Serializable]
	[Category("Highlighting System")]
	[Tooltip("Fade out constant highlighting.")]
	public class ConstantOff : HighlighterAction {
		public override void OnEnter ()
		{
			base.OnEnter ();
			highlighter.ConstantOff();
			Finish ();
		}
		
	}
}
#endif