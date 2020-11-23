#if MASTER_AUDIO
using UnityEngine;

namespace ICode.Actions{
	[Category("MasterAudio")]  
	[Tooltip("Play a clip in the current Playlist by name in Master Audio")] 
	[System.Serializable]
	public class TriggerPlaylistClip : StateAction
	{
		[Tooltip("Name of Playlist Controller to use. Not required if you only have one.")]
		public FsmString playlistControllerName;
		[Tooltip("Name of Master Audio Bus")]
		public FsmString clipName;
		
		public override void OnEnter()
		{
			if (string.IsNullOrEmpty(playlistControllerName.Value)) {
				DarkTonic.MasterAudio.MasterAudio.TriggerPlaylistClip(clipName.Value);
			} else {
				DarkTonic.MasterAudio.MasterAudio.TriggerPlaylistClip(playlistControllerName.Value, clipName.Value);
			}
		}
	}
}
#endif