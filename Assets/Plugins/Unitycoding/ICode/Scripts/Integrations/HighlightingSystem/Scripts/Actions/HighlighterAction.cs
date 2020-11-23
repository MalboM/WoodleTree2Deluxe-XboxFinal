#if HIGHLIGHTING_SYSTEM
using UnityEngine;
using System.Collections;
using HighlightingSystem;

namespace ICode.Actions.HighlightingSystem{
	[System.Serializable]
	public abstract class HighlighterAction : StateAction {
		[SharedPersistent]
		[Tooltip("GameObject to use.")]
		public FsmGameObject gameObject;

		protected Highlighter highlighter;

		public override void OnEnter ()
		{
			highlighter = gameObject.Value.GetComponent<Highlighter> ();
			if (highlighter == null) {
				highlighter=gameObject.Value.AddComponent<Highlighter>();
			}
		}
		
	}
}
#endif
