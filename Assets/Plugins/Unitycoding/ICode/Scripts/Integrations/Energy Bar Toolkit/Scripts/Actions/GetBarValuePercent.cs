#if ENERGY_BAR_TOOLKIT
using UnityEngine;
using System.Collections;

namespace ICode.Actions.EnergyBarToolkit{
	[Category("Energy Bar Toolkit")]
	[Tooltip("Reads current Energy Bar Value percentage.")]
	[System.Serializable]
	public class GetBarValuePercent : StateAction {
		[SharedPersistent]
		[Tooltip("Energy bar on which action should be done")]
		public FsmGameObject gameObject;
		[Shared]
		public FsmFloat storeValue;
		public bool everyFrame;

		GameObject cachedBarObject;
		EnergyBar cachedEnergyBar;
		
		public override void OnEnter() {
			DoGet();
			
			if (!everyFrame) {
				Finish();
			}
		}

		public override void OnUpdate ()
		{
			DoGet ();
		}
		
		private void DoGet() {
			if (cachedBarObject != gameObject.Value) {
				cachedBarObject = gameObject.Value;
				cachedEnergyBar = cachedBarObject.GetComponent<EnergyBar>();
			}
			storeValue.Value = cachedEnergyBar.ValueF;
		}
	}
}
#endif