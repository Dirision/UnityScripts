using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public Transform cameraTransform;
    public Transform targetTransform;


    public float xSensitivity =5f;
    public float ySensitivity =5f;
    public float orbitDampening = 10f;
    private Vector3 offset;


    /* TODO:

        ADD toggle so that mouse movement does not move player actor 
        L-> FIX press shift to freelook

        ADD better smothening to camera (by editing the Lerp call or outright removal of Lerp)
        
    */

	// Use this for initialization
	void Start () {


        this.cameraTransform = this.transform;
        
        offset = targetTransform.position - this.transform.position;

        // if not set, then assume parent
        if ( ! this.targetTransform)
        {
            this.targetTransform = this.transform.parent;
        }


	}
    
    private void LateUpdate()
    {
        Quaternion yRotation =
                Quaternion.AngleAxis(Input.GetAxis("Mouse Y") * ySensitivity, -Vector3.right);

        offset = yRotation * offset;

        Quaternion xRotation;
        // Rotate while turning player
        if (! Input.GetKey(KeyCode.LeftShift))
        {
            float horizontal = Input.GetAxis("Mouse X") * xSensitivity;
            targetTransform.Rotate(0, horizontal, 0);
            float desiredYAngle = targetTransform.eulerAngles.y;
            xRotation = Quaternion.Euler(0, desiredYAngle, 0);
            transform.position = Vector3.Lerp(targetTransform.position, targetTransform.position - xRotation * offset, orbitDampening);

        }
        else
        {
            xRotation = Quaternion.Euler(0,Input.GetAxis("Mouse X") * xSensitivity,0);

            transform.position = Vector3.Lerp(targetTransform.position,  targetTransform.position - xRotation* offset, orbitDampening);
        }
        // transform.position = targetTransform.position - (rotation * offset);
        
        transform.LookAt(targetTransform);
    }

    // Update is called once per frame
    void Update () {
       
    }
}
