using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BullController : MonoBehaviour
{
  // Start is called before the first frame update
  [SerializeField]
  private GameObject _ball;
  [SerializeField] private bool _ballIsRunning;

  [SerializeField]
  private float bullRotationSpeed = 1f;
  [SerializeField]
  private float bullMovementSpeed = 10f;
  private CharacterController _characterController;
  private Rigidbody _rigidbody;
  private Animator _animator;
  private bool _shouldBullStop;
  private NavMeshAgent _agent;
  private BullPhysicsController _bullPhysicsController;

  public AnimationCurve animationCurve;
  public AnimationCurve rotationCurve;

  private void Awake()
  {
    _shouldBullStop = false;
    _ballIsRunning = false;
    _characterController = GetComponent<CharacterController>();
    _animator = GetComponent<Animator>();
    // _agent = GetComponent<NavMeshAgent>();
    _bullPhysicsController = GetComponent<BullPhysicsController>();
    _rigidbody = GetComponent<Rigidbody>();
    // _agent.updatePosition = false;
  }

  // Update is called once per frame
  void Update()
  {
    // if (_ballIsRunning && !_shouldBullStop)
    // {
    //   var currentPosition = transform.position;
    //   Vector3 targetDir = currentPosition - _ball.transform.position;
    //   targetDir.y = 0;
    //
    //   // float angle = Vector3.Angle(targetDir.normalized, transform.forward.normalized);
    //   
    //   // Debug.Log(angle);
    //
    //   Quaternion lookRotation = Quaternion.LookRotation(targetDir);
    //
    //   float value = Mathf.Clamp(targetDir.magnitude, 1, 10);
    //   float valueAnimation = animationCurve.Evaluate((10 - value) / 10);
    //
    //   float valueRotationCurve = rotationCurve.Evaluate(value / 10);
    //
    //   transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * bullRotationSpeed * valueRotationCurve);
    //
    //   _characterController.Move(transform.forward.normalized * (-1 * (Time.deltaTime * bullMovementSpeed) * valueAnimation));
    // }
    
    if (_ballIsRunning && !_shouldBullStop)
    {
      // _agent.SetDestination(new Vector3(_ball.transform.position.x, 0 , _ball.transform.position.z));
      
      Vector3 desiredVelocity = new Vector3(-Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
      Debug.Log(desiredVelocity);
      _bullPhysicsController.SetInputVector(desiredVelocity);
      _agent.nextPosition = _rigidbody.transform.position;
      
      // Debug.Log("Bull desired velocity " + desiredVelocity);
      // // _bullPhysicsController.SetInputVector(new Vector2(desiredVelocity.x, desiredVelocity.z));
      //
      // Vector3 nextPosition = desiredVelocity * (Time.deltaTime * 10);
      // _rigidbody.MovePosition(transform.position + nextPosition);
      // // transform.LookAt( _ball.transform.position);
    }
  }

  public void FollowBall(GameObject ball)
  {
    _ball = ball;
  }

  public void BullStopsChasing()
  {
    _shouldBullStop = true;
  }

  void StartChasingBall()
  {
    _animator.applyRootMotion = true;
    _ballIsRunning = true;
  }

  private void OnTriggerEnter(Collider other)
  {
  }
}
