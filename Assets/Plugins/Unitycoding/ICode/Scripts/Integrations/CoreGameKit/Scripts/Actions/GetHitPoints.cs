#if CORE_GAME_KIT
using UnityEngine;
using System.Collections;
using DarkTonic.CoreGameKit;

namespace ICode.Actions.CoreGameKit{
	[Category("CoreGameKit")]   
	[Tooltip("Get the current Hit Points of a Killable in Core GameKit")]
	[System.Serializable]
	public class GetHitPoints : StateAction {
		[SharedPersistent]
		[Tooltip("The GameObject to use.")]
		public FsmGameObject gameObject;
		[Shared]
		[Tooltip("The variable to store the current Hit Points in.")]
		public FsmInt store;
		[Tooltip("Execute the action every frame.")]
		public bool everyFrame;

		private Killable myKillable;
		
		public override void OnEnter ()
		{
			StoreHitPoints ();
			if (!everyFrame) {
				Finish ();
			}
		}

		public override void OnUpdate ()
		{
			StoreHitPoints ();
		}

		private void StoreHitPoints() {
			if (myKillable == null) {
				GameObject go = (GameObject)gameObject.Value;
				myKillable = go.GetComponent<Killable>();
			}

			store.Value = myKillable.currentHitPoints;
		}
	}
}
#endif