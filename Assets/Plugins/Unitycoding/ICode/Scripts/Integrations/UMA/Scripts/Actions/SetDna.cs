#if UMA
using UnityEngine;
using System.Collections;
using UMA;
using System.Reflection;

namespace ICode.Actions.UMA{
	[Category("UMA")]   
	[Tooltip("Sets the value of a humanoid dna.")]
	[System.Serializable]
	public class SetDna : StateAction {
		[SharedPersistent]
		[Tooltip("UMAData to use.")]
		public FsmGameObject gameObject;
		public UMADnaEnum dna;
		[Tooltip("Value to set.")]
		public FsmFloat value;
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


			if (!Mathf.Approximately(value.Value,(float)info.GetValue(umaDna))) {
				info.SetValue(umaDna,value.Value);
				UpdateUMAShape ();
			}

			if(!everyFrame){
				Finish();
			}
		}

	
		public override void OnUpdate ()
		{
			if (!Mathf.Approximately(value.Value,(float)info.GetValue(umaDna))) {
				info.SetValue(umaDna,value.Value);
				UpdateUMAShape ();
			} 
		}

		public void UpdateUMAAtlas(){
			umaData.isTextureDirty = true;
			umaData.Dirty();	
		}
		
		public void UpdateUMAShape(){
			umaData.isShapeDirty = true;
			umaData.Dirty();
		}
	}
}
#endif