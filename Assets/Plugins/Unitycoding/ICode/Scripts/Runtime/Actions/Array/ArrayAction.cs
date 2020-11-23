using UnityEngine;
using System.Collections;

namespace ICode.Actions.Array{
	[System.Serializable]
	public class ArrayAction : StateAction {
		[Shared]
		[Tooltip("GameObject to use.")]
		public FsmArray array;

		public override void OnEnter ()
		{
			base.OnEnter ();
			if (array.Value == null) {
				array.Value=new object[0];
			}
		}
	}
}