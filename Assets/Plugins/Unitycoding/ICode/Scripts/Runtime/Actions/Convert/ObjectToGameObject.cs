using UnityEngine;
using System.Collections;

namespace ICode.Actions.Convert{
	[Category(Category.Convert)]
	[Tooltip("Converts a FsmObject value to a FsmGameObject value.")]
	[System.Serializable]
	public class ObjectToGameObject : StateAction {
		[InspectorLabel("Object")]
		[Shared]
		[Tooltip("Object to use.")]
		public FsmObject _object;
		[Shared]
		[Tooltip("Store the result.")]
		public FsmGameObject store;
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
			if (_object.Value is GameObject) {
				store.Value = _object.Value as GameObject;
			}
		}
	}
}