#if UFPS
using UnityEngine;
using System.Collections;

namespace ICode.Actions.UFPS{
	[Category("UFPS")]    
	[Tooltip("Gets the health of the player.")]
	[System.Serializable]
	public class GetHealth : StateAction {
		[SharedPersistent]
		[Tooltip("Player GameObject.")]
		public FsmGameObject gameObject;
		[Shared]
		[Tooltip("Result to store.")]
		public FsmFloat store;
		[Tooltip("Execute the action every frame.")]
		public bool everyFrame;

		protected vp_FPPlayerEventHandler player;
		
		public override void OnEnter ()
		{
			player = ((GameObject)gameObject.Value).GetComponent<vp_FPPlayerEventHandler> ();
			DoGetHealth ();
			if (!everyFrame) {
				Finish ();
			}
		}

		public override void OnUpdate ()
		{
			DoGetHealth ();
		}

		private void DoGetHealth(){
			store.Value = player.Health.Get ();
		}
	}
}
#endif