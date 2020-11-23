#if MYSQL
using UnityEngine;
using System.Collections;

namespace ICode.Actions.MySQL{
	[Category("MySQL")]   
	[Tooltip("Load an int from MySQL database.")]
	[System.Serializable]
	public class GetFloat : StateAction {
		[Tooltip("The key identifier.")]
		public FsmString key;
		[Shared]
		[Tooltip("The value to store.")]
		public FsmFloat store;
		
		public override void OnEnter ()
		{	
			string serverAddress = PlayerPrefs.GetString ("ServerAddress");
			if (string.IsNullOrEmpty (serverAddress)) {
				Debug.Log ("Please initialize the database to load values. Use MySQL.Initialize");
			} else {
				GameObject instance = new GameObject ();
				CoroutineInstance routineHandler = instance.AddComponent<CoroutineInstance> ();
				routineHandler.StartCoroutine (LoadFloat (serverAddress, key.Value));
				instance.hideFlags = HideFlags.HideInHierarchy;
			}
			Finish ();
		}
		
		
		private IEnumerator LoadFloat(string serverAddress,string key){
			WWWForm newForm = new WWWForm ();
			newForm.AddField ("key", key);
			
			WWW w = new WWW (serverAddress + "/getFloat.php", newForm);
			
			while (!w.isDone) {
				yield return new WaitForEndOfFrame();
			}
			
			if (w.error != null) {
				Debug.LogError (w.error);
			}
			store.Value = System.Convert.ToSingle(w.text);
		}
	}
}
#endif