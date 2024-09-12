using UnityEngine;

namespace Game.Scripts
{
    public class WalkingState : State
    {
        private Vector3 _destination;
        private static readonly int WalkHash = Animator.StringToHash("Walk");

        public WalkingState(Animal animal, StateMachine stateMachine) : base(animal, stateMachine)
        {
            
        }
        
        public override void EnterState()
        {
            base.EnterState();
            
            _animal.Animator.SetBool(WalkHash, true);

            _destination = _animal.FindPointWalkTo();
        }

        public override void ExitState()
        {
            base.ExitState();
            
            _animal.Animator.SetBool(WalkHash, false);
        }

        public override void FrameUpdate()
        {
            base.FrameUpdate();

            var speed = 3f;
            
            var direction = (_destination - _animal.transform.position).normalized;
            
            _animal.transform.Translate(direction * (speed * Time.deltaTime), Space.World);
            
            _animal.transform.LookAt(_destination);
            
            if (Vector3.Distance(_animal.transform.position, _destination) <= 0.1f)
            {
                _stateMachine.ChangeState(_animal.IdleState);
            }
        }
    }
}