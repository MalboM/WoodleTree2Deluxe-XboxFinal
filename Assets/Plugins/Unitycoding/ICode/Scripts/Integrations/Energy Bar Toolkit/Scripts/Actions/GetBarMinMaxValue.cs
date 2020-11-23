#if ENERGY_BAR_TOOLKIT
using UnityEngine;
using System.Collections;

namespace ICode.Actions.EnergyBarToolkit{
	[Category("Energy Bar Toolkit")]
	[Tooltip("Reads current Energy Bar min/max value")]
	[System.Serializable]
	public class GetBarMinMaxValue : StateAction {
		[SharedPersistent]
		[Tooltip("Energy bar on which action should be done")]
		public FsmGameObject gameObject;
		[NotRequired]
		[Tooltip("Variable to store minimum bar value")]
		public FsmInt storeMinValue;
		[NotRequired]
		[Tooltip("Variable to store maximum bar value")]
		public FsmInt storeMaxValue;

		GameObject cachedBarObject;
		EnergyBar cachedEnergyBar;
	
		public override void OnEnter() {
			if (cachedBarObject != gameObject.Value) {
				cachedBarObject = gameObject.Value;
				cachedEnergyBar = cachedBarObject.GetComponent<EnergyBar>();
			}
			if (!storeMinValue.IsNone) {
				storeMinValue.Value = cachedEnergyBar.valueMin;
			}
			
			if (!storeMaxValue.IsNone) {
				storeMaxValue.Value = cachedEnergyBar.valueMax;
			}
			
			Finish();
		}
	}
} 
#endif