#if MASTER_AUDIO
using UnityEngine;

namespace ICode.Actions{
	[Category("MasterAudio")]  
	[Tooltip("Stop sounds made by a Transform in Master Audio")] 
	[System.Serializable]
	public class StopTransformSound : StateAction
	{
		[SharedPersistent]
		[Tooltip("The GameObject to use.")]
		public FsmGameObject gameObject;
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
			
			GameObject go = (GameObject)gameObject.Value;
			
			if (allGroups.Value) {
				DarkTonic.MasterAudio.MasterAudio.StopAllSoundsOfTransform(go.transform);
			} else {
				if (string.IsNullOrEmpty(soundGroupName.Value)) {
					Debug.LogError("You must either check 'All Groups' or enter the Sound Group Name");
				}
				DarkTonic.MasterAudio.MasterAudio.StopSoundGroupOfTransform(go.transform, soundGroupName.Value);
			}
		}
	}
}
#endif