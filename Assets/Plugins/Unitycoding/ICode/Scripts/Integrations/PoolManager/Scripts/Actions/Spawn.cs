#if POOL_MANAGER
using UnityEngine;
using System.Collections;
using System;
using PathologicalGames;

namespace ICode.Actions.PathologicalPoolManager{
	[Category("PoolManager")]    
	[Tooltip("Use Spawn() instead of Unity's Instantiate(). The function signature is the same but the return type is different and Spawn() will use an instance from the pool if one is available.")]
	[System.Serializable]
	public class Spawn : StateAction {
		[Tooltip("Pool Name")]
		public FsmString poolName;
		[SharedPersistent]
		[Tooltip("GameObject or prefab to despawn.")]
		public FsmGameObject gameObject;
		[Tooltip("Spawn position.")]
		public FsmVector3 position;
		[Tooltip("Euler Angles")]
		public FsmVector3 euler;
		[Shared]
		[NotRequired]
		[Tooltip("Store the spawned game object.")]
		public FsmGameObject store;

		public override void OnEnter ()
		{
			DoSpawn ();
			Finish ();
		}	
		
		private void DoSpawn()
		{
			if (string.IsNullOrEmpty(poolName.Value)){
				return;
			}
			
			if (gameObject.Value == null){
				return;
			}
			Transform result = PoolManager.Pools[poolName.Value].Spawn(((GameObject)gameObject.Value).transform,position.Value,Quaternion.Euler(euler.Value));
			if (result != null) {
				store.Value = result.gameObject;		
			}

		}
		
	}
}
#endif