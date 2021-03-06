﻿//(c) Samantha Stahlke 2020
//Created for INFR 2310.
using UnityEngine;

public class DirectController_COMPLETED : MonoBehaviour
{
    public float speed;
    public bool spin;
    public float spinRadius;
    private float angularSpeed;
    public float deadzone = 0.001f;
    public Camera cam;

    private Animator anim;

    private void Awake()
    {
        anim = this.GetComponent<Animator>();

        //We should travel one full revolution in the same time that we travel
        //in a straight line the same length as our circumference.
        angularSpeed = 360.0f * speed / (2 * Mathf.PI * spinRadius);
    }

    private void Update()
    {
        //Simple keyboard input:
        //W/S forward/backward and A/D left/right.
        Vector3 input = Vector3.zero;

        //We're doing this "manually" since we want
        //"instant" acceleration.
        //In your own projects, you should play around
        //with the settings in Unity's input manager
        //for your project.
        if (Input.GetKey(KeyCode.W))
            input.z = 1.0f;
        else if (Input.GetKey(KeyCode.S))
            input.z = -1.0f;

        if (Input.GetKey(KeyCode.D))
            input.x = 1.0f;
        else if (Input.GetKey(KeyCode.A))
            input.x = -1.0f;

        bool hasInput = input.magnitude > deadzone;

        if (hasInput)
        {
            //We will define our forward relative to the camera for this simple example.
            Vector3 inputDir = input.normalized;
            Vector3 inputCam = cam.transform.rotation * inputDir;
            inputCam.y = 0.0f;

            //We will update our position directly based on our desired movement speed.
            Vector3 newPos = transform.position + speed * inputCam.normalized * Time.deltaTime;
            
            //Just in case of precision errors, since we are not worrying about jumping/varied 
            //terrain in this example, we will not touch our y-position.
            //Normally you would also include gravity and let collision handle this.
            newPos.y = transform.position.y;

            transform.position = newPos;

            if(spin)
            {
                //Calculate a vector perpendicular to our velocity in the XZ plane as
                //a spin axis.
                Vector3 spinAxis = Vector3.Cross(Vector3.up, inputCam);
                transform.Rotate(spinAxis, angularSpeed * Time.deltaTime, Space.World);
            }          
            else
                transform.LookAt(transform.position + inputCam);

        }

        if (anim != null)
            anim.SetBool("isMoving", hasInput);
    }
}
