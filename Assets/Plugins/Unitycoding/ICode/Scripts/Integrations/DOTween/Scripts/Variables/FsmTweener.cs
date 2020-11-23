#if DOTWEEN
using UnityEngine;
using System.Collections;
using DG.Tweening;

namespace ICode{
	[System.Serializable]
	public class FsmTweener : FsmVariable{
		[SerializeField]
		private Tweener value;
		public Tweener Value {
			get {
				return this.value;
			}
			set {
				SetValue(value);
			}
		}
		
		public override System.Type VariableType {
			get {
				return typeof(Tweener);
			}
		}
		
		public override void SetValue (object value)
		{
			this.value = (Tweener)value;
			if(onVariableChange == null){
				onVariableChange= new VariableChangedEvent();
			}
			onVariableChange.Invoke(this.value);
		}
		
		public override object GetValue ()
		{
			return this.value;
		}
		
		public static implicit operator Tweener(FsmTweener value)
		{
			return value.Value;
		}
		
		public static implicit operator FsmTweener(Tweener value) { 
			var variable = ScriptableObject.CreateInstance<FsmTweener>();
			variable.SetValue(value);
			return variable; 
		}
	}
}
#endif
