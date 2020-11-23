#if EASY_TOUCH_4
using UnityEngine;
using System.Collections;

namespace ICode.Actions.EasyTouch4{
	[Category("EasyTouch")]   
	[System.Serializable]
	public  class GetButtonUp : StateAction {
		[Tooltip("The name of the button.")]
		public FsmString buttonName;
		[Shared]
		[Tooltip("Store the result value.")]
		public FsmBool store;
		
		public override void OnUpdate()
		{
			store.Value = ETCInput.GetButtonUp (buttonName.Value);
		}
	}
}
#endif