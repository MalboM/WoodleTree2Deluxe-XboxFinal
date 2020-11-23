#if PUN
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ICode.Actions.Photon{
	[Category("Photon/PhotonNetwork")]   
	[Tooltip("Sets level prefix for PhotonViews instantiated later on.")]
	[System.Serializable]
	public class SetLevelPrefix : StateAction {
		[Tooltip("The name of the level to load.")]
		public FsmInt prefix;
		
		public override void OnEnter ()
		{
			PhotonNetwork.SetLevelPrefix (System.Convert.ToInt16(prefix.Value));	
			Finish ();
		}
	}
}
#endif