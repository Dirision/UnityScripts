using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public Transform cameraTransform;
    public Transform targetTransform;

    private Vector3 offset;
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
    

    /// <summary>
    /// ///////////////////////////////
    /// 
    /// 
    
    /// </summary>

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
        //transform.LookAt(targetTransform.transform);


        // float hInput = Input.GetAxis("Horizontal");
        // if (hInput != 0.0f) { transform.Translate(hInput * Time.deltaTime , 0, 0); };


    // transform.position = target.transform.position + offset;
    }
}
