using Character.Player.StateMachine.States;
using StateMachine;
using UnityEngine;

namespace Character.Player.StateMachine
{
    public class PlayerStateMachine : StateMachine<PlayerCharacter>
    {
        public PlayerMovingState PlayerMovingState { get; }
        public PlayerBehindCoverState PlayerBehindCoverState { get; }
        
        public PlayerStateMachine(PlayerCharacter owner) : base(owner)
        {
            PlayerMovingState = new PlayerMovingState(owner, this);
            PlayerBehindCoverState = new PlayerBehindCoverState(owner, this);
        }

        public void OnFireInput()
        {
            (CurrentState as PlayerState)?.OnFireInput();
        }

        public void OnTriggerEnter(Collider other)
        {
            (CurrentState as PlayerState)?.OnTriggerEnter(other);
        }

        public void OnEnemyDeath()
        {
            (CurrentState as PlayerState)?.OnEnemyDeath();
        }
    }
}