using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Berry : Animable, ICollectable
{
    [Header("Animation")]
    [SerializeField] float rotatingSpeed = 3.0f;
    [SerializeField] float floatingSpeed = 1.0f;
    [SerializeField] float floatingHalfHeight = 0.25f;
    [SerializeField] float collectSpeed = 0.3f;
    [SerializeField] float collectDistance = 1.0f;

    [SerializeField] Transform berryT;

    Vector3 maxFloatingPos;
    Vector3 minFloatingPos;

    float x;

    void Awake()
    {
        maxFloatingPos = berryT.position + berryT.up * floatingHalfHeight;
        minFloatingPos = berryT.position - berryT.up * floatingHalfHeight;
    }

    public override void Animate(float deltaTime)
    {
        x += deltaTime;

        if (x > float.MaxValue)
            x = 0.0f;

        berryT.position = Vector3.Lerp(maxFloatingPos, minFloatingPos, Mathf.Sin(x * floatingSpeed) * 0.5f + 0.5f);
        berryT.Rotate(0.0f, deltaTime * rotatingSpeed, 0.0f);
    }

    public void Collect(Transform target)
    {
        owner.RemoveAnimable(this);
        StartCoroutine(CollectCoroutine(target));
    }

    IEnumerator CollectCoroutine(Transform target)
    {
        Vector3 startScale = berryT.localScale;
        float startDistance = (berryT.position - target.position).magnitude;
        float currentDistance = startDistance;

        while (currentDistance > collectDistance)
        {
            currentDistance = (berryT.position - target.position).magnitude;
            berryT.position = Vector3.Lerp(berryT.position, target.position, Time.deltaTime * collectSpeed);
            berryT.localScale = Vector3.Lerp(Vector3.zero, startScale, currentDistance / startDistance);

            yield return null;
        }

        PlayerManager.GetMainPlayer().CollectBerry();

        Destroy(this.gameObject);
    }
}
