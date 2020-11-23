#if APEX_PATH
using UnityEngine;
using Apex.Steering.Components;

namespace ICode.Actions.ApexPathfinding{
	[Category("Apex Path")]
	[Tooltip("The priority with with this unit's path requests should be processed")] 
	[System.Serializable]
	public class SetPathingPriority : ApexPathAction
	{
		[Tooltip("The value to set.")]
		public FsmInt priority;

		public override void OnEnter (){
			base.OnEnter ();
			options.pathingPriority = priority.Value;
			Finish ();
		}

	}
}
#endif