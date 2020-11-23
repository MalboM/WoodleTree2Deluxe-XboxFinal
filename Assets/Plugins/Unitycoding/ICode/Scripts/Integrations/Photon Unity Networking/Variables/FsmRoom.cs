#if PUN
using UnityEngine;
using System.Collections;

namespace ICode{
	[System.Serializable]
	public class FsmRoom : FsmVariable{
		[SerializeField]
		private RoomInfo value;
		public RoomInfo Value {
			get {
				return this.value;
			}
			set {
				this.value=value;
			}
		}
		
		public override System.Type VariableType {
			get {
				return typeof(RoomInfo);
			}
		}
		
		public override void SetValue (object value)
		{
			this.value = (RoomInfo)value;
		}
		
		public override object GetValue ()
		{
			return this.value;
		}
		
		public static implicit operator RoomInfo(FsmRoom value)
		{
			return value.Value;
		}
		
		public static implicit operator FsmRoom(RoomInfo value) { 
			var variable = ScriptableObject.CreateInstance<FsmRoom>();
			variable.SetValue(value);
			return variable; 
		}
	}
}
#endif
