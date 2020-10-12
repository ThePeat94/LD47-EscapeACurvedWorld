using Player;
using UnityEngine;

namespace States.PlayerStates
{
    public class IdleState : PlayerState
    {
        public IdleState(PlayerController playerController) : base(playerController)
        {
        }

        public override void OnStateEnter()
        {
            this.PlayAnimation(this.m_player.PlayerData.IdleClipName);
            this.m_player.Rigidbody.velocity = Vector3.zero;
        }

        public override void OnStateExit()
        {
        }

        protected override PlayerState GetTransition(float delta)
        {
            if (this.m_player.InputProcessor.HoldingSprint && this.m_player.InputProcessor.MoveAmount > 0)
                return new RunState(this.m_player);

            if (this.m_player.InputProcessor.MoveAmount > 0)
                return new Walkstate(this.m_player);

            if (this.m_player.InputProcessor.JumpTriggered)
                return new StartJumpState(this.m_player);
            
            return null;
        }

        protected override void ExecuteStateActions(float delta)
        {
            if(this.m_player.InputProcessor.UseItemTriggered)
                this.UsePlayerItem();
        }
    }
}