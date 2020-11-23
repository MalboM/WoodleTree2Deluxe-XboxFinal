#if MASTER_AUDIO
using UnityEngine;

namespace ICode.Actions{
	[Category("MasterAudio")]  
	[Tooltip("Fade a Bus in Master Audio to a specific volume over X seconds.")] 
	[System.Serializable]
	public class FadeBusToVolume : StateAction
	{
		[Tooltip("Check this to perform action on all Buses")]
		public FsmBool allBuses;	
		
		[Tooltip("Name of Master Audio Bus")]
		public FsmString busName;

		[Tooltip("Target Bus volume")]
		public FsmFloat targetVolume;

		[Tooltip("Amount of time to complete fade (seconds)")]
		public FsmFloat fadeTime;

		public override void OnEnter()
		{
			if (!allBuses.Value && string.IsNullOrEmpty(busName.Value)) {
				Debug.LogError("You must either check 'All Buses' or enter the Bus Name");
				return;
			}
			
			if (allBuses.Value) {
				var busNames = DarkTonic.MasterAudio.MasterAudio.RuntimeBusNames;
				for (var i = 0; i < busNames.Count; i++) {
					DarkTonic.MasterAudio.MasterAudio.FadeBusToVolume(busNames[i], targetVolume.Value, fadeTime.Value);
				}
			} else {
				DarkTonic.MasterAudio.MasterAudio.FadeBusToVolume(busName.Value, targetVolume.Value, fadeTime.Value);
			}
		}
	}
}
#endif