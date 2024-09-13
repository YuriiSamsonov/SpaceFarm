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
        private PlayerController _playerController;
        
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
        
        private Camera _mainCamera;
             
        public event Action<ItemData> OnMovementComplete;
        
        private void Awake()
        {
            _mainCamera = Camera.main;
        }

        public void StartMovement(List<ItemData> itemsToReturn, Vector3 startPos)
        {
            float delay = 0f;

            foreach (var itemContainer in itemsToReturn)
            {
                DOVirtual.DelayedCall(delay, () =>
                {
                    var startScreenPoint = _mainCamera.WorldToScreenPoint(startPos);
                    var endScreenPoint = _mainCamera.WorldToScreenPoint(_playerController.transform.position);

                    var movingImage = Instantiate(_draggedItemPrefab, startScreenPoint, quaternion.identity, _canvasRectTransform);
                        
                    movingImage.sprite = itemContainer.ItemSprite;

                    var path = GenerateRandomPath(startScreenPoint, endScreenPoint);

                    movingImage.transform
                        .DOPath(path.Select(p => (Vector3) p).ToArray(), _movementSpeed, PathType.CatmullRom).OnComplete(
                            () =>
                            {
                                DestroyImmediate(movingImage.gameObject);
                                OnMovementComplete?.Invoke(itemContainer);
                            });
                });
                
                delay += _sequenceStep;
            }
        }
        
        private List<Vector2> GenerateRandomPath(Vector2 startPos, Vector2 targetPos)
        {
            var randomX = Random.Range(_minSpread, _maxSpread);
            var randomY = Random.Range(_minSpread, _maxSpread);
            
            randomX *= Random.value > 0.5f ? 1 : -1;
            randomY *= Random.value > 0.5f ? 1 : -1;
            
            var controlPoint1 = startPos + new Vector2(randomX, randomY);
            
            return new List<Vector2> { controlPoint1, targetPos };
        }
    }
}