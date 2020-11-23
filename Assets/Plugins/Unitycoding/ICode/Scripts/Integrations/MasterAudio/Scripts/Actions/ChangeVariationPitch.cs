#if MASTER_AUDIO
using UnityEngine;

namespace ICode.Actions{
	[Category("MasterAudio")]  
	[Tooltip("Change the pich of a variation (or all variations) in a Sound Group in Master Audio")]
	[System.Serializable]
	public class ChangeVariationPitch : StateAction
	{
		[SharedPersistent]
		[Tooltip("The GameObject to use.")]
		public FsmGameObject gameObject;
		private Transform trans;
		[Tooltip("Name of Master Audio Sound Group")]
		public FsmString soundGroupName;
		[Tooltip("Name of specific variation (optional)")]
		public FsmString variationName;
		public FsmBool changeAllVariations;
		public FsmFloat pitch;
		[Tooltip("Repeat every frame while the state is active.")]
		public bool everyFrame;
		
		public override void OnEnter() {
			ChangePitch();
			
			if (!everyFrame) {
				Finish();
			}
		}
		
		public override void OnUpdate() {
			ChangePitch();
		}
		
		private void ChangePitch() {
			if (trans == null) {
				trans = ((GameObject)gameObject.Value).transform;
			}
			
			var groupName = soundGroupName.Value;
			var childName = variationName.Value;
			
			if (string.IsNullOrEmpty(childName)) {
				childName = null;
			}
			
			DarkTonic.MasterAudio.MasterAudio.ChangeVariationPitch(groupName, changeAllVariations.Value, childName, pitch.Value);
		}
	}
}
#endif