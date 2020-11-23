using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unitycoding
{
	public class LabelAttribute : PropertyAttribute
	{
		public readonly string label;

		public LabelAttribute (string label)
		{
			this.label = label;
		}
	}
}