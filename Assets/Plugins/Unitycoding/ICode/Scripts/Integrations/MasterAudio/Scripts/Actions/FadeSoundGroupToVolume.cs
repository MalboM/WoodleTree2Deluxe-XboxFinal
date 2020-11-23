#if MASTER_AUDIO
using UnityEngine;

namespace ICode.Actions{
	[Category("MasterAudio")]  
	[Tooltip("Fade a Sound Group in Master Audio to a specific volume over X seconds.")] 
	[System.Serializable]
	public class FadeSoundGroupToVolume : StateAction
	{
		[Tooltip("Check this to perform action on all Sound Groups")]
		public FsmBool allGroups;	
		[Tooltip("Name of Master Audio Sound Group")]
		public FsmString soundGroupName;
		[Tooltip("Target Bus volume")]
		public FsmFloat targetVolume;
		[Tooltip("Amount of time to complete fade (seconds)")]
		public FsmFloat fadeTime;
		
		public override void OnEnter()
		{
			if (!allGroups.Value && string.IsNullOrEmpty(soundGroupName.Value)) {
				Debug.LogError("You must either check 'All Groups' or enter the Sound Group Name");
				return;
			}
			
			if (allGroups.Value) {
				var groupNames = DarkTonic.MasterAudio.MasterAudio.RuntimeSoundGroupNames;
				for (var i = 0; i < groupNames.Count; i++) {
					DarkTonic.MasterAudio.MasterAudio.FadeSoundGroupToVolume(groupNames[i], targetVolume.Value, fadeTime.Value);
				}
			} else {
				DarkTonic.MasterAudio.MasterAudio.FadeSoundGroupToVolume(soundGroupName.Value, targetVolume.Value, fadeTime.Value);
			}
		}
	}
}
#endif