using UnityEngine;
using System.Collections;

namespace ICode{
	public enum NetworkTriggerType {
		OnClientConnected,
		OnClientDisconnect,
		OnClientError,
		OnClientNotReady,
		OnClientSceneChanged,

		OnServerConnect,
		OnServerDisconnect,
		OnServerError,
		OnServerReady,
		OnServerAddPlayer,
		OnServerRemovePlayer,
	}
}