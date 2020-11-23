#if PHOTON_BOLT
using UnityEngine;
using System.Collections;
using Bolt;

namespace ICode.Actions.PhotonBolt{
	[Category("Photon Bolt")]    
	[Tooltip("Sets the Transform property.")]
	[System.Serializable]
	public class SetTransforms : BoltEntityAction {
		[Tooltip("The name of the property.")]
		public FsmString propertyName;
		[Tooltip("Value to set.")]
		public FsmGameObject value;
		
		public override void OnEnter ()
		{
			base.OnEnter ();
			NetworkTransform mTransform = entity.GetState<IState> ().GetDynamic (propertyName.Value) as NetworkTransform;
			mTransform.SetTransforms(value.Value.transform);
			Finish ();
		}
		
	}
}
#endif