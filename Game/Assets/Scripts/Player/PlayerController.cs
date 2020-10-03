using System;
using Data;
using States;
using UnityEngine;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private PlayerData m_playerData;
        [SerializeField] private Animator m_animator;
        private IState m_currentState;
        
        private Transform m_camera;
        private InputProcessor m_inputProcessor;
        private Vector3 m_moveDirection;
        private Vector3 m_targetPosition;
        private Rigidbody m_rigidbody;
        
        void Awake()
        {
            this.m_rigidbody = this.GetComponent<Rigidbody>();
            this.m_inputProcessor = this.GetComponent<InputProcessor>();
            this.m_camera = Camera.main.transform;
            this.m_animator = this.GetComponent<Animator>();
        }
        
        void Update()
        {
            var delta = Time.deltaTime;
            
            this.HandleMovement(delta);
            this.HandleRotation(delta);
        }

        private void HandleMovement(float delta)
        {
            this.m_moveDirection = this.m_camera.forward * this.m_inputProcessor.MovementZ;
            this.m_moveDirection += this.m_camera.right * this.m_inputProcessor.MovementX;
            this.m_moveDirection.y = 0;

            var movementSpeed = this.m_inputProcessor.IsRunTriggered ? this.m_playerData.RunSpeed : this.m_playerData.WalkSpeed;
            this.m_moveDirection = this.m_moveDirection.normalized * movementSpeed;
            
            if(this.m_inputProcessor.IsRunTriggered) Debug.Log("Run triggered");

            var projectedVelocity = Vector3.ProjectOnPlane(this.m_moveDirection, Vector3.up);
            this.m_rigidbody.velocity = projectedVelocity;

            this.m_animator.SetBool("IsIdle", this.m_moveDirection == Vector3.zero);
            this.m_animator.SetBool("IsWalking", this.m_moveDirection != Vector3.zero);
        }

        private void HandleRotation(float delta)
        {
            this.Rotate(delta);
        }

        private void Rotate(float delta)
        {
            var targetDir = Vector3.zero;

            targetDir = this.m_camera.forward * this.m_inputProcessor.MovementZ;
            targetDir += this.m_camera.right * this.m_inputProcessor.MovementX;
            targetDir.y = 0f;

            if (targetDir == Vector3.zero)
                targetDir = this.transform.forward;

            var lookRotation = Quaternion.LookRotation(targetDir.normalized);
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, lookRotation, this.m_playerData.RotationSpeed * delta);
        }
    }
}