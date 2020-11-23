#if MASTER_AUDIO
using UnityEngine;

namespace ICode.Actions{
	[Category("MasterAudio")]  
	[Tooltip("Toggle the mute button of a Sound Group in Master Audio")] 
	[System.Serializable]
	public class ToggleGroupMute : StateAction
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
				
				var groupName = string.Empty;
				for (var i = 0; i < groupNames.Count; i++) {
					groupName = groupNames[i];
					
					var grp = DarkTonic.MasterAudio.MasterAudio.GrabGroup(groupName);
					if (grp != null) {
						if (grp.isMuted) {
							DarkTonic.MasterAudio.MasterAudio.UnmuteGroup(groupName);
						} else {
							DarkTonic.MasterAudio.MasterAudio.MuteGroup(groupName);
						}
					}
				}
			} else {
				var grp = DarkTonic.MasterAudio.MasterAudio.GrabGroup(soundGroupName.Value);
				if (grp != null) {
					if (grp.isMuted) {
						DarkTonic.MasterAudio.MasterAudio.UnmuteGroup(soundGroupName.Value);
					} else {
						DarkTonic.MasterAudio.MasterAudio.MuteGroup(soundGroupName.Value);
					}
				}
			}
		}
	}
}
#endif