#if MASTER_AUDIO
using UnityEngine;

namespace ICode.Actions{
	[Category("MasterAudio")]  
	[Tooltip("Unpause all sound effects and Playlists in Master Audio.")] 
	[System.Serializable]
	public class UnpauseEverything : StateAction
	{
		public override void OnEnter()
		{
			DarkTonic.MasterAudio.MasterAudio.UnpauseEverything();
		}
	}
}
#endif