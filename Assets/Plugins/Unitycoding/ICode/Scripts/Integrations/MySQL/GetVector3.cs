#if MYSQL
using UnityEngine;
using System.Collections;

namespace ICode.Actions.MySQL{
	[Category("MySQL")]   
	[Tooltip("Load a Vector3 from MySQL database.")]
	[System.Serializable]
	public class GetVector3 : StateAction {
		[Tooltip("The key identifier.")]
		public FsmString key;
		[Shared]
		[Tooltip("The value to store.")]
		public FsmVector3 store;
		
		public override void OnEnter ()
		{	
			string serverAddress = PlayerPrefs.GetString ("ServerAddress");
			if (string.IsNullOrEmpty (serverAddress)) {
				Debug.Log ("Please initialize the database to load values. Use MySQL.Initialize");
			} else {
				GameObject instance = new GameObject ();
				CoroutineInstance routineHandler = instance.AddComponent<CoroutineInstance> ();
				routineHandler.StartCoroutine (LoadVector3 (serverAddress, key.Value));
				instance.hideFlags = HideFlags.HideInHierarchy;
			}
			Finish ();
		}
		
		
		private IEnumerator LoadVector3(string serverAddress,string key){
			WWWForm newForm = new WWWForm ();
			newForm.AddField ("key", key);
			
			WWW w = new WWW (serverAddress + "/getVector.php", newForm);
			
			while (!w.isDone) {
				yield return new WaitForEndOfFrame();
			}
			
			if (w.error != null) {
				Debug.LogError (w.error);
			}
			string[] split = w.text.Split (',');

			store.Value =  new Vector3(System.Convert.ToSingle(split[0].Trim()),System.Convert.ToSingle(split[1].Trim()),System.Convert.ToSingle(split[2].Trim()));
		}
	}
}
#endif