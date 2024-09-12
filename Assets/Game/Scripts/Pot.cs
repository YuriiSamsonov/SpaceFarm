using DG.Tweening;
using UnityEngine;

namespace Game.Scripts
{
    public class Pot : MonoBehaviour, IClickableObject
    {
        [field: SerializeField] 
        private GameObject _plantObject;
        
        [field: SerializeField] 
        private Outline _outline;

        [field: SerializeField] 
        private Inventory _inventory;

        [field: SerializeField] 
        private float _maxDistance = 5f;
        
        [field: SerializeField] 
        private float _startPlantScale = 1f;
        
        [field: SerializeField] 
        private float _endPlantScale = 8f;

        [field: SerializeField] 
        private float _secondsToGrow = 5f;

        private PlayerController _player;

        private bool _hasPlant;
        private bool _readyToHarvest;
        
        private float _growthProgress;
        
        private void Awake()
        {
            _outline.enabled = false;
            _player = FindObjectOfType<PlayerController>();
            
            _inventory.OnItemHoverStart += OnPlantStartHover;
            _inventory.OnItemHoverEnd += OnPlantEndHover;
        }

        private void OnDestroy()
        {
            _inventory.OnItemHoverStart -= OnPlantStartHover;
            _inventory.OnItemHoverEnd -= OnPlantEndHover;
        }

        public void PlantSeed()
        {
            StartGrowing();
        }

        public void OnClick()
        {
            if (!IsCloseEnoughToPlayer())
            {
                return;
            }
            
            Harvest();
        }
        
        private void Harvest()
        {
            if (_readyToHarvest)
            {
                _plantObject.SetActive(false);
                _hasPlant = false;
                _readyToHarvest = false;
            }
        }

        private void OnPlantStartHover()
        {
            if(_hasPlant)
            {
                return;
            }

            if (!IsCloseEnoughToPlayer())
            {
                return;
            }
            
            _outline.enabled = true;
        }

        private bool IsCloseEnoughToPlayer()
        {
            if (Vector3.Distance(_player.transform.position, transform.position) > _maxDistance)
            {
                return false;
            }

            return true;
        }

        private void OnPlantEndHover()
        {
            _outline.enabled = false;
        }

        private void StartGrowing()
        {
            _hasPlant = true;
            
            _plantObject.SetActive(true);

            _plantObject.transform.localScale = new Vector3(_startPlantScale, _startPlantScale, _startPlantScale);

            _plantObject.transform
                .DOScale(new Vector3(_endPlantScale, _endPlantScale, _endPlantScale), 1)
                .SetEase(Ease.OutBounce)
                .SetDelay(_secondsToGrow).OnComplete(() =>
                {
                    _readyToHarvest = true;
                });
        }
        
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            
            Gizmos.DrawWireSphere(transform.position, _maxDistance);
        }

       
    }
}