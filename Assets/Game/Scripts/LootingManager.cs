using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Game.Scripts
{
    public class LootingManager : MonoBehaviour
    {
        [field: SerializeField] 
        private RectTransform _canvasRectTransform;
        
        [field: SerializeField] 
        private Image _draggedItemPrefab;

        [field: SerializeField, Tooltip("Speed of movement form start point to player")] 
        private float _movementSpeed = 0.5f;

        [field: SerializeField, Tooltip("Step between spawn of the item")]
        private float _sequenceStep = 0.2f; 
        
        [field: SerializeField, Tooltip("Min spread of object's translation")] 
        private float _minSpread = 150f;
        
        [field: SerializeField, Tooltip("Max spread of object's translation")] 
        private float _maxSpread = 200f;

        private readonly Vector2 _targetPosition = new (640, 360);
        private Camera _mainCamera;
        
        public event Action OnMovementComplete;
        
        private void Awake()
        {
            _mainCamera = Camera.main;
        }

        public void StartMovement(List<ItemData> itemsToReturn)
        {
            float delay = 0f;

            foreach (var itemContainer in itemsToReturn)
            {
                DOVirtual.DelayedCall(delay, () =>
                {
                    var screenPoint = _mainCamera.WorldToScreenPoint(transform.position);

                    var itemReceivingObject = Instantiate(_draggedItemPrefab, screenPoint, quaternion.identity, _canvasRectTransform);
                        
                    itemReceivingObject.sprite = itemContainer.ItemSprite;

                    Vector2[] path = GenerateRandomPath(screenPoint, _targetPosition);

                    itemReceivingObject.transform
                        .DOPath(path.Select(p => (Vector3) p).ToArray(), _movementSpeed, PathType.CatmullRom).OnComplete(
                            () =>
                            {
                                DestroyImmediate(itemReceivingObject.gameObject);
                                OnMovementComplete?.Invoke();
                            });
                });
                
                delay += _sequenceStep;
            }
        }
        
        private Vector2[] GenerateRandomPath(Vector2 startPos, Vector2 targetPos)
        {
            float randomX = Random.Range(_minSpread, _maxSpread);
            float randomY = Random.Range(_minSpread, _maxSpread);
            
            randomX *= Random.value > 0.5f ? 1 : -1;
            randomY *= Random.value > 0.5f ? 1 : -1;
            
            Vector2 controlPoint1 = startPos + new Vector2(randomX, randomY);
        
            return new [] { controlPoint1, targetPos };
        }
    }
}