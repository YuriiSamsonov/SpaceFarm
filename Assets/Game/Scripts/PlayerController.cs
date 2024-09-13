using UnityEngine;

namespace Game.Scripts
{
    public class PlayerController : MonoBehaviour
    {
        [field: SerializeField] 
        private CharacterController _playerController;
            
        [field: SerializeField] 
        private float _speed = 6f;

        [field: SerializeField] 
        private Animator _animator;

        [field: SerializeField] 
        private InputManager _inputManager;
        
        private static readonly int WalkHash = Animator.StringToHash("Walk");

        private void Update()
        {
            if (_inputManager.MovementAmount != Vector2.zero)
            {
                Vector3 scaledMovement = _speed * Time.deltaTime * new Vector3(
                    _inputManager.MovementAmount.x,
                    0,
                    _inputManager.MovementAmount.y
                );

                _playerController.transform.LookAt(_playerController.transform.position + scaledMovement, Vector3.up);

                _playerController.Move(scaledMovement);
            }
            
            ResolveAnimation();
        }

        private void ResolveAnimation()
        {
            _animator.SetBool(WalkHash, _inputManager.MovementAmount != Vector2.zero);
        }
    }
}