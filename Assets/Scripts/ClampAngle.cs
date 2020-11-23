using UnityEngine;
using System.Collections;

public class ClampAngle : MonoBehaviour
{
    public float speed = 1f;
    private Vector3 myRotation;

    void Start()
    {
        myRotation = gameObject.transform.rotation.eulerAngles;
    }

    void Update()
    {

        myRotation.z = Mathf.Clamp(myRotation.z - 1f, -45f, 45f);
        myRotation.x = Mathf.Clamp(myRotation.z - 1f, -45f, 45f);
        myRotation.y = Mathf.Clamp(myRotation.z - 1f, -45f, 45f);
    }

   /* void Update()
    {
        if (Input.GetKey(KeyCode.D))
            transform.position += new Vector3
                (speed * Time.deltaTime, 0.0f, 0.0f);

        if (Input.GetKey(KeyCode.A))
            transform.position -= new Vector3
                (speed * Time.deltaTime, 0.0f, 0.0f);

        if (Input.GetKey(KeyCode.D))
        {
            myRotation.z = Mathf.Clamp(myRotation.z - 1f, -45f, 45f);
            transform.rotation = Quaternion.Euler(myRotation);
        }

        if (Input.GetKey(KeyCode.A))
        {
            myRotation.z = Mathf.Clamp(myRotation.z + 1f, -45f, 45f);
            transform.rotation = Quaternion.Euler(myRotation);
        }
    }*/
}