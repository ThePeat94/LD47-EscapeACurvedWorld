using System.Collections;
using System.Runtime.CompilerServices;
using Player;
using UnityEngine;

namespace States.PlayerStates
{
    public class RespawnState : PlayerState
    {
        private Coroutine m_respawnCoroutine;
        private bool m_transitionToIdle;

        private const float RESPAWN_CLIP_LENGTH = 8.267f;
        
        public RespawnState(PlayerController playerController) : base(playerController)
        {
        }

        public override void OnStateEnter()
        {
            this.m_player.transform.position = GameManager.Instance.CurrentSpawnPoint.transform.position;
            this.PlayAnimation(this.m_player.PlayerData.RespawnClipName);
            this.m_respawnCoroutine = this.m_player.StartCoroutine(this.DelayTransition());
        }

        private IEnumerator DelayTransition()
        {
            yield return new WaitForSeconds(RESPAWN_CLIP_LENGTH);
            this.m_transitionToIdle = true;
            this.m_respawnCoroutine = null;
        }

        public override void OnStateExit()
        {
            if(this.m_respawnCoroutine != null)
                this.m_player.StopCoroutine(this.m_respawnCoroutine);
        }

        protected override PlayerState GetTransition(float delta)
        {
            if (this.m_player.InputProcessor.HoldingSprint && this.m_player.InputProcessor.MoveAmount > 0)
                return new RunState(this.m_player);
            
            if (this.m_player.InputProcessor.MoveAmount > 0)
                return new Walkstate(this.m_player);

            if (this.m_player.InputProcessor.JumpTriggered)
                return new StartJumpState(this.m_player);

            if (this.m_transitionToIdle)
                return new IdleState(this.m_player);
            
            return null;
        }

        protected override void ExecuteStateActions(float delta)
        {
        }
    }
}