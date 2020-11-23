#if ENERGY_BAR_TOOLKIT
using UnityEngine;
using System.Collections;

namespace ICode.Actions.EnergyBarToolkit{
	[Category("Energy Bar Toolkit")]
	[Tooltip("Sets energy bar value to given value.")]
	[System.Serializable]
	public class SetBarValue : StateAction {
		
		[SharedPersistent]
		[Tooltip("Energy bar on which action should be done")]
		public FsmGameObject gameObject;

		[Tooltip("Value of energy bar to set.")]
		public FsmInt value;
		
		public bool everyFrame;
		
		// cache
		GameObject cachedBarObject;
		EnergyBar cachedEnergyBar;
		
		public override void OnEnter() {
			DoSet();
			if (!everyFrame) {
				Finish();
			}
		}
		
		public override void OnUpdate() {
			DoSet();
		}
		
		void DoSet() {
			if (cachedBarObject != gameObject.Value) {
				cachedBarObject = gameObject.Value;
				cachedEnergyBar = cachedBarObject.GetComponent<EnergyBar>();
			}
			cachedEnergyBar.valueCurrent = Mathf.Clamp(value.Value, cachedEnergyBar.valueMin, cachedEnergyBar.valueMax);
		}

	}
} 
#endif