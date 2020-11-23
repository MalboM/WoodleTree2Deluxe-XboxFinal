#if !UNITY_4_6 && !UNITY_4_7 && !UNITY_5_0
using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

namespace ICode.Actions.UnityNetworking{
	[System.Serializable]
	public abstract class NetworkManagerAction : StateAction {
		protected NetworkManager manager;

		public override void OnEnter ()
		{
			manager = NetworkManager.singleton;
		}
	}
}
#endif