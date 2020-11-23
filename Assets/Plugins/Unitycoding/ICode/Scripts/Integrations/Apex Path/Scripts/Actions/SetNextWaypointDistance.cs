#if APEX_PATH
using UnityEngine;
using Apex.Steering.Components;

namespace ICode.Actions.ApexPathfinding{
	[Category("Apex Path")]
	[Tooltip("The distance from the current way point at which the next way point will be requested")] 
	[System.Serializable]
	public class SetNextWaypointDistance: ApexPathAction
	{
		[Tooltip("The distance to set.")]
		public FsmFloat value;
		
		public override void OnEnter (){
			base.OnEnter ();
			options.requestNextWaypointDistance = value.Value;
			Finish ();
		}
		
	}
}
#endif