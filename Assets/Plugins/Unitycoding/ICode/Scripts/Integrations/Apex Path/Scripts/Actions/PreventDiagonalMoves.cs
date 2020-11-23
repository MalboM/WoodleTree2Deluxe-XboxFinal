#if APEX_PATH
using UnityEngine;
using Apex.Steering.Components;

namespace ICode.Actions.ApexPathfinding{
	[Category("Apex Path")]
	[Tooltip("Controls whether the unit is allowed to move to diagonal neighbours.")] 
	[System.Serializable]
	public class PreventDiagonalMoves : ApexPathAction
	{
		[Tooltip("Set to true or false.")]
		public FsmBool value;
		
		public override void OnEnter (){
			base.OnEnter ();
			options.preventDiagonalMoves = value.Value;
			Finish ();
		}
		
	}
}
#endif