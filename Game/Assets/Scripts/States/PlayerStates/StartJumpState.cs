using System.Collections;
using Player;
using UnityEngine;

namespace States.PlayerStates
{
    public class StartJumpState : PlayerState
    {
        private bool m_canTransition = false;
        public StartJumpState(PlayerController playerController) : base(playerController)
        {
        }

        public override void OnStateEnter()
        {
            this.PlayAnimation(this.m_player.PlayerData.StartJumpClipName);
            AudioSource.PlayClipAtPoint(this.m_player.PlayerData.JumpSound, this.m_player.PlayerCamera.position, 0.25f);
            this.m_player.Rigidbody.AddForce(Vector3.up * this.m_player.PlayerData.JumpForce, ForceMode.Impulse);
            this.m_player.StartCoroutine(this.DelayTransition());
        }

        private IEnumerator DelayTransition()
        {
            yield return new WaitForSeconds(0.3f);
            this.m_canTransition = true;
        }
        
        public override void OnStateExit()
        {
        }

        protected override PlayerState GetTransition(float delta)
        {
            if (this.m_canTransition)
                return new MidAirState(this.m_player);
                
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