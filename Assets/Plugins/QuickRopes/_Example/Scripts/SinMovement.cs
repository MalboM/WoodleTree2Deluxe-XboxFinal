using UnityEngine;
using System.Collections;

public class SinMovement : MonoBehaviour 
{
    public float speed = 10;
    public float magnitude = 5;

    public Vector3 specificDirection;

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void FixedUpdate()
    {
        if(specificDirection == Vector3.zero)
            transform.position = (this.transform.forward * Mathf.Sin(Time.time * speed) * magnitude) + startPosition;
        else
            transform.position = (specificDirection * Mathf.Sin(Time.time * speed) * magnitude) + startPosition;
    }
}
