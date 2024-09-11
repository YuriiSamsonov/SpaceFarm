using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.EnhancedTouch;
using ETouch = UnityEngine.InputSystem.EnhancedTouch;

namespace Game.Scripts
{
    public class PlayerController : MonoBehaviour
    {
        [field: SerializeField] 
        private Vector3 _joystickSize = new(300, 300);

        [field: SerializeField] 
        private TouchscreenJoystick _joystick;

        [field: SerializeField] 
        private CharacterController _playerController;
            
        [field: SerializeField] 
        private float _speed = 6f;

        [field: SerializeField] 
        private Animator _animator;
        
        [field: SerializeField]
        private EventSystem _eventSystem;
        
        [field: SerializeField] 
        private ItemData _plant;

        private Finger _finger;
        private Vector2 _movementAmount;
        private int _walkAnimationHash = Animator.StringToHash("Walk");

        private void OnEnable()
        {
            EnhancedTouchSupport.Enable();
            ETouch.Touch.onFingerDown += OnFingerDown;
            ETouch.Touch.onFingerUp += OnFingerUp;
            ETouch.Touch.onFingerMove += OnFingerMove;
        }

        private void OnDisable()
        {
            EnhancedTouchSupport.Disable();
            ETouch.Touch.onFingerDown -= OnFingerDown;
            ETouch.Touch.onFingerUp -= OnFingerUp;
            ETouch.Touch.onFingerMove -= OnFingerMove;
        }

        private void OnFingerMove(Finger finger)
        {
            if (finger == _finger)
            {
                Vector2 knobPosition;
                float maxMovement = _joystickSize.x / 2f;
                ETouch.Touch currentTouch = finger.currentTouch;

                if (Vector2.Distance(currentTouch.screenPosition, _joystick.RectTransform.anchoredPosition) > maxMovement)
                {
                    knobPosition = (currentTouch.screenPosition - _joystick.RectTransform.anchoredPosition).normalized * maxMovement;
                }
                else
                {
                    knobPosition = currentTouch.screenPosition - _joystick.RectTransform.anchoredPosition;
                }

                _joystick.CircleRectTransform.anchoredPosition = knobPosition;
                _movementAmount = knobPosition / maxMovement;
            }
        }

        private void OnFingerUp(Finger finger)
        {
            if (finger == _finger)
            {
                _finger = null;
                _joystick.CircleRectTransform.anchoredPosition = Vector2.zero;
                _joystick.gameObject.SetActive(false);
                _movementAmount = Vector2.zero;
            }
        }

        private void OnFingerDown(Finger finger)
        {
            if (IsTouchOnCanvasGroup(finger))
            {
                return;
            }
            
            if (_finger == null && finger.screenPosition.x <= Screen.width / 2f)
            {
                _finger = finger;
                _movementAmount = Vector2.zero;
                _joystick.gameObject.SetActive(true);
                _joystick.RectTransform.sizeDelta = _joystickSize;
                _joystick.RectTransform.anchoredPosition = ClampStartPosition(finger.screenPosition);
            }
        }
        
        private bool IsTouchOnCanvasGroup(Finger finger)
        {
            Vector2 screenPosition = finger.currentTouch.screenPosition;

            var pointerEventData = new PointerEventData(_eventSystem)
            {
                position = screenPosition
            };

            var raycastResults = new List<RaycastResult>();
            
            _eventSystem.RaycastAll(pointerEventData, raycastResults);

            if (raycastResults.Count > 0)
            {
                return true;
            }

            return false;
        }


        private Vector2 ClampStartPosition(Vector2 startPos)
        {
            if (startPos.x < _joystickSize.x / 2)
            {
                startPos.x = _joystickSize.x / 2;
            }

            if (startPos.y < _joystickSize.y / 2)
            {
                startPos.y = _joystickSize.y / 2;
            }
            else if (startPos.y > Screen.height - _joystickSize.y / 2)
            {
                startPos.y = Screen.height - _joystickSize.y / 2;
            }

            return startPos;
        }
        
        private void Update()
        {
            if (_movementAmount != Vector2.zero)
            {
                Vector3 scaledMovement = _speed * Time.deltaTime * new Vector3(
                    _movementAmount.x,
                    0,
                    _movementAmount.y
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
            _animator.SetBool(_walkAnimationHash, _movementAmount != Vector2.zero);
        }
    }
}