using UnityEngine;

namespace Game.Scripts
{
    public class IdleState : State
    {
        private float _delayTime;
        private float _startTime;
        
        private readonly float _minDelay = 1f;
        private readonly float _maxDelay = 3f;
        

        public IdleState(Animal animal, StateMachine stateMachine) : base(animal, stateMachine)
        {
        }

        public override void EnterState()
        {
            base.EnterState();
            
            _delayTime = Random.Range(_minDelay, _maxDelay);

            _startTime = Time.time;
        }

        public override void FrameUpdate()
        {
            base.FrameUpdate();

            if (_animal.IsMovementBlocked)
            {
                return;
            }
            
            if (Time.time >= _startTime + _delayTime)
            {
                _animal.StateMachine.ChangeState(_animal.WalkingState);
            }
        }
    }
}