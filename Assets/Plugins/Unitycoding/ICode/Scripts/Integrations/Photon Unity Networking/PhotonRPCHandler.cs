
using UnityEngine;
using System.Collections;

namespace ICode{
	public class PhotonRPCHandler : MonoBehaviour {
		#if PUN
		private Animator animator;
		
		private void Start(){
			animator = GetComponent<Animator> ();
		}
		
		[PunRPC]
		private void SetAnimatorBool(string parameter, bool value){
			if (animator != null && !string.IsNullOrEmpty(parameter)) {
				animator.SetBool(parameter,value);
			}
		}
		
		[PunRPC]
		private void SetAnimatorFloat(string parameter, float value){
			if (animator != null && !string.IsNullOrEmpty(parameter)) {
				animator.SetFloat(parameter,value);
			}
		}
		
		[PunRPC]
		private void SetAnimatorInt(string parameter, int value){
			if (animator != null && !string.IsNullOrEmpty(parameter)) {
				animator.SetInteger(parameter,value);
			}
		}
		
		[PunRPC]
		private void SetTrigger(string trigger){
			if (animator != null && !string.IsNullOrEmpty(trigger)) {
				animator.SetTrigger(trigger);
			}
		}
		
		[PunRPC]
		private void CrossFade(string state){
			if (animator != null && !string.IsNullOrEmpty(state)) {
				animator.CrossFade(state,0.1f);
			}
		}
		
		[PunRPC]
		private void LoadLevel(string level){
			if (!string.IsNullOrEmpty (level)) {
				PhotonNetwork.LoadLevel(level);
			}
		}
		
		[PunRPC]
		private void SetPosition(Vector3 position){
			this.transform.position = position;
		}
		#endif
	}
}


