#if UMA
using System.Collections;
using UnityEngine;
using UMA;

namespace ICode.Actions.UMA{
	[Category("UMA")]   
	[Tooltip("Gets the recipe string.")]
	[System.Serializable]
	public class GetRecipe : StateAction {
		[SharedPersistent]
		[Tooltip("UMAData to use.")]
		public FsmGameObject gameObject;
		[Shared]
		[Tooltip("Store the value.")]
		public FsmString store;

		[Tooltip("Execute the action every frame.")]
		public bool everyFrame;

		private UMADynamicAvatar dynamicAvatar;
		private UMAData umaData;
		
		public override void OnEnter ()
		{
			dynamicAvatar = ((GameObject)gameObject.Value).GetComponent<UMADynamicAvatar> ();
			umaData = ((GameObject)gameObject.Value).GetComponent<UMAData> ();

			var asset = ScriptableObject.CreateInstance<UMATextRecipe>();
			asset.Save(umaData.umaRecipe, dynamicAvatar.context);
			store.Value=asset.recipeString;
			if(!everyFrame){
				Finish();
			}
		}
		
		
		public override void OnUpdate ()
		{
			var asset = ScriptableObject.CreateInstance<UMATextRecipe>();
			asset.Save(umaData.umaRecipe, dynamicAvatar.context);
			store.Value=asset.recipeString;
		}	
	}
}
#endif