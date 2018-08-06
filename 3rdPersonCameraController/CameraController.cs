using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {


    /* 
     * 
     * NEW:
     * -- Fixed variable declaration in postUpdate
     * -- Shift now retains proper Y invert when pressed down and looking around
     * 
     * TODO:
     * 
     * DEBUG/CHECK:
     * -- Fix Y movement while shift is held down 
     *  |-> Did a fix that involves changing the axis the camera rotates around, depending on if shift is held. 
            This needs a serious check / highly volitile / hacky 
     * -- Weird issue when shift is held down the max y angle changes slightly due to how fast the user is moving the mouse
        |-> You can force the camera to flip to the other side of the character through this
     * 
     * ADD
     * -- Fixes / slight optimizations 
     * -- Focus target, so we dont have to look directly at the character, but rather at a reticle
     * 
     * 
     */


    public Transform cameraTransform;
    public Transform targetTransform;

    public float yAngleMax = 4f;
    public float yAngleMin = -4f;
    public float xSensitivity =5f;
    public float ySensitivity =5f;
    public float orbitDampening = 10f;

    // offset of the camera from the target 
    private Vector3 offset;
    private float yAngleTotal = 0F;
    private bool shiftHeld = false;
    private float normalYAngleTotal = 0;
    private Vector3 normalOffset;
    private Vector3 normalPosition;
    private float yMovement = 0f;
    private float xMovement = 0f;

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

        if (xMovement != 0 || yMovement != 0)
        {

            
            yAngleTotal = yAngleTotal + yMovement;
            yAngleTotal = Mathf.Clamp(yAngleTotal, yAngleMin, yAngleMax);
            Debug.Log("Y Angle Current: " + yAngleTotal.ToString());
            

            float horizontal = xMovement * xSensitivity;
            Quaternion xRotation;
            // Rotate while turning player
            if (!shiftHeld)
            {
                Debug.Log("Normal");
                if (!(yAngleMin >= yAngleTotal || yAngleTotal >= yAngleMax))
                {

                    Quaternion yRotation =
                            Quaternion.AngleAxis(yMovement * ySensitivity, -Vector3.right);

                    offset = yRotation * offset;
                }

                targetTransform.Rotate(0, horizontal, 0);
                float desiredXAngle = targetTransform.eulerAngles.y;
                xRotation = Quaternion.Euler(0, desiredXAngle, 0);

                transform.position = Vector3.Slerp(targetTransform.position, targetTransform.position - xRotation * offset, orbitDampening);
            }
            // Rotate, keep the players direction
            else
            {
                Debug.Log("Character Lock");
                if (!(yAngleMin >= yAngleTotal || yAngleTotal >= yAngleMax))
                {

                    Quaternion yRotation =
                            Quaternion.AngleAxis(yMovement * ySensitivity, -transform.right);

                    offset = yRotation * offset;
                }

                xRotation = Quaternion.AngleAxis(horizontal,Vector3.up);
                transform.position = Vector3.Lerp(targetTransform.position, targetTransform.position - xRotation * offset, orbitDampening);

            }
            // transform.position = targetTransform.position - (rotation * offset);

           

            transform.LookAt(targetTransform);
        }
    }

    
   
    // Update is called once per frame
    void Update () {


        yMovement = Input.GetAxis("Mouse Y");
        xMovement = Input.GetAxis("Mouse X");


        // Record positions before holding shift 
        // This is like a similar behavior for how ARMA does head turning 
        // record offset and position before 

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            normalYAngleTotal = yAngleTotal;
            normalOffset = offset;
            normalPosition = this.transform.position;
        }
        // restore offset and position on lift
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            yAngleTotal = normalYAngleTotal;
            offset = normalOffset;
            this.transform.position = normalPosition;
        }
        
        if (Input.GetKey(KeyCode.LeftShift))
        {
            Debug.Log("shiftHeld");

            // Why set offset on shift held
            offset = targetTransform.position - this.transform.position;
            shiftHeld = true;
        }
        else
            shiftHeld = false;

        Debug.DrawRay(transform.position, transform.right, Color.red);
        Debug.DrawRay(transform.position, transform.up, Color.green);


    }
}
