using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ICode{
	[AddComponentMenu("ICode/ICodeMaster")]
	public class ICodeMaster : MonoBehaviour {
		public List<ComponentModel> components;

		[System.Serializable]
		public class ComponentModel{
			public ICodeBehaviour component;
			public bool show;

			public ComponentModel(ICodeBehaviour component,bool show){
				this.component=component;
				this.show=show;
			}
		}
	}
}