#if APEX_PATH
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace ICode.Actions.ApexPathfinding{
	[Category("Apex Path")]
	[Tooltip("Asks the object to move to the specified position.")]
	[System.Serializable]
	public class MoveTo: ApexPathAction
	{
		[SharedPersistent]
		[NotRequired]
		[Tooltip("Target position to move to.")]
		public FsmGameObject target;
		[NotRequired]
		[Tooltip("The position to move to.")]
		public FsmVector3 position;
		[Tooltip("If set to true the destination is added as a way point.")]
		public FsmBool append;
	
		public override void OnEnter (){
			base.OnEnter ();
			steerForPath.MoveTo (FsmUtility.GetPosition(target.Value,position.Value), append.Value);
			Finish ();
		}
	}
}
#endif