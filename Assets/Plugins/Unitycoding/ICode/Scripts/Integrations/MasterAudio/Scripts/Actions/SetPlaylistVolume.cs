#if MASTER_AUDIO
using UnityEngine;

namespace ICode.Actions{
	[Category("MasterAudio")]  
	[Tooltip("Set the Playlist Master volume in Master Audio to a specific volume.")] 
	[System.Serializable]
	public class SetPlaylistVolume : StateAction
	{
		[Tooltip("Playlist New Volume")]
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
			DarkTonic.MasterAudio.MasterAudio.PlaylistMasterVolume = volume.Value;
		}
	}
}
#endif