#if CORE_GAME_KIT
using UnityEngine;
using System.Collections;
using DarkTonic.CoreGameKit;

namespace ICode.Actions.CoreGameKit{
	[Category("CoreGameKit")]   
	[Tooltip("Set the value of an int world variable in Core GameKit")]
	[System.Serializable]
	public class SetWorldInt : StateAction {
		[SharedPersistent]
		[Tooltip("The GameObject to use.")]
		public FsmGameObject gameObject;
		[InspectorLabel("Name")]
		[Tooltip("World variable name")]
		public FsmString _name;
		[Tooltip("The value to set.")]
		public FsmInt value;
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
			var modifier = new WorldVariableModifier(_name.Value, WorldVariableTracker.VariableType._integer);
			modifier._modValueIntAmt.curModMode = KillerVariable.ModMode.Set;
			modifier._modValueIntAmt.Value = value.Value;
			
			WorldVariableTracker.ModifyPlayerStat(modifier, trans);
		}

	}
}
#endif