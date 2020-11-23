#if MASTER_AUDIO
using UnityEngine;

namespace ICode.Actions{
	[Category("MasterAudio")]  
	[Tooltip("Turn music ducking on or off in Master Audio.")]
	[System.Serializable]
	public class EnableMusicDucking : StateAction
	{
		[Tooltip("Check this to enable ducking, uncheck it to disable ducking.")]
		public FsmBool enableDucking;	
		
		public override void OnEnter()
		{
			DarkTonic.MasterAudio.MasterAudio.Instance.EnableMusicDucking = enableDucking.Value;
		}
	}
}
#endif