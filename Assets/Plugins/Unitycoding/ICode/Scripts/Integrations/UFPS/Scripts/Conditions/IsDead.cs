#if UFPS
using UnityEngine;
using System.Collections;

namespace ICode.Conditions.UFPS{
	[Category("UFPS")]    
	[Tooltip("Is the player dead?")]
	[System.Serializable]
	public class IsDead : Condition {
		[SharedPersistent]
		[Tooltip("Player GameObject to test.")]
		public FsmGameObject gameObject;
		[Tooltip("Does the result equals this condition.")]
		public FsmBool equals;
		
		private vp_FPPlayerEventHandler player;
		public override void OnEnter ()
		{
			player = ((GameObject)gameObject.Value).GetComponent<vp_FPPlayerEventHandler> ();
		}
		
		public override bool Validate ()
		{
			if(player != null){
				return (player.Dead.Active == equals.Value);
			}
			return false;
		}
	}
}
#endif