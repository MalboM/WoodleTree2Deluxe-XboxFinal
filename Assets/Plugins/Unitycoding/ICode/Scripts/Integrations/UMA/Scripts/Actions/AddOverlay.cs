#if UMA
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UMA;

namespace ICode.Actions.UMA{
	[Category("UMA")]   
	[Tooltip("Adds an overlay to the slot.")]
	[System.Serializable]
	public class AddOverlay : StateAction {
		[SharedPersistent]
		[Tooltip("UMAAvtarBase to use.")]
		public FsmGameObject gameObject;
		public int slotIndex;
		public FsmObject overlay;
		
		private UMAAvatarBase avatar;
		private UMAData umaData;
		public override void OnEnter ()
		{
			avatar = ((GameObject)gameObject.Value).GetComponent<UMAAvatarBase> ();
			umaData = avatar.umaData;

			umaData.GetSlot(slotIndex).AddOverlay((OverlayData)overlay.Value);
			umaData.isShapeDirty = true;
			umaData.isTextureDirty=true;
			umaData.Dirty ();
			Finish ();
		}	
	}
}
#endif