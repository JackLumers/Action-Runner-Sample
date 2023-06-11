using JetBrains.Annotations;
using UnityEngine;

namespace StateMachine
{
    public class StateMachine<T> where T: Component
    {
        protected T Owner;

        [CanBeNull]
        public State<T> CurrentState { get; private set; }
        
        public StateMachine(T owner)
        {
            Owner = owner;
        }
        
        public void SetState(State<T> newState)
        {
            if(CurrentState == newState)
                return;
            
            if(newState == null)
                return;
            
            CurrentState?.OnStateExit();

            CurrentState = newState;
            CurrentState.OnStateEnter();
        }

        public void OnFixedUpdate()
        {
            CurrentState?.OnFixedUpdate();
        }
        
        public void OnUpdate()
        {
            CurrentState?.OnUpdate();
        }
    }
}