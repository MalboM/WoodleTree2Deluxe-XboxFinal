#if APEX_PATH
using UnityEngine;
using Apex.Steering.Components;
using Apex.PathFinding;

namespace ICode.Actions.ApexPathfinding{
	[Category("Apex Path")]
	[Tooltip("Sets the replan mode.")] 
	[System.Serializable]
	public class SetReplanMode: ApexPathAction
	{
		[Tooltip("ReplanMode to set")]
		public ReplanMode replanMode;
	
		public override void OnEnter (){
			base.OnEnter ();
			options.replanMode = replanMode;
			Finish ();
		}
		
	}
}
#endif