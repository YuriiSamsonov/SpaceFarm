using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Scripts
{
    public class Animal : MonoBehaviour, IClickableObject, IWindowHolder
    {
        [field: SerializeField] 
        private float _maxDistance = 5f;

        [field: SerializeField]
        private Animator _animator;
        public Animator Animator => _animator;

        public StateMachine StateMachine { get; private set; }
        public IdleState IdleState { get; private set; }
        public WalkingState WalkingState { get; private set; }

        private PlayerController _player;
        private InteractionWindow _interactionWindow;
        private LootingManager _lootingManager;
        private Collider _walkingCollider;
        private List<ItemData> _itemsToReturn = new();
        
        public bool IsMovementBlocked;

        private Action _onDeathCallback;
        
        public void Init(PlayerController player, InteractionWindow interactionWindow, LootingManager lootingManager,
            Collider walkingCollider, List<ItemData> itemDatas, Action onDeathCallback)
        {
            _player = player;
            _interactionWindow = interactionWindow;
            _lootingManager = lootingManager;
            _walkingCollider = walkingCollider;
            _itemsToReturn = itemDatas;
            _onDeathCallback = onDeathCallback;
        }
        
        private void Awake()
        {
            StateMachine = new StateMachine();
            IdleState = new IdleState(this, StateMachine);
            WalkingState = new WalkingState(this, StateMachine);
        }

        private void Start()
        {
            StateMachine.Initialize(WalkingState);
        }

        private void Update()
        {
            StateMachine.CurrentState.FrameUpdate();
        }

        public Vector3 FindPointWalkTo()
        {
            var bounds = _walkingCollider.bounds;

            var randomX = Random.Range(bounds.min.x, bounds.max.x);
            var randomZ = Random.Range(bounds.min.z, bounds.max.z);

            return new Vector3(randomX, transform.position.y, randomZ);
        }

        public void OnClick()
        {
            if (!IsCloseEnoughToPlayer())
            {
                return;
            }
            
            ShowInteractionMenu();
        }
        
        private bool IsCloseEnoughToPlayer()
        {
            if (Vector3.Distance(_player.transform.position, transform.position) > _maxDistance)
            {
                return false;
            }

            return true;
        }

        private void ShowInteractionMenu()
        {
            StateMachine.ChangeState(IdleState);
            
            IsMovementBlocked = true;
            
            _interactionWindow.Show(this, transform.position);
        }
        
        private void HideInteractionMenu()
        {
            IsMovementBlocked = false;
        }

        public void OnWindowClick()
        {
            _lootingManager.StartMovement(_itemsToReturn, transform.position);

            HideInteractionMenu();

            _onDeathCallback();
            
            Destroy(gameObject);
        }
    }
}