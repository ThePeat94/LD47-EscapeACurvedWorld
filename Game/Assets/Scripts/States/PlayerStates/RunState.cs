using Player;
using UnityEngine;

namespace States.PlayerStates
{
    public class RunState : PlayerState
    {
        public RunState(PlayerController playerController) : base(playerController)
        {
        }

        public override void OnStateEnter()
        {
            PlayAnimation(this.m_player.PlayerData.RunClipName);
        }

        public override void OnStateExit()
        {
        }

        protected override PlayerState GetTransition(float delta)
        {
            if (this.m_player.InputProcessor.MoveAmount == 0)
                return new IdleState(this.m_player);
            
            if (!this.m_player.InputProcessor.HoldingSprint)
                return new Walkstate(this.m_player);
            
            if (this.m_player.InputProcessor.JumpTriggered)
                return new StartJumpState(this.m_player);

            return null;
        }

        protected override void ExecuteStateActions(float delta)
        {
            if (this.IsGrounded())
            {
                this.Move(delta, this.m_player.PlayerData.RunSpeed);
                this.Rotate(delta);
                if(this.m_player.InputProcessor.UseItemTriggered)
                    this.UsePlayerItem();
            }
        }
    }
}