#if PHOTON_BOLT
using UnityEngine;
using System.Collections;
using Bolt;

namespace ICode.Actions.PhotonBolt{
	[Category("Photon Bolt")]    
	[Tooltip("Set property.")]
	[System.Serializable]
	public class SetPropertyDynamic : BoltEntityAction {
		[Tooltip("The name of the property.")]
		public FsmString propertyName;
		[Tooltip("Value to set.")]
		[ParameterType]
		public FsmVariable value;

		public override void OnEnter ()
		{
			base.OnEnter ();
			object mValue = value.GetValue ();
			//entity.GetState<IState>().SetDynamic(propertyName.Value,mValue);
			entity.GetState<ICubeState> ().CubeTransform.SetTransforms (entity.transform);
			Debug.Log((entity.GetState<IState> ().GetDynamic (propertyName.Value) as NetworkTransform).Transform);
			Finish ();
		}
		
	}
}
#endif