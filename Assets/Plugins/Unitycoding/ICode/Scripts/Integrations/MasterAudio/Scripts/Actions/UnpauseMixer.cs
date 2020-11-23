#if MASTER_AUDIO
using UnityEngine;

namespace ICode.Actions{
	[Category("MasterAudio")]  
	[Tooltip("Unpause all sound effects in Master Audio. Does not include Playlists.")] 
	[System.Serializable]
	public class UnpauseMixer : StateAction
	{
		public override void OnEnter()
		{
			DarkTonic.MasterAudio.MasterAudio.UnpauseMixer();
		}
	}
}
#endif