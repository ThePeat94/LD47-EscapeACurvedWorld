using Player;

namespace States.PlayerStates
{
    public class MidAirState : PlayerState
    {
        public MidAirState(PlayerController playerController) : base(playerController)
        {
        }

        public override void OnStateEnter()
        {
            this.PlayAnimation(this.m_player.PlayerData.MidAirClipName);
        }

        public override void OnStateExit()
        {
            this.PlayAnimation(this.m_player.PlayerData.EndJumpClipName);
        }

        protected override PlayerState GetTransition(float delta)
        {
            if (this.IsGrounded())
                return new IdleState(this.m_player);
            
            return null;
        }

        protected override void ExecuteStateActions(float delta)
        {
            this.Rotate(delta);
            if(this.m_player.InputProcessor.UseItemTriggered)
                this.UsePlayerItem();
        }
    }
}