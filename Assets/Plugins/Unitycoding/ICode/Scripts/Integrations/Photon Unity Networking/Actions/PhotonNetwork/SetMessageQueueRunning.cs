#if PUN
using UnityEngine;
using System.Collections;

namespace ICode.Actions.Photon{
	[Category("Photon/PhotonNetwork")]   
	[System.Serializable]
	public class SetMessageQueueRunning : StateAction {
		[Tooltip("Value to set.")]
		public FsmBool value;
		
		public override void OnEnter ()
		{
			PhotonNetwork.isMessageQueueRunning = value.Value;
			Finish ();
		}
	}
}
#endif