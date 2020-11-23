#if NGUI
using UnityEngine;
using System.Collections;

namespace ICode.Actions.NGUI{
	[Category("NGUI")]  
	[Tooltip("Sets the text of an UILabel.")]
	[System.Serializable]
	public class SetLabelValue : StateAction {
		[SharedPersistent]
		[Tooltip("The game object to use.")]
		public FsmGameObject gameObject;
		[Tooltip( "New text to set.")]
		public FsmString text;
		
		[Tooltip("Execute the action every frame.")]
		public bool everyFrame;

		private UILabel label;
		public override void OnEnter ()
		{
			label = gameObject.Value.GetComponent<UILabel> ();
			label.text=text.Value;
			if (!everyFrame) {
				Finish ();			
			}
		}

		public override void OnUpdate ()
		{
			label.text = text.Value;
		}
	}
}
#endif