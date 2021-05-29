using System;
using System.Collections.Generic;

namespace AI_States
{
    public interface IState
    {
        void OnEnter();
        void OnExit();
        IState WatchTransitions(Dictionary<Type, IState> states);
        void Action();
    }
}
