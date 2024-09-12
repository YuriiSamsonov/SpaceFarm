using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts
{
    public class Inventory : MonoBehaviour
    {
        [field: SerializeField] 
        private int _inventorySize = 6;

        [field: SerializeField]
        private GameObject _slotPrefab;

        [field: SerializeField] 
        private Transform _inventoryUIParent;
        
        [field: SerializeField]
        private Image _draggedItemPrefab;
        
        [field: SerializeField]
        private Transform _canvasTransform;
        
        [field: SerializeField] 
        private PlayerController _player;
        
        private List<InventorySlot> _slots = new();
        
        public delegate void ItemHoverEventHandler();
        public event ItemHoverEventHandler OnItemHoverStart;
        public event ItemHoverEventHandler OnItemHoverEnd;
        
        private void Awake()
        {
            for (int i = 0; i < _inventorySize; i++)
            {
                var slot = Instantiate(_slotPrefab, _inventoryUIParent);
                var inventorySlot = slot.GetComponent<InventorySlot>();
                inventorySlot.Init(_draggedItemPrefab, _canvasTransform, this);
                _slots.Add(inventorySlot);
            }
        }

        public void AddItem(ItemData data)
        {
            foreach (var slot in _slots)
            {
                if (slot.HasItem && slot.ItemData.ID == data.ID)
                {
                    slot.AddToStack();
                    return;
                }
            }
            
            foreach (var slot in _slots)
            {
                if (!slot.HasItem)
                {
                    slot.SetItem(data);
                    return;
                }
            }

            Debug.LogWarning("Inventory is full!");
        }

        public void RemoveItem(ItemData data)
        {
            foreach (var slot in _slots)
            {
                if (slot.HasItem && slot.ItemData.ID == data.ID)
                {
                    slot.RemoveFromStack();
                    return;
                }
            }

            Debug.LogWarning("Item not found in inventory!");
        }

        public void StartHoverItem(ItemData data)
        {
            if (data.Type == ItemType.Plant)
            {
                OnItemHoverStart?.Invoke();
            }
        }

        public void EndHoverItem()
        {
            OnItemHoverEnd?.Invoke();
        }
    }
}