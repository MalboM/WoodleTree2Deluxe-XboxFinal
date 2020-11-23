#if HIGHLIGHTING_SYSTEM
using UnityEngine;
using System.Collections;
using HighlightingSystem;

namespace ICode.Actions.HighlightingSystem{
	[System.Serializable]
	[Category("Highlighting System")]
	[Tooltip("Disable occluder mode. Occluders will be used only in case frame depth buffer is not accessible.")]
	public class OccluderOff : HighlighterAction {
		public override void OnEnter ()
		{
			base.OnEnter ();
			highlighter.OccluderOff();
			Finish ();
		}
		
	}
}
#endif