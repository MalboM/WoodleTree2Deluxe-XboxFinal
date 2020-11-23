#if HIGHLIGHTING_SYSTEM
using UnityEngine;
using System.Collections;
using HighlightingSystem;

namespace ICode.Actions.HighlightingSystem{
	[System.Serializable]
	[Category("Highlighting System")]
	[Tooltip("Turn on flashing from color1 to color2 with given frequency.")]
	public class FlashingOn : HighlighterAction {
		public FsmColor color1;
		public FsmColor color2;
		[DefaultValueAttribute(3.0f)]
		public FsmFloat frequency;

		public override void OnEnter ()
		{
			base.OnEnter ();
			highlighter.FlashingOn(color1.Value, color2.Value, frequency.Value);
			Finish ();
		}
		
	}
}
#endif