#if CORE_GAME_KIT
using UnityEngine;
using System.Collections;
using DarkTonic.CoreGameKit;

namespace ICode.Actions.CoreGameKit{
	[Category("CoreGameKit")]   
	[Tooltip("End the current wave in Core GameKit")] 
	[System.Serializable]
	public class EndWave : StateAction {
		
		public override void OnEnter ()
		{
			LevelSettings.EndWave ();
		}
	}
}
#endif