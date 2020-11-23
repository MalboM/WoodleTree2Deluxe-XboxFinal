#if MASTER_AUDIO
using UnityEngine;

namespace ICode.Actions{
	[Category("MasterAudio")]  
	[Tooltip("Pause a Playlist in Master Audio")] 
	[System.Serializable]
	public class PausePlaylist : StateAction
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
					DarkTonic.MasterAudio.MasterAudio.PausePlaylist(pcs[i].name);
				}
			} else {
				if (string.IsNullOrEmpty(playlistControllerName.Value)) {
					DarkTonic.MasterAudio.MasterAudio.PausePlaylist();
				} else {
					DarkTonic.MasterAudio.MasterAudio.PausePlaylist(playlistControllerName.Value);
				}
			}
		}
	}
}
#endif