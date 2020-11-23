#if MASTER_AUDIO
using UnityEngine;

namespace ICode.Actions{
	[Category("MasterAudio")]  
	[Tooltip("Set master volume level in Master Audio")] 
	[System.Serializable]
	public class SetMasterVolume : StateAction
	{
		[Tooltip("Volume")]
		public FsmFloat volume;
		[Tooltip("Repeat every frame while the state is active.")]
		public bool everyFrame;

		public override void OnEnter() {
			SetVolume();
			
			if (!everyFrame) {
				Finish();
			}
		}
		
		public override void OnUpdate() {
			SetVolume();
		}
		
		private void SetVolume() {
			DarkTonic.MasterAudio.MasterAudio.MasterVolumeLevel = volume.Value;
		}
	}
}
#endif