namespace States
{
    public interface IMovementState : IState
    {
        void Move(float delta);
    }
}