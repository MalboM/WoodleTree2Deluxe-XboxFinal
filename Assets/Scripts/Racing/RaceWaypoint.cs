using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceWaypoint : MonoBehaviour
{
    public RaceManager raceManager;
    [HideInInspector] public int waypointID;
    public MeshRenderer[] meshRenderers;
    [HideInInspector] public bool entered;

    void Start()
    {
        
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (!entered && raceManager.raceStarted && other.gameObject == PlayerManager.GetMainPlayer().gameObject)
        {
            raceManager.EnteredWayPoint(this);
        }
    }

    public void ChangeMaterial(Material newMat)
    {
        foreach (MeshRenderer mr in meshRenderers)
            mr.material = newMat;
    }
}
