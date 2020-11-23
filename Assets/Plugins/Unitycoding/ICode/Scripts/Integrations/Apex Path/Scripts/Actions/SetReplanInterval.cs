#if APEX_PATH
using UnityEngine;
using Apex.Steering.Components;

namespace ICode.Actions.ApexPathfinding{
	[Category("Apex Path")]
	[Tooltip("The distance from the final destination where the unit will stop. Any value greater than half the cell size of the grid is treated as half a step size.")] 
	[System.Serializable]
	public class SetReplanInterval: ApexPathAction
	{
		[Tooltip("The inteval to set.")]
		public FsmFloat value;
		
		public override void OnEnter (){
			base.OnEnter ();
			options.replanInterval = value.Value;
			Finish ();
		}
		
	}
}
#endif