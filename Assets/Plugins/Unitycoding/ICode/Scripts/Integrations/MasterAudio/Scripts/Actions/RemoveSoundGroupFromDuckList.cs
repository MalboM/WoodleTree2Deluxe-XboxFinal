#if MASTER_AUDIO
using UnityEngine;

namespace ICode.Actions{
	[Category("MasterAudio")]  
	[Tooltip("Remove a Sound Group in Master Audio from the list of sounds that cause music ducking.")] 
	[System.Serializable]
	public class RemoveSoundGroupFromDuckList : StateAction
	{
		[Tooltip("Name of Master Audio Sound Group")]
		public FsmString soundGroupName;

		public override void OnEnter()
		{
			DarkTonic.MasterAudio.MasterAudio.RemoveSoundGroupFromDuckList(soundGroupName.Value);
		}
	}
}
#endif