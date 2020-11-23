using UnityEngine;
using System.Collections;

/*
public class AnimationsEnemyController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


}*/

    
namespace ICode.Actions.UnityNavMeshAgent
{
    [System.Serializable]
    public  class AnimationsEnemyController : StateAction
    {
        [SharedPersistent]
        [Tooltip("GameObject to use.")]
        public FsmGameObject gameObject;
   
        protected UnityEngine.AI.NavMeshAgent agent;

        public override void OnEnter()
        {
            agent = gameObject.Value.GetComponent<UnityEngine.AI.NavMeshAgent>();
        }
    }
}