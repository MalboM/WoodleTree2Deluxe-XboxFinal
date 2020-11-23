#if PHOTON_BOLT
using UnityEngine;
using System.Collections;
using Bolt;

namespace ICode.Actions.PhotonBolt{
	[Category("Photon Bolt")]    
	[Tooltip("Get property.")]
	[System.Serializable]
	public class GetPropertyDynamic : BoltEntityAction {
		[Tooltip("The name of the property.")]
		public FsmString propertyName;
		[Shared]
		[ParameterType]
		public FsmVariable store;

		public override void OnEnter ()
		{
			base.OnEnter ();
			object property = entity.GetState<IState>().GetDynamic(propertyName.Value);
			store.SetValue (property);
			Finish ();
		}
		
	}
}
#endif