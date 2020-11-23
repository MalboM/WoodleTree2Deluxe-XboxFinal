#if MASTER_AUDIO
using UnityEngine;

namespace ICode.Actions{
	[Category("MasterAudio")]  
	[Tooltip("Solo a Sound Group in Master Audio")] 
	[System.Serializable]
	public class SoloGroup : StateAction
	{
		[Tooltip("Check this to perform action on all Sound Groups")]
		public FsmBool allGroups;	
		[Tooltip("Name of Master Audio Sound Group")]
		public FsmString soundGroupName;
		
		public override void OnEnter()
		{
			if (!allGroups.Value && string.IsNullOrEmpty(soundGroupName.Value)) {
				Debug.LogError("You must either check 'All Groups' or enter the Sound Group Name");
				return;
			}
			
			if (allGroups.Value) {
				var groupNames = DarkTonic.MasterAudio.MasterAudio.RuntimeSoundGroupNames;
				for (var i = 0; i < groupNames.Count; i++) {
					DarkTonic.MasterAudio.MasterAudio.SoloGroup(groupNames[i]);
				}
			} else {
				DarkTonic.MasterAudio.MasterAudio.SoloGroup(soundGroupName.Value);
			}
		}
	}
}
#endif