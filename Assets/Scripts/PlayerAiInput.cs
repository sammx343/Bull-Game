using System;
using System.Collections;
using System.Collections.Generic;
using AI_States;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class PlayerAiInput : MonoBehaviour, IInput
{
    private NavMeshAgent _navMeshAgent;
    private CharacterController _characterController;
    private PlayerBall _playerBall;
    private IState _currentState;
    private IdleState _idleState;
    private FleeState _fleeState;
    private readonly Dictionary<Type, IState> _states = new Dictionary<Type, IState>();

    public UnityAction<Vector3> OnPlayerMovementInput { get; set; }
    public UnityAction<Vector3> OnPlayerClick { get; set; }

    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _characterController = GetComponent<CharacterController>();
        _playerBall = GetComponent<PlayerBall>(); 
        
        _idleState = new IdleState();
        _fleeState = new FleeState(_navMeshAgent, _characterController, OnPlayerMovementInput, _playerBall, this);
        
        _states.Add(typeof(IdleState), _idleState);
        _states.Add(typeof(FleeState), _fleeState);
        
        _currentState = _idleState;
    }

    private void Update()
    {
        IState nextState = _currentState.WatchTransitions(_states);

        if (nextState != _currentState)
        {
            _currentState.OnExit();
            _currentState = nextState;
            nextState.OnEnter();
        }
        else
        {
            _currentState.Action();
        }
    }
}
