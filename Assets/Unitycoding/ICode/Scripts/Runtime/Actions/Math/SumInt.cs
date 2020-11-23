using UnityEngine;
using System.Collections;

namespace ICode.Actions.Math
{
	[Category (Category.Math)]  
	[Tooltip ("Sums up two values.")]
	[HelpUrl ("http://msdn.microsoft.com/en-us/library/k1a63xkz.aspx")]
	[System.Serializable]
	public class SumInt : StateAction
	{
		[Tooltip ("First operand.")]
		public FsmInt first;
		[Tooltip ("Second operand")]
		public FsmInt second;
		[Shared]
		[Tooltip ("Store the result.")]
		public FsmInt store;
		[Tooltip ("Execute the action every frame.")]
		public bool everyFrame;

		public override void OnEnter ()
		{
			store.Value = first.Value + second.Value;
			if (!everyFrame) {
				Finish ();
			}
		}

		public override void OnUpdate ()
		{
			store.Value = first.Value + second.Value;
		}
	}
}