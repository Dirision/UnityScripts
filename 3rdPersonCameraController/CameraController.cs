using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public Transform cameraTransform;
    public Transform targetTransform;

    private Vector3 offset;


    /* TODO:

        ADD proper variables for mouse sensitivity (instead of just "5") 
        ADD toggle so that mouse movement does not move player actor 
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

        Quaternion xRot =
               Quaternion.AngleAxis(Input.GetAxis("Mouse Y") * 5, Vector3.right);

        offset = xRot * offset;

        float horizontal = Input.GetAxis("Mouse X") * 5;
        targetTransform.Rotate(0, horizontal, 0);

        float desiredYAngle = targetTransform.eulerAngles.y;
        Quaternion rotation = Quaternion.Euler(0, desiredYAngle, 0);

        // transform.position = targetTransform.position - (rotation * offset);
        transform.position = Vector3.Lerp(targetTransform.position, targetTransform.position - rotation * offset, 10);
        transform.LookAt(targetTransform);
    }

    // Update is called once per frame
    void Update () {
       
    }
}
