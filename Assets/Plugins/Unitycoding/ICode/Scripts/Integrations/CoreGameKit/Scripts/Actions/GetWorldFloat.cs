#if CORE_GAME_KIT
using UnityEngine;
using System.Collections;
using DarkTonic.CoreGameKit;

namespace ICode.Actions.CoreGameKit{
	[Category("CoreGameKit")]   
	[Tooltip("Get the value of a float world variable in Core GameKit")]
	[System.Serializable]
	public class GetWorldFloat : StateAction {
		[InspectorLabel("Name")]
		[Tooltip("World variable name")]
		public FsmString _name;
		[Shared]
		[Tooltip("Store the result")]
		public FsmFloat store;
		[Tooltip("Execute the action every frame.")]
		public bool everyFrame;
		
		public override void OnEnter ()
		{
			Get ();
			if (!everyFrame) {
				Finish ();
			}
		}
		
		public override void OnUpdate ()
		{
			Get ();
		}
		
		private void Get() {
			var variable = WorldVariableTracker.GetWorldVariable(_name.Value);
			store.Value = variable.CurrentFloatValue;
		}
		
	}
}
#endif