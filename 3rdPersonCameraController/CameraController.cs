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


    /* 
     * TODO:
     * 
     * NEW:
     * -- TODO checklist :)
     * -- Shift now turns camera without moving target
     *
     * DEBUG/CHECK:
     * -- Hold shift to turn camera while keeping object still
     *
     * ADD
     * -- Better smothening to camera (by editing the Lerp call or 
     *    outright removal of Lerp)
     * -- Better transition when returning from a held shift
     * -- Option to include y rotation of target when looking up/down
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
        if (!shiftHeld)
        {
            Debug.Log("Normal");
            float horizontal = Input.GetAxis("Mouse X") * xSensitivity;
            targetTransform.Rotate(0, horizontal, 0);
            float desiredYAngle = targetTransform.eulerAngles.y;
            xRotation = Quaternion.Euler(0, desiredYAngle, 0);
            transform.position = Vector3.Slerp(targetTransform.position, targetTransform.position - xRotation * offset, orbitDampening);

        }
        else
        {
            Debug.Log("Character Lock");
            float horizontal = Input.GetAxis("Mouse X") * xSensitivity;
            xRotation = Quaternion.AngleAxis(horizontal , Vector3.up);
           // offset = xRotation * offset;
            transform.position = Vector3.Lerp(targetTransform.position,  targetTransform.position - xRotation * offset,  orbitDampening);
        }
        // transform.position = targetTransform.position - (rotation * offset);
        
        transform.LookAt(targetTransform);
    }

    
    bool shiftHeld = false;
    Vector3 normalOffset;
    Vector3 normalPosition; 
    // Update is called once per frame
    void Update () {
        // Record positions before holding shift 
        // This is like a similar behavior for how ARMA does head turning 
        // record offset and position before 
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            normalOffset = offset;
            normalPosition = this.transform.position;
        }
        // restore offset and position on lift
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            offset = normalOffset;
            this.transform.position = normalPosition;
        }
        
        if (Input.GetKey(KeyCode.LeftShift))
        {
            Debug.Log("shiftHeld");
            offset = targetTransform.position - this.transform.position;
            shiftHeld = true;
        }
        else
            shiftHeld = false; 

    }
}
