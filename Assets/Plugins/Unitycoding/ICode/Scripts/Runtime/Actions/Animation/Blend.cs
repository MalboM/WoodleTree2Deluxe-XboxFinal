using UnityEngine;
using System.Collections;

namespace ICode.Actions.UnityAnimation{
	/// <summary>
	/// Blends the animation towards targetWeight over the next time seconds.
	/// </summary>
	[Category(Category.Animation)]   
	[Tooltip("Blends the animation towards targetWeight over the next time seconds.")]
	[HelpUrl("http://docs.unity3d.com/ScriptReference/Animation.Blend.html")]
	[System.Serializable]
	public class Blend : AnimationAction {
		[InspectorLabel("Name")]
		[Tooltip("Animation name to blend.")]
		/// <summary>
		/// Animation name to blend.
		/// </summary>
		public FsmString _name;
		[Tooltip("Weight")]
		[DefaultValueAttribute(1.0f)]
		public FsmFloat targetWeight;
		[Tooltip("Fading time.")]
		[DefaultValueAttribute(0.3f)]
		public FsmFloat fadeLenght;

		[Tooltip("Execute the action every frame.")]
		public bool everyFrame;
		
		public override void OnEnter ()
		{
			base.OnEnter ();
			animation.Blend (_name.Value,targetWeight.Value, fadeLenght.Value);
			if (!everyFrame) {
				Finish();			
			}
		}
		
		public override void OnUpdate ()
		{
			animation.Blend (_name.Value,targetWeight.Value, fadeLenght.Value);
		}
	}
}