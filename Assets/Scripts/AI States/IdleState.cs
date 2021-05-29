using System;
using System.Collections.Generic;
using UnityEngine;

namespace AI_States
{
    public class IdleState : IState
    {
        private float _timer;
        private float _maxTime = 0.1f;
    
        public void OnEnter()
        {
            _timer = 0;
        }

        public void OnExit()
        {
        }

        public IState WatchTransitions(Dictionary<Type, IState> states)
        {
            if (_timer >= _maxTime)
            {
                return states[typeof(FleeState)];
            }

            return this;
        }

        public void Action()
        {
            _timer += Time.deltaTime;
        }
    }
}
