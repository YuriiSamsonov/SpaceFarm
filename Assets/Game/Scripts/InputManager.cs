using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.EnhancedTouch;
using ETouch = UnityEngine.InputSystem.EnhancedTouch;

namespace Game.Scripts
{
    
    public class InputManager : MonoBehaviour
    {
        [field: SerializeField] 
        private Vector3 _joystickSize = new(300, 300);

        [field: SerializeField] 
        private TouchscreenJoystick _joystick;
        
        [field: SerializeField]
        private EventSystem _eventSystem;

        private Camera _mainCamera;
        private Finger _finger;
        public Vector2 MovementAmount { get; private set; }

        private void Awake()
        {
            _mainCamera = Camera.main;
        }

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
                MovementAmount = knobPosition / maxMovement;
            }
        }

        private void OnFingerUp(Finger finger)
        {
            if (finger == _finger)
            {
                _finger = null;
                _joystick.CircleRectTransform.anchoredPosition = Vector2.zero;
                _joystick.gameObject.SetActive(false);
                MovementAmount = Vector2.zero;
            }
        }

        private void OnFingerDown(Finger finger)
        {
            if (IsTouchOnCanvasGroup(finger))
            {
                return;
            }

            if (IsTouchingClickableObject(finger))
            {
                return;
            }
            
            if (_finger == null && finger.screenPosition.x <= Screen.width / 2f)
            {
                _finger = finger;
                MovementAmount = Vector2.zero;
                _joystick.gameObject.SetActive(true);
                _joystick.RectTransform.sizeDelta = _joystickSize;
                _joystick.RectTransform.anchoredPosition = ClampStartPosition(finger.screenPosition);
            }
        }
        
        private bool IsTouchOnCanvasGroup(Finger finger)
        {
            var pointerEventData = new PointerEventData(_eventSystem)
            {
                position = finger.currentTouch.screenPosition
            };

            var raycastResults = new List<RaycastResult>();
            
            _eventSystem.RaycastAll(pointerEventData, raycastResults);

            if (raycastResults.Count > 0)
            {
                return true;
            }

            return false;
        }

        private bool IsTouchingClickableObject(Finger finger)
        {
            var ray = _mainCamera.ScreenPointToRay(finger.currentTouch.screenPosition);
    
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.TryGetComponent<IClickableObject>(out var clickable))
                {
                    clickable.OnClick();
                    return true;
                }
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
    }
}