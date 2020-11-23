#if MYSQL
using UnityEngine;
using System.Collections;

namespace ICode.Actions.MySQL{
	[Category("MySQL")]   
	[Tooltip("Save a string to MySQL database.")]
	[System.Serializable]
	public class SetString : StateAction {
		[Tooltip("The key identifier.")]
		public FsmString key;
		[Tooltip("The value to save.")]
		public FsmString value;
		
		public override void OnEnter ()
		{	
			string serverAddress = PlayerPrefs.GetString ("ServerAddress");
			if (string.IsNullOrEmpty (serverAddress)) {
				Debug.Log ("Please initialize the database to save values. Use MySQL.Initialize");
			} else {
				GameObject instance = new GameObject ();
				CoroutineInstance routineHandler = instance.AddComponent<CoroutineInstance> ();
				routineHandler.StartCoroutine (SaveString (serverAddress, key.Value, value.Value));
				instance.hideFlags = HideFlags.HideInHierarchy;
			}
			Finish ();
		}
	
		
		private IEnumerator SaveString(string serverAddress,string key, string value){
			WWWForm newForm = new WWWForm ();
			newForm.AddField ("key", key);
			newForm.AddField ("value", value);
			
			WWW w = new WWW (serverAddress + "/saveString.php", newForm);
			
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