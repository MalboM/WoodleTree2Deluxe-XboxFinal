#if HIGHLIGHTING_SYSTEM
using UnityEngine;
using System.Collections;
using HighlightingSystem;

namespace ICode.Actions.HighlightingSystem{
	[System.Serializable]
	[Category("Highlighting System")]
	[Tooltip("Switch Constant Highlighting.")]
	public class ConstantSwitch : HighlighterAction {
		public override void OnEnter ()
		{
			base.OnEnter ();
			highlighter.ConstantSwitch();
			Finish ();
		}
		
	}
}
#endif