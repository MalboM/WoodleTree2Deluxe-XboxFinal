#if UFPS
using UnityEngine;
using System.Collections;

namespace ICode.Actions.UFPS{
	[Category("UFPS")]    
	[Tooltip("Sets the current weapon by index.")]
	[System.Serializable]
	public class SetWeapon : StateAction {
		[SharedPersistent]
		[Tooltip("Player GameObject.")]
		public FsmGameObject gameObject;
		[Tooltip("Weapon index.")]
		public FsmInt index;
		[Shared]
		[NotRequired]
		[Tooltip("Store the result.")]
		public FsmBool result;
		protected vp_FPPlayerEventHandler player;
		
		public override void OnEnter ()
		{
			player = ((GameObject)gameObject.Value).GetComponent<vp_FPPlayerEventHandler> ();
			result.Value = player.SetWeapon.TryStart (index.Value);
			Finish ();
		}
	}
}
#endif