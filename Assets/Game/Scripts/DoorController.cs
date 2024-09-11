using System;
using DG.Tweening;
using UnityEngine;

namespace Game.Scripts
{
    public class DoorController : MonoBehaviour
    {
        [field: SerializeField] 
        private GameObject _doorRight, _doorLeft;

        [field: SerializeField] 
        private Vector3 _rightDoorOpenPos, _leftDoorOpenPos;
        
        [field: SerializeField] 
        private LayerMask _playerLayer;

        private Vector3 _rightDoorClosePos, _leftDoorClosePos;
        
        private void Start()
        {
            _rightDoorClosePos = _doorRight.transform.localPosition;
            _leftDoorClosePos = _doorLeft.transform.localPosition;
        }

        private void OnTriggerEnter(Collider other)
        {
            if ((1 << other.gameObject.layer & _playerLayer) == 0)
            {
                return;
            }
            
            _doorRight.transform.DOLocalMove(_rightDoorOpenPos, 1f);
            _doorLeft.transform.DOLocalMove(_leftDoorOpenPos, 1f);
        }

        private void OnTriggerExit(Collider other)
        {
            if ((1 << other.gameObject.layer & _playerLayer) == 0)
            {
                return;
            }

            _doorRight.transform.DOLocalMove(_rightDoorClosePos, 1f);
            _doorLeft.transform.DOLocalMove(_leftDoorClosePos, 1f);
        }
    }
}