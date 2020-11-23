#if ENERGY_BAR_TOOLKIT
using UnityEngine;
using System.Collections;

namespace ICode.Actions.EnergyBarToolkit{
	[Category("Energy Bar Toolkit")]
	[Tooltip("Reads current Energy Bar Value")]
	[System.Serializable]
	public class GetBarValue : StateAction {
		[SharedPersistent]
		[Tooltip("Energy bar on which action should be done")]
		public FsmGameObject gameObject;
		[Shared]
		public FsmInt storeValue;

		GameObject cachedBarObject;
		EnergyBar cachedEnergyBar;

		public override void OnEnter() {
			if (cachedBarObject != gameObject.Value) {
				cachedBarObject = gameObject.Value;
				cachedEnergyBar = cachedBarObject.GetComponent<EnergyBar>();
			}
			storeValue.Value = cachedEnergyBar.valueCurrent;
			Finish();
		}
	}
}
#endif