#if A_PATHFINDING_PROJECT
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;

namespace ICode.Actions.AStarProject{
	[Category("AStarProject")]   
	[System.Serializable]
	public class StartPath : StateAction {
		[SharedPersistent]
		[Tooltip("The GameObject to use.")]
		public FsmGameObject gameObject;
		[Tooltip("The destination point.")]
		public FsmVector3 destination;
		[Shared]
		[Tooltip("Store the Vector path.")]
		public FsmArray storePath;
		[Tooltip("OnPathComplete event.")]
		[DefaultValue("OnPathComplete")]
		public FsmString onPathComplete;

		private Seeker seeker;

		public override void OnEnter ()
		{

			seeker = gameObject.Value.GetComponent < Seeker> ();
			seeker.StartPath (seeker.transform.position,destination.Value, OnPathComplete);
		}

		protected virtual void OnPathComplete(Path p){
			if (!p.error) {
				storePath.Value= p.vectorPath.ConvertAll<object>(c=>(object)c).ToArray();
				this.Root.Owner.SendEvent(onPathComplete,null);
			}
		}
	}
}
#endif