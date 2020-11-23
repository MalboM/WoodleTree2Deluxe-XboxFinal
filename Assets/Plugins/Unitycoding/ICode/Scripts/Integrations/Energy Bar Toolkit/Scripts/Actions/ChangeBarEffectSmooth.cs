#if ENERGY_BAR_TOOLKIT
using UnityEngine;
using System.Collections;

namespace ICode.Actions.EnergyBarToolkit{
	[Category("Energy Bar Toolkit")]
	[Tooltip("Changed energy bar effect smooth change settings.")]
	[System.Serializable]
	public class ChangeBarEffectSmooth : StateAction {
		[SharedPersistent]
		[Tooltip("Energy bar on which action should be done")]
		public FsmGameObject gameObject;
		public FsmBool effectEnabled;
		public FsmFloat smoothSpeed;
		[Tooltip("Execute the action every frame.")]
		public bool everyFrame;

		GameObject cachedBarObject;
		EnergyBarBase cachedEnergyBar;

		public override void OnEnter() {
			DoChange();
			
			if (!everyFrame) {
				Finish();
			}
		}

		public override void OnUpdate() {
			DoChange();
		}
		
		private void DoChange() {
			if (cachedBarObject != gameObject.Value) {
				cachedBarObject = gameObject.Value;
				cachedEnergyBar = cachedBarObject.GetComponent<EnergyBarBase>();
			}
			cachedEnergyBar.effectSmoothChange = effectEnabled.Value;
			cachedEnergyBar.effectSmoothChangeSpeed = smoothSpeed.Value;
		}
	}
}
#endif