#if NGUI
using UnityEngine;
using System.Collections;

namespace ICode.Actions.NGUI{
	[Category("NGUI")]  
	[Tooltip("Gets the text of an UILabel.")]
	[System.Serializable]
	public class GetLabelValue : StateAction {
		[SharedPersistent]
		[Tooltip("The game object to use.")]
		public FsmGameObject gameObject;
		[Shared]
		[Tooltip( "Store the value.")]
		public FsmString store;
		
		[Tooltip("Execute the action every frame.")]
		public bool everyFrame;

		private UILabel label;
		public override void OnEnter ()
		{
			label = gameObject.Value.GetComponent<UILabel> ();
			store.Value=label.text;
			if (!everyFrame) {
				Finish ();			
			}
		}
		
		public override void OnUpdate ()
		{
			store.Value=label.text;
		}
	}
}
#endif