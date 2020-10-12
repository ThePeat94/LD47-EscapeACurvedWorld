using System;
using Player;
using UnityEngine;

namespace States.PlayerStates
{
    public abstract class PlayerState
    {
        protected PlayerController m_player;
        protected Vector3 m_moveDirection;
        public PlayerState(PlayerController playerController)
        {
            this.m_player = playerController;
        }

        public abstract void OnStateEnter();
        public abstract void OnStateExit();

        public void Tick(float delta)
        {
            var newState = this.GetTransition(delta);
            if (newState == null)
            {
                this.ExecuteStateActions(delta);
            }
            else
            {
                this.m_player.ChangeState(newState);
            }
        }
        
        protected abstract PlayerState GetTransition(float delta);
        protected abstract void ExecuteStateActions(float delta);
        
        protected void PlayAnimation(string clipname)
        {
            this.m_player.Animator.CrossFadeInFixedTime(clipname, 0.25f, 0);
        }
        
        protected void PlayAnimation(int clipHash)
        {
            this.m_player.Animator.Play(clipHash, 0);
        }
        
        protected void Move(float delta, float movementSpeed)
        {
            this.m_moveDirection = this.m_player.PlayerCamera.forward * this.m_player.InputProcessor.MovementZ;
            this.m_moveDirection += this.m_player.PlayerCamera.right * this.m_player.InputProcessor.MovementX;
            
            this.m_moveDirection = this.m_moveDirection.normalized * movementSpeed;
            var projectedVelocity = Vector3.ProjectOnPlane(this.m_moveDirection, Vector3.up);
            projectedVelocity.y = this.m_player.Rigidbody.velocity.y;
            this.m_player.Rigidbody.velocity = projectedVelocity;
        }
        
        protected void Rotate(float delta)
        {
            var targetDir = Vector3.zero;

            targetDir = this.m_player.PlayerCamera.forward * this.m_player.InputProcessor.MovementZ;
            targetDir += this.m_player.PlayerCamera.right * this.m_player.InputProcessor.MovementX;
            targetDir.y = 0f;

            if (targetDir == Vector3.zero)
                targetDir = this.m_player.transform.forward;

            this.RotateTowards(targetDir, this.m_player.PlayerData.RotationSpeed, delta);
        }

        protected void RotateTowards(Vector3 dir, float rotationSpeed, float delta)
        {
            var lookRotation = Quaternion.LookRotation(dir.normalized);
            this.m_player.transform.rotation = Quaternion.Slerp(this.m_player.transform.rotation, lookRotation, rotationSpeed * delta);
        }
        
        protected bool IsGrounded()
        {
            var boxSize = new Vector3(1f, 0.2f, 1f);
            return Physics.OverlapBox(this.m_player.Feet.transform.position, boxSize, Quaternion.identity, 1 << LayerMask.NameToLayer("Ground")).Length > 0;
        }

        protected void UsePlayerItem()
        {
            this.m_player.UseItem();
        }
    }
}