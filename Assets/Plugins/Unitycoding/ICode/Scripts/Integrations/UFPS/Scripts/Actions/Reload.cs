#if UFPS
using UnityEngine;
using System.Collections;

namespace ICode.Actions.UFPS{
	[Category("UFPS")]    
	[Tooltip("Reloads the current weapon.")]
	[System.Serializable]
	public class Reload : StateAction {
		[SharedPersistent]
		[Tooltip("Player GameObject.")]
		public FsmGameObject gameObject;
		[Tooltip("Item name.")]
		public FsmString itemName;
		[Shared]
		[NotRequired]
		[Tooltip("Store the result.")]
		public FsmBool result;
		protected vp_FPPlayerEventHandler player;
		
		public override void OnEnter ()
		{
			player = ((GameObject)gameObject.Value).GetComponent<vp_FPPlayerEventHandler> ();
			result.Value = player.Reload.TryStart ();
			Finish ();
		}
	}
}
#endif