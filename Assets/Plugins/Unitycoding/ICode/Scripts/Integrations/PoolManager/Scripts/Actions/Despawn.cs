#if POOL_MANAGER
using UnityEngine;
using System.Collections;
using System;
using PathologicalGames;

namespace ICode.Actions.PathologicalPoolManager{
	[Category("PoolManager")]    
	[Tooltip("If the passed transform is managed by the SpawnPool, it will be deactivated and made available to be spawned again.")]
	[System.Serializable]
	public class Despawn : StateAction {
		[Tooltip("Pool Name")]
		public FsmString poolName;
		[SharedPersistent]
		[Tooltip("GameObject or prefab to despawn.")]
		public FsmGameObject gameObject;
		[Tooltip("Delay")]
		public FsmFloat delay;
		
		public override void OnEnter ()
		{
			DoDespawn ();
			Finish ();
		}	
		
		private void DoDespawn()
		{
			if (string.IsNullOrEmpty(poolName.Value)){
				return;
			}
			
			if (gameObject.Value == null)
			{
				return;
			}
			SpawnPool pool;
			if (! PoolManager.Pools.TryGetValue (poolName.Value, out pool)) {
				Debug.Log (string.Format("Pool {0} doesn't exists",poolName.Value));
				return;
			}

			if (delay.Value >0){
				pool.Despawn(((GameObject)gameObject.Value).transform, delay.Value);
			}else{
				pool.Despawn(((GameObject)gameObject.Value).transform);
			}
		}
		
	}
}
#endif