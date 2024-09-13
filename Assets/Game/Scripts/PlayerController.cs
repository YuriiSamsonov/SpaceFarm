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
        private float _gravity = -2f;
        
        [field: SerializeField] 
        private Animator _animator;

        [field: SerializeField] 
        private InputManager _inputManager;
        
        private static readonly int WalkHash = Animator.StringToHash("Walk");
        
        private Vector3 _velocity;

        private void Update()
        {
            if (_playerController.isGrounded && _velocity.y < 0)
            {
                _velocity.y = _gravity;
            }

            if (_inputManager.MovementAmount != Vector2.zero)
            {
                var scaledMovement = _speed * Time.deltaTime * new Vector3(
                    _inputManager.MovementAmount.x,
                    0,
                    _inputManager.MovementAmount.y
                );

                _playerController.transform.LookAt(_playerController.transform.position + scaledMovement, Vector3.up);
                _playerController.Move(scaledMovement);
            }
            
            _velocity.y += _gravity * Time.deltaTime; 
            _playerController.Move(_velocity * Time.deltaTime);

            ResolveAnimation();
        }

        private void ResolveAnimation()
        {
            _animator.SetBool(WalkHash, _inputManager.MovementAmount != Vector2.zero);
        }
    }
}