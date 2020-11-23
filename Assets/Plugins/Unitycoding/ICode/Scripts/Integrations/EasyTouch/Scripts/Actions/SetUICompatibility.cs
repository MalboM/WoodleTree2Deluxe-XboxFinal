#if EASY_TOUCH_4
using UnityEngine;
using System.Collections;

namespace ICode.Actions.EasyTouch4{
	[Category("EasyTouch")]   
	[System.Serializable]
	public  class SetUICompatibility : StateAction {
		public FsmBool value;
		[Tooltip("Execute the action every frame.")]
		public bool everyFrame;

		public override void OnEnter()
		{
			EasyTouch.SetUICompatibily (value.Value);
			if (!everyFrame) {
				Finish();			
			}
		}

		public override void OnUpdate ()
		{
			EasyTouch.SetUICompatibily (value.Value);
		}
	}
}
#endif