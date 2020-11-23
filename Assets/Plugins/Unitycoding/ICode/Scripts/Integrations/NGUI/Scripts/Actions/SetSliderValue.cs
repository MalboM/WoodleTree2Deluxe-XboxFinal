#if NGUI
using UnityEngine;
using System.Collections;

namespace ICode.Actions.NGUI{
	[Category("NGUI")]  
	[Tooltip("Sets the value of an UISilder.")]
	[System.Serializable]
	public class SetSliderValue : StateAction {
		[SharedPersistent]
		[Tooltip("The game object to use.")]
		public FsmGameObject gameObject;
		[Tooltip( "The value to set.")]
		public FsmFloat value;
		
		[Tooltip("Execute the action every frame.")]
		public bool everyFrame;

		private UISlider slider;
		public override void OnEnter ()
		{
			slider = gameObject.Value.GetComponent<UISlider> ();
			slider.value=value.Value;
			if (!everyFrame) {
				Finish ();			
			}
		}
		
		public override void OnUpdate ()
		{
			slider.value=value.Value;
		}
	}
}
#endif