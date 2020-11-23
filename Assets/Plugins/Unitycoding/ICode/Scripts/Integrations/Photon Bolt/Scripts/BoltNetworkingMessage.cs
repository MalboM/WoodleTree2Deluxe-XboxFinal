#if PHOTON_BOLT
using UnityEngine;
using System.Collections;

namespace ICode{
	public enum BoltNetworkingMessage {
		BoltStartBegin,
		BoltStartDone,
		BoltStartFailed,
		SceneLoadLocalBegin,
		SceneLoadLocalDone,
		SceneLoadRemoteDone,
		Connected,
		ConnectFailed,
		ConnectRequest,
		ConnectRefused,
		ConnectAttempt,
		Disconnected,
		ControlOfEntityLost,
		ControlOfEntityGained,
		EntityAttached,
		EntityDetached,
		EntityReceived,
		EntityFrozen,
		EntityThawed,
	}
}
#endif