using System;
using System.Collections;
using Data;
using States;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.TestTools;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private PlayerData m_playerData;
        [SerializeField] private Animator m_animator;
        [SerializeField] private GameObject m_feet;
        
        private IState m_currentState;
        
        private Transform m_camera;
        private InputProcessor m_inputProcessor;
        private Vector3 m_moveDirection;
        private Vector3 m_targetPosition;
        private Rigidbody m_rigidbody;

        private bool m_isRunning;
        private bool m_isGrounded;
        private bool m_isJumping;

        void Awake()
        {
            this.m_rigidbody = this.GetComponent<Rigidbody>();
            this.m_inputProcessor = this.GetComponent<InputProcessor>();
            this.m_camera = Camera.main.transform;
            this.m_animator = this.GetComponent<Animator>();
            this.m_inputProcessor.RunningStateChanged += (sender, args) => this.m_isRunning = args.NewState;
        }
        
        void Update()
        {
            var isGrounded = this.IsGrounded();
            this.m_animator.SetBool("IsGrounded", this.IsGrounded());
            var delta = Time.deltaTime;
            if(isGrounded) 
                this.HandleMovement(delta);
            this.HandleRotation(delta);

            if (this.m_inputProcessor.JumpTriggered && isGrounded)
                StartCoroutine(this.HandleJump());
        }

        private void HandleMovement(float delta)
        {
            this.m_moveDirection = this.m_camera.forward * this.m_inputProcessor.MovementZ;
            this.m_moveDirection += this.m_camera.right * this.m_inputProcessor.MovementX;

            var movementSpeed = this.m_isRunning ? this.m_playerData.RunSpeed : this.m_playerData.WalkSpeed;
            this.m_moveDirection = this.m_moveDirection.normalized * movementSpeed;
            var projectedVelocity = Vector3.ProjectOnPlane(this.m_moveDirection, Vector3.up);
            projectedVelocity.y = this.m_rigidbody.velocity.y;
            this.m_rigidbody.velocity = projectedVelocity;

            this.m_animator.SetBool("IsIdle", this.m_moveDirection == Vector3.zero);
            this.m_animator.SetBool("IsWalking", this.m_moveDirection != Vector3.zero && !this.m_isRunning);
            this.m_animator.SetBool("IsRunning", this.m_moveDirection != Vector3.zero && this.m_isRunning);
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

        private bool IsGrounded()
        {
            var boxSize = new Vector3(1f, 0.2f, 1f);
            return Physics.OverlapBox(this.m_feet.transform.position, boxSize, Quaternion.identity, 1 << LayerMask.NameToLayer("Ground")).Length > 0; // true; // Physics.BoxCast(this.m_feet.transform.position, new Vector3(1f, 1f, 1f) , Vector3.down, Vector3.down, 1 << LayerMask.NameToLayer("Default"));
        }

        private void OnDrawGizmos()
        {
            var boxSize = new Vector3(1f, 0.2f, 1f);
            Gizmos.DrawWireCube(this.m_feet.transform.position, boxSize);
        }

        private IEnumerator HandleJump()
        {
            this.m_animator.SetTrigger("StartJump");
            this.m_rigidbody.AddForce(Vector3.up * this.m_playerData.JumpForce, ForceMode.Impulse);
            while (!this.IsGrounded())
            {

                yield return new WaitForFixedUpdate();
            }

            this.m_animator.SetBool("IsGrounded", true);
        }

        private void OnCollisionEnter(Collision other)
        {
            var hazard = other.gameObject.GetComponent<Hazard>();
            if (hazard != null)
            {
                this.HandleDie();
            }
        }

        private void HandleDie()
        {
            
        }

        private IEnumerator Die()
        {
            yield return null;
        }

        private void Respawn()
        {
            
        }
    }
}