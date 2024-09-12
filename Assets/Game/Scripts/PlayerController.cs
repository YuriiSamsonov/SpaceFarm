using UnityEngine;
using UnityEngine.EventSystems;

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
        private ItemData _plant;
        
        [field: SerializeField] 
        private InputManager _inputManager;
        
        private int _walkAnimationHash = Animator.StringToHash("Walk");

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

            if (Input.GetKeyDown(KeyCode.P))
            {
                FindObjectOfType<Inventory>().AddItem(_plant);
            }
        }

        private void ResolveAnimation()
        {
            _animator.SetBool(_walkAnimationHash, _inputManager.MovementAmount != Vector2.zero);
        }
    }
}