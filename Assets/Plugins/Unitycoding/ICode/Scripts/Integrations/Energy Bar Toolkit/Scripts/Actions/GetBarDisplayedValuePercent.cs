#if ENERGY_BAR_TOOLKIT
using UnityEngine;
using System.Collections;

namespace ICode.Actions.EnergyBarToolkit{
	[Category("Energy Bar Toolkit")]
	[Tooltip("Reads current Energy Bar displayed value as percentage (0 - empty, 1 - full)")]
	[System.Serializable]
	public class GetBarDisplayedValuePercent : StateAction {
		[SharedPersistent]
		[Tooltip("Energy bar on which action should be done")]
		public FsmGameObject gameObject;
		[Shared]
		public FsmFloat store;
		public bool everyFrame;

		GameObject cachedBarObject;
		EnergyBarBase cachedEnergyBar;

		public override void OnEnter() {
			DoGet();
			if (!everyFrame) {
				Finish();
			}
		}
		
		public override void OnUpdate() {
			DoGet();
		}
		
		private void DoGet() {
			if (cachedBarObject != gameObject.Value) {
				cachedBarObject = gameObject.Value;
				cachedEnergyBar = cachedBarObject.GetComponent<EnergyBarBase>();
			}
			store.Value = cachedEnergyBar.displayValue;
		}
	}
} 
#endif