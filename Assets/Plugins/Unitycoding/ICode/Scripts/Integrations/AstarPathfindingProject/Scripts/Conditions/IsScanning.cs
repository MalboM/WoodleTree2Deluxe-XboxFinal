#if A_PATHFINDING_PROJECT
using UnityEngine;
using System.Collections;

namespace ICode.Conditions.AStarProject{
	[Category("AStarProject")]    
	[Tooltip("Checks if the active AstarPath is scanning.")]
	[HelpUrl("http://arongranberg.com/astar/docs/class_astar_path.php#a44b1434adecf54cf9e59eba562b9950d")]
	[System.Serializable]
	public class IsScanning : Condition {
		[Tooltip("Does the result equals this condition.")]
		public FsmBool equals;

		public override bool Validate ()
		{
			return AstarPath.active.isScanning == equals.Value;
		}
	}
}
#endif