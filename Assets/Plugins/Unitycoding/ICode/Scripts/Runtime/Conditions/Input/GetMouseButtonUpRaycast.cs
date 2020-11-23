using UnityEngine;
using System.Collections;

namespace ICode.Conditions{
	[Category(Category.Input)]
	[Tooltip("Returns true during the frame the user releases the given mouse button.")]
	[HelpUrl("http://docs.unity3d.com/ScriptReference/Input.GetMouseButtonUp.html")]
	[System.Serializable]
	public class GetMouseButtonUpRaycast : Condition {
		[Tooltip("Button values are 0 for left button, 1 for right button, 2 for the middle button.")]
		public FsmInt button;
		public LayerMask layerMask;
		[NotRequired]
		[Shared]
		public FsmGameObject hitGameObject;
		public override bool Validate ()
		{
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			RaycastHit hit;
			if (Input.GetMouseButtonUp (button.Value) && Physics.Raycast (ray.origin, ray.direction,out hit, Mathf.Infinity, layerMask)) {
				hitGameObject.Value=hit.collider.gameObject;
				return true;
			}
			return false;
		}
	}
}