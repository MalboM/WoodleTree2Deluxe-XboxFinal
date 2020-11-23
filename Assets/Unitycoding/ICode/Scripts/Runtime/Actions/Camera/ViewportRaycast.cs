using UnityEngine;
using System.Collections;

namespace ICode.Actions.UnityCamera
{
	[Category (Category.Camera)]   
	[System.Serializable]
	public  class ViewportRaycast : StateAction
	{
		[SharedPersistent]
		public FsmGameObject camera;
		public FsmVector3 viewportPoint;
		[Tooltip ("The length of the ray.")]
		public FsmFloat distance;
		[Tooltip ("Layer masks can be used selectively filter game objects for example when casting rays.")]
		public LayerMask layerMask;

		public QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.Ignore;


		[Shared]
		[NotRequired]
		[Tooltip ("The distance from the ray's origin to the impact point.")]
		public FsmFloat hitDistance;
		[Shared]
		[NotRequired]
		[Tooltip ("The normal of the surface the ray hit.")]
		public FsmVector3 hitNormal;
		[Shared]
		[NotRequired]
		[Tooltip ("The impact point in world space where the ray hit the collider.")]
		public FsmVector3 hitPoint;
		[Shared]
		[NotRequired]
		[Tooltip ("The GameObject of the rigidbody or collider that was hit.")]
		public FsmGameObject hitGameObject;
		[NotRequired]
		[Tooltip ("Send a hit event.")]
		public FsmString hitEvent;
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
			Ray ray = this.m_Camera.ViewportPointToRay (viewportPoint.Value);
			RaycastHit hit;
			if (Physics.Raycast (ray.origin, ray.direction, out hit, distance.Value, layerMask, this.queryTriggerInteraction)) {
				if (!hitDistance.IsNone)
					hitDistance.Value = hit.distance;
				if (!hitNormal.IsNone)
					hitNormal.Value = hit.normal;
				if (!hitPoint.IsNone)
					hitPoint.Value = hit.point;
				if (!hitGameObject.IsNone)
					hitGameObject.Value = hit.transform.gameObject;
				if (!hitEvent.IsNone)
					this.Root.Owner.SendEvent (hitEvent.Value, null);
			}
		}
	}
}