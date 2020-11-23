#if EASY_TOUCH_4
using UnityEngine;
using System.Collections;

namespace ICode.Actions.EasyTouch4{
	[Category("EasyTouch")]   
	[System.Serializable]
	public  class Set3DPickableLayer : StateAction {
		public LayerMask value;
		[Tooltip("Execute the action every frame.")]
		public bool everyFrame;

		public override void OnEnter()
		{
			EasyTouch.Set3DPickableLayer (value);
			if (!everyFrame) {
				Finish();			
			}
		}

		public override void OnUpdate ()
		{
			EasyTouch.Set3DPickableLayer (value);
		}
	}
}
#endif