using System;
using Data;
using Scripts;
using EventArgs;
using Hazards;
using Platforms;
using States.PlayerStates;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        private ItemData m_currentItem;
        private int m_currentSceneIndex;
        private PlayerState m_currentPlayerState;
        
        private static PlayerController s_instance;
        public static PlayerController Instance => s_instance ?? FindObjectOfType<PlayerController>();

        public Animator Animator => this.m_animator;
        public InputProcessor InputProcessor => this.m_inputProcessor;
        public Transform PlayerCamera => this.m_camera;
        public PlayerData PlayerData => this.m_playerData;
        public Rigidbody Rigidbody => this.m_rigidbody;
        public GameObject Feet => this.m_feet;

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

        #region  Unity Event Functions
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

            SceneManager.sceneLoaded += (arg0, mode) =>
            {
                this.ChangeState(new RespawnState(this));
                this.m_inputProcessor.enabled = true;
            };
            this.m_rigidbody = this.GetComponent<Rigidbody>();
            this.m_inputProcessor = this.GetComponent<InputProcessor>();
            this.m_camera = Camera.main.transform;
            this.m_animator = this.GetComponent<Animator>();
            this.m_currentPlayerState = new RespawnState(this);
        }

        private void Start()
        {
        }

        void Update()
        {
            if (!this.m_inputProcessor.enabled)
                return;
            
            this.m_currentPlayerState.Tick(Time.deltaTime);
        }
        
        #endregion

        public void ChangeState(PlayerState newPlayerState)
        {
            this.m_currentPlayerState?.OnStateExit();

            this.m_currentPlayerState = newPlayerState;
            
            this.m_currentPlayerState.OnStateEnter();
            this.HandleStateChange();
        }

        private void HandleStateChange()
        {
            var stateType = this.m_currentPlayerState.GetType();
            if (stateType == typeof(RespawnState))
            {
                this.m_respawned?.Invoke(this, System.EventArgs.Empty);
                this.m_currentItem = null;
            }
        }
        
        public void UseItem()
        {
            if (this.m_currentItem == null)
                return;
            
            var bullet = Instantiate(this.m_currentItem.Bullet, this.m_barrel.transform.position, this.m_barrel.transform.rotation);
            var targetDir = this.m_barrel.transform.forward - this.transform.position;
            bullet.GetComponent<Bullet>().Dir = targetDir.normalized;
            this.m_currentItem = null;
            AudioSource.PlayClipAtPoint(this.m_playerData.ShootingSound, this.m_camera.position, 0.25f);
        }
        

        private void OnDrawGizmos()
        {
            var boxSize = new Vector3(1f, 0.2f, 1f);
            Gizmos.DrawWireCube(this.m_feet.transform.position, boxSize);
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
                this.m_rigidbody.velocity = Vector3.zero;
                this.ChangeState(new CheerState(this));
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
            if(this.m_currentPlayerState.GetType() == typeof(DieState))
                return;
            
            this.m_died?.Invoke(this, new PlayerDiedEventArgs(killer));
            this.m_currentItem = null;
            this.ChangeState(new DieState(this));
        }
    }
}