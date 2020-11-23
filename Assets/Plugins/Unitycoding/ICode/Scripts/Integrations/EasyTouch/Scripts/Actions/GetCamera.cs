#if EASY_TOUCH_4
using UnityEngine;
using System.Collections;

namespace ICode.Actions.EasyTouch4{
	[Category("EasyTouch")]   
	[System.Serializable]
	public  class GetCamera : StateAction {
		public FsmInt index;
		[Shared]
		public FsmGameObject store;
		[Tooltip("Execute the action every frame.")]
		public bool everyFrame;

		public override void OnEnter()
		{
			store.Value = EasyTouch.GetCamera (index.Value).gameObject;
			if (!everyFrame) {
				Finish();			
			}
		}

		public override void OnUpdate ()
		{
			store.Value = EasyTouch.GetCamera (index.Value).gameObject;
		}
	}
}
#endif