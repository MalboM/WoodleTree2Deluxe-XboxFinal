#if PUN
using UnityEngine;
using System.Collections;

namespace ICode.Actions.Photon{
	[Category("Photon/PhotonNetwork")]   
	[Tooltip("Initializes the PrefabCache. After this action is called, you do not need to store your prefabs in the resources folder.")]
	[System.Serializable]
	public class InitializePrefabCache : StateAction {
		
		public override void OnEnter ()
		{
			PrefabCache.Initialize ();
			Finish();
		}
	}
}
#endif