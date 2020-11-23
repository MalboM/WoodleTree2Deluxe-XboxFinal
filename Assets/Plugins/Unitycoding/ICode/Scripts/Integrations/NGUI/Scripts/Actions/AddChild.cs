#if NGUI
using UnityEngine;
using System.Collections;

namespace ICode.Actions.NGUI{
	[Category("NGUI")]   
	[Tooltip("Instantiate an object and add it to the specified parent.")]
	[System.Serializable]
	public class AddChild : StateAction {
		[SharedPersistent]
		[Tooltip("The game object to use.")]
		public FsmGameObject parent;
		[Tooltip("The prefab to use.")]
		public FsmGameObject prefab;
		[Shared]
		[NotRequired]
		[Tooltip("Store the instantiated GameObject.")]
		public FsmGameObject store;
		[Tooltip("Execute the action every frame.")]
		public bool everyFrame;

		public override void OnEnter ()
		{
			base.OnEnter ();
			store.Value = NGUITools.AddChild (parent.Value,prefab.Value);
			if (!everyFrame) {
				Finish();			
			}
		}
		
		public override void OnUpdate ()
		{
			store.Value = NGUITools.AddChild ((GameObject)parent.Value,(GameObject) prefab.Value);
		}
	}
}
#endif