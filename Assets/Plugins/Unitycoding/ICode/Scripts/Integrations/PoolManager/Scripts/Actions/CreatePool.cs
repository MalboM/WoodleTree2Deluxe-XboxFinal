#if POOL_MANAGER
using UnityEngine;
using System.Collections;
using System;
using PathologicalGames;

namespace ICode.Actions.PathologicalPoolManager{
	[Category("PoolManager")]    
	[Tooltip("Creates a new GameObject with a SpawnPool Component.")]
	[System.Serializable]
	public class CreatePool : StateAction {
		[Tooltip("Pool Name")]
		public FsmString poolName;
		[Tooltip("True to log pool activities.")]
		public FsmBool logMessages;

		[Shared]
		[Tooltip("Store the pool game object.")]
		public FsmGameObject store;

		
		public override void OnEnter ()
		{
			DoCreatePool ();
			Finish ();
		}	

		private void DoCreatePool()
		{
			if(string.IsNullOrEmpty(poolName.Value)){
				return;
			}		
			SpawnPool pool;
			bool exists = PoolManager.Pools.TryGetValue(poolName.Value,out pool);
			if (!exists)
			{
				pool = PoolManager.Pools.Create(poolName.Value);	
			}
			pool.logMessages = logMessages.Value;
			store.Value = pool.group.gameObject;
		}
	
	}
}
#endif