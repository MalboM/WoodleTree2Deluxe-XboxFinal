#if NGUI
using UnityEngine;
using System.Collections;

namespace ICode.Actions.NGUI{
	[Category("NGUI")]   
	[Tooltip("Reposition the elements in a UITable.")]
	[System.Serializable]
	public class Reposition : StateAction {
		[SharedPersistent]
		[Tooltip("The game object to use.")]
		public FsmGameObject gameObject;
		[Tooltip("Execute the action every frame.")]
		public bool everyFrame;

		private UITable table;
		public override void OnEnter ()
		{
			table = gameObject.Value.GetComponent<UITable> ();
			table.Reposition ();
			if (!everyFrame) {
				Finish();			
			}
		}
		
		public override void OnUpdate ()
		{
			table.Reposition ();
		}
	}
}
#endif