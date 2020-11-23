using UnityEngine;
using System.Collections;
using System;

namespace ICode.Actions.UnityVector3{
	[Obsolete("This action is obsolete, please use GetXYZ!")]
	[Category(Category.Vector3)]    
	[Tooltip("Gets the components x,y,z of a vector3.")]
	[HelpUrl("")]
	[System.Serializable]
	public class GetComponent : StateAction {
		[Tooltip("Vector to use.")]
		public FsmVector3 vector;
		[Shared]
		[NotRequired]
		[Tooltip("X component of the vector.")]
		public FsmFloat x;
		[Shared]
		[NotRequired]
		[Tooltip("Y component of the vector.")]
		public FsmFloat y;
		[Shared]
		[NotRequired]
		[Tooltip("Z component of the vector.")]
		public FsmFloat z;

		[Tooltip("Execute the action every frame.")]
		public bool everyFrame;

		public override void OnEnter ()
		{
			DoGet ();
			if (!everyFrame) {
				Finish ();
			}
		}

		public override void OnUpdate ()
		{
			DoGet ();
		}

		private void DoGet(){
			if(!x.IsNone)
				x.Value = vector.Value.x;
			if(!y.IsNone)
				y.Value = vector.Value.y;
			if(!z.IsNone)
				z.Value = vector.Value.z;
		}
	}
}