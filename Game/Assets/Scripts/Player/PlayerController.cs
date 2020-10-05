using System;
using System.Collections;
using System.Threading.Tasks;
using Data;
using Scripts;
using EventArgs;
using Hazards;
using Platforms;
using States;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private PlayerData m_playerData;
        [SerializeField] private Animator m_animator;
        [SerializeField] private GameObject m_feet;
        [SerializeField] private GameObject m_barrel;

        private Transform m_camera;
        private InputProcessor m_inputProcessor;
        private Vector3 m_moveDirection;
        private Vector3 m_targetPosition;
        private Rigidbody m_rigidbody;

        private bool m_isRunning;
        private bool m_isGrounded;
        private bool m_isJumping;
        private bool m_isDead;
        private ItemData m_currentItem;

        private int m_currentSceneIndex;
        
        private static PlayerController s_instance;

        public static PlayerController Instance => s_instance ?? FindObjectOfType<PlayerController>();

        public bool IsDead => this.m_isDead;
        
        public event EventHandler<PlayerDiedEventArgs> Died
        {
            add => this.m_died += value;
            remove => this.m_died -= value;
        }
        
        public event EventHandler Respawned
        {
            add => this.m_respawned += value;
            remove => this.m_respawned -= value;
        }
        
        public event EventHandler ReachedGoal
        {
            add => this.m_reachedGoal += value;
            remove => this.m_reachedGoal -= value;
        }


        private EventHandler<PlayerDiedEventArgs> m_died;
        private EventHandler m_respawned;
        private EventHandler m_reachedGoal;

        void Awake()
        {
            if (s_instance == null)
            {
                DontDestroyOnLoad(this.gameObject);
                s_instance = this;
            }
            else
            {
                Destroy(this.gameObject);
                return;
            }
            
            SceneManager.sceneLoaded += LevelWasLoaded;
            this.m_rigidbody = this.GetComponent<Rigidbody>();
            this.m_inputProcessor = this.GetComponent<InputProcessor>();
            this.m_camera = Camera.main.transform;
            this.m_animator = this.GetComponent<Animator>();
            this.m_inputProcessor.RunningStateChanged += (sender, args) => this.m_isRunning = args.NewState;
            this.m_inputProcessor.enabled = false;
        }

        private void LevelWasLoaded(Scene arg0, LoadSceneMode arg1)
        {
            this.transform.position = FindObjectOfType<SpawnPoint>().transform.position;
            this.m_currentSceneIndex = arg0.buildIndex;
            StartCoroutine(this.PlayRespawnAndIdle());
        }

        private IEnumerator PlayRespawnAndIdle()
        {
            this.m_animator.Play("Respawn");
            yield return new WaitForSeconds(8.5f);
            this.m_animator.Play("Idle");
            this.m_inputProcessor.enabled = true;
        }

        void Update()
        {
            if (!this.m_inputProcessor.enabled)
                return;

            if (this.m_currentItem != null && this.m_inputProcessor.UseItemTriggered)
            {
                this.UseItem();
            }
            
            var isGrounded = this.IsGrounded();
            this.m_animator.SetBool("IsGrounded", this.IsGrounded());
            var delta = Time.deltaTime;
            if(isGrounded) 
                this.HandleMovement(delta);
            this.HandleRotation(delta);

            if (this.m_inputProcessor.JumpTriggered && isGrounded)
                StartCoroutine(this.HandleJump());
        }

        private void UseItem()
        {
            var bullet = Instantiate(this.m_currentItem.Bullet, this.m_barrel.transform.position, this.m_barrel.transform.rotation);
            var targetDir = this.m_barrel.transform.forward - this.transform.position;
            bullet.GetComponent<Bullet>().Dir = targetDir.normalized;
            this.m_currentItem = null;
            AudioSource.PlayClipAtPoint(this.m_playerData.ShootingSound, this.m_camera.position, 0.25f);
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

            var isMoving = Math.Abs(this.m_moveDirection.x) > 0f || Math.Abs(this.m_moveDirection.z) > 0f;
            
            this.m_animator.SetBool("IsIdle", this.m_moveDirection == Vector3.zero);
            this.m_animator.SetBool("IsWalking", isMoving && !this.m_isRunning);
            this.m_animator.SetBool("IsRunning", isMoving && this.m_isRunning);
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

            this.RotateTowards(targetDir, this.m_playerData.RotationSpeed, delta);
        }

        private void RotateTowards(Vector3 dir, float rotationSpeed, float delta)
        {
            var lookRotation = Quaternion.LookRotation(dir.normalized);
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, lookRotation, rotationSpeed * delta);
        }

        private bool IsGrounded()
        {
            var boxSize = new Vector3(1f, 0.2f, 1f);
            return Physics.OverlapBox(this.m_feet.transform.position, boxSize, Quaternion.identity, 1 << LayerMask.NameToLayer("Ground")).Length > 0;
        }

        private void OnDrawGizmos()
        {
            var boxSize = new Vector3(1f, 0.2f, 1f);
            Gizmos.DrawWireCube(this.m_feet.transform.position, boxSize);
        }

        private IEnumerator HandleJump()
        {
            AudioSource.PlayClipAtPoint(this.m_playerData.JumpSound, this.m_camera.position, 0.25f);
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
            var hazardType = hazard?.GetType();
            var isKillingHazard = hazard != null && hazardType != typeof(MovingPlatform) && hazardType != typeof(FallingPlatform); 
            if (isKillingHazard)
            {
                this.PlayerDie(hazard.gameObject);
            }
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<Goal>() != null)
            {
                StartCoroutine(this.HandleReachedGoal());
                return;
            }

            var item = other.GetComponent<Item>();
            if (item != null)
            {
                AudioSource.PlayClipAtPoint(this.m_playerData.PowerUpSound, this.m_camera.position, 0.25f);
                this.m_currentItem = item.Data;
                Destroy(item.gameObject);
                return;
            }
        }

        private void OnParticleCollision(GameObject other)
        {
            Debug.Log("Particle collision");
            var elementarGun = other.GetComponentInParent<ElementarGun>();
            if (elementarGun != null)
            {
                this.PlayerDie(elementarGun.gameObject);
                return;
            }
        }

        private void PlayerDie(GameObject killer)
        {
            if (this.m_isDead)
                return;
            
            this.m_inputProcessor.enabled = false;
            this.m_isRunning = false;
            this.m_animator.SetBool("IsWalking", false);
            this.m_animator.SetBool("IsIdle", false);
            this.m_animator.SetBool("IsRunning", false);
            this.StartCoroutine(this.HandleDie(killer));
        }

        private IEnumerator HandleDie(GameObject killer)
        {
            this.m_isDead = true;
            this.m_currentItem = null;
            this.m_died?.Invoke(this, new PlayerDiedEventArgs(killer));
            this.m_moveDirection = Vector3.zero;
            this.m_animator.Play("Die");
            yield return new WaitForSeconds(4.34f);
            StartCoroutine(this.HandleRespawn());
        }

        private IEnumerator HandleRespawn()
        {
            this.RespawnPlayer();
            this.m_respawned?.Invoke(this, System.EventArgs.Empty);
            this.m_animator.SetTrigger("Respawn");
            yield return new WaitForSeconds(8.5f);
            this.m_animator.Play("Idle");
            this.m_inputProcessor.enabled = true;
            this.m_isDead = false;
        }

        private void RespawnPlayer()
        {
            this.transform.position = FindObjectOfType<SpawnPoint>().transform.position;
        }

        private IEnumerator HandleReachedGoal()
        {
            AudioSource.PlayClipAtPoint(this.m_playerData.VictorySound, this.m_camera.position, 0.25f);
            this.m_inputProcessor.enabled = false;
            this.m_rigidbody.velocity = Vector3.zero;
            this.m_reachedGoal?.Invoke(this, System.EventArgs.Empty);
            this.m_animator.Play("Cheer");
            this.RotateTowards(Vector3.forward, 100, 1);
            yield return new WaitForSeconds(8.217f);

            if (this.m_currentSceneIndex < SceneManager.sceneCountInBuildSettings)
            {
                SceneManager.LoadScene(this.m_currentSceneIndex + 1);
            }
            else
            {
                SceneManager.LoadScene(0);
            }
        }
        
        
    }
}