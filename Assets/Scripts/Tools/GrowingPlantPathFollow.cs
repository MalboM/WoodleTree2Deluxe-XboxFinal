using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class GrowingPlantPathFollow : MonoBehaviour
{
    public GameObject pathParent;
    List<Transform> waypoints = new List<Transform>();

    Coroutine[] coroutines = new Coroutine[4];
    int[] currentWaypoint = new int[4];

    Player[] inputs = new Player[4];

    public ActivateAnimationOnTrigger plantAAOT;
    bool[] delayAttachment = new bool[4];

    void Start()
    {
        if (pathParent != null)
        {
            foreach (Transform t in pathParent.transform)
            {
                if (t != pathParent.transform)
                    waypoints.Add(t);
            }
        }
        for (int p = 0; p < 4; p++)
            inputs[p] = ReInput.players.GetPlayer(p);
    }
    
    void OnCollisionStay(Collision collision)
    {
        if(plantAAOT.canUseSpline)
        {
            foreach (ContactPoint cp in collision.contacts)
            {
                if (cp.otherCollider.tag == "Player")
                {
                    TPC curTPC = cp.otherCollider.GetComponent<TPC>();
                    if (curTPC.onGround && coroutines[curTPC.pID] == null && !delayAttachment[curTPC.pID])
                    {
                        float shortestDist = 99999f;
                        float curDist = 0f;
                        foreach (Transform t in waypoints)
                        {
                            curDist = (cp.otherCollider.transform.position - t.position).sqrMagnitude;
                            if (curDist < shortestDist)
                            {
                                shortestDist = curDist;
                                currentWaypoint[curTPC.pID] = int.Parse(t.name);
                            }
                        }
                        curTPC.onSpline = true;
                        curTPC.disableControl = true;
                        coroutines[curTPC.pID] = StartCoroutine("PathMovement", curTPC);
                    }
                }
            }
        }
    }

    IEnumerator PathMovement(TPC character)
    {
        float input = Mathf.Abs(inputs[character.pID].GetAxis("LV"));
        if (Mathf.Abs(inputs[character.pID].GetAxis("LH")) > input)
            input = Mathf.Abs(inputs[character.pID].GetAxis("LH"));

        character.CalculateBasicAnimation(0f, input);

        int nextWaypoint = (int)Mathf.Sign(input) + currentWaypoint[character.pID];
        nextWaypoint = Mathf.Clamp(nextWaypoint, 0, waypoints.Count - 1);

        character.relative.transform.forward = (waypoints[nextWaypoint].position - character.transform.position).normalized;
        character.relative.transform.localEulerAngles = new Vector3(0f, character.relative.transform.localEulerAngles.y, 0f);

        character.rb.velocity = Vector3.zero;
        character.rb.MovePosition(Vector3.MoveTowards(character.transform.position, waypoints[nextWaypoint].position, character.groundSpeed * Mathf.Abs(input) * Time.deltaTime));
        if ((character.transform.position - waypoints[nextWaypoint].position).sqrMagnitude < 0.1f)
            currentWaypoint[character.pID] = nextWaypoint;

        yield return null;

        if (inputs[character.pID].GetButtonDown("Jump") || (nextWaypoint == currentWaypoint[character.pID]) && (nextWaypoint == 0 || nextWaypoint == waypoints.Count - 1))
        {
            character.onSpline = false;
            character.disableControl = false;
            coroutines[character.pID] = null;
            delayAttachment[character.pID] = true;
            

            if (inputs[character.pID].GetButtonDown("Jump"))
            {
                yield return null;
                character.onGround = false;
                character.delayed = true;
                character.jumpButtonDown = true;
                character.Jump();
            }

            yield return new WaitForSeconds(0.5f);
            delayAttachment[character.pID] = false;
        }
        else
        {
            coroutines[character.pID] = StartCoroutine("PathMovement", character);
        }
    }
}
