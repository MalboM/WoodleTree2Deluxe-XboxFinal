#if MYSQL
using UnityEngine;
using System.Collections;

namespace ICode.Actions.MySQL{
	[Category("MySQL")]   
	[Tooltip("Save a bool to MySQL database.")]
	[System.Serializable]
	public class SetBool : StateAction {
		[Tooltip("The key identifier.")]
		public FsmString key;
		[Tooltip("The value to save.")]
		public FsmBool value;
		
		public override void OnEnter ()
		{	
			string serverAddress = PlayerPrefs.GetString ("ServerAddress");
			if (string.IsNullOrEmpty (serverAddress)) {
				Debug.Log ("Please initialize the database to save values. Use MySQL.Initialize");
			} else {
				GameObject instance = new GameObject ();
				CoroutineInstance routineHandler = instance.AddComponent<CoroutineInstance> ();
				routineHandler.StartCoroutine (SaveBool (serverAddress, key.Value, value.Value));
				instance.hideFlags = HideFlags.HideInHierarchy;
			}
			Finish ();
		}
		
		
		private IEnumerator SaveBool(string serverAddress,string key, bool value){
			WWWForm newForm = new WWWForm ();
			newForm.AddField ("key", key);
			newForm.AddField ("value", value?1:0);
			
			WWW w = new WWW (serverAddress + "/saveBool.php", newForm);
			
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