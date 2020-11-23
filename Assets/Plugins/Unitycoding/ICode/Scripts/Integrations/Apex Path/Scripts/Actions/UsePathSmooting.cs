#if APEX_PATH
using UnityEngine;
using Apex.Steering.Components;

namespace ICode.Actions.ApexPathfinding{
	[Category("Apex Path")]
	[Tooltip("Whether to use path smoothing. Path smoothing creates more natural routes at a small cost to performance.")] 
	[System.Serializable]
	public class UsePathSmoothing : ApexPathAction
	{
		[Tooltip("Set to true or false.")]
		public FsmBool value;
		
		public override void OnEnter (){
			base.OnEnter ();
			options.usePathSmoothing = value.Value;
			Finish ();
		}
		
	}
}
#endif