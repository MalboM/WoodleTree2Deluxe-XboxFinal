#if MASTER_AUDIO
using UnityEngine;

namespace ICode.Actions{
	[Category("MasterAudio")]  
	[Tooltip("Play a Sound in Master Audio")] 
	[System.Serializable]
	public class PlaySound : StateAction
	{
		[SharedPersistent]
		[Tooltip("The GameObject to use for sound position.")]
		public FsmGameObject gameObject;
		[Tooltip("Name of Master Audio Sound Group")]
		public FsmString soundGroupName;
		[Tooltip("Name of specific variation (optional)")]
		public FsmString variationName;
		public FsmFloat volume;
		public FsmFloat delaySound;
		public FsmBool useThisLocation;
		public FsmBool attachToGameObject;
		public FsmBool useFixedPitch;
		[Tooltip("Fixed Pitch will be used only if 'Use Fixed Pitch' is checked above.")]
		public FsmFloat fixedPitch;
		
		public override void OnEnter() {
			var go = (GameObject)gameObject.Value;
			var trans = go.transform;
			
			var groupName = soundGroupName.Value;
			var childName = variationName.Value;
			var willAttach = attachToGameObject.Value;
			var use3dLocation = useThisLocation.Value;
			var vol = volume.Value;
			var fDelay = delaySound.Value;
			float? pitch = fixedPitch.Value;
			if (!useFixedPitch.Value) {
				pitch = null;
			}
			
			if (string.IsNullOrEmpty (childName)) {
				childName = null;
			}
			
			if (!use3dLocation && !willAttach) {
				DarkTonic.MasterAudio.MasterAudio.PlaySoundAndForget (groupName, vol, pitch, fDelay, childName); 
			} else {
				DarkTonic.MasterAudio.MasterAudio.PlaySound3DAndForget (groupName, trans, willAttach, vol, pitch, fDelay, childName); 
			}
		}
	}
}
#endif