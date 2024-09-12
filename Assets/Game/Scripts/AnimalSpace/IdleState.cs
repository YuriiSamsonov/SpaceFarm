using UnityEngine;

namespace Game.Scripts
{
    public class IdleState : State
    {
        private float _delayTime;
        private float _startTime;

        public IdleState(Animal animal, StateMachine stateMachine) : base(animal, stateMachine)
        {
        }

        public override void EnterState()
        {
            base.EnterState();
            
            _delayTime = Random.Range(1f, 3f);

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