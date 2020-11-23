#if MASTER_AUDIO
using UnityEngine;

namespace ICode.Actions{
	[Category("MasterAudio")]  
	[Tooltip("Stop current Playlist in Master Audio")] 
	[System.Serializable]
	public class StopPlaylist : StateAction
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
					DarkTonic.MasterAudio.MasterAudio.StopPlaylist(pcs[i].name);
				}
			} else {
				if (string.IsNullOrEmpty(playlistControllerName.Value)) {
					DarkTonic.MasterAudio.MasterAudio.StopPlaylist();
				} else {
					DarkTonic.MasterAudio.MasterAudio.StopPlaylist(playlistControllerName.Value);
				}
			}
		}
	}
}
#endif