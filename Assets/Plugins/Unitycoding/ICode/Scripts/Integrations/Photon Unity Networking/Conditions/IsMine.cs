#if PUN
using UnityEngine;
using System.Collections;

namespace ICode.Conditions.Photon{
	[Category("Photon")]
	[System.Serializable]
	public class IsMine : Condition {
		[SharedPersistent]
		[Tooltip("GameObject to test.")]
		public FsmGameObject gameObject;
		[Tooltip("Does the result equals this condition.")]
		public FsmBool equals;
		
		private PhotonView photonView;
		public override void OnEnter ()
		{
			photonView = gameObject.Value.GetComponent<PhotonView> ();

		}
		
		public override bool Validate ()
		{
			return (photonView.isMine == equals.Value);
		}
	}
}
#endif