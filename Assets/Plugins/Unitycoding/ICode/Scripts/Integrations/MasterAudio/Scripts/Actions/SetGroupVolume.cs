#if MASTER_AUDIO
using UnityEngine;

namespace ICode.Actions{
	[Category("MasterAudio")]  
	[Tooltip("Set a single Sound Group volume level in Master Audio")]
	[System.Serializable]
	public class SetGroupVolume : StateAction
	{
		[Tooltip("Check this to perform action on all Sound Groups")]
		public FsmBool allGroups;	
		[Tooltip("Name of Master Audio Sound Group")]
		public FsmString soundGroupName;
		[Tooltip("Volume")]
		public FsmFloat volume;
		[Tooltip("Execute the action every frame.")]
		public bool everyFrame;

		public override void OnEnter()
		{
			SetVolume ();
			if (!everyFrame) {
				Finish();			
			}
		}

		public override void OnUpdate() {
			SetVolume();
		}

		private void SetVolume() {
			if (!allGroups.Value && string.IsNullOrEmpty(soundGroupName.Value)) {
				Debug.LogError("You must either check 'All Groups' or enter the Sound Group Name");
				return;
			}
			
			if (allGroups.Value) {
				var groupNames = DarkTonic.MasterAudio.MasterAudio.RuntimeSoundGroupNames;
				for (var i = 0; i < groupNames.Count; i++) {
					DarkTonic.MasterAudio.MasterAudio.SetGroupVolume(groupNames[i], volume.Value);
				}
			} else {
				DarkTonic.MasterAudio.MasterAudio.SetGroupVolume(soundGroupName.Value, volume.Value);
			}
		}

	}
}
#endif