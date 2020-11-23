#if APEX_PATH
using UnityEngine;
using Apex.Steering.Components;
using Apex.Steering;
using Apex.WorldGeometry;
using Apex;

namespace ICode.Actions.ApexPathfinding{
	[Category("Apex Path")]
	[Tooltip("Wander")] 
	[System.Serializable]
	public class Wander: ApexPathAction
	{
		[NotRequired]
		[Tooltip("Start position to wander around.")]
		public FsmVector3 startPosition;
		[Tooltip("The agent has arrived when the remaining distance is less then this value.")]
		[DefaultValue(0.5f)]
		public FsmFloat threshold;
		[Tooltip("How far away to wander from the current position.")]
		[DefaultValue(20.0f)]
		public FsmFloat wanderDistance;
		[Tooltip("Turn rate.")]
		[DefaultValue(2.0f)]
		public FsmFloat turnRate;

		
		public override void OnEnter (){
			base.OnEnter ();
			Move ();
		}
		
		public override void OnUpdate ()
		{
			if (steerForPath.finalDestination != null && GetSqrDistXZ(steerForPath.finalDestination.Value,unit.position) < threshold.Value) {
				Move();
			}
		}
		
		private void Move(){
			bool pointFound = false;
			Vector3 pos = Vector3.zero;
			var unitMask = unit.attributes;
			
			while (!pointFound)
			{
				pos=startPosition.IsNone?GetTargetPosition():GetTargetPositionWithin(startPosition.Value);
				var grid = GridManager.instance.GetGrid(pos);
				if (grid != null)
				{
					var cell = grid.GetCell(pos);
					pointFound = cell.isWalkable(unitMask);
				}
			}
			steerForPath.MoveTo(pos.AdjustAxis(0.0f,Axis.Y),false);
		}

		private Vector3 GetTargetPosition(){
			Vector3 direction = unit.forward + Random.insideUnitSphere * turnRate.Value;
			return unit.position + direction.normalized * wanderDistance.Value;	
		}

		private Vector3 GetTargetPositionWithin(Vector3 _startPos){
			Vector3	pos = _startPos + (Random.insideUnitSphere * Random.Range(1.0f, unit.radius)).AdjustAxis(0.0f, Axis.Y);
			
			var dir = (pos - unit.position.AdjustAxis(0.0f, Axis.Y));
			var dist = dir.magnitude;
			if (dist < this.wanderDistance)
			{
				pos = unit.position + ((dir / dist) * this.wanderDistance);
			}	
			return pos;
		}

		private float GetSqrDistXZ(Vector3 a, Vector3 b)
		{
			Vector3 vector = new Vector3(a.x - b.x, 0, a.z - b.z);
			return vector.sqrMagnitude;
		}
	}
}
#endif