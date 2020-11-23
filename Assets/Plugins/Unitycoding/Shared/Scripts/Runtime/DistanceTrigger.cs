using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;

namespace Unitycoding{
	public class DistanceTrigger : MonoBehaviour {
		[Tag]
		public string triggerTag="Player";
        public string triggerName = "WoodleCharacter";
		public float distance = 2f;
		public TriggerEvent onTriggerEnter;
		public TriggerEvent onTriggerExit;

		protected virtual void Start(){
			SphereCollider collider = gameObject.AddComponent<SphereCollider> ();
			collider.radius = distance;
			collider.isTrigger = true;
			gameObject.layer = 2;
		}
		
		private void OnTriggerEnter(Collider other) {
			#if PUN
			PhotonView photonView=other.gameObject.GetComponent<PhotonView>();
			if (photonView != null && !photonView.isMine) {
				return;
			}
			#endif
			if (other.tag == triggerTag && other.gameObject.name == triggerName) {

				onTriggerEnter.Invoke(other.gameObject);
			}
		}
		
		private void OnTriggerExit(Collider other) {
			#if PUN
			PhotonView photonView=other.gameObject.GetComponent<PhotonView>();
			if (photonView != null && !photonView.isMine) {
				return;
			}
			#endif
			if (other.tag == triggerTag && other.gameObject.name == triggerName) {
				onTriggerExit.Invoke(other.gameObject);
			}
		}

		[System.Serializable]
		public class TriggerEvent:UnityEvent<GameObject>{}
	}
}