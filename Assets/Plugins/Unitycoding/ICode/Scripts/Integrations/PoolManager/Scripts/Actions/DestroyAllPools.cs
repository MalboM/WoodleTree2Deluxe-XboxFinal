#if POOL_MANAGER
using UnityEngine;
using System.Collections;
using System;
using PathologicalGames;

namespace ICode.Actions.PathologicalPoolManager{
	[Category("PoolManager")]    
	[Tooltip("Destroys ALL SpawnPool in PoolManager.Pools including all instances and references as well as the GameObjects")]
	[System.Serializable]
	public class DestroyAllPools : StateAction {

		public override void OnEnter ()
		{
			PoolManager.Pools.DestroyAll();
			Finish ();
		}	

	}
}
#endif