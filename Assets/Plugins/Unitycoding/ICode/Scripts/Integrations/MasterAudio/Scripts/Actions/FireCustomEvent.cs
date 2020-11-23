#if MASTER_AUDIO
using UnityEngine;

namespace ICode.Actions{
	[Category("MasterAudio")]  
	[Tooltip("Fire a Custom Event in Master Audio")]
	[System.Serializable]
	public class FireCustomEvent : StateAction
	{
		[Tooltip("The Custom Event (defined in Master Audio prefab) to fire.")]
		public FsmString customEventName;
		public FsmVector3 position;

		public override void OnEnter()
		{
			DarkTonic.MasterAudio.MasterAudio.FireCustomEvent(customEventName.Value,position.Value);
		}
	}
}
#endif