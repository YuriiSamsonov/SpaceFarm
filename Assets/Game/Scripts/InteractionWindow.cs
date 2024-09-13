using DG.Tweening;
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
            _isOpened = true;

            _anchorPosition = anchorPosition;
            
            _canvasGroup.DOFade(1, 0.1f).OnComplete(() =>
            {
                _canvasGroup.interactable = true;
                _canvasGroup.blocksRaycasts = true;
                
                _windowHolder = windowHolder;
            });
        }
        
        private void Hide()
        {
            _canvasGroup.DOFade(0, 0.1f).OnComplete(() =>
            {
                _canvasGroup.interactable = false;
                _canvasGroup.blocksRaycasts = false;
                _windowHolder?.OnWindowHide();
                _isOpened = false;
                _windowHolder = null;
            });
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