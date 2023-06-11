using DG.Tweening;
using UnityEngine;

namespace Character.Enemy
{
    public class DummyEnemyAI
    {
        // TODO: Make configurable
        private const float _minHideBehindCoverSeconds = 0.5f;
        private const float _maxHideBehindCoverSeconds = 1f;
        private const float _minExposedSeconds = 0.5f;
        private const float _maxExposedSeconds = 1f;
        
        private readonly DummyEnemy _enemy;
        private readonly Transform _enemyTransform;

        private float _pickedSeconds;
        private float _currentSeconds = 0;
        private bool _isBehindCover;

        private readonly System.Random _random = new();
        
        private Tween _movingTween;

        public DummyEnemyAI(DummyEnemy enemy)
        {
            _enemy = enemy;
            _enemyTransform = enemy.transform;
        }

        public void OnFixedUpdate()
        {
            if (!ReferenceEquals(_enemy.Cover, null))
            {
                switch (_isBehindCover)
                {
                    case true when _currentSeconds > _pickedSeconds:
                        ExitCover();
                        break;
                    case false when _currentSeconds > _pickedSeconds:
                        HideBehindCover();
                        break;
                }

                _currentSeconds += Time.deltaTime;
            }
        }

        private void HideBehindCover()
        {
            _isBehindCover = true;
            _currentSeconds = 0;
            _pickedSeconds = (float) (_random.NextDouble() 
                * (_maxHideBehindCoverSeconds - _minHideBehindCoverSeconds) + _minHideBehindCoverSeconds);
            
            _movingTween?.Kill();
            _movingTween = _enemyTransform.DOMove(_enemy.Cover.UnexposedPoint.position, 0.2f);
        }

        private void ExitCover()
        {
            _isBehindCover = false;
            _currentSeconds = 0;
            _pickedSeconds = (float) (_random.NextDouble() 
                * (_maxExposedSeconds - _minExposedSeconds) + _minExposedSeconds);

            var isLeftPicked = _random.Next(0, 1) > 0;
            var exposedPosition = isLeftPicked
                ? _enemy.Cover.LeftExposedPoint.position
                : _enemy.Cover.RightExposedPoint.position;
            
            _movingTween?.Kill();
            _movingTween = _enemyTransform.DOMove(exposedPosition, 0.2f);
        }
    }
}