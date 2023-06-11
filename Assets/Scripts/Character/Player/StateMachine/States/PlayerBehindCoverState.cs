using DG.Tweening;
using UnityEngine;

namespace Character.Player.StateMachine.States
{
    public class PlayerBehindCoverState : PlayerState
    {
        public PlayerBehindCoverState(PlayerCharacter stateOwner, PlayerStateMachine stateMachine) : base(stateOwner, stateMachine)
        {
        }

        private Tween _movingToCoverTween;

        public override void OnStateEnter()
        {
            base.OnStateEnter();

            StateOwner.SetZeroVelocity();

            _movingToCoverTween?.Kill();
            _movingToCoverTween = MoveToCover();
        }

        private Tween MoveToCover()
        {
            var playerTransform = StateOwner.transform;
            var playerPosition = playerTransform.position;
            var coverUnexposedPosition = StateOwner.Cover.UnexposedPoint.position;
            var goalPosition = new Vector3(coverUnexposedPosition.x, playerPosition.y, playerPosition.z);
            
            return StateOwner.Rigidbody.DOMove(goalPosition, 0.5f);
        }

        public override void OnEnemyDeath()
        {
            base.OnEnemyDeath();

            if (!StateOwner.Cover.Chunk.HasEnemies)
            {
                PlayerStateMachine.SetState(PlayerStateMachine.PlayerMovingState);
            }
        }

        public override void OnFireInput()
        {
            base.OnFireInput();
            StateOwner.FireSelectedWeapon();
        }
        
        public override void OnStateExit()
        {
            base.OnStateExit();
            _movingToCoverTween?.Kill();
            StateOwner.Cover.Free();
        }
    }
}