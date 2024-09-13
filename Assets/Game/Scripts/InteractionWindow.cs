using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.Scripts
{
    public class InteractionWindow : MonoBehaviour, IPointerClickHandler
    {
        [field: SerializeField] 
        private CanvasGroup _canvasGroup;
        
        [field: SerializeField] 
        private RectTransform _windowRectTransform;

        [field: SerializeField] 
        private InputManager _inputManager;
        
        private Vector3 _anchorPosition;
        private Camera _mainCamera;
        private IWindowHolder _windowHolder;
        
        private bool _isOpened;

        private void Awake()
        {
            _mainCamera = Camera.main;
            Hide();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (!_isOpened)
            {
                return;
            }
            
            _windowHolder.OnWindowClick();
            
            Hide();
        }
        
        public void Show(IWindowHolder windowHolder, Vector3 anchorPosition)
        {
            _windowHolder = windowHolder;
            
            _isOpened = true;

            _anchorPosition = anchorPosition;

            _canvasGroup.alpha = 1f;
            _canvasGroup.interactable = true;
            _canvasGroup.blocksRaycasts = true;
        }
        
        private void Hide()
        {
            _windowHolder?.OnWindowHide();
            _canvasGroup.alpha = 0f;
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;
            _isOpened = false;
            _windowHolder = null;
        }
        
        private void Update()
        {
            if (!_isOpened)
            {
                return;
            }

            if (_inputManager.MovementAmount != Vector2.zero)
            {
                Hide();
            }
            
            var screenPos = _mainCamera.WorldToScreenPoint(_anchorPosition);
            
            _windowRectTransform.position = screenPos;
        }
    }
}