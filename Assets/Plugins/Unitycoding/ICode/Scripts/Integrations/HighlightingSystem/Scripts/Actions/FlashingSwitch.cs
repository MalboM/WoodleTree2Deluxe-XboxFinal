#if HIGHLIGHTING_SYSTEM
using UnityEngine;
using System.Collections;
using HighlightingSystem;

namespace ICode.Actions.HighlightingSystem{
	[System.Serializable]
	[Category("Highlighting System")]
	[Tooltip("Switch flashing mode.")]
	public class FlashingSwitch : HighlighterAction {
		public override void OnEnter ()
		{
			base.OnEnter ();
			highlighter.FlashingSwitch();
			Finish ();
		}
		
	}
}
#endif