using UnityEngine;
using System.Collections;

namespace ICode.Conditions{
	[Category(Category.Vector3)]
	[Tooltip("Distance between two Vector3 y values.")]
	[HelpUrl("https://docs.unity3d.com/Documentation/ScriptReference/Vector3.Distance.html")]
	[System.Serializable]
	public class DistanceVertical : Condition {
		[NotRequired]
		[Tooltip("Vector3 value.")]
		public FsmVector3 a;
		[SharedPersistent]
		[NotRequired]
		public FsmGameObject first;
		[NotRequired]
		[Tooltip("Vector3 value.")]
		public FsmVector3 b;
		[Shared]
		[NotRequired]
		public FsmGameObject second;
		[Tooltip("Is the distance greater or less?")]
		public FloatComparer comparer;
		[Tooltip("Value to test with.")]
		public FsmFloat value;
		
		
		public override bool Validate ()
		{
            //	float distance= Vector3.Distance (FsmUtility.GetPosition(first, a),FsmUtility.GetPosition(second, b));
            float distance = Mathf.Abs(FsmUtility.GetPosition(first,a).y - FsmUtility.GetPosition(second, b).y);
			return FsmUtility.CompareFloat (distance, value.Value, comparer);
		}
	}
}