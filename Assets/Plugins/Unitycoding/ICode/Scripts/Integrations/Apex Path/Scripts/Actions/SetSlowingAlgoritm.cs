#if APEX_PATH
using UnityEngine;
using Apex.Steering.Components;
using Apex.Steering;

namespace ICode.Actions.ApexPathfinding{
	[Category("Apex Path")]
	[Tooltip("The algorithm used to slow the unit for arrival. Linear works fine with short slowing distances, but logarithmic shows its worth at longer ones.")] 
	[System.Serializable]
	public class SetSlowingAlgoritm: ApexPathAction
	{
		[Tooltip("Algoritm to set")]
		public SlowingAlgorithm algorithm;
		
		public override void OnEnter (){
			base.OnEnter ();
			steerForPath.slowingAlgorithm = algorithm;
			Finish ();
		}
		
	}
}
#endif