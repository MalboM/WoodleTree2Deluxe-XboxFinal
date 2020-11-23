using UnityEngine;
using System.Collections;

namespace ICode.Conditions.UnityNavMeshAgent{
	public abstract class NavMeshAgentCondition : Condition {
		[SharedPersistent]
		[Tooltip("GameObject to test.")]
		public FsmGameObject gameObject;
		
		protected UnityEngine.AI.NavMeshAgent agent;
		
		public override void OnEnter ()
		{
			agent = gameObject.Value.GetComponent<UnityEngine.AI.NavMeshAgent> ();
		}
	}
}