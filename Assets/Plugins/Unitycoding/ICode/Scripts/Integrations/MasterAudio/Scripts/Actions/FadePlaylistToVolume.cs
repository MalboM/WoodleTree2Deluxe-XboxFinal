#if MASTER_AUDIO
using UnityEngine;

namespace ICode.Actions{
	[Category("MasterAudio")]  
	[Tooltip("Fade the Playlist volume in Master Audio to a specific volume over X seconds.")]
	[System.Serializable]
	public class FadePlaylistToVolume : StateAction
	{
		[Tooltip("Check this to perform action on all Playlist Controllers")]
		public FsmBool allPlaylistControllers;	
		[Tooltip("Name of Playlist Controller to use. Not required if you only have one.")]
		public FsmString playlistControllerName;
		[Tooltip("Target Playlist volume")]
		public FsmFloat targetVolume;
		[Tooltip("Amount of time to complete fade (seconds)")]
		public FsmFloat fadeTime;
		
		public override void OnEnter()
		{
			if (allPlaylistControllers.Value) {
				var pcs = DarkTonic.MasterAudio.PlaylistController.Instances;
				
				for (var i = 0; i < pcs.Count; i++) {
					DarkTonic.MasterAudio.MasterAudio.FadePlaylistToVolume(pcs[i].name, targetVolume.Value, fadeTime.Value);
				}
			} else {
				if (string.IsNullOrEmpty(playlistControllerName.Value)) {
					DarkTonic.MasterAudio.MasterAudio.FadePlaylistToVolume(targetVolume.Value, fadeTime.Value);
				} else {
					DarkTonic.MasterAudio.MasterAudio.FadePlaylistToVolume(playlistControllerName.Value, targetVolume.Value, fadeTime.Value);
				}
			}
		}
	}
}
#endif