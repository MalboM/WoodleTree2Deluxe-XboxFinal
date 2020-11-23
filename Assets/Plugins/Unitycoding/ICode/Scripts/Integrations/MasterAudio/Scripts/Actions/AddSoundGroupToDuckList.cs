#if MASTER_AUDIO
using UnityEngine;

namespace ICode.Actions{
	[Category("MasterAudio")]  
	[Tooltip("Add a Sound Group in Master Audio to the list of sounds that cause music ducking.")]
	[System.Serializable]
	public class AddSoundGroupToDuckList : StateAction
	{
		[Tooltip("Name of Master Audio Sound Group")]
		public FsmString soundGroupName;
		
		[Tooltip("Percentage of time length to start unducking")]
		public FsmFloat beginUnduck;
		[Tooltip("Percentage of original volume")]
		public FsmFloat duckedVolMult_;
		[Tooltip("Amount of time to return music to original time")]
		public FsmFloat unduckTime_;

		
		public override void OnEnter()
		{
			DarkTonic.MasterAudio.MasterAudio.AddSoundGroupToDuckList(soundGroupName.Value, beginUnduck.Value, duckedVolMult_.Value,unduckTime_.Value);

		}
	}
}
#endif