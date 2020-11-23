#if CORE_GAME_KIT
using UnityEngine;
using System.Collections;
using DarkTonic.CoreGameKit;

namespace ICode.Actions.CoreGameKit{
	[Category("CoreGameKit")]   
	[Tooltip("Spawn one item from a Syncro Spawner in Core GameKit")] 
	[System.Serializable]
	public class SpawnOne : StateAction {
		[SharedPersistent]
		[Tooltip("The GameObject to use.")]
		public FsmGameObject gameObject;
		
		public override void OnEnter ()
		{
			Spawn ();
		}
		
		private void Spawn() {
			WaveSyncroPrefabSpawner spawner = ((GameObject)gameObject.Value).GetComponent<WaveSyncroPrefabSpawner>();
			if (spawner != null) {
				spawner.SpawnOneItem ();
			}
		}
		
	}
}
#endif