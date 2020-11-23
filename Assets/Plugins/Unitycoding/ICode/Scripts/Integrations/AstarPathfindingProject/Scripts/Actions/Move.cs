#if A_PATHFINDING_PROJECT
using UnityEngine;
using System.Collections;
using Pathfinding;

namespace ICode.Actions.AStarProject{
	[Category("AStarProject")]   
	[System.Serializable]
	public class Move : StateAction {
		[SharedPersistent]
		[Tooltip("The GameObject to use.")]
		public FsmGameObject gameObject;
		[NotRequired]
		[SharedPersistent]
		[Tooltip("Use a target instead of destination.")]
		public FsmGameObject target;
		[NotRequired]
		[Tooltip("The destination point to move towards.")]
		public FsmVector3 destination;
		[Tooltip("Repath rate")]
		public FsmFloat repathRate;
		[Tooltip("The speed to move with.")]
		public FsmFloat speed;
		[Tooltip("The angular speed.")]
		public FsmFloat rotation;
		[Tooltip("Path point distance to move to the next point.")]
		public FsmFloat threshold;

		private Seeker seeker;
		private CharacterController characterController;
		private Path path;
		private int currentWaypoint = 0;
		private float repathTime;

		public override void OnEnter ()
		{
			seeker = gameObject.Value.GetComponent < Seeker> ();
			characterController = gameObject.Value.GetComponent<CharacterController> ();
		}

		public override void OnUpdate ()
		{
			if (Time.time > repathTime) {
				seeker.StartPath (seeker.transform.position,FsmUtility.GetPosition(target,destination), OnPathComplete);
				repathTime=Time.time+repathRate.Value;
			}
			if (path == null || path.vectorPath.Count <= currentWaypoint) {
				//We have no path to move after yet
				return;
			}
			
			Vector3 dir = (path.vectorPath [currentWaypoint] - seeker.transform.position);
			if (dir != Vector3.zero) {
				dir.y = 0;
				Quaternion wantedRotation = Quaternion.LookRotation (dir);
				seeker.transform.rotation = Quaternion.Slerp (seeker.transform.rotation, wantedRotation, Time.deltaTime * rotation.Value);
			}
			dir = dir.normalized;
			dir *= speed.Value;
			
			characterController.SimpleMove (dir);
			characterController.Move (Vector3.down * 15 * Time.deltaTime);
			if (Vector3.Distance (seeker.transform.position, path.vectorPath [currentWaypoint]) < threshold.Value) {
				currentWaypoint++;
			}
		}

		protected virtual void OnPathComplete(Path p){
			if (!p.error) {
				path = p;
				//Reset the waypoint counter
				currentWaypoint = 1;
			}
		}
	}
}
#endif