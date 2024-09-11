using UnityEngine;

namespace Game.Scripts
{
    public class Pot : MonoBehaviour
    {
        [field: SerializeField] 
        private GameObject _plantObject;
        
        [field: SerializeField] 
        private Outline _outline;

        [field: SerializeField] 
        private Inventory _inventory;

        [field: SerializeField] 
        private float _maxDistance = 5f;

        private bool _canPlant;
        
        private void Awake()
        {
            _outline.enabled = false;
            
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
            _plantObject.SetActive(true);
        }

        private void OnPlantStartHover(Vector3 playerPos)
        {
            if (Vector3.Distance(playerPos, transform.position) > _maxDistance)
            {
                _canPlant = false;
                return;
            }

            _canPlant = true;
            
            _outline.enabled = true;
        }   
        
        private void OnPlantEndHover(Vector3 playerPos)
        {
            _canPlant = false;
            _outline.enabled = false;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            
            Gizmos.DrawWireSphere(transform.position, _maxDistance);
        }
    }
}