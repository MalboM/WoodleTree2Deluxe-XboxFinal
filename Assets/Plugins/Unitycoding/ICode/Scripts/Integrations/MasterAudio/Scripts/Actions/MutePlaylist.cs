#if MASTER_AUDIO
using UnityEngine;

namespace ICode.Actions{
	[Category("MasterAudio")]  
	[Tooltip("Mute a Playlist in Master Audio")]
	[System.Serializable]
	public class MutePlaylist : StateAction
	{
		[Tooltip("Check this to perform action on all Playlist Controllers")]
		public FsmBool allPlaylistControllers;	
		[Tooltip("Name of Playlist Controller to use. Not required if you only have one.")]
		public FsmString playlistControllerName;
		
		public override void OnEnter()
		{
			if (allPlaylistControllers.Value) {
				var pcs = DarkTonic.MasterAudio.PlaylistController.Instances;
				
				for (var i = 0; i < pcs.Count; i++) {
					DarkTonic.MasterAudio.MasterAudio.MuteAllPlaylists();
				}
			} else {
				if (string.IsNullOrEmpty(playlistControllerName.Value)) {
					DarkTonic.MasterAudio.MasterAudio.MutePlaylist();
				} else {
					DarkTonic.MasterAudio.MasterAudio.MutePlaylist(playlistControllerName.Value);
				}
			}
		}
	}
}
#endif