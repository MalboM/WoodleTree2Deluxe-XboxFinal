#if MYSQL
using UnityEngine;
using System.Collections;

namespace ICode.Actions.MySQL{
	[Category("MySQL")]   
	[Tooltip("Save a Vector3 to MySQL database.")]
	[System.Serializable]
	public class SetVector3 : StateAction {
		[Tooltip("The key identifier.")]
		public FsmString key;
		[Tooltip("The value to save.")]
		public FsmVector3 value;
		
		public override void OnEnter ()
		{	
			string serverAddress = PlayerPrefs.GetString ("ServerAddress");
			if (string.IsNullOrEmpty (serverAddress)) {
				Debug.Log ("Please initialize the database to save values. Use MySQL.Initialize");
			} else {
				GameObject instance = new GameObject ();
				CoroutineInstance routineHandler = instance.AddComponent<CoroutineInstance> ();
				routineHandler.StartCoroutine (SaveVector3 (serverAddress, key.Value, value.Value));
				instance.hideFlags = HideFlags.HideInHierarchy;
			}
			Finish ();
		}
		
		
		private IEnumerator SaveVector3(string serverAddress,string key, Vector3 value){
			WWWForm newForm = new WWWForm ();
			newForm.AddField ("key", key);
			newForm.AddField ("x", value.x.ToString());
			newForm.AddField ("y", value.y.ToString());
			newForm.AddField ("z", value.z.ToString());

			WWW w = new WWW (serverAddress + "/saveVector.php", newForm);
			
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