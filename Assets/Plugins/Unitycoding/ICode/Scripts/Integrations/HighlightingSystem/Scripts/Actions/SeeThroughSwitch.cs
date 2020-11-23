#if HIGHLIGHTING_SYSTEM
using UnityEngine;
using System.Collections;
using HighlightingSystem;

namespace ICode.Actions.HighlightingSystem{
	[System.Serializable]
	[Category("Highlighting System")]
	[Tooltip("Switch see-through mode")]
	public class SeeThroughSwitch : HighlighterAction {
		public override void OnEnter ()
		{
			base.OnEnter ();
			highlighter.SeeThroughSwitch();
			Finish ();
		}
		
	}
}
#endif