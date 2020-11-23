#if CORE_GAME_KIT
using UnityEngine;
using System.Collections;
using DarkTonic.CoreGameKit;

namespace ICode.Actions.CoreGameKit{
	[Category("CoreGameKit")]   
	[Tooltip("Set the value of a float world variable in Core GameKit")]
	[System.Serializable]
	public class SetWorldFloat : StateAction {
		[SharedPersistent]
		[Tooltip("The GameObject to use.")]
		public FsmGameObject gameObject;
		[InspectorLabel("Name")]
		[Tooltip("World variable name")]
		public FsmString _name;
		[Tooltip("The value to set.")]
		public FsmFloat value;
		[Tooltip("Execute the action every frame.")]
		public bool everyFrame;
		
		public override void OnEnter ()
		{
			Set ();
			if (!everyFrame) {
				Finish ();
			}
		}
		
		public override void OnUpdate ()
		{
			Set ();
		}
		
		private void Set() {
			var trans = ((GameObject)gameObject.Value).transform;
			var modifier = new WorldVariableModifier(_name.Value, WorldVariableTracker.VariableType._float);
			modifier._modValueFloatAmt.curModMode = KillerVariable.ModMode.Set;
			modifier._modValueFloatAmt.Value = value.Value;
			
			WorldVariableTracker.ModifyPlayerStat(modifier, trans);
		}
		
	}
}
#endif