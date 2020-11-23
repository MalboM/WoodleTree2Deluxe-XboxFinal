#if CORE_GAME_KIT
using UnityEngine;
using System.Collections;
using DarkTonic.CoreGameKit;

namespace ICode.Actions.CoreGameKit{
	[Category("CoreGameKit")]   
	[Tooltip("Despawn a Killable object in Core GameKit")]
	[System.Serializable]
	public class Despawn : StateAction {
		[SharedPersistent]
		[Tooltip("The GameObject to use.")]
		public FsmGameObject gameObject;
		
		public override void OnEnter ()
		{
			Killable killable = ((GameObject)gameObject.Value).GetComponent<Killable>();
			if (killable != null) {
				killable.Despawn (TriggeredSpawner.EventType.CodeTriggered1);
			}
		}

		
	}
}
#endif