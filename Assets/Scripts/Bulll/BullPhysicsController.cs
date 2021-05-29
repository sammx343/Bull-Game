using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BullPhysicsController : MonoBehaviour
{
      [Header("Car settings")]
    public float driftFactor = 0.95f;
    public float accelerationFactor = 30.0f;
    public float turnFactor = 3.5f;
    public float maxSpeed = 20;

    //Local variables
    public float accelerationInput = 0;
    public float steeringInput = 0;

    public float rotationAngle = 0;

    public float velocityVsUp = 0;

    [Header("Factors")] 
    public Vector3 forwardVelocity;
    public Vector3 rightVelocity;

    //Components
    Rigidbody _rigidbody;

    //Awake is called when the script instance is being loaded.
    void Awake()
    {
      _rigidbody = GetComponent<Rigidbody>();
    }
    
    private void FixedUpdate()
    {
      ApplyEngineForce();
      KillOrthogonalVelocity();
      ApplySteering();
    }

    private void ApplyEngineForce()
    {
      //Apply drag if there is no accelerationInput so the car stops when the player lets go of the accelerator
      if (accelerationInput == 0)
        _rigidbody.drag = Mathf.Lerp(_rigidbody.drag, 3.0f, Time.fixedDeltaTime * 3);
      else _rigidbody.drag = 0;
      
      Vector3 engineForceVector = transform.forward * (accelerationInput * accelerationFactor);
      _rigidbody.AddForce(engineForceVector, ForceMode.Force);
    }

    private void ApplySteering()
    {
      rotationAngle -= steeringInput * turnFactor;
      _rigidbody.MoveRotation(Quaternion.Euler(0,rotationAngle,0));
    }

    void KillOrthogonalVelocity()
    {
      forwardVelocity = transform.forward * Vector3.Dot(_rigidbody.velocity, transform.forward);
      rightVelocity = transform.right * Vector3.Dot(_rigidbody.velocity, transform.right);
      
      
      _rigidbody.velocity = forwardVelocity + rightVelocity * driftFactor;
    }

    public void SetInputVector(Vector3 inputVector)
    {
      steeringInput = inputVector.x;
      accelerationInput = inputVector.z;
    }
}
