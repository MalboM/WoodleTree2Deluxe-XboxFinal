using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace ICode.Actions.UnityGUI{
	[Category(Category.GUI)]    
	[Tooltip("Sets the image of an Image component.")]
	[System.Serializable]
	public class SetImage : StateAction {
		[SharedPersistent]
		[Tooltip("GameObject to use.")]
		public FsmGameObject gameObject;
		[Tooltip("Image to set.")]
		public FsmObject image;
		[Tooltip("Execute the action every frame.")]
		public bool everyFrame;
		
		private Image component;
		
		public override void OnEnter ()
		{
			component = gameObject.Value.GetComponent<Image>();
			DoSetImage ();
			if (!everyFrame) {
				Finish ();
			}
		}
		
		public override void OnUpdate ()
		{
			DoSetImage ();
		}

		private void DoSetImage(){
			if (image.Value is Texture2D && !(image.Value is Sprite)) {
				Texture2D tex=image.Value as Texture2D;
				image.Value=Sprite.Create(tex,new Rect(0,0,tex.width,tex.height),new Vector2(0.5f,0.5f));
			}
			component.sprite=image.Value!= null? (Sprite)image.Value:null;
		}
	}
}