#if UFPS
using UnityEngine;
using System.Collections;

namespace ICode.Actions.UFPS{
	[Category("UFPS")]    
	[Tooltip("Sets the health of the player.")]
	[System.Serializable]
	public class SetHealth : StateAction {
		[SharedPersistent]
		[Tooltip("Player GameObject.")]
		public FsmGameObject gameObject;
		[Tooltip("Health to set.")]
		public FsmFloat health;
		[Tooltip("Execute the action every frame.")]
		public bool everyFrame;
		
		protected vp_FPPlayerEventHandler player;
		
		public override void OnEnter ()
		{
			player = ((GameObject)gameObject.Value).GetComponent<vp_FPPlayerEventHandler> ();
			DoSetHealth ();
			if (!everyFrame) {
				Finish ();
			}
		}
		
		public override void OnUpdate ()
		{
			DoSetHealth ();
		}
		
		private void DoSetHealth(){
			player.Health.Set (health.Value);
		}
	}
}
#endif