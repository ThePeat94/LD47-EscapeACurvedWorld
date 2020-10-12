using System;
using System.Collections;
using EventArgs;
using Player;
using UnityEngine;

namespace States.PlayerStates
{
    public class DieState : PlayerState
    {
        private bool m_canTransition = false;
        private const float DIE_CLIP_LENGTH = 4.133f;
        
        
        public DieState(PlayerController playerController) : base(playerController)
        {
        }

        public override void OnStateEnter()
        {
            this.PlayAnimation(this.m_player.PlayerData.DieClipName);
            this.m_player.StartCoroutine(this.DelayTransition());
        }

        private IEnumerator DelayTransition()
        {
            yield return new WaitForSeconds(DIE_CLIP_LENGTH);
            this.m_canTransition = true;
        }

        public override void OnStateExit()
        {
        }

        protected override PlayerState GetTransition(float delta)
        {
            if (this.m_canTransition)
                return new RespawnState(this.m_player);
            
            return null;
        }

        protected override void ExecuteStateActions(float delta)
        {
            // Nothing to do during dying :)
        }
    }
}