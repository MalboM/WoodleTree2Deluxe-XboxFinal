using UnityEngine;
using System.Collections;
using System;

namespace ICode.Conditions
{
	[Obsolete ("This action is obsolete. WebPlayer is no longer supported by unity")]
	[Category (Category.Application)]    
	[Tooltip ("Is the current platform WebPlayer?")] 
	[System.Serializable]
	public class IsWebPlayer : Condition
	{
		[Tooltip ("Does the result equals this condition.")]
		public FsmBool equals;

		public override bool Validate ()
		{
			return false;//Application.isWebPlayer == equals.Value;
		}

	}
}