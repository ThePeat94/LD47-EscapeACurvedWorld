namespace States
{
    public interface IState
    {
        void OnStateEnter();
        void OnStateExit();
        void Tick(float delta);
        void ChangeState(IState targetState);
    }
}