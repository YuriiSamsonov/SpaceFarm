using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game.Scripts
{
    public class InventorySlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public ItemData ItemData;
        public bool HasItem => ItemData != null;

        [field: SerializeField] 
        private Image _itemImage;
        
        [field: SerializeField] 
        private TMP_Text _stackText;
        
        [field: SerializeField] 
        private PlayerController _player;
        
        private Image _draggedItemPrefab;
        private Image _draggedItem;
        private Transform _canvasTransform;
        private Inventory _inventory;
        private Camera _mainCamera;
        
        private Sequence _pulseSequence;
        
        private int _stackSize;

        private void Awake()
        {
            _mainCamera = Camera.main;
        }

        public void Init(Image imagePrefab, Transform canvasTransform, Inventory inventory)
        {
            _draggedItemPrefab = imagePrefab;
            _canvasTransform = canvasTransform;
            _inventory = inventory;
        }

        public void SetItem(ItemData data)
        {
            ItemData = data;
            _itemImage.sprite = data.ItemSprite;
            _stackSize = 1;
            UpdateUI();
            PlayPulseAnimation();
        }
        
        public void AddToStack()
        {
            _stackSize++;
            UpdateUI();
            PlayPulseAnimation();
        }

        public void RemoveFromStack()
        {
            if (_stackSize > 1)
            {
                _stackSize--;
            }
            else
            {
                ClearSlot();
            }
            UpdateUI();
        }

        public void ClearSlot()
        {
            ItemData = null;
            _stackSize = 0;
            _itemImage.sprite = null;
            UpdateUI();
        }

        private void UpdateUI()
        {
            if (HasItem)
            {
                _itemImage.color = new Color(_itemImage.color.r, _itemImage.color.g, _itemImage.color.b, 1);
                _stackText.text = _stackSize > 1 ? _stackSize.ToString() : string.Empty;
            }
            else
            {
                _itemImage.color = new Color(_itemImage.color.r, _itemImage.color.g, _itemImage.color.b, 0);
                _stackText.text = string.Empty;
            }
        }
        
        private void PlayPulseAnimation()
        {
            _pulseSequence?.Kill();
            
            _pulseSequence = DOTween.Sequence()
                .Append(_itemImage.transform.DOScale(Vector3.one * 1.2f, 0.2f))
                .Append(_itemImage.transform.DOScale(Vector3.one, 0.2f));
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (ItemData == null)
            {
                return;
            }
            
            _draggedItem = Instantiate(_draggedItemPrefab, _canvasTransform);
            _draggedItem.sprite = _itemImage.sprite;
            _draggedItem.gameObject.SetActive(true);

            _inventory.StartHoverItem(ItemData);
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (_draggedItem != null)
            {
                _draggedItem.rectTransform.position = eventData.position;
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (_draggedItem != null)
            {
                _inventory.EndHoverItem();
                Destroy(_draggedItem.gameObject);
            }

            var ray = _mainCamera.ScreenPointToRay(eventData.position);
            
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                var pot = hit.collider.GetComponent<Pot>();
                
                if (pot != null)
                {
                    if (pot.TryToPlantSeed(ItemData))
                    {
                        _inventory.RemoveItem(ItemData);
                    }
                }
                else
                {
                    Debug.Log("Not over a pot.");
                }
            }
        }
    }
}