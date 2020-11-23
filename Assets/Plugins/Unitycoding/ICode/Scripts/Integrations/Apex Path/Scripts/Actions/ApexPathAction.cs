#if APEX_PATH
using UnityEngine;
using Apex.PathFinding;
using Apex.Steering.Components;
using Apex.Units;

namespace ICode.Actions.ApexPathfinding{
	[System.Serializable]
	public abstract class ApexPathAction : StateAction
	{
		[SharedPersistent]
		[Tooltip("GameObject to use.")]
		public FsmGameObject gameObject;
		protected SteerForPathComponent steerForPath;
		protected UnitComponent unit;
		protected PathOptionsComponent options;

		public override void OnEnter (){
			steerForPath = gameObject.Value.GetComponent<SteerForPathComponent>();
			unit = gameObject.Value.GetComponent<UnitComponent> ();
			options = gameObject.Value.GetComponent<PathOptionsComponent> ();
		}

	}
}
#endif