#if UMA
using System.Collections;
using UnityEngine;
using UMA;

namespace ICode.Actions.UMA{
	[Category("UMA")]   
	[Tooltip("Sets the recipe string.")]
	[System.Serializable]
	public class SetRecipe : StateAction {
		[SharedPersistent]
		[Tooltip("UMAData to use.")]
		public FsmGameObject gameObject;
		[Tooltip("Recipe to set")]
		public FsmString recipe;
		
		private UMAAvatarBase avatar;

		public override void OnEnter ()
		{
			avatar = ((GameObject)gameObject.Value).GetComponent<UMAAvatarBase> ();
			var asset = ScriptableObject.CreateInstance<UMATextRecipe>();
			asset.recipeString=recipe.Value;
			avatar.Load(asset);
			Finish ();
		}
		
		
	}
}
#endif