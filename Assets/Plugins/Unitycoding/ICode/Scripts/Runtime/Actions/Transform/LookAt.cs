using UnityEngine;
using System.Collections;

namespace ICode.Actions.UnityTransform
{
	[Category (Category.Transform)]    
	[Tooltip ("Rotates the transform so the forward vector points at target's current position.")]
	[HelpUrl ("https://docs.unity3d.com/Documentation/ScriptReference/Transform.LookAt.html")]
	[System.Serializable]
	public class LookAt : TransformAction
	{
		[NotRequired]
		[Tooltip ("Position to look at.")]
		public FsmVector3 position;
		[NotRequired]
		[SharedPersistent]
		[Tooltip ("GameObject to look at.")]
		public FsmGameObject target;

		[Tooltip ("Vector specifying the upward direction.")]
		public FsmVector3 worldUp;
		[Tooltip ("If set to true, the game object will be rotated only in y axis.")]
		public FsmBool inY;
		[Tooltip ("Execute the action every frame.")]
		public bool everyFrame;

		public override void OnEnter ()
		{
			base.OnEnter ();
			DoLookAt ();
			if (!everyFrame) {
				Finish ();
			}
		}

		public override void OnUpdate ()
		{
			DoLookAt ();

		}

		private void DoLookAt ()
		{
			Vector3 targetPosition = FsmUtility.GetPosition (target, position);
			
			Vector3 lookAt = new Vector3 (targetPosition.x,
				                 (inY.Value ? transform.position.y : targetPosition.y),
				                 targetPosition.z
			                 );
			
			transform.LookAt (lookAt, worldUp.Value);
		}
	}
}