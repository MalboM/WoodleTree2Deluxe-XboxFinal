using UnityEngine;
using System.Collections;

namespace ICode.Actions.UnityCamera
{
	[Category (Category.Camera)]   
	[System.Serializable]
	public  class GetViewportPoint : StateAction
	{
		[SharedPersistent]
		public FsmGameObject camera;
		public FsmVector3 point;
		public FsmFloat distance;
		[NotRequired]
		[Shared]
		public FsmVector3 storePoint;
		[NotRequired]
		[Shared]
		public FsmVector3 storeDirection;

		[Tooltip ("Execute the action every frame.")]
		public bool everyFrame;

		private  Camera m_Camera;

		public override void OnEnter ()
		{
			m_Camera = camera.Value.GetComponent<Camera> ();
			DoRay ();
			if (!everyFrame) {
				Finish ();
			}
		}

		public override void OnUpdate ()
		{
			DoRay ();
		}

		private void DoRay ()
		{
			Ray ray = this.m_Camera.ViewportPointToRay (point.Value);
			storePoint.Value = ray.GetPoint (distance.Value);
			storeDirection.Value = ray.direction;
		}
	}
}