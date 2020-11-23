#if PUN
using UnityEngine;
using System.Collections;

namespace ICode.Actions.Photon{
	[Category("Photon/PhotonNetwork")]
	[Tooltip("Adds a new prefab to the PrefabCache.")]
	[System.Serializable]
	public class AddPrefab : StateAction {
		[InspectorLabel("Name")]
		[Tooltip( "Unique name, to be used in Photon.Instantiate.")]
		public FsmString _name;
		[Tooltip( "Prefab to add.")]
		public FsmGameObject prefab;

		public override void OnEnter ()
		{
			if (!PhotonNetwork.PrefabCache.ContainsKey (_name.Value)) {
				PhotonNetwork.PrefabCache.Add (_name.Value, prefab.Value);
			}
			if (!NetworkingPeer.PrefabCache.ContainsKey (_name.Value)) {
				NetworkingPeer.PrefabCache.Add (_name.Value, prefab.Value);
			}
			Finish ();
		}

	}
}
#endif