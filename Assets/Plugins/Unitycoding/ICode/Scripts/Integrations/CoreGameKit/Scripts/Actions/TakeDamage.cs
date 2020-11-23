#if CORE_GAME_KIT
using UnityEngine;
using System.Collections;
using DarkTonic.CoreGameKit;

namespace ICode.Actions.CoreGameKit{
	[Category("CoreGameKit")]   
	[Tooltip("Inflict points of damage on a Killable in Core GameKit")]
	[System.Serializable]
	public class TakeDamage : StateAction {
		[SharedPersistent]
		[Tooltip("The GameObject to use.")]
		public FsmGameObject gameObject;
		[Tooltip("Damage Points to hit the Killable with")]
		public FsmInt damagePoints;

		public override void OnEnter ()
		{
			Killable killable = ((GameObject)gameObject.Value).GetComponent<Killable>();
			killable.TakeDamage(damagePoints.Value);
		}
	}
}
#endif