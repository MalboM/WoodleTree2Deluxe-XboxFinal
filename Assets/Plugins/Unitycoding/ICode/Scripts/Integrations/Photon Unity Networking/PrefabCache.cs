#if PUN
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ICode{
	[System.Serializable]
	public class PrefabCache : ScriptableObject {
		public List<PrefabInstance> prefabs;
		
		public static void Initialize(){
			UnityEngine.Object[] cacheFiles = Resources.LoadAll("PrefabCache", typeof(PrefabCache));
			if (cacheFiles != null && cacheFiles.Length > 0) {
				if (cacheFiles.Length > 1) {
					Debug.LogWarning ("There are more than one PrefabCache files in 'Resources' folder. Check your project to keep only one.");
				}
				PrefabCache cache=(PrefabCache)cacheFiles [0];
				for (int i=0; i< cache.prefabs.Count; i++) {
					if (!PhotonNetwork.PrefabCache.ContainsKey (cache.prefabs [i].name)) {
						PhotonNetwork.PrefabCache.Add (cache.prefabs [i].name, cache.prefabs [i].prefab);
					}
					
					if (!NetworkingPeer.PrefabCache.ContainsKey (cache.prefabs [i].name)) {
						NetworkingPeer.PrefabCache.Add (cache.prefabs [i].name, cache.prefabs [i].prefab);
					}
				}
			} else {
				Debug.LogWarning ("Loading PrefabCache failed. PrefabCache asset must be in any 'Resources' folder as: PrefabCache");
			}
		}
		
		[System.Serializable]
		public class PrefabInstance{
			public string name;
			public GameObject prefab;
			public PrefabInstance(){}
			
			public PrefabInstance(string name,GameObject prefab){
				this.name=name;
				this.prefab=prefab;
			}
		}
	}
}
#endif