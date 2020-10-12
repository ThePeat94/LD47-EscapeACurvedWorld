using Player;
using UnityEngine;

namespace States.PlayerStates
{
    public class Walkstate : PlayerState
    {
        public Walkstate(PlayerController playerController) : base(playerController)
        {
        }

        public override void OnStateEnter()
        {
            this.PlayAnimation(this.m_player.PlayerData.WalkClipName);
        }

        public override void OnStateExit()
        {
        }

        protected override PlayerState GetTransition(float delta)
        {
            if (this.m_player.InputProcessor.MoveAmount == 0)
                return new IdleState(this.m_player);

            if (this.m_player.InputProcessor.HoldingSprint)
                return new RunState(this.m_player);

            if (this.m_player.InputProcessor.JumpTriggered)
                return new StartJumpState(this.m_player);
            
            return null;
        }

        protected override void ExecuteStateActions(float delta)
        {
            if (this.IsGrounded())
            {
                this.Move(delta, this.m_player.PlayerData.WalkSpeed);
                this.Rotate(delta);
                
                if(this.m_player.InputProcessor.UseItemTriggered)
                    this.UsePlayerItem();
            }
        }
    }
}