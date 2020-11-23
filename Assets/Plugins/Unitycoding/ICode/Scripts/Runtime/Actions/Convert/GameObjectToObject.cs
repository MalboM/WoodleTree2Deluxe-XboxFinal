using UnityEngine;
using System.Collections;

namespace ICode.Actions.Convert{
	[Category(Category.Convert)]
	[Tooltip("Converts a FsmGameObject value to a FsmObject value.")]
	[System.Serializable]
	public class GameObjectToObject : StateAction {
		[InspectorLabel("Object")]
		[Shared]
		[Tooltip("GameObject to use.")]
		public FsmGameObject gameObject;
		[Shared]
		[Tooltip("Store the result.")]
		public FsmObject store;
		[Tooltip("Execute the action every frame.")]
		public bool everyFrame;
		
		public override void OnEnter ()
		{
			DoConvert ();
			if (!everyFrame) {
				Finish ();
			}
		}
		
		public override void OnUpdate ()
		{
			DoConvert ();
		}
		
		private void DoConvert(){
			store.Value = gameObject.Value as GameObject;
		}
	}
}