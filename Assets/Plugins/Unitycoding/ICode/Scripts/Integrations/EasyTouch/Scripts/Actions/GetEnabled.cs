#if EASY_TOUCH_4
using UnityEngine;
using System.Collections;

namespace ICode.Actions.EasyTouch4{
	[Category("EasyTouch")]   
	[System.Serializable]
	public  class GetEnabled : StateAction {
		[Shared]
		public FsmBool store;
		[Tooltip("Execute the action every frame.")]
		public bool everyFrame;

		public override void OnEnter()
		{
			store.Value = EasyTouch.GetEnabled ();
			if (!everyFrame) {
				Finish();			
			}
		}

		public override void OnUpdate ()
		{
			store.Value = EasyTouch.GetEnabled ();
		}
	}
}
#endif