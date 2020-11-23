using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ArrayUtility=ICode.ArrayUtility;

namespace ICode.Actions.Array{
	[Category(Category.Array)]  
	[Tooltip("Add a new item of type to the array.")]
	[System.Serializable]
	public class Add : ArrayAction {
		[ParameterType]
		public FsmVariable variable;


		public override void OnEnter ()
		{
			base.OnEnter ();
			object[] mArray = array.Value;
			mArray = ArrayUtility.Add<object> (mArray, variable.GetValue ());
			array.Value = mArray;
			Finish ();
		}
		

	}
}