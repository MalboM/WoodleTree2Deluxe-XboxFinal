using UnityEngine;
using System.Collections;

#if UNITY_5
namespace ICode{
	public class StartStateMachine : StateMachineBehaviour {
		public StateMachine stateMachine;
		public bool stopOnExit=true;
		private ICodeBehaviour behaviour;

		public override void OnStateEnter (Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			if (behaviour == null) {
				behaviour = animator.gameObject.AddBehaviour (stateMachine);
			} else {
				behaviour.EnableStateMachine ();
				(behaviour.ActiveNode as State).Restart ();
			}
		}
		
		public override void OnStateExit (Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			if (stopOnExit) {
				behaviour.DisableStateMachine(false);
			}
		}
	}
}
#endif