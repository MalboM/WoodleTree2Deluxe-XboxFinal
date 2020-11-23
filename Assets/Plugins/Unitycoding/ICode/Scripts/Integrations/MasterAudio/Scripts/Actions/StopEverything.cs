#if MASTER_AUDIO
using UnityEngine;

namespace ICode.Actions{
	[Category("MasterAudio")]  
	[Tooltip("Stop all sound effects and Playlists in Master Audio.")] 
	[System.Serializable]
	public class StopEverything : StateAction
	{
		public override void OnEnter()
		{
			DarkTonic.MasterAudio.MasterAudio.StopEverything();
		}
	}
}
#endif