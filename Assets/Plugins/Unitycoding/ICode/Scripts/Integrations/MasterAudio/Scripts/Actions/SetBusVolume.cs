#if MASTER_AUDIO
using UnityEngine;

namespace ICode.Actions{
	[Category("MasterAudio")]  
	[Tooltip("Set a single bus volume level in Master Audio")] 
	[System.Serializable]
	public class SetBusVolume : StateAction
	{
		[Tooltip("Check this to perform action on all Buses")]
		public FsmBool allBuses;	
		[Tooltip("Name of Master Audio Bus")]
		public FsmString busName;
		[Tooltip("Target Bus volume")]
		public FsmFloat volume;
		[Tooltip("Execute the action every frame.")]
		public bool everyFrame;
		
		public override void OnEnter()
		{
			SetVolume();
			if (!everyFrame) {
				Finish();
			}
		}

		public override void OnUpdate() {
			SetVolume();
		}
		
		private void SetVolume() {
			if (!allBuses.Value && string.IsNullOrEmpty(busName.Value)) {
				Debug.LogError("You must either check 'All Buses' or enter the Bus Name");
				return;
			}
			
			if (allBuses.Value) {
				var busNames = DarkTonic.MasterAudio.MasterAudio.RuntimeBusNames;
				for (var i = 0; i < busNames.Count; i++) {
					DarkTonic.MasterAudio.MasterAudio.SetBusVolumeByName(busNames[i], volume.Value);
				}
			} else {
				DarkTonic.MasterAudio.MasterAudio.SetBusVolumeByName(busName.Value, volume.Value);
			}
		}
	}
}
#endif