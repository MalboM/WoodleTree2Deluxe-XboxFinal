#if A_PATHFINDING_PROJECT
using UnityEngine;
using System.Collections;
using Pathfinding;

namespace ICode.Actions.AStarProject{
	[Category("AStarProject")]   
	[Tooltip("Applies links to the scanned graphs.")]
	[HelpUrl("http://arongranberg.com/astar/docs/class_astar_path.php#a38640086da775ea2217cc45d0a08e305")]
	[System.Serializable]
	public class ApplyLinks : StateAction {
		
		public override void OnEnter ()
		{
			AstarPath.active.ApplyLinks ();
		}
	}
}
#endif