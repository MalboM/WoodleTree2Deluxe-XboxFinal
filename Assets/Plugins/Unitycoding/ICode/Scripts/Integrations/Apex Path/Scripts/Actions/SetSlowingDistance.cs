#if APEX_PATH
using UnityEngine;
using Apex.Steering.Components;

namespace ICode.Actions.ApexPathfinding{
	[Category("Apex Path")]
	[Tooltip("The distance within which the unit will start to slow down for arrival.")] 
	[System.Serializable]
	public class SetSlowingDistance: ApexPathAction
	{
		[Tooltip("The distance to set.")]
		public FsmFloat value;
		
		public override void OnEnter (){
			base.OnEnter ();
			steerForPath.slowingDistance = value.Value;
			Finish ();
		}
		
	}
}
#endif