#if PUN
using UnityEngine;
using System.Collections;

namespace ICode.Actions.Photon{
	[Category("Photon/PhotonNetwork")]
	[Tooltip("Gets the prefab from PrefabCache.")]
	[System.Serializable]
	public class GetPrefab : StateAction {
		[InspectorLabel("Name")]
		[Tooltip( "Name of the prefab.")]
		public FsmString _name;
		[SharedPersistent]
		[Tooltip( "Store the prefab.")]
		public FsmGameObject store;
		
		public override void OnEnter ()
		{
			GameObject prefab;
			if (PhotonNetwork.PrefabCache.TryGetValue (_name.Value, out prefab)) {
				store.Value=prefab;
			}
		
			if (prefab == null) {
				NetworkingPeer.PrefabCache.TryGetValue(_name.Value,out prefab);
				store.Value=prefab;
			}
			Finish ();
		}
		
	}
}
#endif