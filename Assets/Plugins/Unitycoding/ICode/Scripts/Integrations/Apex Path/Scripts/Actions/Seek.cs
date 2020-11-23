#if APEX_PATH
using UnityEngine;
using Apex.Steering.Components;
using Apex.Steering;

namespace ICode.Actions.ApexPathfinding{
	[Category("Apex Path")]
	[Tooltip("Seek a target.")] 
	[System.Serializable]
	public class Seek: ApexPathAction
	{
		[SharedPersistent]
		[Tooltip("Target to seek.")]
		public FsmGameObject target;
		[Tooltip("Minimum time in seconds before new path calculations.")]
		public FsmFloat repathInterval;
		[Tooltip("The agent has arrived when the distance to target is less then this value")]
		[DefaultValueAttribute(1.5f)]
		public FsmFloat stoppingDistance;

		private float repathTime;
		private Transform mTarget;
		private Vector3 destination;

		public override void OnEnter (){
			base.OnEnter ();
			mTarget = target.Value.transform;
			repathTime=Time.time+repathInterval.Value;
			steerForPath.MoveTo (mTarget.position, false);
			destination = mTarget.position;
		}

		public override void OnUpdate ()
		{
			float sqrDist = GetSqrDistXZ (mTarget.position, destination);
			if (sqrDist < stoppingDistance.Value) {
				return;			
			}

			if (Time.time > repathTime) {
				repathTime = Time.time + repathInterval.Value;
				steerForPath.MoveTo (mTarget.position, false);
				destination=mTarget.position;
			} 
		}


		private float GetSqrDistXZ(Vector3 a, Vector3 b)
		{
			Vector3 vector = new Vector3(a.x - b.x, 0, a.z - b.z);
			return vector.sqrMagnitude;
		}
	}
}
#endif