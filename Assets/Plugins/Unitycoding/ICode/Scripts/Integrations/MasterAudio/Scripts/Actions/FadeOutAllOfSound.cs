#if MASTER_AUDIO
using UnityEngine;

namespace ICode.Actions{
	[Category("MasterAudio")]  
	[Tooltip("Fade all of a Sound Group in Master Audio to zero volume over X seconds")]
	[System.Serializable]
	public class FadeOutAllOfSound : StateAction
	{
		[Tooltip("Check this to perform action on all Sound Groups")]
		public FsmBool allGroups;	
		
		[Tooltip("Name of Master Audio Sound Group")]
		public FsmString soundGroupName;
		
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
					DarkTonic.MasterAudio.MasterAudio.FadeOutAllOfSound(groupNames[i], fadeTime.Value);
				}
			} else {
				DarkTonic.MasterAudio.MasterAudio.FadeOutAllOfSound(soundGroupName.Value, fadeTime.Value);
			}
		}
	}
}
#endif