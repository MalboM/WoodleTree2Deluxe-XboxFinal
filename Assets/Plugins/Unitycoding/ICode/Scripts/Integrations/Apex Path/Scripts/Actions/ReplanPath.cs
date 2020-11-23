#if APEX_PATH
using UnityEngine;
using Apex.Steering.Components;

namespace ICode.Actions.ApexPathfinding{
	[Category("Apex Path")]
	[Tooltip("Replans the path.")] 
	[System.Serializable]
	public class ReplanPath: ApexPathAction
	{	
		public override void OnEnter (){
			base.OnEnter ();
			steerForPath.ReplanPath ();
			Finish ();
		}
	}
}
#endif