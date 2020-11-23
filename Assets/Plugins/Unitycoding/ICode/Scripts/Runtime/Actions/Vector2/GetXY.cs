using UnityEngine;
using System.Collections;

namespace ICode.Actions.UnityVector2{
	[Category(Category.Vector2)]    
	[Tooltip("Gets the components x,y of a vector2.")]
	[HelpUrl("")]
	[System.Serializable]
	public class GetXY : StateAction {
		[Tooltip("Vector to use.")]
		public FsmVector2 vector;
		[Shared]
		[NotRequired]
		[Tooltip("X component of the vector.")]
		public FsmFloat x;
		[Shared]
		[NotRequired]
		[Tooltip("Y component of the vector.")]
		public FsmFloat y;

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
		}
	}
}