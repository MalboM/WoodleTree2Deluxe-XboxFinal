#if EASY_TOUCH_4
using UnityEngine;
using System.Collections;

namespace ICode.Actions.EasyTouch4{
	[Category("EasyTouch")]   
	[System.Serializable]
	public  class AddCamera : StateAction {
		[SharedPersistent]
		[Tooltip("Camera GameObject to use.")]
		public FsmGameObject gameObject;
		public FsmBool guiCamera;

		public override void OnEnter()
		{
			EasyTouch.AddCamera (gameObject.Value.camera,guiCamera.Value);
			Finish ();
		}
	}
}
#endif