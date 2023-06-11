using UnityEngine;

namespace StateMachine
{
    public abstract class State<T> where T: Component
    {
        protected T StateOwner;
        
        protected State(T stateOwner)
        {
            StateOwner = stateOwner;
        }

        public virtual void OnStateEnter() { }
        public virtual void OnStateExit() { }
        
        public virtual void OnFixedUpdate() { }
        public virtual void OnUpdate() { }
    }
}