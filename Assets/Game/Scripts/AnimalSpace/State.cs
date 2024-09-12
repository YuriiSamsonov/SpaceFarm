namespace Game.Scripts
{
    public class State
    {
        protected Animal _animal;
        protected StateMachine _stateMachine;

        public State(Animal animal, StateMachine stateMachine)
        {
            _animal = animal;
            _stateMachine = stateMachine;
        }

        public virtual void EnterState()
        {
            
        }

        public virtual void ExitState()
        {
            
        }

        public virtual void FrameUpdate()
        {
            
        }
    }
}