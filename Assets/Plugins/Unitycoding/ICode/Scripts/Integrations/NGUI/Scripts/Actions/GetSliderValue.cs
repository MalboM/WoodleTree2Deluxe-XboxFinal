#if NGUI
using UnityEngine;
using System.Collections;

namespace ICode.Actions.NGUI{
	[Category("NGUI")]  
	[Tooltip("Gets the value of an UISilder.")]
	[System.Serializable]
	public class GetSliderValue : StateAction {
		[SharedPersistent]
		[Tooltip("The game object to use.")]
		public FsmGameObject gameObject;
		[Shared]
		[Tooltip( "Store the value.")]
		public FsmFloat store;
		
		[Tooltip("Execute the action every frame.")]
		public bool everyFrame;

		private UISlider slider;
		public override void OnEnter ()
		{

			slider = gameObject.Value.GetComponent<UISlider> ();
			store.Value=slider.value;
			if (!everyFrame) {
				Finish ();			
			}
		}
		
		public override void OnUpdate ()
		{
			store.Value=slider.value;
		}
	}
}
#endif