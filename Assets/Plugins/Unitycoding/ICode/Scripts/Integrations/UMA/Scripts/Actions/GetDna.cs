#if UMA
using UnityEngine;
using System.Collections;
using UMA;
using System.Reflection;

namespace ICode.Actions.UMA{
	[Category("UMA")]   
	[Tooltip("Gets the value of a humanoid dna.")]
	[System.Serializable]
	public class GetDna : StateAction {
		[SharedPersistent]
		[Tooltip("UMAData to use.")]
		public FsmGameObject gameObject;
		public UMADnaEnum dna;
		[Shared]
		[Tooltip("Store the value.")]
		public FsmFloat store;
		[Tooltip("Execute the action every frame.")]
		public bool everyFrame;


		private UMAData umaData;
		private UMADnaHumanoid umaDna;
		private FieldInfo info;
		
		public override void OnEnter ()
		{
			umaData = ((GameObject)gameObject.Value).GetComponent<UMAData> ();
			umaDna = umaData.GetDna<UMADnaHumanoid> ();
			info=umaDna.GetType().GetField(dna.ToString());
			store.Value=(float)info.GetValue(umaDna);
			if(!everyFrame){
				Finish();
			}
		}
		
		
		public override void OnUpdate ()
		{
			store.Value=(float)info.GetValue(umaDna);
		}
	}
}
#endif