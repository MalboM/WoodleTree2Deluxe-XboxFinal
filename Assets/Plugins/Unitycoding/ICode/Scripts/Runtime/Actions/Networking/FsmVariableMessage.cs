#if !UNITY_4_6 && !UNITY_4_7 && !UNITY_5_0
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;
using System.Collections;
using System;

namespace ICode{
	public class FsmVariableMessage : MessageBase {
		public FsmVariable variable;

		public FsmVariableMessage(FsmVariable variable){
			this.variable = variable;
		}

		public override void Serialize (NetworkWriter writer)
		{
			Type type = variable.GetType ();
			object value = variable.GetValue ();
			writer.Write (type.Name);
			if (type == typeof(FsmInt)) {
				writer.Write ((int)value);
			}else if(type== typeof(FsmString)){
				writer.Write((string)value);
			}else if(type== typeof(FsmFloat)){
				writer.Write((float)value);
			}else if(type== typeof(FsmColor)){
				writer.Write((Color)value);
			}else if(type== typeof(FsmVector2)){
				writer.Write((Vector2)value);
			}else if(type== typeof(FsmVector3)){
				writer.Write((Vector3)value);
			}
		}

		public override void Deserialize (NetworkReader reader)
		{
			string typeString = reader.ReadString ();
			Type type = TypeUtility.GetType(typeString);
			if (variable == null) {
				variable=ScriptableObject.CreateInstance(type) as FsmVariable;
			}
			if (type == typeof(FsmInt)) {
				variable.SetValue (reader.ReadInt32 ());
			}else if(type== typeof(FsmString)){
				variable.SetValue (reader.ReadString ());
			}else if(type== typeof(FsmFloat)){
				variable.SetValue (reader.ReadSingle ());
			}else if(type== typeof(FsmColor)){
				variable.SetValue (reader.ReadColor());
			}else if(type== typeof(FsmVector2)){
				variable.SetValue (reader.ReadVector2 ());
			}else if(type== typeof(FsmVector3)){
				variable.SetValue (reader.ReadVector3());
			}

		}
	}
}
#endif