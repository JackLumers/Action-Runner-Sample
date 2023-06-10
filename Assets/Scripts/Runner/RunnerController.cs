using Character.Player;
using Cinemachine;
using Runner.ChunkGeneration;
using UnityEngine;

namespace Runner
{
    public class RunnerController : MonoBehaviour
    {
        [SerializeField] private PlayerCharacter _playerCharacterPrefab;
        [SerializeField] private Transform _playerSpawnPoint;

        [SerializeField] private CinemachineVirtualCamera _playerViewCamera;

        [SerializeField] private ChunkManager _chunkManager;
        
        private PlayerCharacter _playerCharacterInstance;
        private Transform _playerCharacterTransform;
        
        public void Awake()
        {
            _playerCharacterInstance = Instantiate(_playerCharacterPrefab, _playerSpawnPoint.position, _playerSpawnPoint.rotation);
            _playerCharacterTransform = _playerCharacterInstance.transform;
            
            _playerViewCamera.Follow = _playerCharacterTransform;
            _playerViewCamera.LookAt = _playerCharacterTransform;
            _playerViewCamera.gameObject.SetActive(true);

            _chunkManager.Init(_playerCharacterTransform);
        }

        public void OnPlayerDied()
        {
            GameOver();
        }

        private void OnRestartClicked()
        {
            _playerCharacterTransform.position = _playerSpawnPoint.position;
            _playerCharacterInstance.gameObject.SetActive(true);
            _playerCharacterInstance.Reinit();
        }

        private void GameOver()
        {
            _playerCharacterInstance.gameObject.SetActive(false);
        }
    }
}