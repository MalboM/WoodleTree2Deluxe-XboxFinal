#if A_PATHFINDING_PROJECT
using UnityEngine;
using System.Collections;
using Pathfinding;

namespace ICode.Actions.AStarProject{
	[Category("AStarProject")]   
	[Tooltip("Update all graphs within bounds after delay in seconds.")]
	[HelpUrl("http://arongranberg.com/astar/docs/class_astar_path.php#ae4b513cddd594f1c359e4f0a3e79a8c6")]
	[System.Serializable]
	public class UpdateGraphs : StateAction {
		[Tooltip("Center in worldspace.")]
		public FsmVector3 center;
		[Tooltip("Size in Worldspace")]
		public FsmVector3 size;
		[Tooltip("Delay")]
		public FsmFloat delay;

		public override void OnEnter ()
		{
			var bounds = new UnityEngine.Bounds (center.Value, size.Value);
			AstarPath.active.UpdateGraphs(bounds,delay.Value);
		}
	}
}
#endif