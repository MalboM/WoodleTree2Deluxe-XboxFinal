#if EASY_TOUCH_4
using UnityEngine;
using System.Collections;

namespace ICode.Actions.EasyTouch4{
	[Category("EasyTouch")]   
	[System.Serializable]
	public  class RemoveCamera : StateAction {
		[SharedPersistent]
		[Tooltip("Camera GameObject to use.")]
		public FsmGameObject gameObject;

		public override void OnEnter()
		{
			EasyTouch.RemoveCamera (gameObject.Value.camera);
			Finish ();
		}
	}
}
#endif