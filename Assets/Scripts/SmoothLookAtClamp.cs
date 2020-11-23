//SmoothLookAt.cs
//Written by Jake Bayer
//Written and uploaded November 18, 2012
//This is a modified C# version of the SmoothLookAt JS script.  Use it the same way as the Javascript version.

using UnityEngine;
using System.Collections;

///<summary>
///Looks at a target
///</summary>
[AddComponentMenu("Camera-Control/Smooth Look At CS")]
public class SmoothLookAtClamp : MonoBehaviour
{
    public Transform target;        //an Object to lock on to
    public float damping = 6.0f;    //to control the rotation 
    public bool smooth = true;
    public float minDistance = 10.0f;   //How far the target is from the camera
    public string property = "";

    private Color color;
    private float alpha = 1.0f;
    private Transform _myTransform;
    
    public float rotationx;
    public float rotationy;
    public float rotationz;

    public float rotationxdamp;
    public float rotationydamp;

    public Quaternion rotation;

    private float yVelocity = 0.0f;
    private float xVelocity = 0.0f;

    void Awake()
    {
        _myTransform = transform;

        target = PlayerManager.GetMainPlayer().transform;
    }

    void LateUpdate()
    {
        if (target && Vector3.SqrMagnitude(this.transform.position - target.transform.position) <= 10000f)
        {
            rotationx = rotation.x * 150.0f;
            rotationxdamp = Mathf.SmoothDamp(rotationxdamp, rotationx, ref yVelocity, 0.4f);

            rotationy = rotation.y * 100;
            rotationydamp = Mathf.SmoothDamp(rotationydamp, rotationy, ref xVelocity, 0.4f);

            rotationz = rotation.z * 50;

            if (smooth)
            {

                //Look at and dampen the rotation
                 rotation = Quaternion.LookRotation(target.position - _myTransform.position);

                //  rotation.x = Mathf.Clamp(rotation, 0.1, 0.2);
                //  rotation.y = Mathf.Clamp(rotation, 0.1, 0.2);
                // rotation.z = Mathf.Clamp(rotation, 0.1, 0.2);

                // _myTransform.rotation = Quaternion.Slerp(_myTransform.rotation, rotation, Time.deltaTime * damping);

                //  transform.rotation.x = Quaternion.Euler(rotationx, 0, 0);
                if(!float.IsNaN(rotationxdamp) && !float.IsNaN(rotationydamp) && !float.IsNaN(rotationz))
                    transform.eulerAngles = new Vector3(rotationxdamp, rotationydamp, rotationz);


            }
            else { //Just look at
                _myTransform.rotation = Quaternion.FromToRotation(-Vector3.forward, (new Vector3(target.position.x, target.position.y, target.position.z) - _myTransform.position).normalized);

                float distance = Vector3.Distance(target.position, _myTransform.position);

                if (distance < minDistance)
                {
                    alpha = Mathf.Lerp(alpha, 0.0f, Time.deltaTime * 2.0f);
                }
                else {
                    alpha = Mathf.Lerp(alpha, 1.0f, Time.deltaTime * 2.0f);

                }
                //				if(!string.IsNullOrEmpty(property)) {
                //					color.a = Mathf.Clamp(alpha, 0.0f, 1.0f);

                //					renderer.material.SetColor(property, color);

                //				}
            }
        }


      


    }


}