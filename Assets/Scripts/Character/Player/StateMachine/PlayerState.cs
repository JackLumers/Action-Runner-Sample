using StateMachine;
using UnityEngine;

namespace Character.Player.StateMachine
{
    public abstract class PlayerState : State<PlayerCharacter>
    {
        protected PlayerStateMachine PlayerStateMachine;

        protected PlayerState(PlayerCharacter stateOwner, PlayerStateMachine stateMachine) : base(stateOwner)
        {
            PlayerStateMachine = stateMachine;
        }

        public virtual void OnFireInput() { }
        
        public virtual void OnTriggerEnter(Collider other) { }
        
        public virtual void OnEnemyDeath(){ }
    }
}