using UnityEngine;

namespace Character.Player
{
    [RequireComponent(typeof(PlayerCharacter))]
    public class PlayerCharacterInput : MonoBehaviour
    {
        [SerializeField] private float _maxShotDistance = 100f;
        [SerializeField] private LayerMask _canFireLayer;
        [Space] [SerializeField] private bool _debugMode;
        
        private Camera _playerViewCamera;
        private PlayerCharacter _playerCharacter;
        private PlayerInputActions _playerInputActions;
        
        private GameObject _debugPointerObject;
        
        private void Awake()
        {
            // Better to get it from some Camera manager but will do the work
            _playerViewCamera = Camera.main;

            _playerCharacter = GetComponent<PlayerCharacter>();
            _playerInputActions = new PlayerInputActions();
        }

        private void OnEnable()
        {
            _playerInputActions.Enable();
        }

        private void FixedUpdate()
        {
            var fireActionValue = _playerInputActions.RunnerActionMap.Fire.ReadValue<float>();
            
            if (fireActionValue > 0)
            {
                var screenPoint = _playerInputActions.RunnerActionMap.Look.ReadValue<Vector2>();
                var rayFromScreenPoint = _playerViewCamera.ScreenPointToRay(screenPoint);
                
                if (Physics.Raycast(rayFromScreenPoint, out var hitInfo, _maxShotDistance, _canFireLayer))
                {
                    var hitPoint = hitInfo.point;
                    
                    SetDebugPointer(hitPoint, _debugMode);

                    _playerCharacter.LookingPoint = hitPoint;
                    _playerCharacter.FireSelectedWeapon();
                }
            }
        }

        private void OnDisable()
        {
            _playerInputActions.Disable();
        }

        private void OnDestroy()
        {
            _playerInputActions?.Dispose();
        }
        
        private void SetDebugPointer(Vector3 position, bool show)
        {
            if (show)
            {
                if (ReferenceEquals(_debugPointerObject, null))
                {
                    _debugPointerObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    _debugPointerObject.GetComponent<MeshRenderer>().material.color = Color.red;
                    _debugPointerObject.GetComponent<Collider>().enabled = false;
                    _debugPointerObject.transform.parent = null;
                    _debugPointerObject.name = $"{name}_DebugPointerObject";
                }
                else
                {
                    _debugPointerObject.gameObject.SetActive(true);
                    _debugPointerObject.transform.position = position;
                }
            }
            else
            {
                if (ReferenceEquals(_debugPointerObject, null)) return;
                
                Destroy(_debugPointerObject);
                _debugPointerObject = null;
            }
        }
    }
}