using UnityEngine;
using System.Collections;

namespace ICode.Actions.UnityAnimation{
	[System.Serializable]
	public abstract class AnimationAction : StateAction {
		[SharedPersistent]
		[Tooltip("GameObject to use.")]
		/// <summary>
		/// GameObject to use.
		/// </summary>
		public FsmGameObject gameObject;
		/// <summary>
		/// The animation component.
		/// </summary>
		protected Animation animation;
		
		public override void OnEnter ()
		{
			animation = gameObject.Value.GetComponent<Animation> ();
		}
	}
}