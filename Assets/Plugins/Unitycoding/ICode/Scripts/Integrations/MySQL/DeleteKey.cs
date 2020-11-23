#if MYSQL
using UnityEngine;
using System.Collections;

namespace ICode.Actions.MySQL{
	[Category("MySQL")]   
	[Tooltip("Delete the entry from MySQL database.")]
	[System.Serializable]
	public class DeleteKey : StateAction {
		[Tooltip("The key identifier.")]
		public FsmString key;
	
		public override void OnEnter ()
		{	
			string serverAddress = PlayerPrefs.GetString ("ServerAddress");
			if (string.IsNullOrEmpty (serverAddress)) {
				Debug.Log ("Please initialize the database to save values. Use MySQL.Initialize");
			} else {
				GameObject instance = new GameObject ();
				CoroutineInstance routineHandler = instance.AddComponent<CoroutineInstance> ();
				routineHandler.StartCoroutine (DeleteEntry(serverAddress, key.Value));
				instance.hideFlags = HideFlags.HideInHierarchy;
			}
			Finish ();
		}
		
		
		private IEnumerator DeleteEntry(string serverAddress,string key){
			WWWForm newForm = new WWWForm ();
			newForm.AddField ("key", key);

			WWW w = new WWW (serverAddress + "/deleteEntry.php", newForm);
			
			while (!w.isDone) {
				yield return new WaitForEndOfFrame();
			}
			
			if (w.error != null) {
				Debug.LogError (w.error);
			}
		}
	}
}
#endif