﻿using System.Collections;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    #region Variables (private)

    [SerializeField]
    private float
        distanceAway = 3.0f;
    [SerializeField]
    private float
        distanceUp = 2.0f;
    [SerializeField]
    private float
        yOffset = 1.0f;
    [SerializeField]
    private float
        cameraZoomMax = 25.0f;
    [SerializeField]
    private float
        cameraZoomMin = 4.0f;

    // Private global only
    private Transform playerTransform;
    private Vector3 nextCameraPosition;

    //Smoothing and damping
    private Vector3 velocityCamSmooth = Vector3.zero;
    [SerializeField]
    //Set this value carefully, small value like 0.1 will cause camera shaking.
    private float
        camSmoothDampTime = 0.0f;
    private bool isRotating = false;
    private float rotateXSpeed = 5.0f;
    private float rotateYSpeed = 5.0f;
    private Vector3 toCamera;
    private Quaternion cameraRotate;
    private float yHoriRotate = 0.0f;

    #endregion

    // Use this for initialization
    void Start()
    {
        playerTransform = GameObject.FindWithTag("Player").transform;
        toCamera = playerTransform.up * distanceUp + (-1) * playerTransform.forward * distanceAway;
    }
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0) || Input.GetMouseButton(1)) {
            isRotating = true;
        } else {
            isRotating = false;
        }
    }
    
    void LateUpdate()
    {
        if (isRotating) {
            Vector3 yAxis = Vector3.Cross(playerTransform.position - this.transform.position, Vector3.up);
            cameraRotate = Quaternion.AngleAxis((-1 * Input.GetAxis("Mouse Y")) * rotateYSpeed, yAxis)
                * Quaternion.AngleAxis(Input.GetAxis("Mouse X") * rotateXSpeed, playerTransform.up);

            toCamera = cameraRotate * toCamera;
        }

        this.transform.position = playerTransform.position + toCamera;

        yHoriRotate = (yHoriRotate + Input.GetAxis("Horizontal") * 2.0f);
        this.transform.RotateAround(playerTransform.position, playerTransform.up, yHoriRotate);

        toCamera = toCamera * (1 - Input.GetAxis("Mouse ScrollWheel"));
        if (toCamera.sqrMagnitude < cameraZoomMin) {
            toCamera = Mathf.Sqrt(cameraZoomMin) * toCamera.normalized;
        } else if (toCamera.sqrMagnitude > cameraZoomMax) {
            toCamera = Mathf.Sqrt(cameraZoomMax) * toCamera.normalized;
        }
        
        //look at the player, may add offset to it
        this.transform.LookAt(playerTransform.position + new Vector3(0f, yOffset, 0f));
    }
    
    #region Methods
    
    private void SmoothPosition(Vector3 fromPos, Vector3 toPos)
    {
        //Making a smooth transition between camera's current position and the position it wants to be in
        this.transform.position = Vector3.SmoothDamp(fromPos, toPos, ref velocityCamSmooth, camSmoothDampTime);
    }

    private float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360) {
            angle += 360;
        }

        if (angle > 360) {
            angle -= 360;
        }

        return Mathf.Clamp(angle, min, max);
    }

    #endregion
}
