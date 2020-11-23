#if UFPS
using UnityEngine;
using System.Collections;

namespace ICode.Actions.UFPS{
	[Category("UFPS")]    
	[Tooltip("Adds an item to the inventory.")]
	[System.Serializable]
	public class AddItem : StateAction {
		[SharedPersistent]
		[Tooltip("Player GameObject.")]
		public FsmGameObject gameObject;
		[Tooltip("Item name.")]
		public FsmString itemName;
		[Tooltip("Quantity to add.")]
		public FsmInt quantity;
		[Shared]
		[NotRequired]
		[Tooltip("Store the result.")]
		public FsmBool result;
		protected vp_FPPlayerEventHandler player;
		
		public override void OnEnter ()
		{
			player = ((GameObject)gameObject.Value).GetComponent<vp_FPPlayerEventHandler> ();
			result.Value=player.AddItem.Try(new object[] { itemName.Value, quantity.Value})	;
			Finish ();
		}
	}
}
#endif