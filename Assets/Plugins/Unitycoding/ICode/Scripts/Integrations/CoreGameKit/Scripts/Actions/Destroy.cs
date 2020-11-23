#if CORE_GAME_KIT
using UnityEngine;
using System.Collections;
using DarkTonic.CoreGameKit;

namespace ICode.Actions.CoreGameKit{
	[Category("CoreGameKit")]   
	[Tooltip("Destroy a Killable object in Core GameKit")] 
	[System.Serializable]
	public class Destroy : StateAction {
		[SharedPersistent]
		[Tooltip("The GameObject to use.")]
		public FsmGameObject gameObject;
		[Tooltip("The name of the scenario (World Variable Mod Group)")]
		[DefaultValue(Killable.DestroyedText)]
		public FsmString scenarioName;

		public override void OnEnter ()
		{
			Killable killable = ((GameObject)gameObject.Value).GetComponent<Killable>();
			if (killable != null) {
				killable.DestroyKillable(scenarioName.Value);
			}
		}
	}
}
#endif