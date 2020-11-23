#if CORE_GAME_KIT
using UnityEngine;
using System.Collections;
using DarkTonic.CoreGameKit;

namespace ICode.Actions.CoreGameKit{
	[Category("CoreGameKit")]   
	[Tooltip("Make a Killable object in Core GameKit invincible for X seconds")]
	[System.Serializable]
	public class TemporaryInvincibility : StateAction {
		[SharedPersistent]
		[Tooltip("The GameObject to use.")]
		public FsmGameObject gameObject;
		[Tooltip("Invincible time to set.")]
		public FsmFloat invincibleTime;
	
		public override void OnEnter ()
		{
			Killable killable = ((GameObject)gameObject.Value).GetComponent<Killable>();
			killable.TemporaryInvincibility(invincibleTime.Value);
		}
		

	}
}
#endif