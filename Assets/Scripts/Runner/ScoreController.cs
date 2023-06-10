using Events;
using ReferenceVariables;
using UnityEngine;
using UnityEngine.Events;

namespace Runner
{
    public class ScoreController : GameEventListener
    {
        [SerializeField] private IntVariable _startScoreVariable;
        [SerializeField] private IntVariable _scoreVariable;
        [SerializeField] private UnityEvent _scoreUpdated;

        private void Awake()
        {
            ResetToDefault();
        }

        private void ResetToDefault()
        {
            _scoreVariable.Value = _startScoreVariable.Value;
            _scoreUpdated.Invoke();
        }

        public void IncrementScore()
        {
            _scoreVariable.Value++;
            _scoreUpdated?.Invoke();
        }
    }
}