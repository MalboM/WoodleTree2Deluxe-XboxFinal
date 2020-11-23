#if ENERGY_BAR_TOOLKIT
using UnityEngine;
using System.Collections;

namespace ICode.Actions.EnergyBarToolkit{
	[Category("Energy Bar Toolkit")]
	[Tooltip("Sets energy bar value to given percentage value.")]
	[System.Serializable]
	public class SetBarValuePercent : StateAction {
		[SharedPersistent]
		[Tooltip("Energy bar on which action should be done")]
		public FsmGameObject gameObject;

		[Tooltip("Percentage value of energy bar to set.")]
		public FsmFloat value;
		
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
			cachedEnergyBar.ValueF = value.Value;
		}
	}
} 
#endif