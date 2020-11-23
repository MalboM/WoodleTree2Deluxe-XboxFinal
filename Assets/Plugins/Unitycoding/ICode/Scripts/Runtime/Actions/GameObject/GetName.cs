using UnityEngine;
using System.Collections;

namespace ICode.Actions{
	[Category(Category.GameObject)]
	[Tooltip("Get the name of the game object.")]
	[HelpUrl("https://docs.unity3d.com/Documentation/ScriptReference/Object-name.html")]
	[System.Serializable]
	public class GetName : StateAction {
		[SharedPersistent]
		[Tooltip("GameObject to use.")]
		public FsmGameObject gameObject;
		[Shared]
		[InspectorLabel("Name")]
		[Tooltip("Store the name.")]
		public FsmString _name;
		[Tooltip("Execute the action every frame.")]
		public bool everyFrame;

		public override void OnEnter ()
		{
			_name.Value=gameObject.Value.name;
			if (!everyFrame) {
				Finish ();
			}
		}

		public override void OnUpdate ()
		{
			_name.Value=gameObject.Value.name;
		}
	}
}