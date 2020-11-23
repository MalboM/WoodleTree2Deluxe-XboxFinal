#if PLAYMAKER
using UnityEngine;
using System.Collections;

namespace ICode.Actions.PlayMaker{
	[Category("PlayMaker")]   
	[Tooltip("Send an event to playmaker")]
	[System.Serializable]
	public class SendEvent : StateAction {
		[SharedPersistent]
		[Tooltip("PlayMakerFSM to use.")]
		public FsmGameObject gameObject;
		[Tooltip("Name of the fsm")]
		public FsmString fsmName;
		[Tooltip("Name of the event to send.")]
		public FsmString eventName;

		private PlayMakerFSM fsm;

		public override void OnEnter ()
		{
			PlayMakerFSM[] fsms = gameObject.Value.GetComponents<PlayMakerFSM> ();
			foreach (PlayMakerFSM mFsm in fsms) {
				if(mFsm.FsmName==fsmName.Value){
					fsm=mFsm;
				}
			}
			if (fsm != null) {
				fsm.SendEvent (eventName.Value);
			}
		}

	}
}
#endif