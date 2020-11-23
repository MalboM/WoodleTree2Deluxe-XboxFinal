#if MASTER_AUDIO
using UnityEngine;

namespace ICode.Actions{
	[Category("MasterAudio")]  
	[Tooltip("Mute all sound effects and Playlists in Master Audio.")] 
	[System.Serializable]
	public class MuteEverything : StateAction
	{
		public override void OnEnter()
		{
			DarkTonic.MasterAudio.MasterAudio.MuteEverything();
		}
	}
}
#endif