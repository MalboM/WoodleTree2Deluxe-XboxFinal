#if MASTER_AUDIO
using UnityEngine;

namespace ICode.Actions{
	[Category("MasterAudio")]  
	[Tooltip("Unmute all sound effects and Playlists in Master Audio.")] 
	[System.Serializable]
	public class UnmuteEverything : StateAction
	{
		public override void OnEnter()
		{
			DarkTonic.MasterAudio.MasterAudio.UnmuteEverything();
		}
	}
}
#endif