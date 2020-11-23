#if UMA
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UMA;

namespace ICode.Actions.UMA{
	[Category("UMA")]   
	[Tooltip("Sets the overlay color based on slot and overlay indecies.")]
	[System.Serializable]
	public class SetOverlayColor : StateAction {
		[SharedPersistent]
		[Tooltip("UMAAvtarBase to use.")]
		public FsmGameObject gameObject;
		[Tooltip("Color to set.")]
		public FsmColor color;
		public List<int> slotIndex;
		public List<int> overlayIndex;
		[Tooltip("Execute the action every frame.")]
		public bool everyFrame;

		private UMAAvatarBase avatar;
		private UMAData umaData;

		public override void OnEnter ()
		{

			avatar = ((GameObject)gameObject.Value).GetComponent<UMAAvatarBase> ();
			umaData = avatar.umaData;
			foreach(int mSlotIndex in slotIndex){
				foreach(int mOverlayIndex in overlayIndex){
					OverlayData overlayData= umaData.GetSlot(mSlotIndex).GetOverlay(mOverlayIndex);
					overlayData.color=color.Value;
				}
			}

			umaData.isShapeDirty = true;
			umaData.isTextureDirty=true;
			umaData.Dirty ();
			if(!everyFrame){
				Finish();
			}
		}	

		public override void OnUpdate ()
		{
			foreach(int mSlotIndex in slotIndex){
				foreach(int mOverlayIndex in overlayIndex){
					OverlayData overlayData= umaData.GetSlot(mSlotIndex).GetOverlay(mOverlayIndex);
					overlayData.color=color.Value;
				}
			}
			
			umaData.isShapeDirty = true;
			umaData.isTextureDirty=true;
			umaData.Dirty ();
		}
	}
}
#endif