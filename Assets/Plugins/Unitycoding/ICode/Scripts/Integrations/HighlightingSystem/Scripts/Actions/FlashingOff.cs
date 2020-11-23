#if HIGHLIGHTING_SYSTEM
using UnityEngine;
using System.Collections;
using HighlightingSystem;

namespace ICode.Actions.HighlightingSystem{
	[System.Serializable]
	[Category("Highlighting System")]
	[Tooltip("Turn off flashing.")]
	public class FlashingOff : HighlighterAction {
		public override void OnEnter ()
		{
			base.OnEnter ();
			highlighter.FlashingOff();
			Finish ();
		}
		
	}
}
#endif