#if POOL_MANAGER
using UnityEngine;
using System.Collections;
using System;
using PathologicalGames;

namespace ICode.Actions.PathologicalPoolManager{
	[Category("PoolManager")]    
	[Tooltip("Destroys a SpawnPool in PoolManager.Pools including all instances and references as well as the GameObject." +
	       "IMPORTANT: This is going to destroy the instances, NOT despawn. Make sure you only do this when performance is not an issue, such as when a level ends or a new level is loaded.")]
	[System.Serializable]
	public class DestroyPool : StateAction {
		[Tooltip("Pool Name.")]
		public FsmString poolName;

		public override void OnEnter ()
		{
			PoolManager.Pools.Destroy(poolName.Value);
			Finish ();
		}	
		
	}
}
#endif