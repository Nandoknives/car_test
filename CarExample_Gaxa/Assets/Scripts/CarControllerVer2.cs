using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public enum Axel
{
    Front, 
    Rear
}

[Serializable]
public struct Wheel
{
    public GameObject model;
    public WheelCollider collider;
    public Axel axel;
}

public class CarControllerVer2 : MonoBehaviour
{
    [SerializeField]
    private float acceleration;
    [SerializeField]
    private float turnSensitivity = 1f;
    [SerializeField]
    private float maxSteerangle = 40f;
    [SerializeField]
    private List<Wheel> wheels;
    private float inicialAcceleration;

    public float breakingForce = 3000f;
    [SerializeField]
    private float presentBrakeForce = 0f;

    private float inputX, inputY;

    public Rigidbody rb;
    private float currentSpeed;
    public float maximumSpeed = 140f;
    private float startTime;
    public bool isBreaking;

    void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        isBreaking = false;
        inicialAcceleration = acceleration;
    }
    private void Update()
    {
        AnimateWheels();
        GetInputs();
        currentSpeed = rb.velocity.sqrMagnitude;
        //Debug.Log(currentSpeed);
        if(currentSpeed >=maximumSpeed)
        {
            Debug.Log("Maximum Speed");
            acceleration = currentSpeed * 0.05f; ;
        }
        if (Input.GetKeyDown(KeyCode.W))
        {         
            startTime = Time.time;     
        }
        if (Input.GetKey("w") && Time.time - startTime< 3f)
        {
          Debug.Log((Time.time - startTime).ToString("00:00.00"));
        }
        if (Input.GetKey("w") && Time.time - startTime > 3f)
        {
            acceleration = 30f;
            Debug.Log("Maximum Speed");
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            ApplyBrakes();
        }
        if (Input.GetKeyUp(KeyCode.S))
        {
            isBreaking = false;
        }
    }

    public void ApplyBrakes()
    {
        StartCoroutine(carBrakes());
        isBreaking = true;
    }

    IEnumerator carBrakes()
    {
        presentBrakeForce = breakingForce;
        Debug.Log(currentSpeed);
        foreach (var wheel in wheels)
        {
            wheel.collider.brakeTorque =   presentBrakeForce;
        }
        yield return new WaitForSeconds(0.5f);
        presentBrakeForce = 0f;
        foreach (var wheel in wheels)
        {
            wheel.collider.brakeTorque = presentBrakeForce;
        }
    }

    private void AnimateWheels()
    {
        foreach (var wheel in wheels)
        {
            Quaternion _rot;
            Vector3 _pos;
            wheel.collider.GetWorldPose(out _pos, out _rot);
            wheel.model.transform.position = _pos;
            wheel.model.transform.rotation = _rot;
        }
    }

    private void LateUpdate()
    {       
        Turn();
        Move();
    }

    private void GetInputs()
    {
        inputX = Input.GetAxis("Horizontal");       
        inputY = Input.GetAxis("Vertical");
    }

    private void Move()
    {
        foreach (var wheel in wheels)
        {
            if (currentSpeed >= maximumSpeed)
            {
                acceleration = currentSpeed * 0.05f;
                wheel.collider.motorTorque = inputY * acceleration * 500 * Time.deltaTime;               
            }
            else
            {
                acceleration = inicialAcceleration;
                wheel.collider.motorTorque = inputY * acceleration * 500 * Time.deltaTime;
                
            }
        }
    }

    private void Turn()
    {
        foreach(var wheel in wheels)
        {
            if(wheel.axel==Axel.Front)
            {
                var _steerAngle = inputX * turnSensitivity * maxSteerangle;
                wheel.collider.steerAngle = Mathf.Lerp(wheel.collider.steerAngle, _steerAngle, 0.5f);
            }
        }
    }

}
