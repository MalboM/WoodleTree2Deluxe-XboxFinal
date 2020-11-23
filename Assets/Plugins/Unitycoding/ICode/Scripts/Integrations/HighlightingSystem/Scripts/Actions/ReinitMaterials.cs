#if HIGHLIGHTING_SYSTEM
using UnityEngine;
using System.Collections;
using HighlightingSystem;

namespace ICode.Actions.HighlightingSystem{
	[System.Serializable]
	[Category("Highlighting System")]
	[Tooltip("Renderers reinitialization. Call this method if your highlighted object has changed it's materials, renderers or child objects. Can be called multiple times per update - renderers reinitialization will occur only once.")]
	public class ReinitMaterials : HighlighterAction {

		public override void OnEnter ()
		{
			base.OnEnter ();
			highlighter.ReinitMaterials();
			Finish ();
		}
		
	}
}
#endif