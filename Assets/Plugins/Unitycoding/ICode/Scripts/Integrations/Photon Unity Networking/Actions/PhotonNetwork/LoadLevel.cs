#if PUN
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ICode.Actions.Photon{
	[Category("Photon/PhotonNetwork")]   
	[Tooltip("Loads the level by its name.")]
	[System.Serializable]
	public class LoadLevel : StateAction {
		[Tooltip("The name of the level to load.")]
		public FsmString level;
		
		public override void OnEnter ()
		{
			PhotonNetwork.LoadLevel(level.Value);		
			Finish ();
		}
	}
}
#endif