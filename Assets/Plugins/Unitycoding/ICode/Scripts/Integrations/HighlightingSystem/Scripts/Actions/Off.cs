#if HIGHLIGHTING_SYSTEM
using UnityEngine;
using System.Collections;
using HighlightingSystem;

namespace ICode.Actions.HighlightingSystem{
	[System.Serializable]
	[Category("Highlighting System")]
	[Tooltip("Turn off all types of highlighting. ")]
	public class Off : HighlighterAction {
		public override void OnEnter ()
		{
			base.OnEnter ();
			highlighter.Off();
			Finish ();
		}
		
	}
}
#endif