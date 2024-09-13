using System;
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
        private InventorySlot _slotPrefab;

        [field: SerializeField] 
        private Transform _inventoryUIParent;
        
        [field: SerializeField]
        private Image _draggedItemPrefab;
        
        [field: SerializeField]
        private Transform _canvasTransform;
        
        [field: SerializeField] 
        private LootingManager _lootingManager;
        
        [field: SerializeField] 
        private ItemData _plantData;

        private List<InventorySlot> _slots = new();
        
        public delegate void ItemHoverEventHandler();
        public event ItemHoverEventHandler OnItemHoverStart;
        public event ItemHoverEventHandler OnItemHoverEnd;
        
        private void Awake()
        {
            _lootingManager.OnMovementComplete += AddItem;           
            
            for (int i = 0; i < _inventorySize; i++)
            {
                var slot = Instantiate(_slotPrefab, _inventoryUIParent);
                slot.Init(_draggedItemPrefab, _canvasTransform, this);
                _slots.Add(slot);
            }
            
            AddItem(_plantData);
        }

        private void AddItem(ItemData data)
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

        private void OnDestroy()
        {
            _lootingManager.OnMovementComplete -= AddItem;
        }
    }
}