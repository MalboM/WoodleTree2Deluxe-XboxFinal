using UnityEngine;
using System.Collections;

namespace ICode.Actions.UnityVector2{
	[Category(Category.Vector2)] 
	[Tooltip("Set the x,y component of the Vector3.")]
	[System.Serializable]
	public class SetXY : StateAction {
		[Shared]
		[Tooltip("Vector2 to use")]
		public FsmVector2 vector;
		[NotRequired]
		[Tooltip("The x value to set.")]
		public FsmFloat x;
		[NotRequired]
		[Tooltip("The y value to set.")]
		public FsmFloat y;
		[Tooltip("Execute the action every frame.")]
		public bool everyFrame;
		
		public override void OnEnter ()
		{
			vector.Value = new Vector2 (x.IsNone?vector.Value.x: x.Value,y.IsNone?vector.Value.y: y.Value);
			if (!everyFrame) {
				Finish ();
			}
		}
		
		public override void OnUpdate ()
		{
			vector.Value = new Vector2 (x.IsNone?vector.Value.x: x.Value,y.IsNone?vector.Value.y: y.Value);
		}
	}
}