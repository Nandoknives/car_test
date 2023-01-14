using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    private const string HORIZONTAL = "Horizontal";
    private const string VERTICAL = "Vertical";
    private bool isBreaking;
    private float currentBreakForce;
    private float currentSteerAngle;
    private float acceleration_time = 0f;
    private bool maximumSpeed=false;

    private float horizontalInput;
    private float verticalInput;
    public float speed = 1f;

    [SerializeField] public float motorForce;
    [SerializeField] public float breakForce;
    [SerializeField] public float maxSteeringAngle;

    [SerializeField] private WheelCollider frontLeftCollider;
    [SerializeField] private WheelCollider frontRightCollider;
    [SerializeField] private WheelCollider rearLeftCollider;
    [SerializeField] private WheelCollider rearRightCollider;

    [SerializeField] private Transform frontLeftTransform;
    [SerializeField] private Transform frontRightTransform;
    [SerializeField] private Transform rearLeftTransform;
    [SerializeField] private Transform rearRightTransform;


    private void FixedUpdate()
    {
        GetInput();
        HandleMotor();
        HandleSteering();
        UpdateWheels();
    }

    private void HandleMotor()
    {
        frontLeftCollider.motorTorque = verticalInput * motorForce/10f;
        frontRightCollider.motorTorque = verticalInput * motorForce/10f;


    }

    private void ApplyBreaking()
    {
        frontRightCollider.brakeTorque = currentBreakForce;
        frontLeftCollider.brakeTorque = currentBreakForce;
        rearRightCollider.brakeTorque = currentBreakForce;
        rearLeftCollider.brakeTorque = currentBreakForce;
    }

    private void HandleSteering()
    {
       currentSteerAngle = maxSteeringAngle * horizontalInput;
       frontLeftCollider.steerAngle = currentSteerAngle;
       frontRightCollider.steerAngle = currentSteerAngle;
    }

    private void GetInput()
    {
        horizontalInput = Input.GetAxis(HORIZONTAL);


        verticalInput = Input.GetAxis(VERTICAL);

        isBreaking = Input.GetKey(KeyCode.Space);
    }

    IEnumerator AccelerationClock()
    {
        acceleration_time = +Time.deltaTime;
 
        if (acceleration_time >= 3f)
        {
  
            yield break;
        }
        else 
        yield return new WaitForSeconds(0f);
    }

    private void UpdateWheels()
    {
        UpdateSingleWheel(frontLeftCollider, frontLeftTransform);
        UpdateSingleWheel(frontRightCollider, frontRightTransform);
        UpdateSingleWheel(rearLeftCollider, rearLeftTransform);
        UpdateSingleWheel(rearRightCollider, rearRightTransform);

    }
    private void UpdateSingleWheel(WheelCollider wheelCollider, Transform wheelTransform)
    {
        Vector3 pos;
        Quaternion rot;
        wheelCollider.GetWorldPose(out pos, out rot);
        wheelTransform.rotation = rot;
        wheelTransform.position = pos;
    }
}
