using Covers;
using DG.Tweening;
using UnityEngine;

namespace Character.Player.StateMachine.States
{
    public class PlayerMovingState : PlayerState
    {
        public PlayerMovingState(PlayerCharacter stateOwner, PlayerStateMachine stateMachine) : base(stateOwner, stateMachine)
        {
        }
        
        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();
            StateOwner.MoveSelf(Vector3.right);
        }

        public override void OnFireInput()
        {
            base.OnFireInput();
            StateOwner.FireSelectedWeapon();
        }

        public override void OnTriggerEnter(Collider other)
        {
            base.OnTriggerEnter(other);
            if (other.TryGetComponent<Cover>(out var cover) && cover.Chunk.HasEnemies)
            {
                StateOwner.SetZeroVelocity();
                cover.Occupy(StateOwner);
                PlayerStateMachine.SetState(PlayerStateMachine.PlayerBehindCoverState);
            }
        }
    }
}