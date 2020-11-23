#if HIGHLIGHTING_SYSTEM
using UnityEngine;
using System.Collections;
using HighlightingSystem;

namespace ICode.Actions.HighlightingSystem{
	[System.Serializable]
	[Category("Highlighting System")]
	[Tooltip("Disable see-through mode")]
	public class SeeThroughOff : HighlighterAction {
		public override void OnEnter ()
		{
			base.OnEnter ();
			highlighter.SeeThroughOn();
			Finish ();
		}
		
	}
}
#endif