#if EASY_TOUCH_4
using UnityEngine;
using System.Collections;

namespace ICode.Actions.EasyTouch4{
	[Category("EasyTouch")]   
	[System.Serializable]
	public  class SetAutoUpdatePickedObject : StateAction {
		public FsmBool value;
		[Tooltip("Execute the action every frame.")]
		public bool everyFrame;

		public override void OnEnter()
		{
			EasyTouch.SetAutoUpdatePickedObject (value.Value);
			if (!everyFrame) {
				Finish();			
			}
		}

		public override void OnUpdate ()
		{
			EasyTouch.SetAutoUpdatePickedObject (value.Value);
		}
	}
}
#endif