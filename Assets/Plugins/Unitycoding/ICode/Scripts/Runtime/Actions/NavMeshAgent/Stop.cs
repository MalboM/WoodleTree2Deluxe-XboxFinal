﻿using UnityEngine;
using System.Collections;

namespace ICode.Actions.UnityNavMeshAgent
{
	[Category (Category.NavMeshAgent)]
	[Tooltip ("Stop movement of this agent along its current path.")]
	[HelpUrl ("http://docs.unity3d.com/Documentation/ScriptReference/NavMeshAgent.Stop.html")]
	[System.Serializable]
	public class Stop : NavMeshAgentAction
	{
		[Tooltip ("If true, the GameObject is stopped immediately and not affected by the avoidance system. If false, the NavMeshAgent controls the deceleration.")]
		public FsmBool stopUpdates;
		[Tooltip ("Execute the action every frame.")]
		public bool everyFrame;

		public override void OnEnter ()
		{
			base.OnEnter ();
			#if UNITY_5_3_OR_NEWER
			#if UNITY_5_6_OR_NEWER
            if(agent.isOnNavMesh)   
    			agent.isStopped=true;
#else
			agent.Stop ();
#endif

            if (agent.isOnNavMesh)
                agent.updatePosition = !stopUpdates.Value;
#else
            if(agent.isOnNavMesh)   
			    agent.Stop (stopUpdates.Value);
#endif
            if (!everyFrame) {
				Finish ();
			}
		}

		public override void OnUpdate ()
		{
#if UNITY_5_3_OR_NEWER
#if UNITY_5_6_OR_NEWER
            if (agent.isOnNavMesh)
                agent.isStopped=true;
#else
            if(agent.isOnNavMesh)   
			    agent.Stop ();
#endif
#else
            if(agent.isOnNavMesh)   
			    agent.Stop (stopUpdates.Value);
#endif
        }
    }
}