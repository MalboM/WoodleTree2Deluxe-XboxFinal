using UnityEngine;
using System.Collections;

namespace ICode.Actions.UnityRigidbody2D{
	#if UNITY_5_1
	[Category("Rigidbody2D")]    
	[Tooltip("Use these flags to constrain motion of Rigidbodies.")]
	[HelpUrl("http://docs.unity3d.com/ScriptReference/RigidbodyConstraints.html")]
	[System.Serializable]
	public class SetConstraints : Rigidbody2DAction {

		public RigidbodyConstraints2D constraints;


		public override void OnEnter ()
		{
			base.OnEnter ();

			rigidbody.constraints = constraints;
			proxy.onFixedUpdate-=OnFixedUpdate;
			Finish ();
		}
	}
	#endif
}