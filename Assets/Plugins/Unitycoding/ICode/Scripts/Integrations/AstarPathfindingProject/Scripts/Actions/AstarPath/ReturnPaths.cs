#if A_PATHFINDING_PROJECT
using UnityEngine;
using System.Collections;
using Pathfinding;

namespace ICode.Actions.AStarProject{
	[Category("AStarProject")]   
	[Tooltip("Returns all paths in the return stack.")]
	[HelpUrl("http://arongranberg.com/astar/docs/class_astar_path.php#ae4b513cddd594f1c359e4f0a3e79a8c6")]
	[System.Serializable]
	public class ReturnPaths : StateAction {
		[Tooltip("Do not return all paths at once if it takes a long time, instead return some and wait until the next call.")]
		public FsmBool timeSlice;
		public override void OnEnter ()
		{
			AstarPath.active.ReturnPaths (timeSlice.Value);
		}
	}
}
#endif