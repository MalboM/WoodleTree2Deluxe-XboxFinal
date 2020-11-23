#if ENERGY_BAR_TOOLKIT
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ICode.Actions.EnergyBarToolkit{
	
	[Category("Energy Bar Toolkit")]
	[Tooltip("Change energy bar value by given percentage amount.")]
	[System.Serializable]
	public class ChangeBarValuePercent : StateAction {
		[SharedPersistent]
		[Tooltip("Energy bar on which action should be done")]
		public FsmGameObject gameObject;
		[Tooltip("Amount to add to current energy bar value. Set amount below zero to decrease the value.")]
		public FsmFloat changeValue;
		[Tooltip("Execute the action every frame.")]
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
		
		private void DoSet() {
			if (cachedBarObject != gameObject.Value) {
				cachedBarObject = gameObject.Value;
				cachedEnergyBar = cachedBarObject.GetComponent<EnergyBar>();
			}
			cachedEnergyBar.ValueF += changeValue.Value;
		}
	}
} 
#endif