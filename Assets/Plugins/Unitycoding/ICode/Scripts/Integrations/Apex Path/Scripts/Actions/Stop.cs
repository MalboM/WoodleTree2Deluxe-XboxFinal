#if APEX_PATH
using UnityEngine;
using Apex.Steering.Components;

namespace ICode.Actions.ApexPathfinding{
	[Category("Apex Path")]
	[Tooltip("Stop following the path.")] 
	[System.Serializable]
	public class Stop: ApexPathAction
	{

		public override void OnEnter (){
			base.OnEnter ();
			steerForPath.Stop ();
			Finish ();
		}
		
		
	
	}
}
#endif