using UnityEngine;
using System.Collections;

namespace ICode.Actions.UnityNavMeshAgent{
	[System.Serializable]
	public abstract class NavMeshAgentAction : StateAction {
		[SharedPersistent]
		[Tooltip("GameObject to use.")]
		public FsmGameObject gameObject;

		protected UnityEngine.AI.NavMeshAgent agent;
	
		public override void OnEnter ()
		{
			agent = gameObject.Value.GetComponent<UnityEngine.AI.NavMeshAgent> ();
		}
	}
}