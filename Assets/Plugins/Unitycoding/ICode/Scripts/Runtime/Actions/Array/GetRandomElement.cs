using UnityEngine;
using System.Collections;

namespace ICode.Actions.Array
{
	[Category (Category.Array)]  
	[Tooltip ("Gets a random element.")]
	[System.Serializable]
	public class GetRandomElement : ArrayAction
	{
		[Shared]
		[ParameterType]
		public FsmVariable element;

		public override void OnEnter ()
		{
			base.OnEnter ();
			int index = Random.Range (0, array.Value.Length);
			element.SetValue (array.Value [index]);
			Finish ();
		}
		
		
	}
}