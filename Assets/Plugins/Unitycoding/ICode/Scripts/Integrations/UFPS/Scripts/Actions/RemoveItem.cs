#if UFPS
using UnityEngine;
using System.Collections;

namespace ICode.Actions.UFPS{
	[Category("UFPS")]    
	[Tooltip("Removes an item from the inventory.")]
	[System.Serializable]
	public class RemoveItem : StateAction {
		[SharedPersistent]
		[Tooltip("Player GameObject.")]
		public FsmGameObject gameObject;
		[Tooltip("Item name.")]
		public FsmString itemName;
		[Shared]
		[NotRequired]
		[Tooltip("Store the result.")]
		public FsmBool result;
		protected vp_FPPlayerEventHandler player;
		
		public override void OnEnter ()
		{
			player = ((GameObject)gameObject.Value).GetComponent<vp_FPPlayerEventHandler> ();
			result.Value=player.RemoveItem.Try(new object[] { itemName.Value})	;
		}
	}
}
#endif