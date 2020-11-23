using UnityEngine;
using System.Collections;

namespace ICode.Actions.UnityVector2{
	[Category(Category.Vector2)] 
	[Tooltip("Linearly interpolates between two vectors.")]
	[HelpUrl("http://docs.unity3d.com/ScriptReference/Vector2.Lerp.html")]
	[System.Serializable]
	public class Lerp : StateAction {
		public FsmVector2 from;
		public FsmVector2 to;
		public FsmFloat smooth;
		[Shared]
		[Tooltip("Store the result.")]
		public FsmVector2 store;
		
		public override void OnUpdate ()
		{
			store.Value = Vector2.Lerp (from.Value, to.Value, Time.deltaTime * smooth.Value);
		}
	}
}