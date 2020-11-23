#if UMA
using System.Collections;
using UnityEngine;
using UMA;

namespace ICode.Actions.UMA{
	[Category("UMA")]   
	[Tooltip("Gets the recipe string.")]
	[System.Serializable]
	public class LoadRecipeAsset : StateAction {
		[Tooltip("Asset to load.")]
		public FsmObject recipe;
		[Shared]
		[Tooltip("Store the value.")]
		public FsmString store;

		public override void OnEnter ()
		{
			store.Value=((UMATextRecipe)recipe.Value).recipeString;
			Finish ();
		}
	}
}
#endif