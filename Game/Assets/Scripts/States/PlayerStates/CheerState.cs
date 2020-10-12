using System.Collections;
using Player;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace States.PlayerStates
{
    public class CheerState : PlayerState
    {
        private bool m_canTransition = false;
        private const float CHEER_CLIP_LENGTH = 8.217f;
        private Coroutine m_delayRoutine;
        
        public CheerState(PlayerController playerController) : base(playerController)
        {
        }

        public override void OnStateEnter()
        {
            AudioSource.PlayClipAtPoint(this.m_player.PlayerData.VictorySound, this.m_player.PlayerCamera.position, 0.25f);
            this.PlayAnimation(this.m_player.PlayerData.CheerClipName);
            this.m_player.Rigidbody.velocity = Vector3.zero;
            this.m_delayRoutine = this.m_player.StartCoroutine(this.DelayTransition());
        }

        private IEnumerator DelayTransition()
        {
            this.RotateTowards(Vector3.forward, 1000, 1);
            yield return new WaitForSeconds(CHEER_CLIP_LENGTH);
            this.m_canTransition = true;
            this.m_delayRoutine = null;
        }

        public override void OnStateExit()
        {
            if(this.m_delayRoutine != null)
                this.m_player.StopCoroutine(m_delayRoutine);
        }

        protected override PlayerState GetTransition(float delta)
        {
            return null;
        }

        protected override void ExecuteStateActions(float delta)
        {
            if (this.m_player.InputProcessor.SkipDanceTriggered || this.m_canTransition)
            {
                GameManager.Instance.LoadNextLevel();
            }
        }
    }
}