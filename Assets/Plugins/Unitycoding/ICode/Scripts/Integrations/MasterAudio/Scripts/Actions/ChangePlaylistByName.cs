#if MASTER_AUDIO
using UnityEngine;

namespace ICode.Actions{
	[Category("MasterAudio")]  
	[Tooltip("Start a Playlist by name in Master Audio")]
	[System.Serializable]
	public class ChangePlaylistByName : StateAction
	{
		[Tooltip("Name of Playlist Controller to use. Not required if you only have one.")]
		public FsmString playlistControllerName;
		[Tooltip("Name of playlist to start")]
		public FsmString playlistName;

		public override void OnEnter()
		{
			if (string.IsNullOrEmpty(playlistControllerName.Value)) {
				DarkTonic.MasterAudio.MasterAudio.ChangePlaylistByName(playlistName.Value, true);
			} else {
				DarkTonic.MasterAudio.MasterAudio.ChangePlaylistByName(playlistControllerName.Value, playlistName.Value, true);
			}
		}
	}
}
#endif