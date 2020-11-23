#if POOL_MANAGER
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using PathologicalGames;

namespace ICode.Actions.PathologicalPoolManager{
	[Category("PoolManager")]    
	[Tooltip("Creates a new PrefabPool in this Pool.")]
	[System.Serializable]
	public class CreatePrefabPool : StateAction {
		[Tooltip("Pool Name.")]
		public FsmString poolName;
		[Tooltip("Prefab to preload.")]
		public FsmGameObject prefab;
		[Tooltip("Preload amount.")]
		public FsmInt preloadAmount;

		[Tooltip("Limits the number of instances allowed in the game. Turning this ON " +
		         "means when 'Limit Amount' is hit, no more instances will be created." +
		         "CALLS TO SpawnPool.Spawn() WILL BE IGNORED, and return null!")]
		public FsmBool limitInstances;
		
		[Tooltip("This is the max number of instances allowed if 'limitInstances' is ON.")]
		public FsmInt limitAmount;
		
		[Tooltip("Turn this ON to activate the culling feature for this Pool. " +
		         "Use this feature to remove despawned (inactive) instances from the pool" +
		         "if the size of the pool grows too large. " +
		         "DO NOT USE THIS UNLESS YOU NEED TO MANAGE MEMORY ISSUES!")]
		public FsmBool cullDespawned;
		
		[Tooltip("The number of TOTAL (spawned + despawned) instances to keep.")]
		public FsmInt cullAbove;
		
		[Tooltip("The amount of time, in seconds, to wait before culling. This is timed " +
		         "from the moment when the Queue's TOTAL count (spawned + despawned) " +
		         "becomes greater than 'Cull Above'. Once triggered, the timer is repeated " +
		         "until the count falls below 'Cull Above'.")]
		public FsmInt cullDelay;
		
		public override void OnEnter ()
		{
			base.OnEnter ();
			DoCreatePrefabPool ();
			Finish ();
		}	
		
		private void DoCreatePrefabPool()
		{
			if (poolName.Value == "") {
				Debug.LogError ("Pool name not set");
				return;
			}
			
			foreach(KeyValuePair<string,SpawnPool> item in PoolManager.Pools)
			{
				Debug.Log(item.Key);
			}

			SpawnPool pool;
			if (!PoolManager.Pools.TryGetValue( poolName.Value, out pool)) {
				Debug.Log("Pool does not exists!");
				return;
			}

			if (prefab.Value == null) {
				Debug.Log("Prefab not set");
				return;
				
			}
			
			PrefabPool prefabPool = new PrefabPool (((GameObject)prefab.Value).transform);
			bool isAlreadyPool = pool.GetPrefab (prefabPool.prefab) == null ? false : true;
			
			if (isAlreadyPool) {
				return;
			}
			
			prefabPool.preloadAmount = preloadAmount.Value;
			prefabPool.cullDespawned = cullDespawned.Value;
			prefabPool.cullAbove = cullAbove.Value;
			prefabPool.cullDelay = cullDelay.Value;
			prefabPool.limitInstances = limitInstances.Value;
			prefabPool.limitAmount = limitAmount.Value;

			try
			{
				pool.CreatePrefabPool (prefabPool);
			}catch(Exception e)
			{
				Debug.LogWarning(e.Message);
				return;
			}
		}
		
	}
}
#endif