#if EASY_TOUCH_4
using UnityEngine;
using System.Collections;

namespace ICode.Actions.EasyTouch4{
	[Category("EasyTouch")]   
	[System.Serializable]
	public  class GetUICompatibility : StateAction {
		[Shared]
		[Tooltip("Store the value.")]
		public FsmBool store;
		[Tooltip("Execute the action every frame.")]
		public bool everyFrame;

		public override void OnEnter()
		{
			store.Value = EasyTouch.GetUIComptability ();
			if (!everyFrame) {
				Finish();			
			}
		}
		
		public override void OnUpdate ()
		{
			store.Value = EasyTouch.GetUIComptability ();
		}
	}
}
#endif