#if EASY_TOUCH_4
using UnityEngine;
using System.Collections;

namespace ICode.Actions.EasyTouch4{
	[Category("EasyTouch")]   
	[System.Serializable]
	public  class GetAxis : StateAction {
		[Tooltip("The name of the axis")]
		public FsmString axisName;
		[Shared]
		[Tooltip("Store the axis value.")]
		public FsmFloat store;

		public override void OnUpdate()
		{
			store.Value = ETCInput.GetAxis(axisName.Value);
		}
	}
}
#endif