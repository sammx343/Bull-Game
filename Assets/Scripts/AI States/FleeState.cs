using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using Random = UnityEngine.Random;

namespace AI_States
{
    public class FleeState : IState
    {
        private Vector3 _randomPosition;
        private readonly NavMeshAgent _agent;
        private readonly PlayerBall _playerBall;
        private readonly CharacterController _characterController;
        private readonly UnityAction<Vector3> _onPlayerMovementInput;
        private const float AcceptableDistance = 0.5f;
        private NavMeshHit hit;
        private PlayerDead _playerDead;
        private PlayerAiInput _playerAiInput;
        
        public FleeState(NavMeshAgent agent, CharacterController characterController, UnityAction<Vector3> onPlayerMovementInput, PlayerBall playerBall, PlayerAiInput playerAiInput)
        {
            _agent = agent;
            _characterController = characterController;
            _onPlayerMovementInput = onPlayerMovementInput;
            _playerBall = playerBall;
            _agent.updatePosition = false;
            _playerAiInput = playerAiInput;
        }

        public void OnEnter()
        {
            int steps = 0;
            do
            {
                _randomPosition = Random.insideUnitSphere * 20;
                steps++;
            } 
            while (!IsNewPositionValid() &&  steps < 20);
            
            _agent.SetDestination(_characterController.transform.position +  new Vector3(_randomPosition.x, 0, _randomPosition.z));
        }

        private bool IsNewPositionValid()
        {
             return NavMesh.SamplePosition(
                _characterController.transform.position + new Vector3(_randomPosition.x, 0, _randomPosition.z),
                out hit, 1, NavMesh.AllAreas);
        }

        public void OnExit()
        {
        }

        public IState WatchTransitions(Dictionary<Type, IState> states)
        {
            if (_agent.remainingDistance < AcceptableDistance)
            {
                return states[typeof(IdleState)];
            }
            return this;
        }
        
        public void Action()
        {
            Move();
        }

        private void Move()
        {
            _playerAiInput.OnPlayerMovementInput?.Invoke(_agent.desiredVelocity);
            _agent.nextPosition = _characterController.transform.position;
        }

    }
}
