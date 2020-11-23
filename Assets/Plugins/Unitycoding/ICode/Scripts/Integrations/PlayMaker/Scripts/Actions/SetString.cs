#if PLAYMAKER
using UnityEngine;
using System.Collections;

namespace ICode.Actions.PlayMaker{
	[Category("PlayMaker")]   
	[Tooltip("Synchronize a variable from or to playmaker.")]
	[System.Serializable]
	public class SetString : StateAction {
		[SharedPersistent]
		[Tooltip("PlayMakerFSM to use.")]
		public FsmGameObject gameObject;
		[Tooltip("Name of the fsm")]
		public FsmString fsmName;
		[Tooltip("The name of the variable to synchronize.")]
		public FsmString variableName;
		public SetDirection direction;
		[Tooltip("Execute the action every frame.")]
		public bool everyFrame;
		
		private PlayMakerFSM fsm;
		
		public override void OnEnter ()
		{
			PlayMakerFSM[] fsms = gameObject.Value.GetComponents<PlayMakerFSM> ();
			foreach (PlayMakerFSM mFsm in fsms) {
				if (mFsm.FsmName == fsmName.Value) {
					fsm = mFsm;
				}
			}
			DoSet ();
			if (!everyFrame) {
				Finish ();
			}
		}
		
		public override void OnUpdate ()
		{
			DoSet ();
		}
		
		private void DoSet(){
			if (fsm == null) {
			return;			
			}
			FsmVariable icodeVariable = this.Root.GetVariable (variableName.Value);
			HutongGames.PlayMaker.FsmString fsmVar= fsm.FsmVariables.FindFsmString(variableName.Value);
			if (direction == SetDirection.ToPlayMaker) {
				if(fsmVar != null&& icodeVariable != null && icodeVariable is FsmString){
					fsmVar.Value=(string)icodeVariable.GetValue();
				}
			} else {
				if(fsmVar != null&& icodeVariable!= null && icodeVariable is FsmString){
					icodeVariable.SetValue(fsmVar.Value);
				}
			}
		}
	}
}
#endif