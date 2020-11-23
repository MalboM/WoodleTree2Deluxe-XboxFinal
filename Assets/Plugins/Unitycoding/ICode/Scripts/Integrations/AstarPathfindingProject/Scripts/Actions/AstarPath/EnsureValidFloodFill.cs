#if A_PATHFINDING_PROJECT
using UnityEngine;
using System.Collections;
using Pathfinding;

namespace ICode.Actions.AStarProject{
	[Category("AStarProject")]   
	[Tooltip("If a WorkItem needs to have a valid flood fill during execution, call this method to ensure there are no pending flood fills.")]
	[HelpUrl("http://arongranberg.com/astar/docs/class_astar_path.php#ae4b513cddd594f1c359e4f0a3e79a8c6")]
	[System.Serializable]
	public class EnsureValidFloodFill : StateAction {
		
		public override void OnEnter ()
		{
			AstarPath.active.BlockUntilPathQueueBlocked ();
		}
	}
}
#endif