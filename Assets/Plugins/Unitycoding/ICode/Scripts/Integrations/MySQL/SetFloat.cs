#if MYSQL
using UnityEngine;
using System.Collections;

namespace ICode.Actions.MySQL{
	[Category("MySQL")]   
	[Tooltip("Save a float to MySQL database.")]
	[System.Serializable]
	public class SetFloat : StateAction {
		[Tooltip("The key identifier.")]
		public FsmString key;
		[Tooltip("The value to save.")]
		public FsmFloat value;
		
		public override void OnEnter ()
		{	
			string serverAddress = PlayerPrefs.GetString ("ServerAddress");
			if (string.IsNullOrEmpty (serverAddress)) {
				Debug.Log ("Please initialize the database to save values. Use MySQL.Initialize");
			} else {
				GameObject instance = new GameObject ();
				CoroutineInstance routineHandler = instance.AddComponent<CoroutineInstance> ();
				routineHandler.StartCoroutine (SaveFloat (serverAddress, key.Value, value.Value));
				instance.hideFlags = HideFlags.HideInHierarchy;
			}
			Finish ();
		}
		
		
		private IEnumerator SaveFloat(string serverAddress,string key, float value){
			WWWForm newForm = new WWWForm ();
			newForm.AddField ("key", key);
			newForm.AddField ("value", value.ToString());
			
			WWW w = new WWW (serverAddress + "/saveFloat.php", newForm);
			
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