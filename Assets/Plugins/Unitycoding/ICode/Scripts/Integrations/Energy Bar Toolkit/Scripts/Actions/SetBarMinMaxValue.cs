#if ENERGY_BAR_TOOLKIT
using UnityEngine;
using System.Collections;

namespace ICode.Actions.EnergyBarToolkit{
	[Category("Energy Bar Toolkit")]
	[Tooltip("Sets energy bar min and max values.")]
	[System.Serializable]
	public class SetBarMinMaxValue : StateAction {
		[SharedPersistent]
		[Tooltip("Energy bar on which action should be done")]
		public FsmGameObject gameObject;
		[NotRequired]
		[Tooltip("Minimum allowed value.")]
		public FsmInt minValue;
		[NotRequired]
		[Tooltip("Maximum allowed value.")]
		public FsmInt maxValue;
		public bool everyFrame;

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
			
			if (!minValue.IsNone) {
				cachedEnergyBar.valueMin = minValue.Value;
			}
			
			if (!maxValue.IsNone) {
				cachedEnergyBar.valueMax = maxValue.Value;
			}
		}
	}
} 
#endif