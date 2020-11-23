using UnityEngine;
using System.Collections;

namespace ICode.Actions.UnityAnimation{
	/// <summary>
	/// Adds a clip to the animation with name.
	/// </summary>
	[Category(Category.Animation)]   
	[Tooltip("Adds a clip to the animation with name.")]
	[HelpUrl("http://docs.unity3d.com/ScriptReference/Animation.AddClip.html")]
	[System.Serializable]
	public class AddClip : AnimationAction {
		/// <summary>
		/// Clip to add.
		/// </summary>
		[Tooltip("Clip to add.")]
		public FsmObject clip;
		/// <summary>
		/// The name of the clip.
		/// </summary>
		[InspectorLabel("Name")]
		[Tooltip("New name.")]
		public FsmString _name;
		
		public override void OnEnter ()
		{
			base.OnEnter ();
			animation.AddClip ((AnimationClip)clip.Value, _name.Value);
		}

	}
}