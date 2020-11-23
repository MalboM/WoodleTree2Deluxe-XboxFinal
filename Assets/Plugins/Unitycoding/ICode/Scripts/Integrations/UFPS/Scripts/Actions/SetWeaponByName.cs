#if UFPS
using UnityEngine;
using System.Collections;

namespace ICode.Actions.UFPS{
	[Category("UFPS")]    
	[Tooltip("Sets the current weapon by name.")]
	[System.Serializable]
	public class SetWeaponByName : StateAction {
		[SharedPersistent]
		[Tooltip("Player GameObject.")]
		public FsmGameObject gameObject;
		[Tooltip("Weapon name.")]
		public FsmString weaponName;
		[Shared]
		[NotRequired]
		[Tooltip("Store the result.")]
		public FsmBool result;
		protected vp_FPPlayerEventHandler player;
		
		public override void OnEnter ()
		{
			player = ((GameObject)gameObject.Value).GetComponent<vp_FPPlayerEventHandler> ();
			result.Value = player.SetWeaponByName.Try (weaponName.Value);
			Finish ();
		}
	}
}
#endif