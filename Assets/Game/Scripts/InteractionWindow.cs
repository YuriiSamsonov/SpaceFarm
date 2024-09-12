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
        
        private Vector3 _anchorPosition;
        private Camera _mainCamera;
        private IWindowHolder _windowHolder;
        
        private bool _isOpened;

        private void Awake()
        {
            _mainCamera = Camera.main;
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
        }
        
        private void Hide()
        {
            _canvasGroup.alpha = 0f;
            _isOpened = false;
        }
        
        private void Update()
        {
            if (!_isOpened)
            {
                return;
            }
            
            var screenPos = _mainCamera.WorldToScreenPoint(_anchorPosition);
            
            _windowRectTransform.position = screenPos;
        }
    }
}