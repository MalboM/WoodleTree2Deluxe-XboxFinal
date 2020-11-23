#if MASTER_AUDIO
using UnityEngine;

namespace ICode.Actions{
	[Category("MasterAudio")]  
	[Tooltip("Play the next clip in a Playlist in Master Audio")] 
	[System.Serializable]
	public class TriggerNextPlaylistClip : StateAction
	{
		[Tooltip("Check this to perform action on all Playlist Controllers")]
		public FsmBool allPlaylistControllers;
		[Tooltip("Name of Playlist Controller containing the Playlist. Not required if you only have one Playlist Controller.")]
		public FsmString playlistControllerName;
		
		public override void OnEnter()
		{
			if (allPlaylistControllers.Value) {
				var pcs = DarkTonic.MasterAudio.PlaylistController.Instances;
				
				for (var i = 0; i < pcs.Count; i++) {
					DarkTonic.MasterAudio.MasterAudio.TriggerNextPlaylistClip(pcs[i].name);
				}
			} else {
				if (string.IsNullOrEmpty(playlistControllerName.Value)) {
					DarkTonic.MasterAudio.MasterAudio.TriggerNextPlaylistClip();
				} else {
					DarkTonic.MasterAudio.MasterAudio.TriggerNextPlaylistClip(playlistControllerName.Value);
				}
			}
		}
	}
}
#endif