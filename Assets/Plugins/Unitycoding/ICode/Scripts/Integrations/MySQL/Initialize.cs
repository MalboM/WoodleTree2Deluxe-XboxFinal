#if MYSQL
using UnityEngine;
using System.Collections;

namespace ICode.Actions.MySQL{
	[Category("MySQL")]   
	[Tooltip("Initialize MySQL to use saving actions.")]
	[System.Serializable]
	public class Initialize : StateAction {
		[Tooltip("Link to the folder with the php scripts on your host.")]
		public FsmString serverAddress;
		
		public override void OnEnter ()
		{
			PlayerPrefs.SetString ("ServerAddress", serverAddress.Value);
			Finish ();
		}
	}
}
#endif