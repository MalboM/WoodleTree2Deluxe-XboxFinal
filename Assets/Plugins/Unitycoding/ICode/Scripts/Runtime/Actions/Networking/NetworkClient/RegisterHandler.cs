#if !UNITY_4_6 && !UNITY_4_7 && !UNITY_5_0
using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

namespace ICode.Actions.UnityNetworking.Client{
	[Category("UnityNetworking/NetworkClient")]
	[Tooltip("Called when this client reveives a response from server.")]
	[System.Serializable]
	public class RegisterHandler : StateAction {
		public FsmInt msgType;
		[Tooltip("Execute a state machine on receive. You can create an int variable called Info and it will be set automaticly.")]
		public StateMachine execute;

		public override void OnEnter ()
		{
			base.OnEnter ();
			NetworkClient.allClients[0].RegisterHandler ((short)msgType.Value, OnReceive);
			Finish ();
		}

		private void OnReceive(NetworkMessage message){
			FsmVariableMessage mMessage= new FsmVariableMessage(null);
			message.ReadMessage<FsmVariableMessage>(mMessage);
			if (execute != null) {
				GameObject go= new GameObject("Handler");
				ICodeBehaviour behaviour= go.AddBehaviour(execute);
				behaviour.stateMachine.SetVariable("Info",mMessage.variable.GetValue());
			}
		}
	}
}
#endif