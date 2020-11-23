#if PUN
using UnityEngine;
using System.Collections;

namespace ICode.Actions.Photon.Variables{
	[Category("Photon/Variable")]
	[Tooltip("Sets the float value of a variable.")]
	[System.Serializable]
	public class SetFloat : StateAction {
		[SharedPersistent]
		[Tooltip("The GameObject to use.")]
		public FsmGameObject gameObject;
		[Tooltip("The variable to use.")]
		public FsmFloat variable;
		[Tooltip("The value to set.")]
		public FsmFloat value;

		public PhotonTargets targets;
		private PhotonView photonView;

		public override void OnEnter ()
		{
			photonView = PhotonView.Get ((GameObject)gameObject.Value);
			photonView.RPC ("SetFsmFloat", targets, variable.Name,value.Value);
			Finish ();
		}
	}
}
#endif