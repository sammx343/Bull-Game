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
  private Rigidbody _rigidbody;
  private Animator _animator;
  private bool _shouldBullStop;
  private NavMeshAgent _agent;
  private BullPhysicsController _bullPhysicsController;
  private TopDownCarController _topDownCarController;
  private float angle;
  private Vector3 input;
  public float turnFactor = 5;
  public float rotationSpeed = 5;

  public AnimationCurve animationCurve;
  public AnimationCurve rotationCurve;

  private void Awake()
  {
    angle = 0;
    input = Vector3.zero;
    _shouldBullStop = false;
    _ballIsRunning = false;
    _animator = GetComponent<Animator>();
    _topDownCarController = GetComponent<TopDownCarController>();
    _bullPhysicsController = GetComponent<BullPhysicsController>();
    _rigidbody = GetComponent<Rigidbody>();
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

      Vector3 direction = _ball.transform.position - transform.position;
      angle = Mathf.LerpAngle(angle, Vector3.SignedAngle(direction.normalized, transform.forward, Vector3.up), Time.deltaTime * rotationSpeed)%360; 
      input.x = (-angle/360) * turnFactor; 
      input.z = 1 * (180 - angle)/ 180;
      _topDownCarController.SetInputVector(new Vector2(input.x, input.z));
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
    _ballIsRunning = true;
    _animator.enabled = false;
  }

  private void OnTriggerEnter(Collider other)
  {
  }
}
