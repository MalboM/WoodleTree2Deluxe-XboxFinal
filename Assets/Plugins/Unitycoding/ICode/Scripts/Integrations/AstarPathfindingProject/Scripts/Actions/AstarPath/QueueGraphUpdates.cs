#if A_PATHFINDING_PROJECT
using UnityEngine;
using System.Collections;
using Pathfinding;

namespace ICode.Actions.AStarProject{
	[Category("AStarProject")]   
	[Tooltip("Will apply queued graph updates as soon as possible, regardless of limitGraphUpdates.")]
	[HelpUrl("http://arongranberg.com/astar/docs/class_astar_path.php#ae4b513cddd594f1c359e4f0a3e79a8c6")]
	[System.Serializable]
	public class QueueGraphUpdates : StateAction {
		
		public override void OnEnter ()
		{
			AstarPath.active.QueueGraphUpdates ();
		}
	}
}
#endif