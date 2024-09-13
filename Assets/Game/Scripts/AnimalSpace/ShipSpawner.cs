using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Scripts
{
    public class ShipSpawner : MonoBehaviour
    {
        [field: SerializeField] 
        private int _numberToSpawn;
        
        [field: SerializeField] 
        private Animal _animalPrefab;
        
        [field: SerializeField] 
        private Collider _walkingCollider;

        [field: SerializeField] 
        private LootingManager _lootingManager;

        [field: SerializeField] 
        private List<ItemData> _itemsToReturn = new();
        
        [field: SerializeField] 
        private PlayerController _player;

        [field: SerializeField] 
        private InteractionWindow _interactionWindow;

        private int _aliveShips;

        private void Start()
        {
            TryToSpawnShips();
        }

        private void TryToSpawnShips()
        {
            if (_aliveShips > 0)
            {
                return;
            }
            
            for (int i = 0; i < _numberToSpawn; i++)
            {
                var ship = Instantiate(_animalPrefab, FindRandomPoint(),
                    new Quaternion(Random.Range(0, 360),
                        Quaternion.identity.y,
                        Quaternion.identity.z,
                        Quaternion.identity.w));
                
                ship.Init(_player, _interactionWindow, _lootingManager, _walkingCollider, _itemsToReturn, OnShipDeath);

                _aliveShips++;
            }
        }

        private void OnShipDeath()
        {
            _aliveShips--;
            
            TryToSpawnShips();
        }
        
        private Vector3 FindRandomPoint()
        {
            var bounds = _walkingCollider.bounds;

            var randomX = Random.Range(bounds.min.x, bounds.max.x);
            var randomZ = Random.Range(bounds.min.z, bounds.max.z);

            return new Vector3(randomX, bounds.max.y, randomZ);
        }
    }
}